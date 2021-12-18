using QRCoder;
using SPDC.Common;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.AdminPrivileges;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.QRCodeGenerator;

namespace SPDC.Service.Services
{
    public interface ISystemPrivilegeService
    {
        bool DeleteMulti(int userId, int[] ids);

        bool AddMulti(int userId, int[] ids);

        Task<List<SystemPrivilege>> GetCriteriasAsync(int userId, string courseCode, string courseName);
        Task<PaginationSet<CoursePrivilegesViewModel>> GetCoursePrivileges(PrivilegesBindingModel seach, int langCode, int adminId);
        Task<PaginationSet<AccountSystemPrivilegesViewModel>> GetAccountSystemPrivileges(PrivilegesBindingModel search, int adminId);
        Task<PaginationSet<ContentPrivilegesViewModel>> GetContentPrivileges(PrivilegesBindingModel search, int adminId);
        Task<ResultModel<bool>> UpdateCoursePrivileges(CoursePrivilegesBindingModel model, int idAdmin);
        Task<ResultModel<bool>> UpdateAccountSystemPrivileges(AccountSystemPrivilegesBindingModel model, int idAdmin);
        Task<ResultModel<bool>> UpdateContentPrivileges(ContentPrivilegesBindingModel model, int idAdmin);
        Task<ResultModel<bool>> SuspendActiveUser(SuspendActiveBindingModel modelBinding, int idAdmin);
        Task<ResultModel<bool>> EditAdminAccount(AdminAccountBindingModel model, int idAdmin);
        Task<ResultModel<AdminPermissionViewModel>> GetPermissionAccount(int adminId);

        Task<List<ApplicationUser>> GetUserPermissons(string ldapacc, int courseId, int idAdmin);
        Task<ApplicationUser> GetUserPermissonsById(int id);
        Task<ResultModel<FileReturnViewModel>> DownloadQRCode(string link);
        Task<ResultModel<FileReturnViewModel>> DownloadImageQRcode(string link);
    }

    public class SystemPrivilegeService : ISystemPrivilegeService
    {

        private IUnitOfWork _unitOfWork;

        private ISystemPrivilegeRepository _repository;

        private IAdminPermissionRepository _adminPermissionRepository;

        private IUserRepository _userRepository;

        public SystemPrivilegeService(IUnitOfWork unitOfWork, ISystemPrivilegeRepository repository, IAdminPermissionRepository adminPermissionRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _adminPermissionRepository = adminPermissionRepository;
            _userRepository = userRepository;
        }

