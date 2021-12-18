using SPDC.Common;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace SPDC.Service.Services
{
    public interface IDocumentService
    {
        Task<List<UserDocumentsBindingModel>> GetListUserDocument(int userId);

        Task<bool> UpdateUserDocument(List<int> lstFileDelete, HttpFileCollection lstFileToInsert, int userId, string cicNumber);
        Task<bool> UpdateUserDocumentTempToTable(List<UserDocumentTempViewModel> model, int userId, string cicNumber);
        Task<bool> DeleteUserDocumentTemp(List<UserDocumentTempViewModel> model, int userId, string cicNumber);
        Task<bool> UpdateUserDocumentTemp(List<int> lstFileDelete, HttpFileCollection lstFileToInsert, int userId, string cicNumber);

        Task<ResultModel<FileReturnViewModel>> DownloadUserDocument(int documentId);

        Task<ResultModel<FileReturnViewModel>> DownloadUserDocumentTemp(int documentId);

        Task<List<CourseDocument>> GetCourseDocuments();

        Task<List<CourseDocument>> GetCourseDocumentsByCourseId(int courseId);

        Document GetDocumentById(int id);

    }
    public class DocumentService : IDocumentService
    {
        IUnitOfWork _unitOfWork;
        IUserDocumentRepository _userDocumentRepository;
        IDocumentRepository _documentRepository;

        IUserDocumentTempRepository _userDocumentTempRepository;
        ICourseDocumentsRepository _courseDocumentsRepository;
        public DocumentService(IUnitOfWork unitOfWork, IUserDocumentRepository userDocumentRepository, IDocumentRepository documentRepository, ICourseDocumentsRepository courseDocumentsRepository,
           IUserDocumentTempRepository userDocumentTempRepository)
        {
            _unitOfWork = unitOfWork;
            _userDocumentRepository = userDocumentRepository;
            _documentRepository = documentRepository;
            _userDocumentTempRepository = userDocumentTempRepository;
            _courseDocumentsRepository = courseDocumentsRepository;
        }

        public async Task<List<UserDocumentsBindingModel>> GetListUserDocument(int userId)
        {
            List<UserDocumentsBindingModel> lstFileNameReturn = new List<UserDocumentsBindingModel>();
            var lstFileName = await _documentRepository.GetMulti(x => x.UserDocuments.Any(n => n.UserId == userId), new string[] { "UserDocuments" });
            try
            {
                var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);



                for (int i = 0; i < lstFileName.Count; i++)
                {
                    var FileName = Path.GetFileName(lstFileName[i].Url);
                    if (!String.IsNullOrEmpty(FileName))
                    {
                        lstFileNameReturn.Add(new UserDocumentsBindingModel()
                        {
                            Id = lstFileName[i].Id,
                            FileName = Path.GetFileName(FileName)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            _unitOfWork.Commit();
            return lstFileNameReturn;
        }

        public async Task<bool> UpdateUserDocument(List<int> lstFileDelete, HttpFileCollection lstFileToUpload, int userId, string cicNumber)
        {
            var lstUserDocument = await _documentRepository.GetMulti(x => x.UserDocuments.Any(n => n.UserId == userId), new string[] { "UserDocuments" });
            try
            {
                var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

                for (int i = 0; i < lstFileDelete.Count; i++)
                {
                    var doc = lstUserDocument.FirstOrDefault(x => x.Id == lstFileDelete[i]);
                    if (doc != null)
                    {
                        var userDoc = doc.UserDocuments.FirstOrDefault(x => x.DocumentId == doc.Id);
                        _userDocumentRepository.Delete(userDoc);

                        var tempUrl = serPath + doc.Url;
                        if (doc != null && File.Exists(tempUrl))
                        {
                            File.Delete(tempUrl);
                        }
                        _documentRepository.Delete(doc);
                    }
                }

                for (int i = 0; i < lstFileToUpload.Count; i++)
                {
                    var file = lstFileToUpload[i];
                    if (file.ContentLength > StaticConfig.MaximumFileLength)
                    {
                        return false;
                    }
                    string originalFileExtension = file.ContentType;
                    string originalFileName = cicNumber + "\\" + Path.GetFileName(file.FileName);

                    if (!Directory.Exists(serPath + cicNumber))
                    {
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(serPath + cicNumber);
                    }

                    var path = Common.Common.GenFileNameDuplicate(serPath + originalFileName);
                    file.SaveAs(path);

                    var newFileName = cicNumber + "\\" + Path.GetFileName(path);
                    _documentRepository.Add(EntityHelpers.ToDocument(newFileName, originalFileExtension, Path.GetFileName(path), userId));
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateUserDocumentTempToTable(List<UserDocumentTempViewModel> model, int userId, string cicNumber)
        {
            try
            {
                // Step1: delete all form table data
                var lstDocument = await _documentRepository.GetMulti(x => x.UserDocuments.Any(n => n.UserId == userId), new string[] { "UserDocuments" });

                foreach (var item in lstDocument)
                {
                    _userDocumentRepository.DeleteMulti(n => n.UserId == userId);
                    var doc = lstDocument.FirstOrDefault(x => x.Id == item.Id);
                    _documentRepository.Delete(doc);
                }
                //Get all data temp and inster table data
                var lstUserDocument = await _documentRepository.GetMulti(x => x.UserDocumentsTemps.Any(n => n.UserId == userId), new string[] { "UserDocumentsTemps" });
                foreach (var item in lstUserDocument)
                {
                    _documentRepository.Add(EntityHelpers.ToDocument(item.Url, item.ContentType, Path.GetFileName(item.FileName), userId));

                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateUserDocumentTemp(List<int> lstFileDelete, HttpFileCollection lstFileToUpload, int userId, string cicNumber)
        {
            var lstUserDocument = await _documentRepository.GetMulti(x => x.UserDocumentsTemps.Any(n => n.UserId == userId), new string[] { "UserDocumentsTemps" });
            try
            {
                var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

                for (int i = 0; i < lstFileDelete.Count; i++)
                {
                    var doc = lstUserDocument.FirstOrDefault(x => x.Id == lstFileDelete[i]);
                    if (doc != null)
                    {
                        var userDoc = doc.UserDocumentsTemps.FirstOrDefault(x => x.DocumentId == doc.Id);
                        _userDocumentTempRepository.Delete(userDoc);

                        var tempUrl = serPath + doc.Url;
                        if (doc != null && File.Exists(tempUrl))
                        {
                            File.Delete(tempUrl);
                        }
                        _documentRepository.Delete(doc);
                    }
                }

                for (int i = 0; i < lstFileToUpload.Count; i++)
                {
                    var file = lstFileToUpload[i];
                    if (file.ContentLength > StaticConfig.MaximumFileLength)
                    {
                        return false;
                    }
                    string originalFileExtension = file.ContentType;
                    string originalFileName = cicNumber + "\\" + Path.GetFileName(file.FileName);

                    if (!Directory.Exists(serPath + cicNumber))
                    {
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(serPath + cicNumber);
                    }

                    var path = Common.Common.GenFileNameDuplicate(serPath + originalFileName);
                    file.SaveAs(path);

                    var newFileName = cicNumber + "\\" + Path.GetFileName(path);
                    _documentRepository.Add(EntityHelpers.ToDocumentForTempUser(newFileName, originalFileExtension, Path.GetFileName(path), userId));
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> DeleteUserDocumentTemp(List<UserDocumentTempViewModel> model, int userId, string cicNumber)
        {
            var lstUserDocument = await _documentRepository.GetMulti(x => x.UserDocumentsTemps.Any(n => n.UserId == userId), new string[] { "UserDocumentsTemps" });
            try
            {
                foreach (var item in lstUserDocument)
                {
                    _userDocumentTempRepository.DeleteMulti(n => n.UserId == userId);
                    var doc = lstUserDocument.FirstOrDefault(x => x.Id == item.Id);
                    _documentRepository.Delete(doc);
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        public async Task<ResultModel<FileReturnViewModel>> DownloadUserDocument(int documentId)
        {
            var doc = _documentRepository.GetSingleById(documentId);
            //string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            var result = new ResultModel<FileReturnViewModel>();
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }

            var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
            var pathDoc = Path.Combine(serPath, doc.Url);


            if (File.Exists(pathDoc))
            {
                var stream = new MemoryStream(File.ReadAllBytes(pathDoc));
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = stream,
                    FileType = doc.ContentType,
                    FileName = doc.FileName
                };
                //var tempDirectory = ConfigHelper.GetByKey("TempFileToDownload");
                //string uniqueTempFolder = doc.Url.Substring(0, doc.Url.IndexOf('\\') - 1) + "-" + DateTime.Now.ToString(StaticConfig.cFormatShortDateTime);
                //var tempFileToDownloadPhysicalPath = Path.Combine(tempDirectory, "User_Document/", uniqueTempFolder + "/" + doc.FileName + doc.ContentType);
                //try
                //{
                //    tempFileToDownloadPhysicalPath = System.Web.HttpContext.Current.Server.MapPath(tempFileToDownloadPhysicalPath);
                //    string folderPath = Path.GetDirectoryName(tempFileToDownloadPhysicalPath);
                //    if (!Directory.Exists(folderPath))
                //    {
                //        Common.Common.CreateDirectoryAndGrantFullControlPermission(folderPath);
                //    }

                //    File.Copy(pathDoc, tempFileToDownloadPhysicalPath, true);

                //    if (File.Exists(tempFileToDownloadPhysicalPath))
                //    {
                //        var index = tempDirectory.IndexOf('/') + 1;

                //        string tempFileToDownloadVirtualPath = Common.Uri.Combine(domainName, HostingEnvironment.ApplicationVirtualPath, tempDirectory.Substring(index, tempDirectory.Length - index) + "User_Document/", uniqueTempFolder, doc.FileName + doc.ContentType);
                //        tempFileToDownloadVirtualPath = tempFileToDownloadVirtualPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                //        Common.Common.CleanupTempFolderTaskExcute(tempFileToDownloadPhysicalPath);
                //        return tempFileToDownloadVirtualPath;
                //    }
                //}
                //catch (IOException ex)
                //{
                //    throw;
                //}
            }

            result.Message = "Failed to export";
            return result;
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadUserDocumentTemp(int documentId)
        {
            var doc = _documentRepository.GetSingleById(documentId);
            var result = new ResultModel<FileReturnViewModel>();
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }

            var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
            var pathDoc = Path.Combine(serPath, doc.Url);

            if (File.Exists(pathDoc))
            {
                var stream = new MemoryStream(File.ReadAllBytes(pathDoc));
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = stream,
                    FileType = doc.ContentType,
                    FileName = doc.FileName
                };
            }
            result.Message = "Failed to export";
            return result;
        }

        public async Task<List<CourseDocument>> GetCourseDocuments()
        {
            var result = await _courseDocumentsRepository.GetMulti(x => true, new string[] { "Course", "Document" });
            return result;
        }

        public async Task<List<CourseDocument>> GetCourseDocumentsByCourseId(int courseId)
        {
            var result = await _courseDocumentsRepository.GetMulti(x => x.CourseId == courseId, new string[] { "Course", "Document" });
            return result;
        }

        public Document GetDocumentById(int id)
        {
            return _documentRepository.GetSingleById(id);
        }
    }
}
