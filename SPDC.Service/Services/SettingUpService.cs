using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels.Approval;
using SPDC.Model.Models;
using SPDC.Model.ViewModels.Approval;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static SPDC.Common.StaticConfig;

namespace SPDC.Service.Services
{
    public interface ISettingUpService
    {
        Task<PaginationSet<CourseApprovedHistoryViewModel>> GetListCourseApprovedHistoryByCourseId(CourseHistoryFilter filter);
        Task<PaginationSet<ClassApprovedHistoryViewModel>> GetListClassApprovedHistoryByCourseId(ClassHistoryFilter filter);
        Task<PaginationSet<SubClassApprovedHistoryViewModel>> GetListSubClassApprovalHistories(SubClassHistoryFilter filter);
        Task<ResultModel<bool>> ChangeApprovedStatusCourseSetup(int userId, int courseId, int approvedStatus, string remarks, HttpFileCollection files, string langCode);
        Task<ResultModel<bool>> ChangeApprovedStatusClassSetup(int userId, int courseId, int approvedStatus, string remarks, HttpFileCollection files);
        Task<ResultModel<bool>> SubmitCancelledOrPostponedSubClassSetup(CancelPostponedSubClassBindingModel model, int userId, HttpFileCollection files);
        Task<ResultModel<bool>> ChangeApprovedStatusSubClass(CancelApprovalSubClassBindingModel model, int userId, HttpFileCollection files);
    }

    public class SettingUpService : ISettingUpService
    {
        IUnitOfWork _unitOfWork;
        ISystemPrivilegeRepository _systemPrivilegeRepository;
        ICourseRepository _courseRepository;
        IClassRepository _classRepository;
        IDocumentRepository _documentRepository;
        ICourseAppovedStatusHistoryRepository _courseAppovedStatusHistoryRepository;
        IClassAppovedStatusHistoryRepository _classAppovedStatusHistoryRepository;
        ICourseHistoryDocumentRepository _courseHistoryDocumentRepository;
        IClassHistoryDocumentRepository _classHistoryDocumentRepository;
        ISubClassApprovedStatusHistoryRepository _subClassApprovedStatusHistoryRepository;
        ISubClassHistoryDocumentRepository _subClassHistoryDocumentRepository;
        ISubClassDraftRepository _subClassDraftRepository;
        IClientRepository _clientRepository;
        ICommonDataService _commonDataService;
        IUserRepository _userRepository;

        public SettingUpService(IUnitOfWork unitOfWork, ISystemPrivilegeRepository systemPrivilegeRepository, ICourseRepository courseRepository, IClassRepository classRepository
            , IDocumentRepository documentRepository, ICourseAppovedStatusHistoryRepository courseAppovedStatusHistoryRepository, IClassAppovedStatusHistoryRepository classAppovedStatusHistoryRepository,
            ICourseHistoryDocumentRepository courseHistoryDocumentRepository, IClassHistoryDocumentRepository classHistoryDocumentRepository,
            ISubClassApprovedStatusHistoryRepository subClassApprovedStatusHistoryRepository, ISubClassHistoryDocumentRepository subClassHistoryDocumentRepository,
            ISubClassDraftRepository subClassDraftRepository, IClientRepository clientRepository, ICommonDataService commonDataService, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _systemPrivilegeRepository = systemPrivilegeRepository;
            _courseRepository = courseRepository;
            _classRepository = classRepository;
            _documentRepository = documentRepository;
            _courseAppovedStatusHistoryRepository = courseAppovedStatusHistoryRepository;
            _classAppovedStatusHistoryRepository = classAppovedStatusHistoryRepository;
            _courseHistoryDocumentRepository = courseHistoryDocumentRepository;
            _classHistoryDocumentRepository = classHistoryDocumentRepository;
            _subClassApprovedStatusHistoryRepository = subClassApprovedStatusHistoryRepository;
            _subClassHistoryDocumentRepository = subClassHistoryDocumentRepository;
            _subClassDraftRepository = subClassDraftRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _commonDataService = commonDataService;
        }