        public bool AddMulti(int userId, int[] ids)
        {
            try
            {
                foreach (var courseId in ids)
                {
                    _repository.Add(new SystemPrivilege()
                    {
                        UserId = userId,
                        CourseId = courseId
                    });
                }
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool DeleteMulti(int userId, int[] ids)
        {
            try
            {
                _repository.DeleteMulti(x => ids.Contains(x.CourseId) && x.UserId == userId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<List<SystemPrivilege>> GetCriteriasAsync(int userId, string courseCode, string courseName)
        {
            var dblst = await _repository.GetMulti(x => x.UserId == userId && x.Course.CourseCode.Contains(courseCode) && x.Course.CourseTrans.Any(y => y.CourseName.Contains(courseName)), new string[] { "Course", "Course.CourseTrans" });

            return dblst;
        }

        public async Task<PaginationSet<CoursePrivilegesViewModel>> GetCoursePrivileges(PrivilegesBindingModel search, int langCode, int adminId)
        {
            var result = (await _repository.GetMultiPaging(x =>
            (x.UserId != adminId) &&
           (string.IsNullOrEmpty(search.LDAPAccount) ? true : x.User.UserName.Contains(search.LDAPAccount)) &&
           (string.IsNullOrEmpty(search.CourseCode) ? true : x.Course.CourseCode.Contains(search.CourseCode)) &&
           (string.IsNullOrEmpty(search.CourseName) ? true : x.Course.CourseTrans.Any(y => y.CourseName.Contains(search.CourseName)) // x.Course.CourseTrans.Any(n => n.CourseName.Contains(search.CourseName)
           ),
            "Id",
            true,
            search.Index,
            search.Size,
            new string[] { "Course", "User", "Course.CourseTrans" }
            )).Select(n =>
            n.ToCoursePrivilegesViewModel(langCode));

            var total = await _repository.Count(x =>
            (x.UserId != adminId) &&
           (string.IsNullOrEmpty(search.LDAPAccount) ? true : x.User.UserName.Contains(search.LDAPAccount)) &&
           (string.IsNullOrEmpty(search.CourseCode) ? true : x.Course.CourseCode.Contains(search.CourseCode)) &&
           (string.IsNullOrEmpty(search.CourseName) ? true : x.Course.CourseTrans.Any(n => n.CourseName.Contains(search.CourseName))));

            return new PaginationSet<CoursePrivilegesViewModel>()
            {
                Items = result,
                Page = search.Index,
                TotalCount = total
            };
        }

        public async Task<PaginationSet<AccountSystemPrivilegesViewModel>> GetAccountSystemPrivileges(PrivilegesBindingModel search, int adminId)
        {
            var result = (await _adminPermissionRepository.GetMultiPaging(x =>
            (x.Id != adminId) &&
            (string.IsNullOrEmpty(search.LDAPAccount) ? true : x.User.UserName.Contains(search.LDAPAccount)) &&
           (string.IsNullOrEmpty(search.CourseCode) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseCode.Contains(search.CourseCode))) &&
           (string.IsNullOrEmpty(search.CourseName) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseTrans.Any(m => m.CourseName.Contains(search.CourseName)))),
            "Id",
            true,
            search.Index,
            search.Size,
            new string[] { "User" }
            )).Select(n => n.ToAdminAccountBindingModel(n.User.UserName, n.User.AdminEmail));

            var total = await _adminPermissionRepository.Count(x =>
            (x.Id != adminId) &&
            (string.IsNullOrEmpty(search.LDAPAccount) ? true : x.User.UserName.Contains(search.LDAPAccount)) &&
           (string.IsNullOrEmpty(search.CourseCode) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseCode.Contains(search.CourseCode))) &&
           (string.IsNullOrEmpty(search.CourseName) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseTrans.Any(m => m.CourseName.Contains(search.CourseName)))));

            return new PaginationSet<AccountSystemPrivilegesViewModel>()
            {
                Items = result,
                Page = search.Index,
                TotalCount = total
            };
        }

        public async Task<PaginationSet<ContentPrivilegesViewModel>> GetContentPrivileges(PrivilegesBindingModel search, int adminId)
        {
            var result = (await _adminPermissionRepository.GetMultiPaging(x =>
            (x.Id != adminId) &&
            (string.IsNullOrEmpty(search.LDAPAccount) ? true : x.User.UserName.Contains(search.LDAPAccount)) &&
           (string.IsNullOrEmpty(search.CourseCode) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseCode.Contains(search.CourseCode))) &&
           (string.IsNullOrEmpty(search.CourseName) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseTrans.Any(m => m.CourseName.Contains(search.CourseName)))),
            "Id",
            true,
            search.Index,
            search.Size,
            new string[] { "User" }
            )).Select(n => n.ToContentPrivilegesViewModel(n.User.UserName, n.User.AdminEmail));

            var total = await _adminPermissionRepository.Count(x =>
            (x.Id != adminId) &&
            (string.IsNullOrEmpty(search.LDAPAccount) ? true : x.User.UserName.Contains(search.LDAPAccount)) &&
           (string.IsNullOrEmpty(search.CourseCode) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseCode.Contains(search.CourseCode))) &&
           (string.IsNullOrEmpty(search.CourseName) ? true : x.User.SystemPrivileges.Any(n => n.Course.CourseTrans.Any(m => m.CourseName.Contains(search.CourseName)))));

