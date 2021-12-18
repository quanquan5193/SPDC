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

namespace SPDC.Service.Services
{
    public interface IUploadFileService
    {
        Task<bool> UploadFile(HttpFileCollection lstFileToUpload, int userId);

        Task<bool> DeleteFile(int docId);
        Task<bool> DeleteFileTemp(int docId);

        Task<IEnumerable<UserDocument>> GetAllFile(int userID);
        Task<IEnumerable<UserDocumentTemp>> GetAllFileTemp(int userID);

        Task<ResultModel<FileReturnViewModel>> DownloadCourseDocument(int documentId);
        Task<ResultModel<FileReturnViewModel>> DownloadCourseDocumentTemp(int documentId);
    }

    public class UploadFileService : IUploadFileService
    {

        private IDocumentRepository _documentRepository;
        private IUnitOfWork _unitOfWork;
        private IUserDocumentRepository _userDocumentRepository;
       
        private IUserDocumentTempRepository _userDocumentTempRepository;


        public UploadFileService(IDocumentRepository documentRepository, IUnitOfWork unitOfWork, IUserDocumentRepository userDocumentRepository,
             IUserDocumentTempRepository userDocumentTempRepository)
        {
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
            _userDocumentRepository = userDocumentRepository;
            _userDocumentTempRepository = userDocumentTempRepository;
        }

        public async Task<IEnumerable<UserDocument>> GetAllFile(int userID)
        {
            var documents = await _userDocumentRepository.GetMulti(x => x.UserId == userID, new string[] { "Document" });

            return documents;
        }


        public async Task<IEnumerable<UserDocumentTemp>> GetAllFileTemp(int userID)
        {
            var documents = await _userDocumentTempRepository.GetMulti(x => x.UserId == userID, new string[] { "Document" });

            return documents;
        }

        public async Task<bool> DeleteFile(int docId)
        {
            try
            {
                var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

                var result = await _documentRepository.GetSingleByCondition(x => x.Id == docId);

                var path = (serPath + result.FileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                _userDocumentRepository.DeleteMulti(n => n.DocumentId == result.Id);
                _documentRepository.Delete(result);


                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return true;
                throw;
            }
        }


        public async Task<bool> DeleteFileTemp(int docId)
        {
            try
            {
                var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

                var result = await _documentRepository.GetSingleByCondition(x => x.Id == docId);

                var path = (serPath + result.FileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                _userDocumentTempRepository.DeleteMulti(n => n.Document.Id == result.Id);
                //_documentTempRepository.Delete(result);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return true;
                throw;
            }
        }

        public async Task<bool> UploadFile(HttpFileCollection lstFileToUpload, int userId)
        {
            try
            {
                var directory = ConfigHelper.GetByKey("UserDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

                for (int i = 0; i < lstFileToUpload.Count; i++)
                {
                    var file = lstFileToUpload[i];
                    if (file.ContentLength > StaticConfig.MaximumFileLength)
                    {
                        return false;
                    }
                    string originalFileExtension = file.ContentType;
                    string originalFileName = Path.GetFileName(file.FileName);

                    if (!Directory.Exists(serPath))
                    {
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(serPath);
                    }

                    var path = Common.Common.GenFileNameDuplicate(serPath + originalFileName);
                    file.SaveAs(path);

                    var newFileName = Path.GetFileName(path);
                    _documentRepository.Add(EntityHelpers.ToDocument(newFileName, originalFileExtension, Path.GetFileName(path), userId));
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return true;
                throw;
            }
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadCourseDocument(int documentId)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var doc = _documentRepository.GetSingleById(documentId);
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

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
            result.Message = "File is not found";
            return result;
        }


        public async Task<ResultModel<FileReturnViewModel>> DownloadCourseDocumentTemp(int documentId)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var doc = _documentRepository.GetSingleById(documentId);
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

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
            result.Message = "File is not found";
            return result;
        }
    }

}