        public async Task<PaginationSet<CourseApprovedHistoryViewModel>> GetListCourseApprovedHistoryByCourseId(CourseHistoryFilter filter)
        {
            var listHistory = (await _courseAppovedStatusHistoryRepository.GetMultiPaging(x => x.CourseId == filter.CourseId, "Id", false, filter.Page, filter.Size,
                new string[] { "CourseHistoryDocuments", "CourseHistoryDocuments.Document" })).Select(x => x.ToCourseApprovedHistoryViewModel(_userRepository.GetSingleById(x.ApprovalUpdatedBy).DisplayName));
            var total = await _courseAppovedStatusHistoryRepository.Count(x => x.CourseId == filter.CourseId);

            return new PaginationSet<CourseApprovedHistoryViewModel>()
            {
                Items = listHistory,
                Page = filter.Page,
                TotalCount = total
            };
        }

        public async Task<PaginationSet<ClassApprovedHistoryViewModel>> GetListClassApprovedHistoryByCourseId(ClassHistoryFilter filter)
        {
            var listHistory = (await _classAppovedStatusHistoryRepository.GetMultiPaging(x => x.CourseId == filter.CourseId, "Id", false, filter.Page, filter.Size,
                new string[] { "ClassHistoryDocuments", "ClassHistoryDocuments.Document" })).Select(x => x.ToClassApprovedHistoryViewModel(_userRepository.GetSingleById(x.ApprovalUpdatedBy).DisplayName));
            var total = await _classAppovedStatusHistoryRepository.Count(x => x.CourseId == filter.CourseId);

            return new PaginationSet<ClassApprovedHistoryViewModel>()
            {
                Items = listHistory,
                Page = filter.Page,
                TotalCount = total
            };
        }

        public async Task<PaginationSet<SubClassApprovedHistoryViewModel>> GetListSubClassApprovalHistories(SubClassHistoryFilter filter)
        {
            var listHistory = (await _subClassApprovedStatusHistoryRepository.GetMultiPaging(x => x.ClassId == filter.ClassId, "Id", false, filter.Page, filter.Size,
                new string[] { "SubClassHistoryDocuments", "SubClassHistoryDocuments.Document", "SubClassDraft", "Class", "SubClassHistoryDocuments.Document" })).Select(x => x.ToSubClassApprovedHistoryViewModel(_userRepository.GetSingleById(x.ApprovalUpdatedBy).DisplayName));
            var total = await _subClassApprovedStatusHistoryRepository.Count(x => x.ClassId == filter.ClassId);

            return new PaginationSet<SubClassApprovedHistoryViewModel>()
            {
                Items = listHistory,
                Page = filter.Page,
                TotalCount = total
            };
        }

        public async Task<ResultModel<bool>> ChangeApprovedStatusCourseSetup(int userId, int courseId, int approvedStatus, string remarks, HttpFileCollection files, string langCode)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == courseId, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }

            var course = await _courseRepository.GetSingleByCondition(x => x.Id == courseId, new string[] { "CourseAppovedStatusHistories", "Classes" });