            return new PaginationSet<ContentPrivilegesViewModel>()
            {
                Items = result,
                Page = search.Index,
                TotalCount = total
            };
        }


        public async Task<ResultModel<bool>> UpdateCoursePrivileges(CoursePrivilegesBindingModel model, int idAdmin)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var adminPermission = _adminPermissionRepository.GetSingleById(idAdmin);

            if (adminPermission.Status != 1)
            {
                result.Message = "Your Account is Suspended";
                return result;
            }
            if (!adminPermission.IsAssignAdmin)
            {
                result.Message = "No permission for this action";
                return result;
            }

            var priviUpdate = await _repository.GetSingleByCondition(x => x.UserId == model.UserId && x.CourseId == model.CourseId);
            if (priviUpdate == null)
            {
                result.Message = "User's permission is no more exist for this course";
                return result;
            }

            UpdatedCoursePrivileges(priviUpdate, model);

            _repository.Update(priviUpdate);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> UpdateAccountSystemPrivileges(AccountSystemPrivilegesBindingModel model, int idAdmin)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var adminPermission = _adminPermissionRepository.GetSingleById(idAdmin);

            if (adminPermission.Status != 1)
            {
                result.Message = "Your Account is Suspended";
                return result;
            }
            if (!adminPermission.IsAssignAdmin)
            {
                result.Message = "No permission for this action";
                return result;
            }

            var priviUpdate = await _adminPermissionRepository.GetSingleByCondition(x => x.Id == model.UserId);

            UpdatedAccountSystemPrivileges(priviUpdate, model);

            _adminPermissionRepository.Update(priviUpdate);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> UpdateContentPrivileges(ContentPrivilegesBindingModel model, int idAdmin)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var adminPermission = _adminPermissionRepository.GetSingleById(idAdmin);

            if (adminPermission.Status != 1)
            {
                result.Message = "Your Account is Suspended";
                return result;
            }
            if (!adminPermission.IsAssignAdmin)
            {
                result.Message = "No permission for this action";
                return result;
            }

            var priviUpdate = await _adminPermissionRepository.GetSingleByCondition(x => x.Id == model.UserId);

            UpdatedContentPrivileges(priviUpdate, model);

            _adminPermissionRepository.Update(priviUpdate);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> SuspendActiveUser(SuspendActiveBindingModel modelBinding, int idAdmin)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var adminPermission = _adminPermissionRepository.GetSingleById(idAdmin);
            var userUpdate = _adminPermissionRepository.GetSingleById(modelBinding.UserId);

            if (userUpdate == null)
            {
                result.Message = "User's permission is no more exist for this course";
                return result;
            }

            if ((modelBinding.IsActive == true && userUpdate.Status == 1) || (modelBinding.IsActive == false && userUpdate.Status == 0))
            {
                result.Message = modelBinding.IsActive ? "User is already active" : "User is already suspend";
                result.IsSuccess = true;
                return result;
            }

            if (modelBinding.IsActive == false && userUpdate.Status == 1 && adminPermission.IsSuspendAdmin)
            {
                userUpdate.Status = modelBinding.IsActive ? (byte)1 : (byte)0;
            }

            if (modelBinding.IsActive == true && userUpdate.Status == 0 && adminPermission.IsActiveAdmin)
            {
                userUpdate.Status = modelBinding.IsActive ? (byte)1 : (byte)0;
            }

            _adminPermissionRepository.Update(userUpdate);
            _unitOfWork.Commit();
            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        private void UpdatedAccountSystemPrivileges(AdminPermission priviUpdate, AccountSystemPrivilegesBindingModel modelBinding)
        {
            priviUpdate.IsCreateAdmin = modelBinding.IsCreateAdmin;
            priviUpdate.IsSuspendAdmin = modelBinding.IsSuspendAdmin;
            priviUpdate.IsActiveAdmin = modelBinding.IsActiveAdmin;
            priviUpdate.IsEditAdmin = modelBinding.IsEditAdmin;
            priviUpdate.IsAssignAdmin = modelBinding.IsAssignAdmin;
        }

        private void UpdatedContentPrivileges(AdminPermission priviUpdate, ContentPrivilegesBindingModel modelBinding)
        {
            priviUpdate.IsCreateContent = modelBinding.IsCreateContent;
            priviUpdate.IsViewContent = modelBinding.IsViewContent;
            priviUpdate.IsEditContent = modelBinding.IsEditContent;
            priviUpdate.IsDeleteContent = modelBinding.IsDeleteContent;
            priviUpdate.IsApproveContent = modelBinding.IsApproveContent;
            priviUpdate.IsUnapproveContent = modelBinding.IsUnapproveContent;
            priviUpdate.IsPublishContent = modelBinding.IsPublishContent;
            priviUpdate.IsUnpublishContent = modelBinding.IsUnpublishContent;
            priviUpdate.IsCreateContent = modelBinding.IsCreateContent;

            priviUpdate.IsBatchApplication = modelBinding.IsBatchApplication;
            priviUpdate.IsAssessment = modelBinding.IsAssessment;
            priviUpdate.IsBatchPayment = modelBinding.IsBatchPayment;
            priviUpdate.IsAttendance = modelBinding.IsAttendance;
        }

        private void UpdatedCoursePrivileges(SystemPrivilege priviUpdate, CoursePrivilegesBindingModel modelBinding)
        {
            priviUpdate.IsCreateCourse = modelBinding.IsCreateCourse;
            priviUpdate.IsViewCourse = modelBinding.IsViewCourse;
            priviUpdate.IsEditCourse = modelBinding.IsEditCourse;

            priviUpdate.IsUserCalendar = modelBinding.IsUserCalendar;
            priviUpdate.IsSubmitAndCancelCourse = modelBinding.IsSubmitAndCancelCourse;
            priviUpdate.IsFirstApproveAndRejectCourse = modelBinding.IsFirstApproveAndRejectCourse;
            priviUpdate.IsSecondpproveAndRejectCourse = modelBinding.IsSecondpproveAndRejectCourse;
            priviUpdate.IsThirdApproveAndRejectCourse = modelBinding.IsThirdApproveAndRejectCourse;
            priviUpdate.IsSubmitAndCancelClass = modelBinding.IsSubmitAndCancelClass;
            priviUpdate.IsFirstApproveAndRejectClass = modelBinding.IsFirstApproveAndRejectClass;
            priviUpdate.IsSecondpproveAndRejectClass = modelBinding.IsSecondpproveAndRejectClass;
            priviUpdate.IsThirdApproveAndRejectClass = modelBinding.IsThirdApproveAndRejectClass;
        }

        public async Task<ResultModel<bool>> EditAdminAccount(AdminAccountBindingModel model, int idAdmin)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            if (_adminPermissionRepository.GetSingleById(idAdmin).IsEditAdmin == false)
            {
                result.Message = "No permission for this action";
                return result;
            }
            var userUpdate = await _userRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "SystemPrivileges" });
            if (!model.Email.Trim().Equals(userUpdate.AdminEmail.Trim()))
            {
                var checkEmailExist = await _userRepository.CheckContains(x => x.AdminEmail.Trim().Equals(model.Email.Trim()) || x.Email.Trim().Equals(model.Email.Trim()));
                if (checkEmailExist)
                {
                    result.Message = "Email already existed";
                    return result;
                }
                userUpdate.AdminEmail = model.Email.Trim();
            }

            List<int> lstIdDelete = new List<int>();

            var lstCourseId = userUpdate.SystemPrivileges.Select(x => x.CourseId);

            foreach (var item in lstCourseId)
            {
                if (!model.CourseIds.Contains(item))
                {
                    lstIdDelete.Add(item);
                }
            }

            foreach (var item in model.CourseIds)
            {
                if (!lstCourseId.Contains(item))
                {
                    //lstIdInsert.Add(item);
                    userUpdate.SystemPrivileges.Add(new SystemPrivilege()
                    {
                        UserId = userUpdate.Id,
                        CourseId = item,
                    });
                }
            }

            foreach (var i in lstIdDelete)
            {
                userUpdate.SystemPrivileges.Remove(userUpdate.SystemPrivileges.Single(x => x.CourseId == i));
            }


            _userRepository.Update(userUpdate);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<AdminPermissionViewModel>> GetPermissionAccount(int adminId)
        {
            ResultModel<AdminPermissionViewModel> result = new ResultModel<AdminPermissionViewModel>();
            AdminPermissionViewModel data = new AdminPermissionViewModel();
            var cmsPermission = _adminPermissionRepository.GetSingleById(adminId);
            data.Permission = cmsPermission == null ? null : cmsPermission.ToAccountPermissionViewModel();
            data.CoursePermission = (await _repository.GetMulti(x => x.UserId == adminId)).Select(v => v.ToCoursePermissionViewModel());
            result.IsSuccess = true;
            //result.Message = data.Permission != null ? "Success" : "User do not have CMS permisstion";
            result.Data = data;
            return result;
        }

        public async Task<List<ApplicationUser>> GetUserPermissons(string ldapacc, int courseId, int idAdmin)
        {
            //var result = await _userRepository.GetMulti(x => x.UserName.Contains(ldapacc) && x.Roles.Any(y => y.RoleId == 2), new string[] { "SystemPrivileges", "AdminPermission", "SystemPrivileges.Course", "SystemPrivileges.Course.CourseTrans" });

            var result2 = await _userRepository.GetMulti(x =>
                                                //(x.Id != idAdmin) && 
                                                ((x.AdminPermission != null)
                                                || (x.SystemPrivileges.Count > 0))
                                                && (string.IsNullOrEmpty(ldapacc) ? true : x.UserName.Contains(ldapacc))
                                                && (courseId == 0 ? true : x.SystemPrivileges.Any(n => n.CourseId == courseId))
                                                && (x.Roles.Any(y => y.RoleId == 2)), new string[] { "SystemPrivileges", "AdminPermission", "SystemPrivileges.Course", "SystemPrivileges.Course.CourseTrans" });
            return result2;
        }

        public async Task<ApplicationUser> GetUserPermissonsById(int id)
        {
            var result = await _userRepository.GetSingleByCondition(x => x.Id == id && x.Roles.Any(y => y.RoleId == 2), new string[] { "SystemPrivileges", "AdminPermission", "SystemPrivileges.Course", "SystemPrivileges.Course.CourseTrans" });

            return result;
        }

        private string CreateDirectory(string serPath)
        {
            var subfix = DateTime.Now.Ticks;
            var newSerPath = serPath + subfix;
            if (!Directory.Exists(newSerPath))
            {
                Common.Common.CreateDirectoryAndGrantFullControlPermission(newSerPath);
                return newSerPath;
            }
            else
            {
                subfix = subfix - 10000;
                return CreateDirectory(serPath + subfix);
            }
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadQRCode(string link)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var directory = ConfigHelper.GetByKey("QRcodeDirectory");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

            var applicantPortal = ConfigHelper.GetByKey("ApplicantPortal");
            var pos = applicantPortal.Length - 1;
            if (pos < 0)
            {
                return result;
            }

            #region Generate Folder and File QR Code
            var folderDirect = CreateDirectory(serPath);
            using (var generator = new QRCodeGenerator())
            {
                for (int i = 0; i < 3; i++)
                {
                    var newLink = i == 0 ? link.Substring(0, pos) + "/EN" + link.Substring(pos) : (i == 1 ? link.Substring(0, pos) + "/CN" + link.Substring(pos) : link.Substring(0, pos) + "/HK" + link.Substring(pos));
                    using (var data = generator.CreateQrCode(newLink, ECCLevel.M))
                    {
                        using (var code = new QRCode(data))
                        {
                            using (var bitmap = code.GetGraphic(Common.StaticConfig.PixelPerModule, Color.Black, Color.White, true))
                            {
                                var subFixFileName = i == 0 ? "EN" : (i == 1 ? "CN" : "HK");
                                var fileDirect = Common.Common.GenFileNameDuplicate(folderDirect + @"\link" + subFixFileName + ".png");
                                bitmap.Save(fileDirect, ImageFormat.Png);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Generate zip file
            var zipName = folderDirect + "_zip.zip";
            ZipFile.CreateFromDirectory(folderDirect, zipName);
            #endregion
            #region Download zip file
            if (File.Exists(zipName))
            {
                var stream = new MemoryStream(File.ReadAllBytes(zipName));
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = stream,
                    FileName = zipName.Substring(zipName.LastIndexOf(@"\")),
                    FileType = "application/zip"
                };

                // Delete File and Folder
                if (Directory.Exists(folderDirect))
                {
                    Directory.Delete(folderDirect, true);
                }
                if (File.Exists(zipName))
                {
                    File.Delete(zipName);
                }

            }
            #endregion

            return result;
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadImageQRcode(string link)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var directory = ConfigHelper.GetByKey("QRcodeDirectory");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);

            var newLink = ConfigHelper.GetByKey("ApplicantPortal") + link;

            var folderDirect = CreateDirectory(serPath);
            var fileDirect = "";
            using (var generator = new QRCodeGenerator())
            {
                using (var data = generator.CreateQrCode(newLink, ECCLevel.M))
                {
                    using (var code = new QRCode(data))
                    {
                        using (var bitmap = code.GetGraphic(Common.StaticConfig.PixelPerModule, Color.Black, Color.White, true))
                        {
                            fileDirect = Common.Common.GenFileNameDuplicate(folderDirect + @"\link.png");
                            bitmap.Save(fileDirect, ImageFormat.Png);
                        }
                    }
                }
            }

            if (File.Exists(fileDirect))
            {
                var stream = new MemoryStream(File.ReadAllBytes(fileDirect));
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = stream,
                    FileName = fileDirect.Substring(fileDirect.LastIndexOf(@"\")),
                    FileType = "image/png"
                };
                if (Directory.Exists(folderDirect))
                {
                    Directory.Delete(folderDirect, true);
                }
            }

            return result;
        }
    }
}