            var iSuccess = false;
            switch (course.CourseApprovedStatus)
            {
                case (int)CourseApprovedStatus.Created:
                case (int)CourseApprovedStatus.Cancel:
                case (int)CourseApprovedStatus.FirstReject:
                case (int)CourseApprovedStatus.SecondReject:
                case (int)CourseApprovedStatus.ThirdReject:
                    if (approvedStatus == (int)CourseApprovedStatus.Submitted && privilege.IsSubmitAndCancelCourse)
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.Created, Common.Enums.CourseApprovedStatus.Submitted, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }

                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)CourseApprovedStatus.Submitted:
                    if (approvedStatus == (int)CourseApprovedStatus.Cancel && (privilege.IsSubmitAndCancelCourse /*|| privilege.IsFirstApproveAndRejectCourse*/))
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.Submitted, Common.Enums.CourseApprovedStatus.Cancel, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }

                        break;
                    }
                    else if (approvedStatus == (int)CourseApprovedStatus.FirstReject && privilege.IsFirstApproveAndRejectCourse)
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.Submitted, Common.Enums.CourseApprovedStatus.FirstReject, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (approvedStatus == (int)CourseApprovedStatus.FirstApproved && privilege.IsFirstApproveAndRejectCourse)
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.Submitted, Common.Enums.CourseApprovedStatus.FirstApproved, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)CourseApprovedStatus.FirstApproved:
                    if (approvedStatus == (int)CourseApprovedStatus.SecondReject && privilege.IsSecondpproveAndRejectCourse)
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.FirstApproved, Common.Enums.CourseApprovedStatus.SecondReject, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (approvedStatus == (int)CourseApprovedStatus.SecondApproved && privilege.IsSecondpproveAndRejectCourse)
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.FirstApproved, Common.Enums.CourseApprovedStatus.SecondApproved, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)CourseApprovedStatus.SecondApproved:
                    if (approvedStatus == (int)CourseApprovedStatus.ThirdReject && privilege.IsThirdApproveAndRejectCourse)
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.SecondApproved, Common.Enums.CourseApprovedStatus.ThirdReject, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (approvedStatus == (int)CourseApprovedStatus.ThirdApproved && privilege.IsThirdApproveAndRejectCourse)
                    {
                        iSuccess = UpdateCourseApprovedStatus(course, Common.Enums.CourseApprovedStatus.SecondApproved, Common.Enums.CourseApprovedStatus.ThirdApproved, userId, remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)CourseApprovedStatus.ThirdApproved:
                    result.Message = "This course is approved";
                    return result;
                default:
                    result.Message = "Action cannot perform";
                    return result;
            }

            _unitOfWork.Commit();
            if (iSuccess)
            {
                await SendCourseEmailApproved(course.CourseApprovedStatus, course);
                _unitOfWork.Commit();
            }
            result.Message = FileHelper.GetServerMessage("update_success", "ServerMessages", langCode);
            result.IsSuccess = true;
            return result;
        }

        private async Task SendCourseEmailApproved(int approvedStatus, Course course)
        {
            #region Send Mail
            string callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;

            var _submittedAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSubmitAndCancelCourse == true)).Select(x => x.UserId);
            var _submittedAdminEmails = (await _userRepository.GetMulti(x => _submittedAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _firstAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsFirstApproveAndRejectCourse == true)).Select(x => x.UserId);
            var _firstAdminEmails = (await _userRepository.GetMulti(x => _firstAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _secondAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSecondpproveAndRejectCourse == true)).Select(x => x.UserId);
            var _secondAdminEmails = (await _userRepository.GetMulti(x => _secondAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _thirdAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsThirdApproveAndRejectCourse == true)).Select(x => x.UserId);
            var _thirdAdminEmails = (await _userRepository.GetMulti(x => _thirdAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);
            switch (approvedStatus)
            {
                case (int)CourseApprovedStatus.Submitted:

                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item26", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem26Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    break;
                case (int)CourseApprovedStatus.FirstApproved:
                    foreach (var e in _secondAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item28", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem28Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)CourseApprovedStatus.FirstReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)CourseApprovedStatus.SecondApproved:
                    foreach (var e in _thirdAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item29", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem29Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)CourseApprovedStatus.SecondReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)CourseApprovedStatus.ThirdApproved:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)CourseApprovedStatus.ThirdReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)CourseApprovedStatus.Cancel:
                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item27", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem27Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
            }
            #endregion
        }

        private async Task SendClassEmailApproved(int approvedStatus, Course course)
        {
            #region Send Mail
            string callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;

            var _submittedAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSubmitAndCancelClass == true)).Select(x => x.UserId);
            var _submittedAdminEmails = (await _userRepository.GetMulti(x => _submittedAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _firstAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsFirstApproveAndRejectClass == true)).Select(x => x.UserId);
            var _firstAdminEmails = (await _userRepository.GetMulti(x => _firstAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _secondAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSecondpproveAndRejectClass == true)).Select(x => x.UserId);
            var _secondAdminEmails = (await _userRepository.GetMulti(x => _secondAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _thirdAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsThirdApproveAndRejectClass == true)).Select(x => x.UserId);
            var _thirdAdminEmails = (await _userRepository.GetMulti(x => _thirdAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);
            switch (approvedStatus)
            {
                case (int)ClassApprovedStatus.Submitted:

                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item26", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem26Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    break;
                case (int)ClassApprovedStatus.FirstApproved:
                    foreach (var e in _secondAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item28", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem28Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.FirstReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.SecondApproved:
                    foreach (var e in _thirdAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item29", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem29Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.SecondReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.ThirdApproved:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.ThirdReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.Cancel:
                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item27", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem27Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
            }
            #endregion
        }

        public async Task<ResultModel<bool>> ChangeApprovedStatusClassSetup(int userId, int courseId, int approvedStatus, string remarks, HttpFileCollection files)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == courseId, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }
            var course = await _courseRepository.GetSingleByCondition(x => x.Id == courseId, new string[] { "CourseAppovedStatusHistories", "Classes" });
            if (course.CourseApprovedStatus != (int)CourseApprovedStatus.ThirdApproved)
            {
                result.Message = "You have to approve course setup first";
                return result;
            }

            var isSuccess = false;
            switch (course.ClassApprovedStatus)
            {
                case (int)ClassApprovedStatus.Created:
                case (int)ClassApprovedStatus.Cancel:
                case (int)ClassApprovedStatus.FirstReject:
                case (int)ClassApprovedStatus.SecondReject:
                case (int)ClassApprovedStatus.ThirdReject:
                    if (approvedStatus == (int)ClassApprovedStatus.Submitted && privilege.IsSubmitAndCancelClass)
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.Created, ClassApprovedStatus.Submitted, userId, remarks, files);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }

                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)ClassApprovedStatus.Submitted:
                    if (approvedStatus == (int)ClassApprovedStatus.Cancel && (privilege.IsSubmitAndCancelClass /*|| privilege.IsFirstApproveAndRejectClass*/))
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.Submitted, ClassApprovedStatus.Cancel, userId, remarks, files);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (approvedStatus == (int)ClassApprovedStatus.FirstReject && privilege.IsFirstApproveAndRejectClass)
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.Submitted, ClassApprovedStatus.FirstReject, userId, remarks, files, true);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (approvedStatus == (int)ClassApprovedStatus.FirstApproved && privilege.IsFirstApproveAndRejectClass)
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.Submitted, ClassApprovedStatus.FirstApproved, userId, remarks, files);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)ClassApprovedStatus.FirstApproved:
                    if (approvedStatus == (int)ClassApprovedStatus.SecondReject && privilege.IsSecondpproveAndRejectClass)
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.FirstApproved, ClassApprovedStatus.SecondReject, userId, remarks, files, true);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (approvedStatus == (int)ClassApprovedStatus.SecondApproved && privilege.IsSecondpproveAndRejectClass)
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.FirstApproved, ClassApprovedStatus.SecondApproved, userId, remarks, files);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)ClassApprovedStatus.SecondApproved:
                    if (approvedStatus == (int)ClassApprovedStatus.ThirdReject && privilege.IsThirdApproveAndRejectClass)
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.SecondApproved, ClassApprovedStatus.ThirdReject, userId, remarks, files, true);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (approvedStatus == (int)ClassApprovedStatus.ThirdApproved && privilege.IsThirdApproveAndRejectClass)
                    {
                        isSuccess = UpdateClassApprovedStatus(course, ClassApprovedStatus.SecondApproved, ClassApprovedStatus.ThirdApproved, userId, remarks, files);
                        if (!isSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)ClassApprovedStatus.ThirdApproved:
                    result.Message = "This class is approved";
                    return result;
                default:
                    result.Message = "Action cannot perform";
                    return result;
            }

            _unitOfWork.Commit();
            if (isSuccess)
            {
                await SendClassEmailApproved(approvedStatus, course);
                _unitOfWork.Commit();
            }
            result.Message = "Update Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> SubmitCancelledOrPostponedSubClassSetup(CancelPostponedSubClassBindingModel model, int userId, HttpFileCollection files)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var aClass = await _classRepository.GetSingleByCondition(x => x.Id == model.ClassId,
                new string[] { "Course", "SubClassApprovedStatusHistories", "SubClassApprovedStatusHistories.SubClassHistoryDocuments"
                , "SubClassApprovedStatusHistories.SubClassHistoryDocuments.Document", "SubClassDrafts" });

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == aClass.CourseId, new string[] { "User", "User.AdminPermission" });

            #region condition
            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }

            if (aClass == null)
            {
                result.Message = "Class was not found";
                return result;
            }

            if (aClass.Course.CourseApprovedStatus != (int)CourseApprovedStatus.ThirdApproved || aClass.Course.ClassApprovedStatus != (int)ClassApprovedStatus.ThirdApproved
                /*|| aClass.Course.IsSetApplicationSetup*/)
            {
                result.Message = "Action cannot perform";
                return result;
            }

            if (aClass.Course.CourseApprovedStatus != (int)CourseApprovedStatus.ThirdApproved)
            {
                result.Message = "This course is not approved";
                return result;

            }

            if (aClass.Course.ClassApprovedStatus != (int)ClassApprovedStatus.ThirdApproved)
            {
                result.Message = "This class is not approved";
                return result;
            }
            //else if (aClass.Course.IsSetApplicationSetup)
            //{
            //    result.Message = "This application setup was approved";
            //    return result;
            //}
            var listAcceptedStatusCancelledPostponed = new List<int>() { (int)SubClassStatus.Cancelled, (int)SubClassStatus.Postponed };

            if (!privilege.IsSubmitAndCancelClass || !listAcceptedStatusCancelledPostponed.Contains(model.NewClassStatus))
            {
                result.Message = "You dont have permission for this action";
                return result;
            }

            var listSubClassApprovedStatus = new List<int>() { (int)SubClassApprovedStatus.Created, (int)SubClassApprovedStatus.ThirdApproved, (int)SubClassApprovedStatus.Cancel,
            (int)SubClassApprovedStatus.FirstReject, (int)SubClassApprovedStatus.SecondReject, (int)SubClassApprovedStatus.ThirdReject };

            if (!listSubClassApprovedStatus.Contains(aClass.SubClassApprovedStatus))
            {
                result.Message = "This class is changing status";
                return result;
            }

            if (!string.IsNullOrEmpty(model.NewClassCode) && ((await _classRepository.Count(x => x.ClassCode.Equals(model.NewClassCode))) > 0 || (await _subClassDraftRepository.Count(x => x.NewClassCode.Equals(model.NewClassCode))) > 0))
            {
                result.Message = "Class code was existed";
                return result;
            }
            #endregion

            SubmitCancelledOrPostponedSubClass(model, aClass, userId, files);
            result.Message = "Success";
            result.IsSuccess = true;
            return result;

        }

        public async Task<ResultModel<bool>> ChangeApprovedStatusSubClass(CancelApprovalSubClassBindingModel model, int userId, HttpFileCollection files)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var aClass = await _classRepository.GetSingleByCondition(x => x.Id == model.ClassId,
                new string[] { "Course", "SubClassApprovedStatusHistories", "SubClassApprovedStatusHistories.SubClassHistoryDocuments"
                , "SubClassApprovedStatusHistories.SubClassHistoryDocuments.Document", "SubClassApprovedStatusHistories.SubClassDraft" });

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == aClass.CourseId, new string[] { "User", "User.AdminPermission" });

            #region condition
            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }

            if (aClass == null)
            {
                result.Message = "Class was not found";
                return result;
            }

            if (aClass.Course.CourseApprovedStatus != (int)CourseApprovedStatus.ThirdApproved || aClass.Course.ClassApprovedStatus != (int)ClassApprovedStatus.ThirdApproved
                /*|| aClass.Course.IsSetApplicationSetup*/)
            {
                result.Message = "Action cannot perform";
                return result;
            }

            if (aClass.Course.CourseApprovedStatus != (int)CourseApprovedStatus.ThirdApproved)
            {
                result.Message = "This course is not approved";
                return result;

            }
            else if (aClass.Course.ClassApprovedStatus != (int)ClassApprovedStatus.ThirdApproved)
            {
                result.Message = "This class is not approved";
                return result;
            }
            //else if (aClass.Course.IsSetApplicationSetup)
            //{
            //    result.Message = "This application setup was approved";
            //    return result;
            //}
            #endregion

            switch (aClass.SubClassApprovedStatus)
            {
                case (int)SubClassApprovedStatus.Sumitted:
                    if (model.ApprovedStatus == (int)SubClassApprovedStatus.Cancel && (privilege.IsSubmitAndCancelClass /*|| privilege.IsFirstApproveAndRejectClass*/))
                    {
                        UpdateSubClassApprovedStatus(aClass, SubClassApprovedStatus.Sumitted, SubClassApprovedStatus.Cancel, userId, model.Remarks, files);
                        break;
                    }
                    else if (model.ApprovedStatus == (int)SubClassApprovedStatus.FirstReject && (/*privilege.IsSubmitAndCancelClass ||*/ privilege.IsFirstApproveAndRejectClass))
                    {
                        UpdateSubClassApprovedStatus(aClass, SubClassApprovedStatus.Sumitted, SubClassApprovedStatus.FirstReject, userId, model.Remarks, files);
                        break;
                    }
                    else if (model.ApprovedStatus == (int)SubClassApprovedStatus.FirstApproved && privilege.IsFirstApproveAndRejectClass)
                    {
                        UpdateSubClassApprovedStatus(aClass, SubClassApprovedStatus.Sumitted, SubClassApprovedStatus.FirstApproved, userId, model.Remarks, files);
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)SubClassApprovedStatus.FirstApproved:
                    if (model.ApprovedStatus == (int)SubClassApprovedStatus.SecondReject && privilege.IsSecondpproveAndRejectClass)
                    {
                        UpdateSubClassApprovedStatus(aClass, SubClassApprovedStatus.SecondReject, SubClassApprovedStatus.SecondReject, userId, model.Remarks, files);
                        break;
                    }
                    else if (model.ApprovedStatus == (int)SubClassApprovedStatus.SecondApproved && privilege.IsSecondpproveAndRejectClass)
                    {
                        UpdateSubClassApprovedStatus(aClass, SubClassApprovedStatus.FirstApproved, SubClassApprovedStatus.SecondApproved, userId, model.Remarks, files);
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                case (int)SubClassApprovedStatus.SecondApproved:
                    if (model.ApprovedStatus == (int)SubClassApprovedStatus.ThirdReject && privilege.IsThirdApproveAndRejectClass)
                    {
                        UpdateSubClassApprovedStatus(aClass, SubClassApprovedStatus.SecondApproved, SubClassApprovedStatus.ThirdReject, userId, model.Remarks, files);
                        break;
                    }
                    else if (model.ApprovedStatus == (int)SubClassApprovedStatus.ThirdApproved && privilege.IsThirdApproveAndRejectClass)
                    {
                        var draftClassCode = aClass.SubClassApprovedStatusHistories.FirstOrDefault()?.SubClassDraft.NewClassCode;
                        var isExistClassCode = await _classRepository.CheckContains(x => x.ClassCode.Equals(draftClassCode));
                        if (isExistClassCode)
                        {
                            result.Message = "Class code was existed";
                            return result;
                        }
                        UpdateSubClassApprovedStatus(aClass, SubClassApprovedStatus.SecondApproved, SubClassApprovedStatus.ThirdApproved, userId, model.Remarks, files);
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                default:
                    result.Message = "Action cannot perform";
                    return result;
            }

            result.Message = "Update Success";
            result.IsSuccess = true;
            return result;
        }

        #region Private function
        private bool UpdateCourseApprovedStatus(Course course, CourseApprovedStatus statusFrom, CourseApprovedStatus statusTo, int userId, string remarks, HttpFileCollection files)
        {
            if (course == null || course.Classes.Count() > 0) return false;
            course.CourseApprovedStatus = (int)statusTo;

            var approveHistory = new CourseAppovedStatusHistory();
            approveHistory.ApprovalUpdatedBy = userId;
            approveHistory.AppovalStatusFrom = (int)statusFrom;
            approveHistory.ApprovalStatusTo = (int)statusTo;
            approveHistory.ApprovalUpdatedTime = DateTime.Now;
            approveHistory.ApprovalRemarks = remarks;

            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + course.CourseCode + "\\" + DistinguishDocType.CourseSetUp.ToString();

                if (!Directory.Exists(pathDirectory))
                {
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    string originalFileExtension = file.ContentType;
                    string pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + file.FileName);
                    string originalFileName = Path.GetFileName(pathFile);
                    string url = pathFile.Substring(pathFile.IndexOf(course.CourseCode));
                    file.SaveAs(pathFile);

                    approveHistory.CourseHistoryDocuments.Add(new CourseHistoryDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.Now
                        }
                    });
                }
            }
            course.CourseAppovedStatusHistories.Add(approveHistory);

            _courseRepository.Update(course);

            return true;
        }

        private bool UpdateClassApprovedStatus(Course course, ClassApprovedStatus statusFrom, ClassApprovedStatus statusTo, int userId, string remarks, HttpFileCollection files, bool isReject = false)
        {
            if (course == null || course.CourseApprovedStatus != (int)CourseApprovedStatus.ThirdApproved || course.IsSetApplicationSetup) return false;
            course.ClassApprovedStatus = (int)statusTo;

            var approveHistory = new ClassAppovedStatusHistory();
            approveHistory.ApprovalUpdatedBy = userId;
            approveHistory.AppovalStatusFrom = (int)statusFrom;
            approveHistory.ApprovalStatusTo = (int)statusTo;
            approveHistory.ApprovalUpdatedTime = DateTime.Now;
            approveHistory.ApprovalRemarks = remarks;


            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + course.CourseCode + "\\" + DistinguishDocType.ClassSetUp.ToString();

                if (!Directory.Exists(pathDirectory))
                {
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    string originalFileExtension = file.ContentType;
                    string pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + file.FileName);
                    string originalFileName = Path.GetFileName(pathFile);
                    string url = pathFile.Substring(pathFile.IndexOf(course.CourseCode));
                    file.SaveAs(pathFile);

                    approveHistory.ClassHistoryDocuments.Add(new ClassHistoryDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.Now
                        }
                    });
                }
            }

            var listRejectCancelApplicationStatus = new List<int>() { (int)ClassApprovedStatus.Cancel, (int)ClassApprovedStatus.FirstReject,
                (int)ClassApprovedStatus.SecondReject, (int)ClassApprovedStatus.ThirdReject };

            if (listRejectCancelApplicationStatus.Contains((int)statusTo))
            {
                if (isReject)
                {
                    foreach (var item in course.Classes)
                    {
                        item.SubClassStatus = (int)SubClassStatus.Rejected;
                    }
                }
                else
                {
                    foreach (var item in course.Classes)
                    {
                        item.SubClassStatus = (int)SubClassStatus.Created;
                    }
                }
            }
            else if (statusTo == ClassApprovedStatus.Submitted)
            {
                foreach (var item in course.Classes)
                {
                    item.SubClassStatus = (int)SubClassStatus.Submitted;
                }
            }
            else if (statusTo == ClassApprovedStatus.ThirdApproved)
            {
                foreach (var item in course.Classes)
                {
                    if (item.CommencementDate.ToString("ddMMyyy").Equals(DateTime.Now.ToString("ddMMyyyy")))
                    {
                        item.SubClassStatus = (int)SubClassStatus.Openned;
                    }
                    else
                    {
                        item.SubClassStatus = (int)SubClassStatus.Actived;
                    }
                }
            }

            course.ClassAppovedStatusHistories.Add(approveHistory);
            _courseRepository.Update(course);

            return true;
        }

        private void SubmitCancelledOrPostponedSubClass(CancelPostponedSubClassBindingModel model, Class aClass, int userId, HttpFileCollection files)
        {
            var subClassHistory = new SubClassApprovedStatusHistory();
            subClassHistory.Remarks = model.Remarks;
            subClassHistory.ApprovalUpdatedBy = userId;
            subClassHistory.ApprovalStatusFrom = aClass.SubClassApprovedStatus;
            subClassHistory.ApprovalStatusTo = (int)SubClassApprovedStatus.Sumitted;
            subClassHistory.ApprovalUpdatedTime = DateTime.Now;

            var classDraft = new SubClassDraft();
            classDraft.NewClassCode = model.NewClassCode;
            classDraft.NewAttendanceRequirement = model.NewAttendanceRequirement;
            classDraft.NewClassCommencementDate = model.NewClassCommencementDate;
            classDraft.NewClassCompletionDate = model.NewClassCompletionDate;
            classDraft.NewClassCapacity = model.NewClassCapacity;
            classDraft.NewClassStatus = model.NewClassStatus;
            classDraft.NewAttendanceRequirementTypeId = model.NewAttendanceRequirementTypeId;

            aClass.SubClassDrafts.Add(classDraft);
            aClass.SubClassApprovedStatus = (int)SubClassApprovedStatus.Sumitted;


            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + aClass.Course.CourseCode + "\\" + DistinguishDocType.CancelledPostponed.ToString();

                if (!Directory.Exists(pathDirectory))
                {
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    string originalFileExtension = file.ContentType;
                    string pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + file.FileName);
                    string originalFileName = Path.GetFileName(pathFile);
                    string url = pathFile.Substring(pathFile.IndexOf(aClass.Course.CourseCode));
                    file.SaveAs(pathFile);

                    subClassHistory.SubClassHistoryDocuments.Add(new SubClassHistoryDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.Now
                        }
                    });
                }
            }

            aClass.SubClassApprovedStatusHistories.Add(subClassHistory);
            _classRepository.Update(aClass);
            _unitOfWork.Commit();
            SendSubClassEmailApproved(aClass.SubClassApprovedStatus, aClass.Course);
        }

        private async void SendSubClassEmailApproved(int approvedStatus, Course course)
        {
            #region Send Mail
            string callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;

            var _submittedAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSubmitAndCancelClass == true)).Select(x => x.UserId);
            var _submittedAdminEmails = (await _userRepository.GetMulti(x => _submittedAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _firstAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsFirstApproveAndRejectClass == true)).Select(x => x.UserId);
            var _firstAdminEmails = (await _userRepository.GetMulti(x => _firstAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _secondAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSecondpproveAndRejectClass == true)).Select(x => x.UserId);
            var _secondAdminEmails = (await _userRepository.GetMulti(x => _secondAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _thirdAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsThirdApproveAndRejectClass == true)).Select(x => x.UserId);
            var _thirdAdminEmails = (await _userRepository.GetMulti(x => _thirdAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);
            switch (approvedStatus)
            {
                case (int)SubClassApprovedStatus.Sumitted:

                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item26", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem26Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    break;
                case (int)SubClassApprovedStatus.FirstApproved:
                    foreach (var e in _secondAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item28", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem28Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)SubClassApprovedStatus.FirstReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)SubClassApprovedStatus.SecondApproved:
                    foreach (var e in _thirdAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item29", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem29Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)SubClassApprovedStatus.SecondReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)SubClassApprovedStatus.ThirdApproved:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/course_search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)SubClassApprovedStatus.ThirdReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/course_search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)SubClassApprovedStatus.Cancel:
                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item27", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem27Email(course.CourseCode, callbackurl + "/course_search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
            }
            #endregion
        }

        private void UpdateSubClassApprovedStatus(Class aClass, SubClassApprovedStatus statusFrom, SubClassApprovedStatus statusTo, int userId, string remarks, HttpFileCollection files)
        {
            aClass.SubClassApprovedStatus = (int)statusTo;

            var approveHistory = new SubClassApprovedStatusHistory();
            approveHistory.ApprovalUpdatedBy = userId;
            approveHistory.ApprovalStatusFrom = (int)statusFrom;
            approveHistory.ApprovalStatusTo = (int)statusTo;
            approveHistory.ApprovalUpdatedTime = DateTime.Now;
            approveHistory.Remarks = remarks;
            approveHistory.SubClassDraftId = aClass.SubClassApprovedStatusHistories.FirstOrDefault().SubClassDraftId;

            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + aClass.Course.CourseCode + "\\" + DistinguishDocType.CancelledPostponed.ToString();

                if (!Directory.Exists(pathDirectory))
                {
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    string originalFileExtension = file.ContentType;
                    string pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + file.FileName);
                    string originalFileName = Path.GetFileName(pathFile);
                    string url = pathFile.Substring(pathFile.IndexOf(aClass.Course.CourseCode));
                    file.SaveAs(pathFile);

                    approveHistory.SubClassHistoryDocuments.Add(new SubClassHistoryDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.Now
                        }
                    });
                }
            }

            aClass.SubClassApprovedStatusHistories.Add(approveHistory);

            if (statusTo == SubClassApprovedStatus.ThirdApproved)
            {
                var subClassHistory = aClass.SubClassApprovedStatusHistories.FirstOrDefault()?.SubClassDraft;


                aClass.ClassCode = !string.IsNullOrEmpty(subClassHistory.NewClassCode) ? subClassHistory.NewClassCode : aClass.ClassCode;
                aClass.AttendanceRequirement = subClassHistory.NewAttendanceRequirement != null ? subClassHistory.NewAttendanceRequirement : aClass.AttendanceRequirement;
                aClass.ClassCommonId = subClassHistory.NewAttendanceRequirementTypeId != null ? subClassHistory.NewAttendanceRequirementTypeId : aClass.ClassCommonId;
                aClass.CommencementDate = subClassHistory.NewClassCommencementDate.HasValue ? (subClassHistory.NewClassCommencementDate ?? DateTime.Now) : aClass.CommencementDate;
                aClass.Capacity = subClassHistory.NewClassCapacity != null ? (subClassHistory.NewClassCapacity ?? 1) : aClass.Capacity;
                aClass.SubClassStatus = subClassHistory.NewClassStatus;
            }

            _classRepository.Update(aClass);
            _unitOfWork.Commit();
            SendSubClassEmailApproved(aClass.SubClassApprovedStatus, aClass.Course);
        }



        #endregion
    }
}
