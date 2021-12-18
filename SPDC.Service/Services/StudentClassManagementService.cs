using ClosedXML.Excel;
using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels;
using SPDC.Model.BindingModels.ApplicationManagement;
using SPDC.Model.BindingModels.BatchApplication;
using SPDC.Model.BindingModels.Invoice;
using SPDC.Model.BindingModels.StudentAndClassManagement;
using SPDC.Model.BindingModels.Transaction;
using SPDC.Model.BindingModels.Transaction.RefundTransaction;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.ApplicationManagement;
using SPDC.Model.ViewModels.BatchPayment;
using SPDC.Model.ViewModels.BatchApplication;
using SPDC.Model.ViewModels.Invoice;
using SPDC.Model.ViewModels.StudentAndClassManagement;
using SPDC.Model.ViewModels.Transaction;
using SPDC.Model.ViewModels.Transaction.RefundTransaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using static SPDC.Common.StaticConfig;
using SPDC.Model.BindingModels.BatchPayment;
using System.Web.Script.Serialization;
using SPDC.Model.ViewModels.Assessment;
using SPDC.Model.BindingModels.Assessment;
using System.Text;

namespace SPDC.Service.Services
{
    public interface IStudentClassManagementService
    {
        Task<PaginationSet<StudentClassManageViewModel>> ListApplicationByFilter(StudentClassManageFilterBindingModel model);

        Task<ApplicationDetailViewModel> ApplicationDetailById(int id, int langId);

        Task<PaginationSet<ApplicationApprovedHistoryViewModel>> GetListApplicationeApprovedHistoryByCourseId(ApplicationHistoryFilter filter);

        Task<ResultModel<bool>> ChangeApprovedStatusApplication(ApprovedRejectedApplicationBindingModel model, HttpFileCollection files, int userId);

        Task<ResultModel<FileReturnViewModel>> DownloadApplicationApprovalDocument(int documentId);

        Task<ResultModel<bool>> ReplaceApplication(int appicationId, int userId);

        Task<PaginationSet<SummaryTableViewModel>> ViewSummaryTable(SummaryTableFilter filter);

        Task<ResultModel<bool>> AssignedClassForApplication(AssignedClassBindingModel model, int id);
        Task<ResultModel<bool>> CreateInvoice(InvoiceCreateBindingModel model, int id);
        Task<ResultModel<bool>> EditInvoice(InvoiceCreateBindingModel model, int id);
        Task<ResultModel<bool>> SendInvoiceOffered(int invoiceId, int id);
        Task<PaginationSet<WaivedPaymentApprovedHistoryViewModel>> GetListWaiverApprovedHistory(WaivedHisrotyFilter filter);
        Task<ResultModel<bool>> ChangeWaivedPaymentApprovedStatus(WaivedPaymentApprovedHistoryBindingModel model, HttpFileCollection files, int userId);
        Task<ResultModel<bool>> CheckInvoiceOverdue();
        Task<ResultModel<bool>> CreateTransaction(TransactionBindingModel model, HttpFileCollection files, int userId);
        Task<ResultModel<bool>> EditTransaction(TransactionBindingModel model, HttpFileCollection files, int userId);
        Task<PaginationSet<TransactionViewModel>> GetListTransaction(TransactionFilter filter);
        Task<ResultModel<FileReturnViewModel>> DownloadTransactionDocument(int documentId);
        Task<ResultModel<bool>> DeleteTransactionById(int transId);
        Task<ResultModel<TransactionViewModel>> GetTransactionById(int transId);
        Task<ResultModel<bool>> EditRefundTransaction(TransactionBindingModel model, HttpFileCollection files, int userId);
        Task<ResultModel<RefundTransactionViewModel>> GetRefundTransactionById(int transId);
        Task<ResultModel<bool>> ChangeApprovedStatusRefundTransaction(int userId, ApprvovedRefundTransactionBindingModel model, HttpFileCollection files);
        Task<PaginationSet<RefundTransactionApprovedHistoryViewModel>> GetListRefundTrasactionById(RefundTransactionFilter model);
        Task<ResultModel<bool>> DeleteRefundTransactionById(int transactionId);
        Task<ResultModel<bool>> ChangeInvoiceToRefunded(int invoiceId, int userId);
        Task<PaginationSet<BatchPaymentViewModel>> GetListBatchPaymentByPeriodTime(BatchPaymentFilter filter);
        Task<IEnumerable<UserForSettledViewModel>> GetListUserByCICNumber(string cicNumber);
        Task<IEnumerable<UserForSettledViewModel>> GetListUserByName(string name);

        Task<List<BatchApplicationViewModel>> BatchApplicationUpload(IList<HttpPostedFile> files);
        Task<BatchPaymentItemViewModel> GetUserApplicationsByUserId(int userId, bool isChineseName);
        Task<ResultModel<BatchPaymentViewModel>> CreateBatchPayment(BatchPaymentDetailBindingModel model, int createUser, HttpFileCollection files);
        Task<ResultModel<BatchPaymentViewModel>> UpdateBatchPayment(BatchPaymentDetailBindingModel model, int createUser, HttpFileCollection files);
        Task<ResultModel<BatchPaymentDetailViewModel>> GetBatchPaymentDetailById(int batchPaymentId);
        Task<ResultModel<FileReturnViewModel>> DownloadDocument(int docId, string kindOfFile);
        Task<ResultModel<object>> SettledAllBatchPayment(int batchPaymentId, int adminId);
        Task<IEnumerable<BankCodeBankNameViewModel>> GetBankCodeBankName();
        Task<AssessmentTableViewModel> GetAssessmentListByClassId(int classId);
        Task<ResultModel<bool>> UpdateAssessment(List<AssessmentRecordBindingModel> lstModel, HttpFileCollection files, int userId);
        Task<ResultModel<bool>> SendIneligibleForAttendanceCertificateEmail(List<int> lstApplicationId, LanguageCode lang);
        Task<ResultModel<bool>> SendCollectAttendanceOrCourseCompletionEmail(List<int> lstApplicationId, LanguageCode languageCode, TypeAssessmentEmail typeEmail);
        Task<ResultModel<bool>> SendExamResult(List<int> lstApplicationId, LanguageCode languageCode, int examType);
        Task<ResultModel<bool>> SendReExamDetail(IEnumerable<int> lstApplicationId, LanguageCode languageCode, int examType);
        Task<ResultModel<bool>> AssignReExamTimeslot(AssignReExamTimeslotBindingModel model);
        Task<List<ReExamFromViewModel>> GetAssignReExamFrom(int classId);
        Task<ResultModel<FileReturnViewModel>> DownloadInvoiceAttachment(int invoiceId, int id);
    }

    public class StudentClassManagementService : IStudentClassManagementService
    {
        private IApplicationRepository _applicationRepository;
        private ISystemPrivilegeRepository _systemPrivilegeRepository;
        private IApplicationHistoryDocumentRepository _applicationHistoryDocumentRepository;
        private IApplicationApprovedStatusHistoryRepository _applicationApprovedStatusHistoryRepository;
        private IUserRepository _userRepository;
        private IDocumentRepository _documentRepository;
        private IClassRepository _classRepository;
        private IInvoiceRepository _invoiceRepository;
        private IInvoiceItemRepository _invoiceItemRepository;
        private IWaiverApprovedStatusHistoryRepository _waiverApprovedStatusHistoryRepository;
        private IWaivedHistoryDocumentRepository _waivedHistoryDocumentRepository;
        private IPaymentTransactionDocumentRepository _paymentTransactionDocumentRepository;
        private IPaymentTransactionRepository _paymentTransactionRepository;
        private IRefundTransactionRepository _refundTransactionRepository;
        private IRefundTransactionApprovedStatusHistoryRepository _refundTransactionApprovedStatusHistoryRepository;
        private IClientRepository _clientRepository;
        private INotificationRepository _notificationRepository;
        private IBatchPaymentRepository _batchPaymentRepository;
        private ICourseRepository _courseRepository;
        private IAcceptedBankRepository _acceptedBank;
        private IUnitOfWork _unitOfWork;

        private IApplicationService _applicationService;
        private ILessionRepository _lessionRepository;
        private IExamRepository _examRepository;
        private ICommonDataService _commonDataService;

        public StudentClassManagementService(IApplicationRepository applicationRepository, ISystemPrivilegeRepository systemPrivilegeRepository,
            IApplicationHistoryDocumentRepository applicationHistoryDocumentRepository, IApplicationApprovedStatusHistoryRepository applicationApprovedStatusHistoryRepository,
            IUserRepository userRepository, IDocumentRepository documentRepository, IClassRepository classRepository,
            IInvoiceRepository invoiceRepository, IInvoiceItemRepository invoiceItemRepository,
            IWaiverApprovedStatusHistoryRepository waiverApprovedStatusHistoryRepository, IWaivedHistoryDocumentRepository waivedHistoryDocumentRepository,
            IPaymentTransactionDocumentRepository paymentTransactionDocumentRepository, IPaymentTransactionRepository paymentTransactionRepository,
            IRefundTransactionRepository refundTransactionRepository, IRefundTransactionApprovedStatusHistoryRepository refundTransactionApprovedStatusHistoryRepository,
            IBatchPaymentRepository batchPaymentRepository, IApplicationService applicationService,
            IClientRepository clientRepository, INotificationRepository notificationRepository, ICourseRepository courseRepository,
            IAcceptedBankRepository acceptedBank, ILessionRepository lessionRepository, IExamRepository examRepository, IUnitOfWork unitOfWork,
            ICommonDataService commonDataService)
        {
            _applicationRepository = applicationRepository;
            _systemPrivilegeRepository = systemPrivilegeRepository;
            _applicationHistoryDocumentRepository = applicationHistoryDocumentRepository;
            _applicationApprovedStatusHistoryRepository = applicationApprovedStatusHistoryRepository;
            _userRepository = userRepository;
            _documentRepository = documentRepository;
            _classRepository = classRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _waiverApprovedStatusHistoryRepository = waiverApprovedStatusHistoryRepository;
            _waivedHistoryDocumentRepository = waivedHistoryDocumentRepository;
            _paymentTransactionDocumentRepository = paymentTransactionDocumentRepository;
            _paymentTransactionRepository = paymentTransactionRepository;
            _refundTransactionRepository = refundTransactionRepository;
            _refundTransactionApprovedStatusHistoryRepository = refundTransactionApprovedStatusHistoryRepository;
            _clientRepository = clientRepository;
            _notificationRepository = notificationRepository;
            _batchPaymentRepository = batchPaymentRepository;
            _courseRepository = courseRepository;
            _acceptedBank = acceptedBank;

            _unitOfWork = unitOfWork;

            _applicationService = applicationService;
            _lessionRepository = lessionRepository;
            _examRepository = examRepository;
            _commonDataService = commonDataService;
        }

        public StudentClassManagementService()
        {
        }

        public async Task<PaginationSet<StudentClassManageViewModel>> ListApplicationByFilter(StudentClassManageFilterBindingModel model)
        {
            int classPreferId = !string.IsNullOrEmpty(model.StudentPreferredClassCode) ? (await _classRepository.GetSingleByCondition(x => x.ClassCode.Equals(model.StudentPreferredClassCode))).Id : 0;
            int classAdminAssignId = !string.IsNullOrEmpty(model.AdminAssignedClassCode) ? (await _classRepository.GetSingleByCondition(x => x.ClassCode.Equals(model.AdminAssignedClassCode))).Id : 0;

            var result = await _applicationRepository.SearchApplicationManagementByFilter(model, classPreferId, classAdminAssignId);

            return result;
            #region Old Code
            //var result = (await _applicationRepository.GetMultiPaging(x => string.IsNullOrEmpty(model.CourseCode) ? true : x.Course.CourseCode.ToLower().Contains(model.CourseCode.ToLower())
            //                //Fix after re-design database
            //                //&& string.IsNullOrEmpty(model.AcademicYear) ? true : x.StudentPreferredClassModel.AcademicYear.Equals(model.AcademicYear)
            //                && string.IsNullOrEmpty(model.CourseNameEnglish) ? true : x.StudentPreferredClassModel.Course.CourseTrans.Any(v => v.CourseName.ToLower().Contains(model.CourseNameEnglish.ToLower())
            //                && v.LanguageId == (int)LanguageCode.EN)
            //                && string.IsNullOrEmpty(model.CourseNameChinese) ? true : x.StudentPreferredClassModel.Course.CourseTrans.Any(v => v.CourseName.ToLower().Contains(model.CourseNameChinese.ToLower())
            //                && (v.LanguageId == (int)LanguageCode.CN || v.LanguageId == (int)LanguageCode.HK))
            //                && !model.ClassCommencementDate.HasValue ? true : (x.StudentPreferredClassModel.CommencementDate.Day == (model.ClassCommencementDate ?? DateTime.UtcNow).Day
            //                && x.StudentPreferredClassModel.CommencementDate.Month == (model.ClassCommencementDate ?? DateTime.UtcNow).Month
            //                && x.StudentPreferredClassModel.CommencementDate.Year == (model.ClassCommencementDate ?? DateTime.UtcNow).Year)
            //                && !model.ClassCompletionDate.HasValue ? true : (x.StudentPreferredClassModel.CompletionDate.Day == (model.ClassCompletionDate ?? DateTime.UtcNow).Day
            //                && x.StudentPreferredClassModel.CompletionDate.Month == (model.ClassCompletionDate ?? DateTime.UtcNow).Month
            //                && x.StudentPreferredClassModel.CompletionDate.Year == (model.ClassCompletionDate ?? DateTime.UtcNow).Year)
            //                && model.CourseStatus == null ? true : x.StudentPreferredClassModel.Course.Status == model.CourseStatus
            //                && string.IsNullOrEmpty(model.StudentPreferredClassCode) ? true : x.StudentPreferredClassModel.ClassCode.ToLower().Contains(model.StudentPreferredClassCode.ToLower())
            //                && string.IsNullOrEmpty(model.AdminAssignedClassCode) ? true : x.AdminAssignedClassModel.ClassCode.ToLower().Contains(model.AdminAssignedClassCode.ToLower())
            //                && model.StudyMode == null ? true : x.StudentPreferredClassModel.Course.CourseTypeId == model.StudyMode
            //                && string.IsNullOrEmpty(model.ApplicationNumber) ? true : x.ApplicationNumber.ToLower().Equals(model.ApplicationNumber.ToLower())
            //                && model.InvoiceStatus == null ? true : true
            //                && model.EnrollmentStatus == null ? true : true
            //                && string.IsNullOrEmpty(model.StudenNameEnglish) ? true : (x.User.Particular.SurnameEN + x.User.Particular.GivenNameEN).ToLower().Contains(model.StudenNameEnglish.ToLower())
            //                && string.IsNullOrEmpty(model.StudentNameChinese) ? true : (x.User.Particular.SurnameCN + x.User.Particular.GivenNameCN).ToLower().Contains(model.StudentNameChinese.ToLower())
            //            , "Id"
            //            , true
            //            , model.Page
            //            , model.Size
            //            , new string[] { "StudentPreferredClassModel", "AdminAssignedClassModel", "AdminAssignedClassModel.AdminAssignedApplicationModels", "User", "StudentPreferredClassModel.Course", "User.Particular", "Invoices", "Invoices.InvoiceItems", "Course", "Course.Classes" }
            //            )).Select(x => x.ToStudentClassManageViewModel());

            //var total = await _applicationRepository.Count(x =>
            //string.IsNullOrEmpty(model.CourseCode) ? true : x.Course.CourseCode.ToLower().Contains(model.CourseCode.ToLower()) &&
            ////Fix after re-design database
            ////string.IsNullOrEmpty(model.AcademicYear) ? true : x.StudentPreferredClassModel.AcademicYear.Equals(model.AcademicYear) &&
            //string.IsNullOrEmpty(model.CourseNameEnglish) ? true : x.StudentPreferredClassModel.Course.CourseTrans.Any(v => v.CourseName.ToLower().Contains(model.CourseNameEnglish.ToLower()) && v.LanguageId == (int)LanguageCode.EN) &&
            //string.IsNullOrEmpty(model.CourseNameChinese) ? true : x.StudentPreferredClassModel.Course.CourseTrans.Any(v => v.CourseName.ToLower().Contains(model.CourseNameChinese.ToLower()) && (v.LanguageId == (int)LanguageCode.CN || v.LanguageId == (int)LanguageCode.HK)) &&
            //!model.ClassCommencementDate.HasValue ? true : (x.StudentPreferredClassModel.CommencementDate.Day == (model.ClassCommencementDate ?? DateTime.UtcNow).Day &&
            //x.StudentPreferredClassModel.CommencementDate.Month == (model.ClassCommencementDate ?? DateTime.UtcNow).Month &&
            //x.StudentPreferredClassModel.CommencementDate.Year == (model.ClassCommencementDate ?? DateTime.UtcNow).Year)
            //&& !model.ClassCompletionDate.HasValue ? true : (x.StudentPreferredClassModel.CompletionDate.Day == (model.ClassCompletionDate ?? DateTime.UtcNow).Day &&
            //x.StudentPreferredClassModel.CompletionDate.Month == (model.ClassCompletionDate ?? DateTime.UtcNow).Month &&
            //x.StudentPreferredClassModel.CompletionDate.Year == (model.ClassCompletionDate ?? DateTime.UtcNow).Year)
            //&&
            //model.CourseStatus == null ? true : x.StudentPreferredClassModel.Course.Status == model.CourseStatus &&
            //string.IsNullOrEmpty(model.StudentPreferredClassCode) ? true : x.StudentPreferredClassModel.ClassCode.ToLower().Contains(model.StudentPreferredClassCode.ToLower()) &&
            //string.IsNullOrEmpty(model.AdminAssignedClassCode) ? true : x.AdminAssignedClassModel.ClassCode.ToLower().Contains(model.AdminAssignedClassCode.ToLower()) &&
            //model.StudyMode == null ? true : x.StudentPreferredClassModel.Course.CourseTypeId == model.StudyMode &&
            //string.IsNullOrEmpty(model.ApplicationNumber) ? true : x.ApplicationNumber.ToLower().Equals(model.ApplicationNumber.ToLower()) &&
            //model.InvoiceStatus == null ? true : true &&
            //model.EnrollmentStatus == null ? true : true &&
            //string.IsNullOrEmpty(model.StudenNameEnglish) ? true : (x.User.Particular.SurnameEN + x.User.Particular.GivenNameEN).ToLower().Contains(model.StudenNameEnglish.ToLower()) &&
            //string.IsNullOrEmpty(model.StudentNameChinese) ? true : (x.User.Particular.SurnameCN + x.User.Particular.GivenNameCN).ToLower().Contains(model.StudentNameChinese.ToLower()));

            //return new PaginationSet<StudentClassManageViewModel>()
            //{
            //    Items = result,
            //    Page = model.Page,
            //    TotalCount = total
            //};
            #endregion
        }

        public async Task<ApplicationDetailViewModel> ApplicationDetailById(int id, int langId)
        {
            var result = (await _applicationRepository.GetSingleByCondition(x => x.Id == id, new string[] { "StudentPreferredClassModel", "StudentPreferredClassModel.Course", "Course", "Course.CourseTrans",
            "User", "User.Particular", "User.Particular.ParticularTrans", "StudentPreferredClassModel.Course.CourseType"})).ToApplicationDetailViewModel(langId);

            return result;
        }

        public async Task<PaginationSet<ApplicationApprovedHistoryViewModel>> GetListApplicationeApprovedHistoryByCourseId(ApplicationHistoryFilter filter)
        {
            var listHistory = (await _applicationApprovedStatusHistoryRepository.GetMultiPaging(x => x.ApplicationId == filter.ApplicationId, "Id", false, filter.Page, filter.Size,
                new string[] { "ApplicationHistoryDocuments", "ApplicationHistoryDocuments.Document" })).Select(x => x.ToApplicationApprovedHistoryViewModel(_userRepository.GetSingleById(x.ApprovalUpdatedBy).DisplayName));
            var total = await _applicationApprovedStatusHistoryRepository.Count(x => x.ApplicationId == filter.ApplicationId);

            return new PaginationSet<ApplicationApprovedHistoryViewModel>()
            {
                Items = listHistory,
                Page = filter.Page,
                TotalCount = total
            };
        }

        public async Task<ResultModel<bool>> ChangeApprovedStatusApplication(ApprovedRejectedApplicationBindingModel model, HttpFileCollection files, int userId)
        {
            var result = new ResultModel<bool>();

            var application = await _applicationRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Course.CourseTrans", "ApplicationApprovedStatusHistories" });

            if (application == null)
            {
                result.Message = "Aplicaiton was not found";
                return result;
            }

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == application.CourseId, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }


            switch (application.Status)
            {
                case (int)ApplicationStatus.Submitted:
                case (int)ApplicationStatus.ReturnFirstApproved:
                    if (model.ApprovedStatus == (int)ApplicationStatus.SupplementaryInformation && privilege.IsFirstApproveAndRejectCourse)
                    {
                        var iSuccess = await UpdateApplicationApprovedStatus(application, application.Status, ApplicationStatus.SupplementaryInformation, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)ApplicationStatus.Rejected && privilege.IsFirstApproveAndRejectCourse)
                    {
                        var iSuccess = await UpdateApplicationApprovedStatus(application, application.Status, ApplicationStatus.Rejected, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)ApplicationStatus.FirstApproved && privilege.IsFirstApproveAndRejectCourse)
                    {
                        var iSuccess = await UpdateApplicationApprovedStatus(application, application.Status, ApplicationStatus.FirstApproved, userId, model.Remarks, files);
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
                case (int)ApplicationStatus.FirstApproved:
                    if (model.ApprovedStatus == (int)ApplicationStatus.Rejected && privilege.IsSecondpproveAndRejectCourse)
                    {
                        var iSuccess = await UpdateApplicationApprovedStatus(application, application.Status, ApplicationStatus.Rejected, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)ApplicationStatus.SecondApproved && privilege.IsSecondpproveAndRejectCourse)
                    {
                        var iSuccess = await UpdateApplicationApprovedStatus(application, application.Status, ApplicationStatus.SecondApproved, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)ApplicationStatus.ReturnFirstApproved && privilege.IsSecondpproveAndRejectCourse)
                    {
                        var iSuccess = await UpdateApplicationApprovedStatus(application, application.Status, ApplicationStatus.ReturnFirstApproved, userId, model.Remarks, files);
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
                case (int)ApplicationStatus.SecondApproved:
                    result.Message = "This application is approved";
                    return result;
                default:
                    result.Message = "Action cannot perform";
                    return result;
            }

            #region Send email
            var user = await _userRepository.GetSingleByCondition(x => x.Id == application.UserId, new string[] { "UserDevices" });
            string emailSubject = null;
            string emailTemplate = null;
            switch (((ApplicationStatus)model.ApprovedStatus))
            {
                case ApplicationStatus.SupplementaryInformation:
                    emailSubject = FileHelper.GetEmailSubject("item6", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                    emailTemplate = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item6EN.cshtml" : "Item6TC.cshtml";
                    break;
                case ApplicationStatus.Rejected:
                    emailSubject = FileHelper.GetEmailSubject("item7", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                    emailTemplate = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item7EN.cshtml" : "Item7TC.cshtml";
                    break;
            }

            if (!string.IsNullOrWhiteSpace(emailSubject) || !string.IsNullOrWhiteSpace(emailTemplate))
            {
                string courseName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == 1).CourseName : application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == 3).CourseName;

                var supplementaryInformationRequired = string.IsNullOrWhiteSpace(model.Remarks) ? new string[] { } : model.Remarks.Split(',');

                bool isSuccesss = MailHelper.SendMail(user.Email, emailSubject, Common.Common.GenerateItem6Email(courseName, user.DisplayName, supplementaryInformationRequired, emailTemplate));
                if (isSuccesss)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
                else
                {
                    result.Message = "An error occurred while processing your request";
                    return result;
                }
            }
            #endregion

            _unitOfWork.Commit();
            result.Message = "Update Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadApplicationApprovalDocument(int documentId)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var doc = _documentRepository.GetSingleById(documentId);
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

            var directory = ConfigHelper.GetByKey("AppicationApprovedDocumentDirectory");
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

        private async Task<bool> UpdateApplicationApprovedStatus(Application application, int statusFrom, ApplicationStatus statusTo, int userId, string remarks, HttpFileCollection files)
        {
            if (application == null) return false;
            application.Status = (int)statusTo;

            var approveHistory = new ApplicationApprovedStatusHistory();
            approveHistory.ApprovalUpdatedBy = userId;
            approveHistory.AppovalStatusFrom = statusFrom;
            approveHistory.ApprovalStatusTo = (int)statusTo;
            approveHistory.ApprovalUpdatedTime = DateTime.UtcNow;
            approveHistory.ApprovalRemarks = remarks;

            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("AppicationApprovedDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + application.Course.CourseCode + "\\" + application.Id;

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
                    string url = pathFile.Substring(pathFile.IndexOf("ApplicationApproval") + "ApplicationApproval".Length);
                    file.SaveAs(pathFile);

                    approveHistory.ApplicationHistoryDocuments.Add(new ApplicationHistoryDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.UtcNow
                        }
                    });
                }
            }

            string callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("ApplicantPortal"))).ClientUrl;

            if (statusTo == ApplicationStatus.FirstApproved && application.Course.LevelOfApprovalId > (int)LevelApprovalType.FirstLevel)
            {
                //Send email item 5 to second approval admin

                var adminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == application.CourseId && x.IsSecondpproveAndRejectClass == true)).Select(x => x.UserId);
                var adminEmails = (await _userRepository.GetMulti(x => adminIds.Contains(x.Id))).Select(x => x.AdminEmail);

                callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;
                foreach (var e in adminEmails)
                {
                    string emailSubject = FileHelper.GetEmailSubject("item5", "EmailSubject", "EN");
                    bool isSuccess = MailHelper.SendMail(e, emailSubject, Common.Common.GenerateItem5Email(application.Course.CourseCode, callbackurl + "student-class-management/search", "Item5EN.cshtml"));
                    if (isSuccess)
                    {
                        CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                        emailCommonData.ValueInt++;
                        _commonDataService.Update(emailCommonData);
                    }
                }

            }

            if (statusFrom == (int)ApplicationStatus.Created && statusTo == ApplicationStatus.SupplementaryInformation)
            {
                // Send email item 6 to student
                // Send notification item 6 to student

                var user = await _userRepository.GetSingleByCondition(x => x.Id == application.UserId, new string[] { "UserDevices" });
                string courseName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == 1).CourseName : application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == 3).CourseName;
                string emailTemplate = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item6EN.cshtml" : "Item6TC.cshtml";
                var supplementaryInformationRequired = new string[] { };
                // TODO: Check required Supplementary Information Required

                string emailSubject = FileHelper.GetEmailSubject("item6", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");

                bool isSuccess = MailHelper.SendMail(user.Email, emailSubject, Common.Common.GenerateItem6Email(courseName, user.DisplayName, supplementaryInformationRequired, emailTemplate));
                if (isSuccess)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
                if (user.UserDevices != null && user.UserDevices.Count() > 0)
                {
                    string notificationTitle = FileHelper.GetNotificationTitle("item6", "NotificationTitles", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                    Notification notification = new Notification()
                    {
                        Body = "",
                        DataId = application.Id,
                        Title = notificationTitle,
                        Type = (int)NotificationType.Application
                    };

                    foreach (var device in user.UserDevices)
                    {
                        NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                    }

                    notification.NotificationUsers.Add(new NotificationUser()
                    {
                        CreatedDate = DateTime.UtcNow,
                        IsFavourite = false,
                        IsRead = false,
                        IsRemove = false,
                        UserId = user.Id
                    });
                    _notificationRepository.Add(notification);
                }
            }

            if (statusFrom == (int)ApplicationStatus.FirstApproved && statusTo == ApplicationStatus.Rejected)
            {
                // Send email item 7 to student
                // Send notification item 7 to student

                var user = await _userRepository.GetSingleByCondition(x => x.Id == application.UserId, new string[] { "UserDevices" });
                string courseName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == 1).CourseName : application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == 3).CourseName;
                string emailSubject = FileHelper.GetEmailSubject("item7", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                string emailTemplate = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item7EN.html" : "Item7TC.html";
                var reason = new string[] { };
                // TODO: Check Reason
                bool isSuccess = MailHelper.SendMail(user.Email, emailSubject, Common.Common.GenerateItem6Email(courseName, user.DisplayName, reason, emailTemplate));
                if (isSuccess)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
                if (user.UserDevices != null && user.UserDevices.Count() > 0)
                {
                    string notificationTitle = FileHelper.GetNotificationTitle("item7", "NotificationTitles", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");

                    Notification notification = new Notification()
                    {
                        Body = "",
                        DataId = application.Id,
                        Title = notificationTitle,
                        Type = (int)NotificationType.Application
                    };

                    foreach (var device in user.UserDevices)
                    {
                        NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                    }

                    notification.NotificationUsers.Add(new NotificationUser()
                    {
                        CreatedDate = DateTime.UtcNow,
                        IsFavourite = false,
                        IsRead = false,
                        IsRemove = false,
                        UserId = user.Id
                    });
                    _notificationRepository.Add(notification);
                }

            }

            if (statusFrom == (int)ApplicationStatus.FirstApproved && statusTo == ApplicationStatus.SecondApproved && application.Course.LevelOfApprovalId > (int)LevelApprovalType.FirstLevel)
            {
                // Send email item 8 to first approval admin

                var adminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == application.CourseId && x.IsFirstApproveAndRejectClass == true)).Select(x => x.UserId);
                var adminEmails = (await _userRepository.GetMulti(x => adminIds.Contains(x.Id))).Select(x => x.AdminEmail);

                foreach (var e in adminEmails)
                {
                    string emailSubject = FileHelper.GetEmailSubject("item8", "EmailSubject", "EN");
                    callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;
                    bool isSuccess = MailHelper.SendMail(e, emailSubject, Common.Common.GenerateItem8Email(application.Course.CourseCode, callbackurl + "student-class-management/search", "Item8EN.cshtml"));
                    if (isSuccess)
                    {
                        CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                        emailCommonData.ValueInt++;
                        _commonDataService.Update(emailCommonData);
                    }
                }
            }

            if (statusFrom == (int)ApplicationStatus.FirstApproved && statusTo == ApplicationStatus.ReturnFirstApproved && application.Course.LevelOfApprovalId > (int)LevelApprovalType.FirstLevel)
            {
                // Send email item 9 to first approval admin

                var adminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == application.CourseId && x.IsFirstApproveAndRejectClass == true)).Select(x => x.UserId);
                var adminEmails = (await _userRepository.GetMulti(x => adminIds.Contains(x.Id))).Select(x => x.AdminEmail);

                foreach (var e in adminEmails)
                {
                    string emailSubject = FileHelper.GetEmailSubject("item9", "EmailSubject", "EN");

                    callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;
                    bool isSuccess = MailHelper.SendMail(e, emailSubject, Common.Common.GenerateItem9Email(application.Course.CourseCode, callbackurl + "student-class-management/search", "Item9EN.cshtml"));
                    if (isSuccess)
                    {
                        CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                        emailCommonData.ValueInt++;
                        _commonDataService.Update(emailCommonData);
                    }
                }
            }

            if (statusTo == ApplicationStatus.Withdrawal)
            {
                // Send email item 10 to course admin
                var adminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == application.CourseId)).Select(x => x.UserId);
                var adminEmails = (await _userRepository.GetMulti(x => adminIds.Contains(x.Id))).Select(x => x.AdminEmail);

                foreach (var e in adminEmails)
                {
                    string emailSubject = FileHelper.GetEmailSubject("item10", "EmailSubject", "EN");
                    bool isSuccess = MailHelper.SendMail(e, emailSubject, Common.Common.GenerateItem10Email(application.Course.CourseCode, callbackurl + "student-class-management/search", "Item10EN.cshtml"));
                    if (isSuccess)
                    {
                        CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                        emailCommonData.ValueInt++;
                        _commonDataService.Update(emailCommonData);
                    }
                }
            }

            application.ApplicationApprovedStatusHistories.Add(approveHistory);
            _applicationRepository.Update(application);
            _unitOfWork.Commit();

            return true;
        }

        public async Task<ResultModel<bool>> ReplaceApplication(int appicationId, int userId)
        {
            var result = new ResultModel<bool>();

            var application = await _applicationRepository.GetSingleByCondition(x => x.Id == appicationId, new string[] { "AdminAssignedClassModel", "Invoices" });

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == application.CourseId, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
            }
            else if (!privilege.IsSubmitAndCancelCourse)
            {
                result.Message = "You dont have permission";
            }

            if (application.Status != (int)ApplicationStatus.Assigned)
            {
                result.Message = "This application have not assigned any invoice yet";
            }
            else if (application.Invoices.Count() == 0)
            {
                result.Message = "This application have not created any invoice yet";
            }
            else if (application.Invoices.Any(x => x.Status != (int)Common.Enums.InvoiceStatus.Created))
            {
                result.Message = "There are an invoice offered";
            }
            else
            {
                application.AdminAssignedClassModel.EnrollmentNumber -= 1;
                application.AdminAssignedClass = null;
                application.Status = (int)ApplicationStatus.Replaced;
                _applicationRepository.Update(application);
                _unitOfWork.Commit();
                result.Message = "Success";
                result.IsSuccess = true;
            }

            return result;
        }

        public async Task<PaginationSet<SummaryTableViewModel>> ViewSummaryTable(SummaryTableFilter filter)
        {
            Expression<Func<Class, bool>> filterClause = x => !filter.ListCourseId.Any() || filter.ListCourseId.Contains(x.CourseId);
            var listSummarTable = (await _classRepository
                                            .GetMultiPaging(filterClause, "CourseId", false, filter.Page, filter.Size,
                                                            new string[] { "AdminAssignedApplicationModels", "AdminAssignedApplicationModels.Invoices.InvoiceItems" })
                                  ).Select(x => x.ToSummaryTableViewModel());

            var total = await _classRepository.Count(filterClause);

            return new PaginationSet<SummaryTableViewModel>()
            {
                Items = listSummarTable,
                Page = filter.Page,
                TotalCount = total
            };
        }

        public async Task<ResultModel<bool>> AssignedClassForApplication(AssignedClassBindingModel model, int id)
        {
            var result = new ResultModel<bool>();


            var application = await _applicationRepository.GetSingleByCondition(x => x.Id == model.ApplicationId, new string[] { "Course", "Course.Classes" });

            if (application == null)
            {
                result.Message = "Application was not found";
                return result;
            }
            var appicationStatusForAssigned = new List<int>() { (int)ApplicationStatus.SecondApproved, (int)ApplicationStatus.Assigned, (int)ApplicationStatus.Replaced };
            if (appicationStatusForAssigned.IndexOf(application.Status) < 0)
            {
                result.Message = "Application have not approved yet";
                return result;
            }

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.CourseId == application.CourseId && x.UserId == id, new string[] { "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }
            else if (!privilege.IsSubmitAndCancelCourse)
            {
                result.Message = "You dont have permission";
                return result;
            }

            //model.ClassId = 302;
            var isInvalidClass = application.Course.Classes.Select(x => x.Id).Contains(model.ClassId);
            if (!isInvalidClass)
            {
                result.Message = "This class is not valid";
                return result;
            }
            if (application.AdminAssignedClass.HasValue)
            {
                application.Course.Classes.FirstOrDefault(x => x.Id == application.AdminAssignedClass).EnrollmentNumber -= 1;
            }

            if (application.Course.Classes.FirstOrDefault(x => x.Id == model.ClassId).EnrollmentNumber.HasValue)
            {
                application.Course.Classes.FirstOrDefault(x => x.Id == model.ClassId).EnrollmentNumber += 1;
            }
            else
            {
                application.Course.Classes.FirstOrDefault(x => x.Id == model.ClassId).EnrollmentNumber = 1;
            }


            application.Status = (int)ApplicationStatus.Assigned;
            application.AdminAssignedClass = model.ClassId;
            application.LastModifiedBy = id;
            application.LastModifiedDate = DateTime.UtcNow;
            _applicationRepository.Update(application);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> CreateInvoice(InvoiceCreateBindingModel model, int id)
        {
            var result = new ResultModel<bool>();

            try
            {
                var application = await _applicationRepository.GetSingleByCondition(x => x.Id == model.ApplicationId, new string[] { "Course", "AdminAssignedClassModel.Exams" });

                if (application.Status != (int)ApplicationStatus.Assigned)
                {
                    result.Message = "This appliction is not assigned";
                    return result;
                }

                var lastInvoiceNumber = _invoiceRepository.GetLastInvoice()?.InvoiceNumber;

                var invoice = model.ToInvoiceCreateBindingModel(id, application, new Invoice(), lastInvoiceNumber, courseCode: application.Course.CourseCode);
                if (invoice == null)
                {
                    result.Message = "Failed to create invoice";
                    return result;
                }

                _invoiceRepository.Add(invoice);
                _unitOfWork.Commit();

                result.Message = "Success";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<ResultModel<bool>> EditInvoice(InvoiceCreateBindingModel model, int id)
        {
            var result = new ResultModel<bool>();

            try
            {
                var application = (await _applicationRepository.GetSingleByCondition(x => x.Id == model.ApplicationId, new string[] { "Course", "AdminAssignedClassModel.Exams" }));

                var modelInvoice = await _invoiceRepository.GetSingleByCondition(x => x.Id == model.InvoiceId, new string[] { "InvoiceItems" });

                _invoiceItemRepository.DeleteMulti(x => x.InvoiceId == model.InvoiceId);

                if (modelInvoice == null)
                {
                    result.Message = "Invoice was not found";
                    return result;
                }
                if (modelInvoice.Status != (int)Common.Enums.InvoiceStatus.Created)
                {
                    result.Message = "You can only edit invoice at Created Status";
                    return result;
                }

                var invoice = model.ToInvoiceCreateBindingModel(id, application, modelInvoice);

                _invoiceRepository.Update(invoice);
                _unitOfWork.Commit();

                result.Message = "Success";
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<ResultModel<bool>> SendInvoiceOffered(int invoiceId, int userId)
        {
            var result = new ResultModel<bool>();

            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.Id == invoiceId, new string[] { "Application", "Application.Course.CourseTrans",
                "Application.User.Particular", "InvoiceItems", "PaymentTransactions.TransactionType1", "PaymentTransactions.AcceptedBank" });

            if (invoice == null)
            {
                result.Message = "Can not find invoice";
                return result;
            }

            if (invoice.Status != (int)SPDC.Common.Enums.InvoiceStatus.Created)
            {
                result.Message = "This payment invoice was sent";
                return result;
            }

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == invoice.Application.Course.Id, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }

            invoice.Status = (int)SPDC.Common.Enums.InvoiceStatus.Offered;
            _invoiceRepository.Update(invoice);
            _unitOfWork.Commit();

            var invoiceTemplatePath = ConfigHelper.GetByKey("InvoiceTemplate");
            string serPath = null;
            string subject = null;

            var langCode = invoice.Application.User.CommunicationLanguage ?? 1;

            if (invoice.InvoiceItems.Any(x => x.InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee))
            {
                serPath = System.Web.HttpContext.Current.Server.MapPath(invoiceTemplatePath) + (langCode == (int)LanguageCode.EN ? "paymentEN.html" : "paymentCN.html");
                subject = langCode == (int)LanguageCode.EN ? "Your Application For Enrollment Of The Course Is Accepted" : "閣下申請報讀的課程已被接納";
            }

            if (serPath != null)
            {
                string text = File.ReadAllText(serPath);

                var userName = langCode == (int)LanguageCode.EN ? invoice.Application.User.Particular.SurnameEN + " " + invoice.Application.User.Particular.GivenNameEN : invoice.Application.User.Particular.SurnameCN + " " + invoice.Application.User.Particular.GivenNameCN;
                var courseName = invoice.Application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == langCode)?.CourseName;

                text = text.Replace("@UserName", userName);
                text = text.Replace("@CourseName", courseName);

                var invoiceType = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount)?.InvoiceItemTypeId;
                var adminUser = _userRepository.GetSingleById(userId);

                var userReceived = "";
                var quantity = "";
                var note1 = "";
                var note2 = "";
                var note3 = "";
                if (invoice.Status == (int)Common.Enums.InvoiceStatus.SettledByBatch && invoiceType == (int)SPDC.Common.Enums.InvoiceItemType.CourseFee)
                {

                    var listBatchPayment = (await _batchPaymentRepository.GetMulti(x => x.IsSettled)).ToList();
                    for (int i = 0; i < listBatchPayment.Count(); i++)
                    {
                        if (string.IsNullOrEmpty(listBatchPayment[i].ListApplication))
                        {
                            continue;
                        }
                        var listInvoiceId = listBatchPayment[i].ListApplication.Split(',');
                        if (listInvoiceId.Contains(invoice.Id.ToString()))
                        {
                            userReceived = listBatchPayment[i].Payee;
                            quantity = listBatchPayment[i].ListApplication.Split(',').Length.ToString();
                            break;
                        }
                    }
                }
                else
                {
                    userReceived = invoice.Application.User.Particular.SurnameCN + invoice.Application.User.Particular.GivenNameCN
                        + invoice.Application.User.Particular.SurnameEN.ToUpper() + ", " + invoice.Application.User.Particular.GivenNameCN;
                    note1 = "付款人以" + invoice.PaymentTransactions.LastOrDefault()?.TransactionType1.NameCN + "作為付款:";
                    note2 = invoice.PaymentTransactions.LastOrDefault()?.TransactionType1.NameEN + " is the payment of for the following persons:";
                    note3 = userReceived;
                }

                var path = GenerateInvoiceAttachment((SPDC.Common.Enums.InvoiceItemType)invoiceType, invoice, adminUser.UserName, userReceived, quantity, note1, note2, note3);
                var sent = MailHelper.SendMail(invoice.Application.User.Email, subject, text, path: path, isDeleteFile: true);
                // TODO: notification

                if (sent)
                {
                    result.Message = "Success";
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
                else
                {
                    result.Message = "Can not send email";
                }
            }

            result.IsSuccess = true;
            return result;
        }

        private string GenerateInvoiceAttachment(SPDC.Common.Enums.InvoiceItemType typeInvoice, Invoice invoice, string adminName, string listUserName, string quantity, string note1
            , string note2, string note3)
        {
            #region Data mapping

            var receiptNumber = invoice.InvoiceNumber.Substring(0, invoice.InvoiceNumber.Length - 1) + "R";
            var today = DateTime.Now.ToShortDateString();
            bool isDiscount = invoice.InvoiceItems.Any(x => x.InvoiceItemTypeId == (int)SPDC.Common.Enums.InvoiceItemType.Discount);
            string typeOfPaymentCn1 = "";
            string typeOfPaymentEn1 = "";
            string itemNameCn1 = "";
            string itemNameEn1 = "";
            string typeOfPaymentCn2 = "";
            string typeOfPaymentEn2 = "";
            string itemNameCn2 = "";
            string itemNameEn2 = "";
            string amount1 = "";
            string amount2 = "";
            string paymentMethodCn = invoice.PaymentTransactions.LastOrDefault()?.TransactionType1.NameCN;
            string paymentMethodEn = invoice.PaymentTransactions.LastOrDefault()?.TransactionType1.NameEN;
            string bankCodeCn = invoice.PaymentTransactions.LastOrDefault()?.AcceptedBank.NameCN;
            string bankCodeEn = invoice.PaymentTransactions.LastOrDefault()?.AcceptedBank.NameEN;
            string paymentTime = invoice.PaymentTransactions.LastOrDefault()?.PaymentDate.Hour.ToString() + ":"
                + invoice.PaymentTransactions.LastOrDefault()?.PaymentDate.Minute.ToString("D2");
            string paymentDate = invoice.PaymentTransactions.LastOrDefault()?.PaymentDate.ToShortDateString();

            switch (typeInvoice)
            {
                case Common.Enums.InvoiceItemType.CourseFee:
                    typeOfPaymentCn1 = "課程費用";
                    typeOfPaymentEn1 = "Course Fee";
                    itemNameCn1 = invoice.Application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == (int)LanguageCode.CN).CourseName;
                    itemNameEn1 = invoice.Application.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == (int)LanguageCode.EN).CourseName;
                    amount1 = invoice.Application.Course.CourseFee.ToString();
                    break;
                case Common.Enums.InvoiceItemType.ReExamFee:
                    typeOfPaymentCn1 = "Re Exam Fee(CN)";
                    typeOfPaymentEn1 = "Re Exam Fee";
                    itemNameCn1 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount).ChineseName;
                    itemNameEn1 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount).EnglishName;
                    amount1 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount).Price.ToString();
                    break;
                case Common.Enums.InvoiceItemType.Others:
                    typeOfPaymentCn1 = "Other Fee(CN)";
                    typeOfPaymentEn1 = "Other Fee";
                    itemNameCn1 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount).ChineseName;
                    itemNameEn1 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount).EnglishName;
                    amount1 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount).Price.ToString();
                    break;
            }

            if (isDiscount)
            {
                typeOfPaymentCn2 = "折扣";
                typeOfPaymentEn2 = "Discount";
                itemNameCn2 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId == (int)SPDC.Common.Enums.InvoiceItemType.Discount).ChineseName;
                itemNameEn2 = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId == (int)SPDC.Common.Enums.InvoiceItemType.Discount).EnglishName;
                amount2 = invoice.Application.Course.DiscountFee.ToString();
            }

            var attachmentData = new List<KeyValueModel>() {
                new KeyValueModel{Key="@ReceiptNo",Value=$"{receiptNumber}"},
                new KeyValueModel{Key="@PrintDate",Value=$"{today}"},
                new KeyValueModel{Key="@LDAPaccount",Value=$"{adminName}"},
                new KeyValueModel{Key="@ReceivedFrom",Value=$"{listUserName}"},
                new KeyValueModel{Key="@TotalAmount",Value=$"{invoice.Fee}"},
                new KeyValueModel{Key="@CourseCode",Value=$"({invoice.Application.Course.CourseCode}"},
                new KeyValueModel{Key="@TypeOfPaymentCN1",Value=$"{typeOfPaymentCn1}"},
                new KeyValueModel{Key="@TypeOfPaymentEN1",Value=$"{typeOfPaymentEn1}"},
                new KeyValueModel{Key="@ItemNameCN1",Value=$"{itemNameCn1}"},
                new KeyValueModel{Key="@ItemNameEN1",Value=$"{itemNameEn1}"},
                new KeyValueModel{Key="@Quantity1",Value=$"{quantity}"},
                new KeyValueModel{Key="@Amount1",Value=$"{amount1}"},
                new KeyValueModel{Key="@Remark",Value=$"{""}"},
                new KeyValueModel{Key="@TypeOfPaymentCN2",Value=$"{typeOfPaymentCn2}"},
                new KeyValueModel{Key="@TypeOfPaymentEN2",Value=$"{typeOfPaymentEn2}"},
                new KeyValueModel{Key="@ItemNameCN2",Value=$"{itemNameCn2}"},
                new KeyValueModel{Key="@ItemNameEN2",Value=$"{itemNameEn2}"},
                new KeyValueModel{Key="@Quantity2",Value=$"{quantity}"},
                new KeyValueModel{Key="@Amount2",Value=$"{amount2}"},
                new KeyValueModel{Key="@PaymentMethodCN2",Value=$"{paymentMethodCn}"},
                new KeyValueModel{Key="@PaymentMethodEN2",Value=$"{paymentMethodEn}"},
                new KeyValueModel{Key="@BankCodeBankNameCN",Value=$"{bankCodeCn}"},
                new KeyValueModel{Key="@BankCodeBankNameEN",Value=$"{bankCodeEn}"},
                new KeyValueModel{Key="@PaymentTime",Value=$"{paymentTime}"},
                new KeyValueModel{Key="@PaymentDate",Value=$"{paymentTime}"},
                new KeyValueModel{Key="@Note1",Value=$"{note1}"},
                new KeyValueModel{Key="@NoteSec",Value=$"{note2}"},
                new KeyValueModel{Key="@NoteThir",Value=$"{note3}"},
                };
            #endregion
            var directory = ConfigHelper.GetByKey("InvoiceAttachment");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
            var pathDir = serPath + "receipt.html";
            if (!File.Exists(pathDir))
            {
                return null;
            }

            return Common.Common.GenerateInvoiceAttachment(serPath, attachmentData, System.Web.HttpContext.Current.Server);
        }

        public async Task<PaginationSet<WaivedPaymentApprovedHistoryViewModel>> GetListWaiverApprovedHistory(WaivedHisrotyFilter filter)
        {
            var result = new PaginationSet<WaivedPaymentApprovedHistoryViewModel>();
            var approvalHistories = (await _waiverApprovedStatusHistoryRepository.GetMultiPaging(x => x.InvoiceId == filter.InvoiceId, "Id", false, filter.Page, filter.Size,
                new string[] { "WaivedHistoryDocuments.Document" })).Select(c => c.ToWaivedPaymentApprovedHistoryViewModel(_userRepository.GetSingleById(c.ApprovalUpdatedBy).DisplayName));
            var total = await _waiverApprovedStatusHistoryRepository.Count(x => x.InvoiceId == filter.InvoiceId);

            result.Items = approvalHistories;
            result.TotalCount = total;
            return result;

        }

        public async Task<ResultModel<bool>> ChangeWaivedPaymentApprovedStatus(WaivedPaymentApprovedHistoryBindingModel model, HttpFileCollection files, int userId)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var courseId = (await _invoiceRepository.GetSingleByCondition(x => x.Id == model.InvoiceId, new string[] { "Application" }))?.Application?.CourseId;
            if (!courseId.HasValue)
            {
                result.Message = "No course match with the invoice id";
                return result;
            }

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == courseId, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }

            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.Id == model.InvoiceId, new string[] { "WaiverApprovedStatusHistories.WaivedHistoryDocuments.Document", "Application.Course" });

            if (invoice.Status != (int)Common.Enums.InvoiceStatus.Created)
            {
                result.Message = "Invoice was offered";
                return result;
            }

            switch (invoice.WaiverApprovedStatus)
            {
                case (int)WaivedPaymentStatus.Cancelled:
                case (int)WaivedPaymentStatus.FirstReject:
                case (int)WaivedPaymentStatus.SecondReject:
                case (int)WaivedPaymentStatus.ThirdReject:
                    #region Cancelled/FirstReject/SecondReject/ThirdReject
                    if (model.ApprovedStatus == (int)WaivedPaymentStatus.Submitted && privilege.IsSubmitAndCancelCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.Submitted, userId, model.Remarks, files);
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
                #endregion
                case (int)WaivedPaymentStatus.Submitted:
                    #region Submitted
                    if (model.ApprovedStatus == (int)WaivedPaymentStatus.Cancelled && privilege.IsSubmitAndCancelCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.Cancelled, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)WaivedPaymentStatus.FirstReject && privilege.IsFirstApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.FirstReject, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)WaivedPaymentStatus.FirstApproved && privilege.IsFirstApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.FirstApproved, userId, model.Remarks, files);
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
                #endregion
                case (int)WaivedPaymentStatus.FirstApproved:
                    #region FirstApproved
                    if (model.ApprovedStatus == (int)WaivedPaymentStatus.SecondReject && privilege.IsSecondpproveAndRejectCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.SecondReject, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)WaivedPaymentStatus.SecondApproved && privilege.IsSecondpproveAndRejectCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.SecondApproved, userId, model.Remarks, files);
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
                #endregion
                case (int)WaivedPaymentStatus.SecondApproved:
                    #region SecondApproved
                    if (model.ApprovedStatus == (int)WaivedPaymentStatus.ThirdReject && privilege.IsThirdApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.ThirdReject, userId, model.Remarks, files);

                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }


                        break;
                    }
                    else if (model.ApprovedStatus == (int)WaivedPaymentStatus.ThirdApproved && privilege.IsThirdApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateWaivedApprovedStatus(invoice, invoice.WaiverApprovedStatus, WaivedPaymentStatus.ThirdApproved, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        #region Change Enrollment status

                        var application = await _applicationRepository.GetSingleByCondition(x => x.Id == invoice.ApplicationId, new string[] { "EnrollmentStatusStorages" });

                        EnrollmentStatusStorage status = new EnrollmentStatusStorage()
                        {
                            Status = (int)Common.Enums.EnrollmentStatus.Enrolled,
                            LastModifiedBy = userId,
                            LastModifiedDate = DateTime.Now
                        };
                        application.EnrollmentStatusStorages.Add(status);

                        var lessons = await _lessionRepository.GetMulti(x => x.ClassId == application.AdminAssignedClass);
                        var exams = await _examRepository.GetMulti(x => x.ClassId == application.AdminAssignedClass);

                        foreach (var les in lessons)
                        {
                            application.LessonAttendances.Add(new LessonAttendance()
                            {
                                IsMakeUp = false,
                                IsTakeAttendance = true,
                                UserId = application.UserId,
                                LessonId = les.Id,
                                ApplicationId = application.Id
                            });
                        }

                        foreach (var ex in exams)
                        {
                            application.ExamApplications.Add(new ExamApplication()
                            {
                                ApplicationId = application.Id,
                                AssessmentMark = null,
                                AssessmentResult = null,
                                ExamId = ex.Id,
                                IsMakeUp = false
                            });
                        }
                        _applicationRepository.Update(application);

                        #endregion
                        break;
                    }
                    else
                    {
                        result.Message = "You dont have permission";
                        return result;
                    }
                #endregion
                case (int)WaivedPaymentStatus.ThirdApproved:
                    result.Message = "This invoice was waived";
                    return result;
                default:
                    result.Message = "Action cannot perform";
                    return result;
            }
            try
            {
                _unitOfWork.Commit();

                #region Send Mail
                string callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;

                var _submittedAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == invoice.Application.Course.Id && x.IsSubmitAndCancelCourse == true)).Select(x => x.UserId);
                var _submittedAdminEmails = (await _userRepository.GetMulti(x => _submittedAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

                var _firstAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == invoice.Application.Course.Id && x.IsFirstApproveAndRejectCourse == true)).Select(x => x.UserId);
                var _firstAdminEmails = (await _userRepository.GetMulti(x => _firstAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

                var _secondAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == invoice.Application.Course.Id && x.IsSecondpproveAndRejectCourse == true)).Select(x => x.UserId);
                var _secondAdminEmails = (await _userRepository.GetMulti(x => _secondAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

                var _thirdAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == invoice.Application.Course.Id && x.IsThirdApproveAndRejectCourse == true)).Select(x => x.UserId);
                var _thirdAdminEmails = (await _userRepository.GetMulti(x => _thirdAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);
                switch (model.ApprovedStatus)
                {
                    case (int)WaivedPaymentStatus.Submitted:

                        foreach (var e in _firstAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item26", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem26Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search"));
                            if (isSuccess)
                            {
                                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                                emailCommonData.ValueInt++;
                                _commonDataService.Update(emailCommonData);
                            }
                        }

                        break;
                    case (int)WaivedPaymentStatus.FirstApproved:
                        foreach (var e in _secondAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item28", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem28Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search"));
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
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem30Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search", "1st approver"));
                            if (isSuccess)
                            {
                                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                                emailCommonData.ValueInt++;
                                _commonDataService.Update(emailCommonData);
                            }
                        }
                        break;
                    case (int)WaivedPaymentStatus.FirstReject:

                        foreach (var e in _submittedAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem31Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search", "1st approver"));
                            if (isSuccess)
                            {
                                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                                emailCommonData.ValueInt++;
                                _commonDataService.Update(emailCommonData);
                            }
                        }
                        break;
                    case (int)WaivedPaymentStatus.SecondApproved:
                        foreach (var e in _thirdAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item29", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem29Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search"));
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
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem30Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search", "2nd approver"));
                            if (isSuccess)
                            {
                                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                                emailCommonData.ValueInt++;
                                _commonDataService.Update(emailCommonData);
                            }
                        }
                        break;
                    case (int)WaivedPaymentStatus.SecondReject:

                        foreach (var e in _submittedAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem31Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search", "2nd approver"));
                            if (isSuccess)
                            {
                                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                                emailCommonData.ValueInt++;
                                _commonDataService.Update(emailCommonData);
                            }
                        }
                        break;
                    case (int)WaivedPaymentStatus.ThirdApproved:

                        foreach (var e in _submittedAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem30Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search", "3rd approver"));
                            if (isSuccess)
                            {
                                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                                emailCommonData.ValueInt++;
                                _commonDataService.Update(emailCommonData);
                            }
                        }
                        break;
                    case (int)WaivedPaymentStatus.ThirdReject:

                        foreach (var e in _submittedAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem31Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search", "3rd approver"));
                            if (isSuccess)
                            {
                                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                                emailCommonData.ValueInt++;
                                _commonDataService.Update(emailCommonData);
                            }
                        }
                        break;
                    case (int)WaivedPaymentStatus.Cancelled:
                        foreach (var e in _firstAdminEmails)
                        {
                            string emailSubject = FileHelper.GetEmailSubject("item27", "EmailSubject", "EN");
                            bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, invoice.Application.Course.CourseCode), Common.Common.GenerateItem27Email(invoice.Application.Course.CourseCode, callbackurl + "/student-class-management/search"));
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
            catch (Exception ex)
            {
                throw;
            }
            result.Message = "Success";
            result.IsSuccess = true;

            return result;
        }

        private bool UpdateWaivedApprovedStatus(Invoice invoice, int statusFrom, WaivedPaymentStatus statusTo, int userId, string remarks, HttpFileCollection files)
        {
            invoice.WaiverApprovedStatus = (int)statusTo;

            var waivedHistory = new WaiverApprovedStatusHistory();
            waivedHistory.ApprovalUpdatedBy = userId;
            waivedHistory.ApprovalStatusFrom = statusFrom;
            waivedHistory.ApprovalStatusTo = (int)statusTo;
            waivedHistory.ApprovalUpdatedTime = DateTime.UtcNow;
            waivedHistory.ApprovalRemarks = remarks;

            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("AppicationApprovedDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var courseCode = invoice.Application.Course.CourseCode;
                string pathDirectory = serPath + courseCode + "\\" + DistinguishDocType.WaivedPayment.ToString();

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
                    string url = pathFile.Substring(pathFile.IndexOf(courseCode));
                    file.SaveAs(pathFile);

                    waivedHistory.WaivedHistoryDocuments.Add(new WaivedHistoryDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.UtcNow
                        }
                    });
                }
            }
            invoice.WaiverApprovedStatusHistories.Add(waivedHistory);

            if (statusTo == WaivedPaymentStatus.ThirdApproved)
            {
                var paymentTransaction = new PaymentTransaction();
                paymentTransaction.TransactionType = (int)PaymentTransactionType.Waive;
                paymentTransaction.Amount = -invoice.Fee;
                paymentTransaction.AcceptedBankId = (int)Common.Enums.AcceptedBank.None;
                paymentTransaction.RefNo = null;
                paymentTransaction.ReasonForRefund = null;
                paymentTransaction.PaymentDate = DateTime.UtcNow;

                invoice.PaymentTransactions.Add(paymentTransaction);
                invoice.Status = (int)Common.Enums.InvoiceStatus.Waived;
            }

            _invoiceRepository.Update(invoice);

            return true;
        }

        public async Task<ResultModel<bool>> CreateTransaction(TransactionBindingModel model, HttpFileCollection files, int userId)
        {
            var result = new ResultModel<bool>();
            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.Id == model.InvoiceId, new string[] { "PaymentTransactions", "RefundTransactions.RefundTransactionDocuments"
                , "InvoiceItems" });
            if (invoice == null)
            {
                result.Message = "No invoice matched";
                return result;
            }

            #region Condition create transaction
            if (invoice.Status == (int)Common.Enums.InvoiceStatus.Created)
            {
                result.Message = "Invoice was not offered";
                return result;
            }

            if (invoice.Status == (int)Common.Enums.InvoiceStatus.Waived)
            {
                result.Message = "Invoice was not waived";
                return result;
            }

            if (invoice.Status == (int)Common.Enums.InvoiceStatus.RefundPendingForApproval)
            {
                result.Message = "Invoice is pending for refund";
                return result;
            }
            #endregion

            #region Handle refund transaction
            if (model.TypeOfPayment == (int)PaymentTransactionType.Refund)
            {
                if (invoice.Status != (int)Common.Enums.InvoiceStatus.Overpaid)
                {
                    result.Message = "Invoice status is not Overdue";
                    return result;
                }

                bool success = CreateRefundTransaction(model, files, invoice);
                if (success)
                {
                    _invoiceRepository.Update(invoice);
                    _unitOfWork.Commit();

                    result.Message = "Success";
                    result.IsSuccess = true;
                }
                else
                {
                    result.Message = "Failed to create refund";
                }
                return result;

            }
            #endregion
            var transaction = new PaymentTransaction();

            #region Binding model
            //transaction.InvoiceId = model.InvoiceId;
            transaction.TransactionType = model.TypeOfPayment;
            transaction.Amount = model.Amount;
            transaction.AcceptedBankId = model.BankCodeAndBankName;
            transaction.RefNo = model.RefNo;
            transaction.PaymentDate = model.PaymentDate;
            transaction.Remarks = model.Remarks;
            invoice.PaymentTransactions.Add(transaction);

            decimal paidFee = 0;
            foreach (var item in invoice.PaymentTransactions)
            {
                paidFee += item.Amount;
            }

            //if (invoice.Fee - paidFee == invoice.Fee)
            //{
            //    invoice.Status = (int)Common.Enums.InvoiceStatus.Offered;
            //}
            //else 
            if (invoice.Fee - paidFee > 0)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.PaidPartially;
            }
            else if (invoice.Fee - paidFee == 0)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.Settled;
            }
            else if (invoice.Fee - paidFee < 0)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.Overpaid;
            }
            #endregion

            #region Add files
            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("TransactionDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + invoice.InvoiceNumber;
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
                    string url = pathFile.Substring(pathFile.IndexOf(invoice.InvoiceNumber));
                    file.SaveAs(pathFile);

                    transaction.PaymentTransactionDocuments.Add(new PaymentTransactionDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.UtcNow
                        }
                    });
                }
            }
            #endregion

            #region Change Enrollment status

            if (invoice.Status == (int)Common.Enums.InvoiceStatus.Settled
                || invoice.Status == (int)Common.Enums.InvoiceStatus.Overpaid
                || invoice.Status == (int)Common.Enums.InvoiceStatus.Waived)
            {
                var application = await _applicationRepository.GetSingleByCondition(x => x.Id == invoice.ApplicationId, new string[] { "EnrollmentStatusStorages" });
                EnrollmentStatusStorage status;
                if (invoice.InvoiceItems.LastOrDefault().InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.ReExamFee)
                {
                    status = new EnrollmentStatusStorage()
                    {
                        Status = (int)Common.Enums.EnrollmentStatus.Resit,
                        LastModifiedBy = userId,
                        LastModifiedDate = DateTime.Now
                    };
                    application.EnrollmentStatusStorages.Add(status);
                }
                else //if (invoice.InvoiceItems.LastOrDefault().InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee)
                {
                    status = new EnrollmentStatusStorage()
                    {
                        Status = (int)Common.Enums.EnrollmentStatus.Enrolled,
                        LastModifiedBy = userId,
                        LastModifiedDate = DateTime.Now
                    };
                    application.EnrollmentStatusStorages.Add(status);

                    var lessons = await _lessionRepository.GetMulti(x => x.ClassId == application.AdminAssignedClass);
                    var exams = await _examRepository.GetMulti(x => x.ClassId == application.AdminAssignedClass);

                    foreach (var les in lessons)
                    {
                        application.LessonAttendances.Add(new LessonAttendance()
                        {
                            IsMakeUp = false,
                            IsTakeAttendance = true,
                            UserId = application.UserId,
                            LessonId = les.Id,
                            ApplicationId = application.Id
                        });
                    }

                    foreach (var ex in exams)
                    {
                        application.ExamApplications.Add(new ExamApplication()
                        {
                            ApplicationId = application.Id,
                            AssessmentMark = null,
                            AssessmentResult = null,
                            ExamId = ex.Id,
                            IsMakeUp = false
                        });
                    }
                }
                _applicationRepository.Update(application);
            }

            #endregion

            _invoiceRepository.Update(invoice);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        private bool CreateRefundTransaction(TransactionBindingModel model, HttpFileCollection files, Invoice invoice)
        {
            try
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.RefundPendingForApproval;

                var refundTransaction = new RefundTransaction();

                decimal paidFee = 0;
                foreach (var item in invoice.PaymentTransactions)
                {
                    paidFee += item.Amount;
                }

                if (invoice.Fee - paidFee > 0)
                {
                    return false;
                }

                refundTransaction.TransactionTypeId = model.TypeOfPayment;
                refundTransaction.Amount = 0 - paidFee;
                refundTransaction.AcceptedBankId = model.BankCodeAndBankName;
                refundTransaction.RefNo = model.RefNo;
                refundTransaction.PaymentDate = model.PaymentDate;
                refundTransaction.Remarks = model.Remarks;
                refundTransaction.ReasonForRefund = model.ReasonForRefund;
                refundTransaction.RefundApprovedStatus = (int)RefundStatus.Created;



                invoice.RefundTransactions.Add(refundTransaction);

                #region Add files
                if (files != null && files.Count > 0)
                {
                    var directory = ConfigHelper.GetByKey("TransactionDirectory");
                    var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                    string pathDirectory = serPath + invoice.InvoiceNumber;
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
                        string url = pathFile.Substring(pathFile.IndexOf(invoice.InvoiceNumber));
                        file.SaveAs(pathFile);

                        refundTransaction.RefundTransactionDocuments.Add(new SPDC.Model.Models.RefundTransactionDocument()
                        {
                            Document = new Document()
                            {
                                Url = url,
                                ContentType = originalFileExtension,
                                FileName = originalFileName,
                                ModifiedDate = DateTime.UtcNow
                            }
                        });
                    }
                }
                #endregion
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ResultModel<bool>> EditTransaction(TransactionBindingModel model, HttpFileCollection files, int userId)
        {
            var result = new ResultModel<bool>();
            var transaction = await _paymentTransactionRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "PaymentTransactionDocuments.Document", "Invoice.PaymentTransactions" });
            if (transaction == null)
            {
                result.Message = "Transaction was not found";
                return result;
            }

            List<int> invoiceStatusAccepted = new List<int>() { (int)Common.Enums.InvoiceStatus.Created, (int)Common.Enums.InvoiceStatus.Waived, (int)Common.Enums.InvoiceStatus.RefundPendingForApproval };
            if (invoiceStatusAccepted.Contains(transaction.Invoice.Status))
            {
                result.Message = "Cannot edit transaction";
                return result;
            }

            transaction.TransactionType = model.TypeOfPayment;
            transaction.Amount = model.Amount;
            transaction.AcceptedBankId = model.BankCodeAndBankName;
            transaction.RefNo = model.RefNo;
            transaction.PaymentDate = model.PaymentDate;
            transaction.Remarks = model.Remarks;

            #region Update files
            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("TransactionDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + transaction.Invoice.InvoiceNumber;
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
                    string url = pathFile.Substring(pathFile.IndexOf(transaction.Invoice.InvoiceNumber));
                    file.SaveAs(pathFile);

                    transaction.PaymentTransactionDocuments.Add(new PaymentTransactionDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.UtcNow
                        }
                    });
                }
            }

            if (model.ListFileToDelete != null && model.ListFileToDelete.Count() > 0)
            {
                _documentRepository.DeleteMulti(x => model.ListFileToDelete.Contains(x.Id));
                _paymentTransactionDocumentRepository.DeleteMulti(x => model.ListFileToDelete.Contains(x.DocumentId));
            }
            #endregion

            #region Change Invoice Status
            decimal paidFee = 0;
            foreach (var item in transaction.Invoice.PaymentTransactions)
            {
                paidFee += item.Amount;
            }

            if (transaction.Invoice.Fee - paidFee > 0)
            {
                transaction.Invoice.Status = (int)Common.Enums.InvoiceStatus.PaidPartially;
            }
            else if (transaction.Invoice.Fee - paidFee == 0)
            {
                transaction.Invoice.Status = (int)Common.Enums.InvoiceStatus.Settled;
            }
            else if (transaction.Invoice.Fee - paidFee < 0)
            {
                transaction.Invoice.Status = (int)Common.Enums.InvoiceStatus.Overpaid;
            }
            #endregion
            _paymentTransactionRepository.Update(transaction);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<PaginationSet<TransactionViewModel>> GetListTransaction(TransactionFilter filter)
        {

            var listTransaction = (await _paymentTransactionRepository.GetMultiPaging(x => x.InvoiceId == filter.InvoiceId,
                "Id", false, filter.Page, filter.Size, new string[] { "PaymentTransactionDocuments.Document" })).Select(x => x.ToTransactionViewModel());

            var total = await _paymentTransactionRepository.Count(x => x.InvoiceId == filter.InvoiceId);

            return new PaginationSet<TransactionViewModel>()
            {
                Items = listTransaction,
                Page = filter.Page,
                TotalCount = total
            };
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadTransactionDocument(int documentId)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var doc = _documentRepository.GetSingleById(documentId);
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

            var directory = ConfigHelper.GetByKey("TransactionDirectory");
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

        public async Task<ResultModel<bool>> DeleteTransactionById(int transId)
        {
            var result = new ResultModel<bool>();
            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.PaymentTransactions.Any(c => c.Id == transId), new string[] { "PaymentTransactions" });

            if (invoice == null)
            {
                result.Message = "Transaction was not found";
                return result;
            }

            #region Condition create transaction
            if (invoice.Status == (int)Common.Enums.InvoiceStatus.Created)
            {
                result.Message = "Invoice was not offered";
                return result;
            }

            if (invoice.Status == (int)Common.Enums.InvoiceStatus.Waived)
            {
                result.Message = "Invoice was not waived";
                return result;
            }

            if (invoice.Status == (int)Common.Enums.InvoiceStatus.RefundPendingForApproval)
            {
                result.Message = "Invoice is pending for refund";
                return result;
            }
            #endregion

            #region Change Invoice Status
            decimal paidFee = 0;
            foreach (var item in invoice.PaymentTransactions)
            {
                if (item.Id == transId)
                {
                    continue;
                }
                paidFee += item.Amount;
            }

            if (invoice.Fee - paidFee == invoice.Fee)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.Offered;
            }
            else if (invoice.Fee - paidFee > 0)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.PaidPartially;
            }
            else if (invoice.Fee - paidFee == 0)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.Settled;
            }
            else if (invoice.Fee - paidFee < 0)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.Overpaid;
            }
            #endregion

            _invoiceRepository.Update(invoice);
            _paymentTransactionRepository.DeleteMulti(x => x.Id == transId);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<TransactionViewModel>> GetTransactionById(int transId)
        {
            var result = new ResultModel<TransactionViewModel>();

            var transaction = (await _paymentTransactionRepository.GetSingleByCondition(x => x.Id == transId)).ToTransactionViewModel();

            if (transaction == null)
            {
                result.Message = "Transaction was not found";
                return result;
            }
            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = transaction;
            return result;
        }

        public async Task<ResultModel<bool>> EditRefundTransaction(TransactionBindingModel model, HttpFileCollection files, int userId)
        {
            var result = new ResultModel<bool>();

            var refundTransaction = await _refundTransactionRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Invoice" });

            if (refundTransaction == null)
            {
                result.Message = "Transaction was not found";
                return result;
            }

            var listStatusEditRefund = new List<int>() { (int)RefundStatus.Cancelled, (int)RefundStatus.Created };
            if (refundTransaction.Invoice.Status != (int)Common.Enums.InvoiceStatus.RefundPendingForApproval)
            {
                result.Message = "Invoice status is not RefundPendingForApproval";
                return result;
            }
            else if (!listStatusEditRefund.Contains(refundTransaction.RefundApprovedStatus))
            {
                result.Message = "Refund transaction was submitted";
                return result;
            }


            #region Binding Model
            refundTransaction.AcceptedBankId = model.BankCodeAndBankName;
            refundTransaction.RefNo = model.RefNo;
            refundTransaction.PaymentDate = model.PaymentDate;
            refundTransaction.Remarks = model.Remarks;
            refundTransaction.ReasonForRefund = model.ReasonForRefund;
            #endregion

            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("TransactionDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + refundTransaction.Invoice.InvoiceNumber;
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
                    string url = pathFile.Substring(pathFile.IndexOf(refundTransaction.Invoice.InvoiceNumber));
                    file.SaveAs(pathFile);

                    refundTransaction.RefundTransactionDocuments.Add(new SPDC.Model.Models.RefundTransactionDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.UtcNow
                        }
                    });
                }
            }

            _refundTransactionRepository.Update(refundTransaction);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<RefundTransactionViewModel>> GetRefundTransactionById(int transId)
        {
            var result = new ResultModel<RefundTransactionViewModel>();

            var refundTransaction = (await _refundTransactionRepository.GetSingleByCondition(x => x.Id == transId, new string[] { "RefundTransactionDocuments.Document" })).ToRefundTransactionViewModel();

            if (refundTransaction == null)
            {
                result.Message = "Transaction was not found";
            }
            else
            {
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = refundTransaction;
            }
            return result;
        }

        public async Task<ResultModel<bool>> ChangeApprovedStatusRefundTransaction(int userId, ApprvovedRefundTransactionBindingModel model, HttpFileCollection files)
        {
            ResultModel<bool> result = new ResultModel<bool>();

            var refundTransaction = await _refundTransactionRepository.GetSingleByCondition(x => x.Id == model.RefundTransactionId,
                new string[] { "Invoice.Application", "RefundTransactionDocuments.Document" });

            if (refundTransaction == null)
            {
                result.Message = "Transaction was not found";
                return result;
            }

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == refundTransaction.Invoice.Application.CourseId, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }

            switch (refundTransaction.RefundApprovedStatus)
            {
                case (int)RefundStatus.Created:
                case (int)RefundStatus.Cancelled:
                case (int)RefundStatus.FirstReject:
                case (int)RefundStatus.SecondReject:
                case (int)RefundStatus.ThirdReject:
                    #region Submit
                    if (model.ApprovedStatus == (int)RefundStatus.Submitted && privilege.IsSubmitAndCancelCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.Created, (int)RefundStatus.Submitted, userId, model.Remarks, files);
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
                #endregion
                case (int)RefundStatus.Submitted:
                    #region First Approved
                    if (model.ApprovedStatus == (int)RefundStatus.Cancelled && privilege.IsSubmitAndCancelCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.Submitted, (int)RefundStatus.Cancelled, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)RefundStatus.FirstReject && privilege.IsFirstApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.Submitted, (int)RefundStatus.FirstReject, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)RefundStatus.FirstApproved && privilege.IsFirstApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.Submitted, (int)RefundStatus.FirstApproved, userId, model.Remarks, files);
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
                #endregion
                case (int)RefundStatus.FirstApproved:
                    #region Second Approved
                    if (model.ApprovedStatus == (int)RefundStatus.SecondReject && privilege.IsSecondpproveAndRejectCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.FirstApproved, (int)RefundStatus.SecondReject, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)RefundStatus.SecondApproved && privilege.IsSecondpproveAndRejectCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.FirstApproved, (int)RefundStatus.SecondApproved, userId, model.Remarks, files);
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
                #endregion
                case (int)RefundStatus.SecondApproved:
                    #region Third Approved
                    if (model.ApprovedStatus == (int)RefundStatus.ThirdReject && privilege.IsThirdApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.SecondApproved, (int)RefundStatus.ThirdReject, userId, model.Remarks, files);
                        if (!iSuccess)
                        {
                            result.Message = "Action cannot perform";
                            return result;
                        }
                        break;
                    }
                    else if (model.ApprovedStatus == (int)RefundStatus.ThirdApproved && privilege.IsThirdApproveAndRejectCourse)
                    {
                        var iSuccess = UpdateRefundApprovedStatus(refundTransaction, (int)RefundStatus.SecondApproved, (int)RefundStatus.ThirdApproved, userId, model.Remarks, files);
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
                #endregion
                case (int)RefundStatus.ThirdApproved:
                    result.Message = "This transaction was third approved";
                    return result;
                default:
                    result.Message = "Action cannot perform";
                    return result;
            }

            _unitOfWork.Commit();
            result.Message = "Update Success";
            result.IsSuccess = true;
            return result;
        }

        private bool UpdateRefundApprovedStatus(RefundTransaction refundTransaction, int statusFrom, int statusTo, int userId, string remarks, HttpFileCollection files)
        {
            if (refundTransaction == null) return false;
            refundTransaction.RefundApprovedStatus = statusTo;

            var refundHistory = new RefundTransactionApprovedStatusHistory();
            refundHistory.ApprovalUpdatedBy = userId;
            refundHistory.ApprovalStatusFrom = statusFrom;
            refundHistory.ApprovalStatusTo = statusTo;
            refundHistory.ApprovalUpdatedTime = DateTime.UtcNow;
            refundHistory.ApprovalRemarks = remarks;

            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("TransactionDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + "RefundTransaction" + "\\" + refundHistory.Id;

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
                    string url = pathFile.Substring(pathFile.IndexOf("RefundTransaction"));
                    file.SaveAs(pathFile);

                    refundHistory.RefundTransactionHistoryDocuments.Add(new RefundTransactionHistoryDocument()
                    {
                        Document = new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.UtcNow
                        }
                    });
                }
            }

            refundTransaction.RefundTransactionApprovedStatusHistories.Add(refundHistory);
            _refundTransactionRepository.Update(refundTransaction);

            #region Handle third approved
            if (statusTo == (int)RefundStatus.ThirdApproved)
            {
                var paymentTransaction = new PaymentTransaction();
                paymentTransaction.TransactionType = refundTransaction.TransactionTypeId;
                paymentTransaction.Amount = refundTransaction.Amount;
                paymentTransaction.PaymentDate = refundTransaction.PaymentDate;
                paymentTransaction.RefNo = refundTransaction.RefNo;
                paymentTransaction.AcceptedBankId = refundTransaction.AcceptedBankId;
                paymentTransaction.Remarks = refundTransaction.Remarks;
                paymentTransaction.ReasonForRefund = refundTransaction.ReasonForRefund;

                if (refundTransaction.RefundTransactionDocuments.Count() > 0)
                {
                    var refundDirectory = ConfigHelper.GetByKey("TransactionDirectory");
                    var refundSerPath = System.Web.HttpContext.Current.Server.MapPath(refundDirectory);

                    var transactionDirectory = ConfigHelper.GetByKey("TransactionDirectory");
                    var transactionSerPath = System.Web.HttpContext.Current.Server.MapPath(transactionDirectory);
                    string transactionPathDirectory = transactionSerPath + refundTransaction.Invoice.InvoiceNumber;

                    if (!Directory.Exists(transactionPathDirectory))
                    {
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(transactionPathDirectory);
                    }
                    foreach (var item in refundTransaction.RefundTransactionDocuments)
                    {
                        var pathRefundFile = Path.Combine(refundSerPath, item.Document.Url);

                        var pathTransactionFile = Common.Common.GenFileNameDuplicate(transactionPathDirectory + "\\" + item.Document.FileName);

                        if (File.Exists(pathRefundFile))
                        {
                            FileInfo fileRefund = new FileInfo(pathRefundFile);
                            fileRefund.CopyTo(pathTransactionFile);

                            var doc = new Document();
                            doc.Url = pathTransactionFile.Substring(pathTransactionFile.IndexOf(refundTransaction.Invoice.InvoiceNumber));
                            doc.ContentType = item.Document.ContentType;
                            doc.FileName = fileRefund.Name;
                            doc.ModifiedDate = DateTime.UtcNow;

                            paymentTransaction.PaymentTransactionDocuments.Add(new PaymentTransactionDocument()
                            {
                                Document = doc
                            });
                        }
                    }
                }

                refundTransaction.Invoice.Status = (int)Common.Enums.InvoiceStatus.PendingForRefund;
                refundTransaction.Invoice.PaymentTransactions.Add(paymentTransaction);
            }
            #endregion
            return true;
        }

        public async Task<PaginationSet<RefundTransactionApprovedHistoryViewModel>> GetListRefundTrasactionById(RefundTransactionFilter filter)
        {
            List<RefundTransactionApprovedHistoryViewModel> listHistory = new List<RefundTransactionApprovedHistoryViewModel>();
            try
            {
                listHistory = (await _refundTransactionApprovedStatusHistoryRepository.GetMultiPaging(x
                => x.RefundTransaction.InvoiceId == filter.InvoiceId && x.RefundTransaction.RefundApprovedStatus != (int)RefundStatus.ThirdApproved,
                "Id", false, filter.Page, filter.Size,
                new string[] { "RefundTransactionHistoryDocuments.Document", "RefundTransaction.RefundTransactionDocuments.Document" }))
                .Select(c => c.ToRefundTransactionApprovedHistoryViewModel(_userRepository.GetSingleById(c.ApprovalUpdatedBy).DisplayName)).ToList();

                if (listHistory.Count() == 0)
                {
                    //listHistory = new List<RefundTransactionApprovedHistoryViewModel>();
                    var refundTransaction = (await _refundTransactionRepository.GetSingleByCondition(x => x.InvoiceId == filter.InvoiceId && x.RefundApprovedStatus != (int)RefundStatus.ThirdApproved,
                      new string[] { "RefundTransactionDocuments.Document" }))?.
                      ToRefundTransactionApprovedHistoryViewModelFromRefundTransaction();
                    if (refundTransaction != null)
                    {
                        listHistory.Add(refundTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            var total = await _refundTransactionApprovedStatusHistoryRepository.Count(x => x.RefundTransactionId == filter.InvoiceId);

            return new PaginationSet<RefundTransactionApprovedHistoryViewModel>()
            {
                Items = (listHistory.Count() == 0 || listHistory == null) ? null : listHistory,
                Page = filter.Page,
                TotalCount = total
            };

        }

        public async Task<ResultModel<bool>> DeleteRefundTransactionById(int transactionId)
        {
            var result = new ResultModel<bool>();
            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.RefundTransactions.Any(c => c.Id == transactionId), new string[] { "RefundTransactions" });

            if (invoice == null)
            {
                result.Message = "Transaction was not found";
                return result;
            }

            var listStatusEditRefund = new List<int>() { (int)RefundStatus.Cancelled, (int)RefundStatus.Created };

            if (!listStatusEditRefund.Contains(invoice.RefundTransactions.FirstOrDefault(x => x.Id == transactionId).RefundApprovedStatus))
            {
                result.Message = "Refund transaction was submitted";
                return result;
            }

            invoice.Status = (int)Common.Enums.InvoiceStatus.Overpaid;
            _refundTransactionApprovedStatusHistoryRepository.DeleteMulti(x => x.RefundTransactionId == transactionId);
            _refundTransactionRepository.DeleteMulti(x => x.Id == transactionId);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> ChangeInvoiceToRefunded(int invoiceId, int userId)
        {
            var result = new ResultModel<bool>();

            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.Id == invoiceId, new string[] { "Application" });

            if (invoice == null)
            {
                result.Message = "Invoice was not found";
                return result;
            }

            var privilege = await _systemPrivilegeRepository.GetSingleByCondition(x => x.UserId == userId && x.CourseId == invoice.Application.CourseId, new string[] { "User", "User.AdminPermission" });

            if (privilege == null || privilege.User?.AdminPermission?.Status == 0)
            {
                result.Message = "Your account is suspended";
                return result;
            }

            if (invoice.Status == (int)Common.Enums.InvoiceStatus.PendingForRefund)
            {
                invoice.Status = (int)Common.Enums.InvoiceStatus.Refunded;
                invoice.LastModifiedBy = userId;
                invoice.LastModifiedDate = DateTime.UtcNow;
            }

            _invoiceRepository.Update(invoice);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> CheckInvoiceOverdue()
        {
            var result = new ResultModel<bool>();

            var lstInvoice = await _invoiceRepository.GetMulti(x => x.Status == (int)Common.Enums.InvoiceStatus.Offered);

            if (lstInvoice == null)
            {
                result.Message = "No invoices offered";
                result.IsSuccess = true;
                return result;
            }

            foreach (var item in lstInvoice)
            {
                var dateCheck = item.PaymentDueDate;
                var today = DateTime.UtcNow;
                if (dateCheck.Day == today.Day && dateCheck.Month == today.Month && dateCheck.Year == today.Year)
                {
                    item.Status = (int)Common.Enums.InvoiceStatus.Overdue;
                    _invoiceRepository.Update(item);
                }
            }

            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<PaginationSet<BatchPaymentViewModel>> GetListBatchPaymentByPeriodTime(BatchPaymentFilter filter)
        {
            var result = new PaginationSet<BatchPaymentViewModel>();
            if (filter.StartDate.HasValue)
                filter.StartDate = filter.StartDate.Value.ToStartOfTheDay();
            if (filter.EndDate.HasValue)
                filter.EndDate = filter.EndDate.Value.ToEndOfTheDay();

            Expression<Func<BatchPayment, bool>> filterClause = x => (!filter.StartDate.HasValue || filter.StartDate.HasValue && x.PaymentDate >= filter.StartDate) &&
            (!filter.EndDate.HasValue || filter.EndDate.HasValue && x.PaymentDate <= filter.EndDate.Value);

            var lstBatchPayment = (await _batchPaymentRepository.GetMultiPaging(filterClause, "Id", true, filter.Page, filter.Size)).Select(c => c.ToBatchPaymentViewModel());
            var total = await _batchPaymentRepository.Count(filterClause);

            result.Items = lstBatchPayment;
            result.Page = filter.Page;
            result.TotalCount = total;
            return result;
        }

        public async Task<IEnumerable<UserForSettledViewModel>> GetListUserByCICNumber(string cicNumber)
        {
            var batchPaymentQueryable = await _userRepository.GetListUsesBatcchPaymentByCICnumber(cicNumber);
            var result = batchPaymentQueryable.Select(a => new UserForSettledViewModel()
            {
                Id = a.Id,
                ApplicantName = a.Particular.SurnameEN + " " + a.Particular.GivenNameEN,
                CICNumber = a.CICNumber,
                IsChineseName = false
            });

            return result;
        }

        public async Task<IEnumerable<UserForSettledViewModel>> GetListUserByName(string name)
        {
            name = name.Trim().ToLower();
            var batchPaymentQueryable = await _userRepository.GetListUsesBatcchPaymentByName(name);
            var result = batchPaymentQueryable.Select(x => x.ToUserForSettledViewModel(name));

            return result;
        }


        #region Batch Application
        public async Task<List<BatchApplicationViewModel>> BatchApplicationUpload(IList<HttpPostedFile> files)
        {
            var records = await GetApplicationFromFiles(files);

            List<BatchApplicationBindingModel> applicationModels = records.Item1;
            List<BatchApplicationViewModel> returnModels = records.Item2;

            if (returnModels.Count(x => x.IsError) > 0)
            {
                return returnModels;
            }

            foreach (var model in applicationModels)
            {
                var createResult = await CreateBatchApplication(model);
            }

            return returnModels;
        }

        private async Task<int> CreateBatchApplication(BatchApplicationBindingModel model)
        {
            var appIdResult = await _applicationService.CreateApplication(new ApplicationCreateBindingModel() { CourseId = model.CourseId, IsSubmit = false }, model.UserId, 1);
            if (appIdResult.Data == 0)
            {
                appIdResult.Data = await CreateBatchApplication(model);
            }

            // Send mail and notification item 35
            // send email
            var user = await _userRepository.GetSingleByCondition(x => x.Id == model.UserId, new string[] { "UserDevices" });
            string courseName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? model.CourseNameEN : model.CourseNameTC;

            string emailSubject = FileHelper.GetEmailSubject("item35", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
            emailSubject = emailSubject.Replace("@Model.CourseName", courseName);

            string emailTemplate = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item35EN.cshtml" : "Item35TC.cshtml";

            string displayName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? model.GivenNameEN : model.GivenNameCN;

            string url = $"{(await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("ApplicantPortal"))).ClientUrl}en/application";

            bool isSuccess = MailHelper.SendMail(user.Email, emailSubject, Common.Common.GenerateItem35Email(courseName, displayName, url, emailTemplate));
            if (isSuccess)
            {
                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                emailCommonData.ValueInt++;
                _commonDataService.Update(emailCommonData);
            }

            // send notification
            if (user.UserDevices != null && user.UserDevices.Count() > 0)
            {
                string notificationTitle = FileHelper.GetNotificationTitle("item35", "NotificationTitles", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                notificationTitle = notificationTitle.Replace("@Model.CourseName", courseName);
                Notification notification = new Notification()
                {
                    Body = "",
                    DataId = appIdResult.Data,
                    Title = notificationTitle,
                    Type = (int)NotificationType.Application
                };

                foreach (var device in user.UserDevices)
                {
                    NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                }

                notification.NotificationUsers.Add(new NotificationUser()
                {
                    CreatedDate = DateTime.UtcNow,
                    IsFavourite = false,
                    IsRead = false,
                    IsRemove = false,
                    UserId = user.Id
                });
                _notificationRepository.Add(notification);
            }

            return appIdResult.Data;
        }

        private async Task<Tuple<List<BatchApplicationBindingModel>, List<BatchApplicationViewModel>>> GetApplicationFromFiles(IList<HttpPostedFile> files)
        {
            List<BatchApplicationBindingModel> rtmApplications = new List<BatchApplicationBindingModel>();
            List<BatchApplicationViewModel> rtnList = new List<BatchApplicationViewModel>();
            IEnumerable<BatchApplicationApplicant> existCICNumberUsers = _userRepository.GetAllCICNumber();
            IEnumerable<BatchApplicationCourse> existCourseCode = _courseRepository.GetAllCourseCode();

            for (int i = 0; i < files.Count; i++)
            {
                BatchApplicationViewModel model = new BatchApplicationViewModel();
                model.FileName = files[i].FileName;

                var file = new XLWorkbook(files[i].InputStream);
                var ws1 = file.Worksheet(1);

                var rowCount = ws1.RowsUsed().Count();

                if (rowCount == 1)
                {
                    model.IsError = true;
                    model.Messages.Add("File is empty");
                }
                else
                {
                    List<BatchApplicationBindingModel> applications = new List<BatchApplicationBindingModel>();

                    for (int j = 2; j <= rowCount; j++)
                    {
                        if (string.IsNullOrWhiteSpace(ws1.Cell(j, 1).Value.ToString()) || string.IsNullOrWhiteSpace(ws1.Cell(j, 2).Value.ToString()))
                        {
                            if (string.IsNullOrWhiteSpace(ws1.Cell(j, 1).Value.ToString()))
                            {
                                model.IsError = true;
                                model.Messages.Add("CIC Number is blank");
                            }

                            if (string.IsNullOrWhiteSpace(ws1.Cell(j, 2).Value.ToString()))
                            {
                                model.IsError = true;
                                model.Messages.Add("Course Code is blank");
                            }
                        }
                        else
                        {
                            BatchApplicationBindingModel batchApplicationBindingModel = new BatchApplicationBindingModel()
                            {
                                CICNumber = ws1.Cell(j, 1).Value.ToString(),
                                CourseCode = ws1.Cell(j, 2).Value.ToString(),
                                SurnameEN = ws1.Cell(j, 3).Value.ToString(),
                                GivenNameEN = ws1.Cell(j, 4).Value.ToString(),
                                SurnameCN = ws1.Cell(j, 5).Value.ToString(),
                                GivenNameCN = ws1.Cell(j, 6).Value.ToString()
                            };

                            var course = existCourseCode.SingleOrDefault(x => x.CourseCode.Equals(batchApplicationBindingModel.CourseCode));
                            if (course == null)
                            {
                                model.IsError = true;
                                model.Messages.Add($"Course Code: {batchApplicationBindingModel.CourseCode} does not exist in System.");
                            }
                            else
                            {
                                if (!course.IsHaveTargetClass)
                                {
                                    model.IsError = true;
                                    model.Messages.Add($"Course Code: {batchApplicationBindingModel.CourseCode} have no target class.");
                                }
                                else
                                {
                                    batchApplicationBindingModel.CourseNameEN = course.CourseNameEN;
                                    batchApplicationBindingModel.CourseNameTC = course.CourseNameTC;
                                }
                            }

                            var applicant = existCICNumberUsers.SingleOrDefault(x => x.CICNumber.Equals(batchApplicationBindingModel.CICNumber));
                            if (applicant == null)
                            {
                                model.IsError = true;
                                model.Messages.Add($"CIC Number: {batchApplicationBindingModel.CICNumber} does not exist in System.");
                            }
                            else
                            {
                                applicant.SurnameCN = string.IsNullOrWhiteSpace(applicant.SurnameCN) ? "" : applicant.SurnameCN;
                                applicant.GivenNameCN = string.IsNullOrWhiteSpace(applicant.GivenNameCN) ? "" : applicant.GivenNameCN;

                                if (!applicant.SurnameEN.Equals(batchApplicationBindingModel.SurnameEN))
                                {
                                    model.IsError = true;
                                    model.Messages.Add($"CIC Number: {batchApplicationBindingModel.CICNumber} does not match Surname (English)");
                                }

                                if (!applicant.GivenNameEN.Equals(batchApplicationBindingModel.GivenNameEN))
                                {
                                    model.IsError = true;
                                    model.Messages.Add($"CIC Number: {batchApplicationBindingModel.CICNumber} does not match Given Name (English)");
                                }

                                if (!applicant.SurnameCN.Trim().Equals(batchApplicationBindingModel.SurnameCN.Trim()))
                                {
                                    model.IsError = true;
                                    model.Messages.Add($"CIC Number: {batchApplicationBindingModel.CICNumber} does not match Surname (Chinese)");
                                }

                                if (!applicant.GivenNameCN.Trim().Equals(batchApplicationBindingModel.GivenNameCN.Trim()))
                                {
                                    model.IsError = true;
                                    model.Messages.Add($"CIC Number: {batchApplicationBindingModel.CICNumber} does not match Surname Given Name (Chinese)");
                                }
                            }

                            if (applicant != null && course != null)
                            {
                                int count = await _applicationRepository.Count(x => x.CourseId == course.Id
                                                                            && x.UserId == applicant.Id
                                                                            && (x.Status != (int)ApplicationStatus.Rejected && x.Status != (int)ApplicationStatus.Withdrawal && x.Status != (int)ApplicationStatus.Replaced));
                                if (count > 0)
                                {
                                    model.IsError = true;
                                    model.Messages.Add($"Application already existed for {applicant.CICNumber}");
                                }
                            }

                            batchApplicationBindingModel.UserId = applicant != null ? applicant.Id : 0;
                            batchApplicationBindingModel.CourseId = course != null ? course.Id : 0;
                            applications.Add(batchApplicationBindingModel);
                        }
                    }

                    rtmApplications.AddRange(applications);
                }
                rtnList.Add(model);
            }

            var countDuplicate = rtmApplications.GroupBy(x => new { x.CICNumber, x.CourseCode }).Select(x => new { CICNumber = x.Key.CICNumber, CourseCode = x.Key.CourseCode, Count = x.Count() });
            if (countDuplicate.Any(x => x.Count > 1))
            {
                BatchApplicationViewModel lastModel = new BatchApplicationViewModel();
                lastModel.FileName = "All";
                lastModel.IsError = true;
                foreach (var item in countDuplicate)
                {
                    if (item.Count > 1)
                    {
                        lastModel.Messages.Add($"CIC Number {item.CICNumber} and Course Code {item.CourseCode} is duplicate.");
                    }
                }
                rtnList.Add(lastModel);
            }

            return Tuple.Create(rtmApplications, rtnList);
        }
        #endregion

        public async Task<BatchPaymentItemViewModel> GetUserApplicationsByUserId(int userId, bool isChineseName)
        {
            var batchPaymentItem = await Task.Run(() => _applicationRepository.GetBatchPaymentItemOfferStatus(userId, isChineseName));

            return batchPaymentItem;
        }

        public async Task<ResultModel<BatchPaymentViewModel>> CreateBatchPayment(BatchPaymentDetailBindingModel model, int createUser, HttpFileCollection files)
        {
            var batchPayment = new BatchPayment();
            batchPayment.TransactionTypeId = model.TypeOfPaymentId;
            batchPayment.AcceptedBankId = model.BankCodeBankNameId;
            batchPayment.PaymentDate = model.BatchPaymentDate;
            batchPayment.BatchPaymentAmount = model.BatchPaymentAmount;
            batchPayment.RefNo = model.ReferenceNumber;
            batchPayment.Payee = model.Payee;
            batchPayment.Remarks = model.Remarks;

            if (model.ListApplication != null)
            {
                batchPayment.ListApplication = new JavaScriptSerializer().Serialize(model.ListApplication);
                batchPayment.TotalCount = model.ListApplication.Count();
            }

            _batchPaymentRepository.Add(batchPayment);
            _unitOfWork.Commit();

            if (files != null && files.Count > 0)
            {
                var serPath = System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.GetByKey("BatchPaymentDirectory"));
                string pathDirectory = serPath + "BatchId_" + batchPayment.Id;

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
                    string url = pathFile.Substring(pathFile.IndexOf("BatchId_" + batchPayment.Id));
                    file.SaveAs(pathFile);

                    batchPayment.Documents.Add(new Document()
                    {
                        Url = url,
                        ContentType = originalFileExtension,
                        FileName = originalFileName,
                        ModifiedDate = DateTime.UtcNow
                    });
                }
            }

            _batchPaymentRepository.Update(batchPayment);
            _unitOfWork.Commit();

            var result = new ResultModel<BatchPaymentViewModel>();
            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = batchPayment.ToBatchPaymentViewModel();

            return result;
        }

        public async Task<ResultModel<BatchPaymentViewModel>> UpdateBatchPayment(BatchPaymentDetailBindingModel model, int createUser, HttpFileCollection files)
        {
            var result = new ResultModel<BatchPaymentViewModel>();
            var batchPayment = await _batchPaymentRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Documents" });
            if (batchPayment == null)
            {
                result.Message = "Batch payment was not found";
                return result;
            }

            batchPayment.TransactionTypeId = model.TypeOfPaymentId;
            batchPayment.AcceptedBankId = model.BankCodeBankNameId;
            batchPayment.PaymentDate = model.BatchPaymentDate;
            batchPayment.BatchPaymentAmount = model.BatchPaymentAmount;
            batchPayment.RefNo = model.ReferenceNumber;
            batchPayment.Payee = model.Payee;
            batchPayment.Remarks = model.Remarks;

            if (model.ListApplication != null)
            {
                batchPayment.ListApplication = new JavaScriptSerializer().Serialize(model.ListApplication);
                batchPayment.TotalCount = model.ListApplication.Count();
            }

            if (model.ListFileToDelete.Count > 0)
            {
                var lstDeleteDoc = await _documentRepository.GetMulti(x => model.ListFileToDelete.Contains(x.Id));
                var serPath = System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.GetByKey("BatchPaymentDirectory"));
                foreach (var item in lstDeleteDoc)
                {
                    var filePath = serPath + item.Url;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        _documentRepository.Delete(item);
                    }
                }
            }

            if (files != null && files.Count > 0)
            {
                var serPath = System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.GetByKey("BatchPaymentDirectory"));
                string pathDirectory = serPath + "BatchId_" + batchPayment.Id;

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
                    string url = pathFile.Substring(pathFile.IndexOf("BatchId_" + batchPayment.Id));
                    file.SaveAs(pathFile);

                    batchPayment.Documents.Add(new Document()
                    {
                        Url = url,
                        ContentType = originalFileExtension,
                        FileName = originalFileName,
                        ModifiedDate = DateTime.UtcNow
                    });
                }
            }

            _batchPaymentRepository.Update(batchPayment);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = batchPayment.ToBatchPaymentViewModel();
            return result;
        }

        public async Task<ResultModel<BatchPaymentDetailViewModel>> GetBatchPaymentDetailById(int batchPaymentId)
        {
            var result = new ResultModel<BatchPaymentDetailViewModel>();
            var batchPaymentModel = await _batchPaymentRepository.GetSingleByCondition(x => x.Id == batchPaymentId, new string[] { "Documents" });

            var viewModel = new BatchPaymentDetailViewModel();
            viewModel.Id = batchPaymentModel.Id;
            viewModel.TypeOfPaymentId = batchPaymentModel.TransactionTypeId;
            viewModel.BankCodeBankNameId = batchPaymentModel.AcceptedBankId;
            viewModel.BatchPaymentDate = batchPaymentModel.PaymentDate;
            viewModel.BatchPaymentAmount = batchPaymentModel.BatchPaymentAmount;
            viewModel.ReferenceNumber = batchPaymentModel.RefNo;
            viewModel.Payee = batchPaymentModel.Payee;
            viewModel.Remarks = batchPaymentModel.Remarks;
            viewModel.IsSettled = batchPaymentModel.IsSettled;

            foreach (var item in batchPaymentModel.Documents)
            {
                var doc = new BatchPaymentFile();
                doc.Id = item.Id;
                doc.FileName = item.FileName;
                doc.Url = ConfigHelper.GetByKey("DMZAPI") + $"api/Proxy?url=ApplicationManagement/DownloadBatchPayment?docId={item.Id}&kindOfFile=BatchPaymentDirectory";

                viewModel.BatchPaymentFiles.Add(doc);
            }

            List<ApplicationForBatchPayment> listApplication = null;
            if (batchPaymentModel.ListApplication != null)
            {
                listApplication = new JavaScriptSerializer().Deserialize<List<ApplicationForBatchPayment>>(batchPaymentModel.ListApplication);
            }

            if (listApplication != null && listApplication.Count > 0)
            {
                foreach (var item in listApplication)
                {
                    var batchPaymentItem = _applicationRepository.GetBatchPaymentItem(item.UserId, item.IsChineseName, item.ApplicationId);
                    viewModel.TotalAmount += batchPaymentItem.ListCourseAvailable.FirstOrDefault(x => x.ApplicationId == batchPaymentItem.ApplicationIdSelected).CourseFee ?? 0;
                    viewModel.ListBatchPaymentItem.Add(batchPaymentItem);
                }
            }

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = viewModel;

            return result;
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadDocument(int docId, string kindOfFile)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var doc = _documentRepository.GetSingleById(docId);
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }

            var directory = ConfigHelper.GetByKey(kindOfFile);
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

        public async Task<ResultModel<object>> SettledAllBatchPayment(int batchPaymentId, int adminId)
        {
            var result = new ResultModel<object>();
            if (batchPaymentId == 0)
            {
                result.Message = "BatchPaymentId must different by 0";
                return result;
            }

            var batchPayment = await _batchPaymentRepository.GetSingleByCondition(x => x.Id == batchPaymentId, new string[] { "Documents" });

            if (batchPayment == null)
            {
                result.Message = "BatchPayment was not found";
                return result;
            }

            var listApplicationId = batchPayment.ListApplication == null ? null : new JavaScriptSerializer().Deserialize<List<ApplicationForBatchPayment>>(batchPayment.ListApplication).Select(x => x.ApplicationId);

            if (listApplicationId == null || listApplicationId.Count() == 0)
            {
                result.Message = "No application id for this batch payment";
                return result;
            }

            var listApplication = await _applicationRepository.GetMulti(x => listApplicationId.Contains(x.Id), new string[] { "Invoices.PaymentTransactions", "Invoices.InvoiceItems" });

            var listAcceptInvoiceStatus = new List<int>() { (int)Common.Enums.InvoiceStatus.Offered, (int)Common.Enums.InvoiceStatus.PaidPartially };

            var listApplicationIdNotOffered = listApplication.Where(x =>
            (!listAcceptInvoiceStatus.Contains(x.Invoices.LastOrDefault()?.Status ?? 0)) ||
            !(x.Invoices.LastOrDefault().InvoiceItems.Any(c => c.InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee)))
                .Select(v => v.Id);

            if (listApplicationIdNotOffered.Count() > 0)
            {
                result.Message = "Some Application is not Offered status anymore or not include CourseFee item";
                result.Data = listApplicationIdNotOffered;
                return result;
            }

            //TODO 
            decimal totalAmount = 0;
            foreach (var item in listApplication)
            {
                var lastInvoice = item.Invoices.LastOrDefault();

                var transaction = new PaymentTransaction();
                transaction.TransactionType = batchPayment.TransactionTypeId;
                transaction.AcceptedBankId = batchPayment.AcceptedBankId;
                transaction.RefNo = batchPayment.RefNo;
                transaction.PaymentDate = batchPayment.PaymentDate;
                transaction.Remarks = batchPayment.Remarks;

                if (lastInvoice.Status == (int)Common.Enums.InvoiceStatus.PaidPartially)
                {
                    decimal tempFee = 0;

                    foreach (var trans in lastInvoice.PaymentTransactions)
                    {
                        tempFee += trans.Amount;
                    }

                    transaction.Amount = lastInvoice.Fee - tempFee;
                    totalAmount += transaction.Amount;
                }
                else
                {
                    transaction.Amount = lastInvoice.Fee;
                    totalAmount += transaction.Amount;
                }

                foreach (var itemDoc in batchPayment.Documents)
                {
                    var doc = new Document()
                    {
                        Url = itemDoc.Url,
                        ContentType = itemDoc.ContentType,
                        FileName = itemDoc.FileName,
                        ModifiedDate = DateTime.UtcNow
                    };
                    transaction.PaymentTransactionDocuments.Add(new PaymentTransactionDocument()
                    {
                        Document = doc,
                        TypeOfDocument = (int)Common.Enums.TypeFileTransaction.CreateByBatchPayment
                    });
                }
                lastInvoice.Status = (int)Common.Enums.InvoiceStatus.SettledByBatch;
                lastInvoice.PaymentTransactions.Add(transaction);
                item.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                {
                    Status = (int)Common.Enums.EnrollmentStatus.Enrolled,
                    LastModifiedBy = adminId,
                    LastModifiedDate = DateTime.Now
                });

                var lessons = await _lessionRepository.GetMulti(x => x.ClassId == item.AdminAssignedClass);
                var exams = await _examRepository.GetMulti(x => x.ClassId == item.AdminAssignedClass);

                foreach (var les in lessons)
                {
                    item.LessonAttendances.Add(new LessonAttendance()
                    {
                        IsMakeUp = false,
                        IsTakeAttendance = true,
                        UserId = item.UserId,
                        LessonId = les.Id,
                        ApplicationId = item.Id
                    });
                }

                foreach (var ex in exams)
                {
                    item.ExamApplications.Add(new ExamApplication()
                    {
                        ApplicationId = item.Id,
                        AssessmentMark = null,
                        AssessmentResult = null,
                        ExamId = ex.Id,
                        IsMakeUp = false
                    });
                }

                _applicationRepository.Update(item);
            }

            if (batchPayment.BatchPaymentAmount != totalAmount)
            {
                result.Message = "The Batch Payment Amount (HKD) must equal with Total Amount to be Settled. Please check again.";
                return result;
            }

            batchPayment.IsSettled = true;
            _batchPaymentRepository.Update(batchPayment);

            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<IEnumerable<BankCodeBankNameViewModel>> GetBankCodeBankName()
        {
            var result = (await _acceptedBank.GetMulti(x => x.Id != (int)Common.Enums.AcceptedBank.None)).Select(c => new BankCodeBankNameViewModel()
            {
                Id = c.Id,
                NameEN = c.NameEN,
                NameCN = c.NameCN,
                NameHK = c.NameHK,
            });

            return result;
        }

        public async Task<AssessmentTableViewModel> GetAssessmentListByClassId(int classId)
        {
            var lstAssessment = (await _applicationRepository.GetMulti(x => x.Status == (int)SPDC.Common.Enums.ApplicationStatus.Assigned && x.AdminAssignedClass == classId,
                new string[] { "AdminAssignedClassModel.Exams", "User.Particular", "LessonAttendances.Lesson", "MakeUpAttendences",
            "EnrollmentStatusStorages", "ApplicationAssessmentDocuments.Document", "Invoices.InvoiceItems","Course"}));

            var result = lstAssessment.ToAssessmentRecordViewModel();

            return result;
        }

        public async Task<ResultModel<bool>> UpdateAssessment(List<AssessmentRecordBindingModel> lstModel, HttpFileCollection files, int userId)
        {
            var result = new ResultModel<bool>();

            var listApplicationId = lstModel.Select(x => x.Id);
            var listApplication = await _applicationRepository.GetMulti(x => listApplicationId.Contains(x.Id), new string[] { "ExamApplications.Exam", "EnrollmentStatusStorages",
                "ApplicationAssessmentDocuments.Document", "EnrollmentStatusStorages" });

            var folder = ConfigHelper.GetByKey("ApplicationDocumentDirectory") + ApplicationTypeDocument.AssessmentDocument.ToString();
            var serPath = System.Web.HttpContext.Current.Server.MapPath(folder);

            foreach (var item in listApplication)
            {
                var assessment = lstModel.FirstOrDefault(x => x.Id == item.Id);

                item.EligibleForAttendanceCertification = assessment.EligibleForAttendance;
                item.AttendanceCertificateIssueDate = item.EligibleForAttendanceCertification ? assessment.AttendanceCertificateIssueDate : null;
                var examApplicationResult = item.ExamApplications.FirstOrDefault(x => x.Exam.Type == (int)SPDC.Common.Enums.ExamType.Exam);
                if (examApplicationResult != null)
                {
                    examApplicationResult.AssessmentMark = assessment.ExamAssessmentMarks;
                    examApplicationResult.AssessmentResult = assessment.ExamAssessmentResult;

                }
                var examApplicationFirstReExam = item.ExamApplications.FirstOrDefault(x => x.Exam.Type == (int)SPDC.Common.Enums.ExamType.FirstReExam);
                if (examApplicationFirstReExam != null)
                {
                    examApplicationFirstReExam.AssessmentMark = assessment.ExamAssessmentMarks;
                    examApplicationFirstReExam.AssessmentResult = assessment.ExamAssessmentResult;
                }
                var examApplicationSecondReExam = item.ExamApplications.FirstOrDefault(x => x.Exam.Type == (int)SPDC.Common.Enums.ExamType.SecondReExam);
                if (examApplicationSecondReExam != null)
                {
                    examApplicationSecondReExam.AssessmentMark = assessment.ExamAssessmentMarks;
                    examApplicationSecondReExam.AssessmentResult = assessment.ExamAssessmentResult;
                }

                #region Change Enrollment Status
                if (assessment.EnrollmentStatus.HasValue)
                {
                    if (!(assessment.EnrollmentStatus == (int)Common.Enums.EnrollmentStatus.Enrolled
                        || assessment.EnrollmentStatus == (int)Common.Enums.EnrollmentStatus.Completed))
                    {
                        item.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                        {
                            Status = (int)assessment.EnrollmentStatus,
                            LastModifiedDate = DateTime.UtcNow,
                            LastModifiedBy = userId
                        });
                    }
                }
                #endregion


                item.EligibleForCourseCompletionCertification = assessment.EligibleForCourseCompletion;
                item.CourseCompletionCertificateIssueDate = item.EligibleForCourseCompletionCertification ? assessment.CourseCompletionCertificateIssueDate : null;
                item.CreditAcquired = assessment.CreditAcquired;
                item.RemarksExam = assessment.Remarks;
                item.EligibleForResitExam = assessment.EligibleForResitExam;

                var listFile = files.GetMultiple(item.Id.ToString());
                if (listFile != null)
                {
                    foreach (var file in listFile)
                    {
                        var path = serPath + "\\applicationId";
                        if (!Directory.Exists(path))
                        {
                            Common.Common.CreateDirectoryAndGrantFullControlPermission(path);
                        }

                        var pathFile = Common.Common.GenFileNameDuplicate(path + "\\" + file.FileName);
                        file.SaveAs(pathFile);
                        var fileNameToSaveDb = Path.GetFileName(pathFile);

                        item.ApplicationAssessmentDocuments.Add(new ApplicationAssessmentDocument()
                        {
                            Document = new Document()
                            {
                                Url = pathFile,
                                ContentType = file.ContentType,
                                FileName = fileNameToSaveDb
                            }
                        });
                    }
                }

                if (item.ExamApplications.Any(x => x.AssessmentResult == (int)ExamResult.Pass))
                {
                    item.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                    {
                        Status = (int)Common.Enums.EnrollmentStatus.Graduated,
                        LastModifiedDate = DateTime.UtcNow,
                        LastModifiedBy = userId
                    });
                }
                else if (item.ExamApplications.All(x => x.AssessmentResult == (int)ExamResult.Absent || x.AssessmentResult == (int)ExamResult.Failed))
                {
                    item.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                    {
                        Status = (int)Common.Enums.EnrollmentStatus.Failed,
                        LastModifiedDate = DateTime.UtcNow,
                        LastModifiedBy = userId
                    });
                }


                _applicationRepository.Update(item);

            }
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> SendIneligibleForAttendanceCertificateEmail(List<int> lstApplicationId, LanguageCode lang)
        {
            var result = new ResultModel<bool>();

            var listApplication = await _applicationRepository.GetMulti(x => lstApplicationId.Contains(x.Id), new string[] { "Course.CourseTrans", "AdminAssignedClassModel",
                "User.UserDevices", "User.Particular" });
            if (listApplication == null)
            {
                result.Message = "No application was found";
                return result;
            }

            var applicationInEligible = listApplication.Where(x => x.EligibleForAttendanceCertification);
            if (applicationInEligible.Count() > 0)
            {
                result.Message = "Wrong selected item. Please check and try again.";
                return result;
            }

            var courseName = listApplication[0].Course.CourseTrans.FirstOrDefault(x => x.LanguageId == (int)lang).CourseName;
            var classCode = listApplication[0].AdminAssignedClassModel.ClassCode;
            var itemName = lang == LanguageCode.EN ? "Item19EN.cshtml" : "Item19TC.cshtml";
            var subject = lang == LanguageCode.EN ? "Insufficient Attendance Notification (" + classCode + ")" : "出席率不足 (" + classCode + ")";

            var dateSendEmail = DateTime.Now.ToShortDateString();

            foreach (var e in applicationInEligible)
            {
                if (e.User.UserDevices != null && e.User.UserDevices.Count() > 0)
                {
                    string notificationTitle = FileHelper.GetNotificationTitle("item19", "NotificationTitles", e.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                    Notification notification = new Notification()
                    {
                        Body = "",
                        DataId = e.Id,
                        Title = notificationTitle,
                        Type = (int)NotificationType.Application
                    };

                    foreach (var device in e.User.UserDevices)
                    {
                        NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                    }

                    notification.NotificationUsers.Add(new NotificationUser()
                    {
                        CreatedDate = DateTime.UtcNow,
                        IsFavourite = false,
                        IsRead = false,
                        IsRemove = false,
                        UserId = e.User.Id
                    });
                    _notificationRepository.Add(notification);
                }

                var displayName = lang == LanguageCode.EN ? (e.User.Particular.SurnameEN + " " + e.User.Particular.GivenNameEN) :
                    (e.User.Particular.SurnameCN + " " + e.User.Particular.GivenNameCN);
                var contentEmail = Common.Common.GenerateItem19Email(classCode, displayName, courseName, itemName);

                e.EmailStatus = string.IsNullOrEmpty(e.EmailStatus) ? "Ineligible for Attendance Certificate Email sent on " + dateSendEmail
                  : e.EmailStatus + ", Ineligible for Attendance Certificate Email sent on " + dateSendEmail;
                _applicationRepository.Update(e);

                var isSuccess = MailHelper.SendMail(e.User.Email, subject, contentEmail);
                if (isSuccess)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
            }

            _unitOfWork.Commit();
            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> SendCollectAttendanceOrCourseCompletionEmail(List<int> lstApplicationId, LanguageCode lang, TypeAssessmentEmail typeEmail)
        {
            var result = new ResultModel<bool>();

            var listApplication = await _applicationRepository.GetMulti(x => lstApplicationId.Contains(x.Id), new string[] { "Course.CourseTrans", "AdminAssignedClassModel",
                "User.UserDevices", "User.Particular", "Course.Enquiries", "Course.CourseTrans" });
            if (listApplication == null)
            {
                result.Message = "No application was found";
                return result;
            }

            var applicationEligible = listApplication.Where(x => !x.EligibleForAttendanceCertification);
            var courseCompletionEligible = listApplication.Where(x => !x.EligibleForCourseCompletionCertification);

            if (typeEmail == TypeAssessmentEmail.CollectAttendance && applicationEligible.Count() > 0)
            {
                result.Message = "Wrong selected item. Please check and try again.";
                return result;
            }

            if (typeEmail == TypeAssessmentEmail.CollectCourseCompletion && courseCompletionEligible.Count() > 0)
            {
                result.Message = "Wrong selected item. Please check and try again.";
                return result;
            }

            var courseName = listApplication[0].Course.CourseTrans.FirstOrDefault(x => x.LanguageId == (int)lang).CourseName;
            var classCode = listApplication[0].AdminAssignedClassModel.ClassCode;
            var itemName = lang == LanguageCode.EN ? "Item22EN.cshtml" : "Item22TC.cshtml";
            var subject = lang == LanguageCode.EN ? "Insufficient Attendance Notification (" + classCode + ")" : "出席率不足 (" + classCode + ")";

            var dateSendEmail = DateTime.Now;
            var server = System.Web.HttpContext.Current.Server;

            foreach (var e in applicationEligible)
            {
                if (e.User.UserDevices != null && e.User.UserDevices.Count() > 0)
                {
                    string notificationTitle = FileHelper.GetNotificationTitle("item22", "NotificationTitles", e.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                    Notification notification = new Notification()
                    {
                        Body = "",
                        DataId = e.Id,
                        Title = notificationTitle,
                        Type = (int)NotificationType.Application
                    };

                    foreach (var device in e.User.UserDevices)
                    {
                        NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                    }

                    notification.NotificationUsers.Add(new NotificationUser()
                    {
                        CreatedDate = DateTime.UtcNow,
                        IsFavourite = false,
                        IsRead = false,
                        IsRemove = false,
                        UserId = e.User.Id
                    });
                    _notificationRepository.Add(notification);
                }

                var displayName = lang == LanguageCode.EN ? (e.User.Particular.SurnameEN + " " + e.User.Particular.GivenNameEN) :
                    (e.User.Particular.SurnameCN + " " + e.User.Particular.GivenNameCN);

                var contentEmail = Common.Common.GenerateItem22Email(classCode, displayName, courseName, itemName, server);

                if (typeEmail == TypeAssessmentEmail.CollectAttendance)
                {
                    e.EmailStatus = string.IsNullOrEmpty(e.EmailStatus) ? "Collect Attendance Certification Email sent on " + dateSendEmail.ToShortDateString()
                      : e.EmailStatus + ", Collect Attendance Certification Email sent on " + dateSendEmail.ToShortDateString();
                }
                else if (typeEmail == TypeAssessmentEmail.CollectCourseCompletion)
                {
                    e.EmailStatus = string.IsNullOrEmpty(e.EmailStatus) ? "Collect Course Completion Certification Email sent on " + dateSendEmail.ToShortDateString()
                      : e.EmailStatus + ", Collect Course Completion Certification Email sent on " + dateSendEmail.ToShortDateString();
                }

                _applicationRepository.Update(e);

                #region Attachment
                var attachmentData = new List<KeyValueModel>() {
                // (0001) in M/CT/TSRC/S/XXXX (0001) is a serial number XXXX is the Course Code => Model.Ref  TODO: Check serial number
                new KeyValueModel{Key="Model.Ref",Value="(0001) in M/CT/TSRC/S/" + e.Course.CourseCode },
                new KeyValueModel{Key="Model.EmailSentYear",Value=dateSendEmail.Year.ToString()},
                new KeyValueModel{Key="Model.EmailSentMonth",Value=dateSendEmail.Month.ToString()},
                new KeyValueModel{Key="Model.EmailSentDay",Value=dateSendEmail.Day.ToString()},
                new KeyValueModel{Key="Model.CourseName",Value=e.Course.CourseTrans.FirstOrDefault(x=>x.LanguageId == (int)LanguageCode.CN).CourseName},
                new KeyValueModel{Key="Model.EnquiryFirstPhone",Value=e.Course.Enquiries.FirstOrDefault()?.Phone},
                new KeyValueModel{Key="Model.EnquiryFirstName",Value=e.Course.Enquiries.FirstOrDefault()?.ContactPersonCN},
                };

                var directory = ConfigHelper.GetByKey("ApplicationDocumentDirectory");
                var serPath = server.MapPath(directory);
                var pathDirectory = serPath + ApplicationTypeDocument.AssessmentDocument.ToString() + "\\" + e.Id + "_" + e.ApplicationNumber;

                string path = Common.Common.GenerateItem22Attachment(pathDirectory, attachmentData, server);
                #endregion

                bool isSuccess = MailHelper.SendMail(e.User.Email, subject, contentEmail, path: path);
                if (isSuccess)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
            }

            //Parallel.ForEach(applicationEligible, async e =>
            // {
            // });

            _unitOfWork.Commit();
            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> SendExamResult(List<int> lstApplicationId, LanguageCode lang, int examType)
        {
            var result = new ResultModel<bool>();

            var listApplication = await _applicationRepository.GetMulti(x => lstApplicationId.Contains(x.Id), new string[] { "Course.CourseTrans", "AdminAssignedClassModel",
                "User.UserDevices", "User.Particular", "ExamApplications.Exam", "Course.Enquiries" });
            if (listApplication == null)
            {
                result.Message = "No application was found";
                return result;
            }

            var applicationUnCorrespondType = listApplication.Where(x => (!x.ExamApplications?.Any(c => c.Exam.Type == examType)) ?? true);
            if (applicationUnCorrespondType.Count() > 0)
            {
                result.Message = "Wrong selected item. Please check and try again.";
                return result;
            }

            var courseName = listApplication[0].Course.CourseTrans.FirstOrDefault(x => x.LanguageId == (int)lang).CourseName;
            var classCode = listApplication[0].AdminAssignedClassModel.ClassCode;
            var itemName = lang == LanguageCode.EN ? "Item23EN.cshtml" : "Item23TC.cshtml";
            var subject = lang == LanguageCode.EN ? "Notification on Examination Result (" + classCode + ")" : "出席率不足 (" + classCode + ")";

            var dateSendEmail = DateTime.Now;

            foreach (var e in applicationUnCorrespondType)
            {
                if (e.User.UserDevices != null && e.User.UserDevices.Count() > 0)
                {
                    string notificationTitle = FileHelper.GetNotificationTitle("item23", "NotificationTitles", e.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                    Notification notification = new Notification()
                    {
                        Body = "",
                        DataId = e.Id,
                        Title = notificationTitle,
                        Type = (int)NotificationType.Application
                    };

                    foreach (var device in e.User.UserDevices)
                    {
                        NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                    }

                    notification.NotificationUsers.Add(new NotificationUser()
                    {
                        CreatedDate = DateTime.UtcNow,
                        IsFavourite = false,
                        IsRead = false,
                        IsRemove = false,
                        UserId = e.User.Id
                    });
                    _notificationRepository.Add(notification);
                }

                var displayName = lang == LanguageCode.EN ? (e.User.Particular.SurnameEN + " " + e.User.Particular.GivenNameEN) :
                    (e.User.Particular.SurnameCN + " " + e.User.Particular.GivenNameCN);
                var contentEmail = Common.Common.GenerateItem23Email(classCode, displayName, courseName, itemName);

                e.EmailStatus = string.IsNullOrEmpty(e.EmailStatus) ? "Exam Result Email sent on " + dateSendEmail.ToShortDateString()
                  : e.EmailStatus + ", Exam Result Email sent on " + dateSendEmail.ToShortDateString();
                _applicationRepository.Update(e);

                // Attachment
                #region Attachment
                var examApplication = e.ExamApplications.FirstOrDefault(x => x.AssessmentResult == examType);
                var isPass = examApplication.AssessmentResult == (int)ExamResult.Pass;
                var isExam = examApplication.Exam.Type == (int)ExamType.Exam;
                List<KeyValueModel> attachmentData = null;
                if (examApplication != null && isPass && isExam)
                {
                    var honorific = e.User.Particular.Honorific == 1 ? Honorific.Mr.ToString() : (e.User.Particular.Honorific == 2 ? Honorific.Mrs.ToString()
                      : (e.User.Particular.Honorific == 3 ? Honorific.Ms.ToString() : (e.User.Particular.Honorific == 4 ? Honorific.Dr.ToString() : Honorific.Ir.ToString())));

                    string check = "☒";
                    string pass = "☐";
                    string fail = "☐";
                    string absent = "☐";

                    if (examApplication.AssessmentResult == (int)ExamResult.Pass)
                    {
                        pass = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Failed)
                    {
                        fail = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Absent)
                    {
                        absent = check;
                    }

                    attachmentData = new List<KeyValueModel>() {
                    // (0001) in M/CT/TSRC/S/XXXX (0001) is a serial number XXXX is the Course Code => Model.Ref  TODO: Check serial number
                    new KeyValueModel{Key="Model.Ref",Value="(0001) in M/CT/TSRC/S/" + e.Course.CourseCode },
                    new KeyValueModel{Key="Model.CourseName",Value=e.Course.CourseTrans.FirstOrDefault(x=>x.LanguageId == (int)LanguageCode.CN).CourseName},
                    new KeyValueModel{Key="Model.ExamYear",Value=examApplication.Exam.Date.Year.ToString()},
                    new KeyValueModel{Key="Model.ExamMonth",Value=examApplication.Exam.Date.Month.ToString()},
                    new KeyValueModel{Key="Model.ExamDay",Value=examApplication.Exam.Date.Day.ToString()},
                    new KeyValueModel{Key="Model.Pass",Value=pass},
                    new KeyValueModel{Key="Model.Fail",Value=fail},
                    new KeyValueModel{Key="Model.Absent",Value=absent},
                    new KeyValueModel{Key="Model.EnquiryFirstPhone",Value=e.Course.Enquiries.FirstOrDefault()?.Phone},
                    new KeyValueModel{Key="Model.EnquiryFirstEmail",Value=e.Course.Enquiries.FirstOrDefault()?.Email},
                    new KeyValueModel{Key="Model.EnquiryFirstName",Value=e.Course.Enquiries.FirstOrDefault()?.ContactPersonCN},
                    new KeyValueModel{Key="Model.StudentHonorific",Value= honorific},
                    new KeyValueModel{Key="Model.StudentSurNameEnglish",Value=e.User.Particular.SurnameEN},
                    new KeyValueModel{Key="Model.StudentGivenNameEnglish",Value=e.User.Particular.GivenNameEN},
                    new KeyValueModel{Key="Model.StudentSurNameChinese",Value=e.User.Particular.SurnameCN},
                    new KeyValueModel{Key="Model.StudentGivenNameChinese",Value=e.User.Particular.GivenNameCN},
                    new KeyValueModel{Key="Model.StudentApplicationNumber",Value=e.ApplicationNumber},
                    new KeyValueModel{Key="Model.EmailSentYear",Value=dateSendEmail.Year.ToString()},
                    new KeyValueModel{Key="Model.EmailSentMonth",Value=dateSendEmail.Month.ToString()},
                    new KeyValueModel{Key="Model.EmailSentDay",Value=dateSendEmail.Day.ToString()},
                    };
                }
                else if (examApplication != null && !isPass && isExam)
                {
                    var honorific = e.User.Particular.Honorific == 1 ? Honorific.Mr.ToString() : (e.User.Particular.Honorific == 2 ? Honorific.Mrs.ToString()
                      : (e.User.Particular.Honorific == 3 ? Honorific.Ms.ToString() : (e.User.Particular.Honorific == 4 ? Honorific.Dr.ToString() : Honorific.Ir.ToString())));

                    string check = "☒";
                    string pass = "☐";
                    string fail = "☐";
                    string absent = "☐";

                    if (examApplication.AssessmentResult == (int)ExamResult.Pass)
                    {
                        pass = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Failed)
                    {
                        fail = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Absent)
                    {
                        absent = check;
                    }

                    attachmentData = new List<KeyValueModel>() {
                    // (0001) in M/CT/TSRC/S/XXXX (0001) is a serial number XXXX is the Course Code => Model.Ref  TODO: Check serial number
                    new KeyValueModel{Key="Model.Ref",Value="(0001) in M/CT/TSRC/S/" + e.Course.CourseCode },
                    new KeyValueModel{Key="Model.CourseName",Value=e.Course.CourseTrans.FirstOrDefault(x=>x.LanguageId == (int)LanguageCode.CN).CourseName},
                    new KeyValueModel{Key="Model.ExamYear",Value=examApplication.Exam.Date.Year.ToString()},
                    new KeyValueModel{Key="Model.ExamMonth",Value=examApplication.Exam.Date.Month.ToString()},
                    new KeyValueModel{Key="Model.ExamDay",Value=examApplication.Exam.Date.Day.ToString()},
                    new KeyValueModel{Key="Model.Pass",Value="☒"},
                    new KeyValueModel{Key="Model.Fail",Value="☐"},
                    new KeyValueModel{Key="Model.Absent",Value="☐"},
                    new KeyValueModel{Key="Model.EnquiryFirstPhone",Value=e.Course.Enquiries.FirstOrDefault()?.Phone},
                    new KeyValueModel{Key="Model.EnquiryFirstEmail",Value=e.Course.Enquiries.FirstOrDefault()?.Email},
                    new KeyValueModel{Key="Model.EnquiryFirstName",Value=e.Course.Enquiries.FirstOrDefault()?.ContactPersonCN},
                    new KeyValueModel{Key="Model.StudentHonorific",Value= honorific},
                    new KeyValueModel{Key="Model.StudentSurNameEnglish",Value=e.User.Particular.SurnameEN},
                    new KeyValueModel{Key="Model.StudentGivenNameEnglish",Value=e.User.Particular.GivenNameEN},
                    new KeyValueModel{Key="Model.StudentSurNameChinese",Value=e.User.Particular.SurnameCN},
                    new KeyValueModel{Key="Model.StudentGivenNameChinese",Value=e.User.Particular.GivenNameCN},
                    new KeyValueModel{Key="Model.StudentApplicationNumber",Value=e.ApplicationNumber},
                    new KeyValueModel{Key="Model.EmailSentYear",Value=dateSendEmail.Year.ToString()},
                    new KeyValueModel{Key="Model.EmailSentMonth",Value=dateSendEmail.Month.ToString()},
                    new KeyValueModel{Key="Model.EmailSentDay",Value=dateSendEmail.Day.ToString()},
                    };
                }
                else if (examApplication != null && isPass && !isExam)
                {
                    var honorific = e.User.Particular.Honorific == 1 ? Honorific.Mr.GetStringValue() : (e.User.Particular.Honorific == 2 ? Honorific.Mrs.GetStringValue()
                      : (e.User.Particular.Honorific == 3 ? Honorific.Ms.GetStringValue() : (e.User.Particular.Honorific == 4 ? Honorific.Dr.GetStringValue() : Honorific.Ir.GetStringValue())));

                    string check = "☒";
                    string pass = "☐";
                    string fail = "☐";
                    string absent = "☐";

                    if (examApplication.AssessmentResult == (int)ExamResult.Pass)
                    {
                        pass = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Failed)
                    {
                        fail = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Absent)
                    {
                        absent = check;
                    }

                    attachmentData = new List<KeyValueModel>() {
                    // (0001) in M/CT/TSRC/S/XXXX (0001) is a serial number XXXX is the Course Code => Model.Ref  TODO: Check serial number
                    new KeyValueModel{Key="Model.Ref",Value="(0001) in M/CT/TSRC/S/" + e.Course.CourseCode },
                    new KeyValueModel{Key="Model.CourseName",Value=e.Course.CourseTrans.FirstOrDefault(x=>x.LanguageId == (int)LanguageCode.CN).CourseName},
                    new KeyValueModel{Key="Model.ExamYear",Value=examApplication.Exam.Date.Year.ToString()},
                    new KeyValueModel{Key="Model.ExamMonth",Value=examApplication.Exam.Date.Month.ToString()},
                    new KeyValueModel{Key="Model.ExamDay",Value=examApplication.Exam.Date.Day.ToString()},
                    new KeyValueModel{Key="Model.Pass",Value="☒"},
                    new KeyValueModel{Key="Model.Fail",Value="☐"},
                    new KeyValueModel{Key="Model.Absent",Value="☐"},
                    new KeyValueModel{Key="Model.EnquiryFirstPhone",Value=e.Course.Enquiries.FirstOrDefault()?.Phone},
                    new KeyValueModel{Key="Model.EnquiryFirstEmail",Value=e.Course.Enquiries.FirstOrDefault()?.Email},
                    new KeyValueModel{Key="Model.EnquiryFirstName",Value=e.Course.Enquiries.FirstOrDefault()?.ContactPersonCN},
                    new KeyValueModel{Key="Model.StudentHonorific",Value= honorific},
                    new KeyValueModel{Key="Model.StudentSurNameEnglish",Value=e.User.Particular.SurnameEN},
                    new KeyValueModel{Key="Model.StudentGivenNameEnglish",Value=e.User.Particular.GivenNameEN},
                    new KeyValueModel{Key="Model.StudentSurNameChinese",Value=e.User.Particular.SurnameCN},
                    new KeyValueModel{Key="Model.StudentGivenNameChinese",Value=e.User.Particular.GivenNameCN},
                    new KeyValueModel{Key="Model.StudentApplicationNumber",Value=e.ApplicationNumber},
                    new KeyValueModel{Key="Model.EmailSentYear",Value=dateSendEmail.Year.ToString()},
                    new KeyValueModel{Key="Model.EmailSentMonth",Value=dateSendEmail.Month.ToString()},
                    new KeyValueModel{Key="Model.EmailSentDay",Value=dateSendEmail.Day.ToString()},
                    };
                }
                else if (examApplication != null && !isPass && !isExam)
                {
                    var honorific = e.User.Particular.Honorific == 1 ? Honorific.Mr.GetStringValue() : (e.User.Particular.Honorific == 2 ? Honorific.Mrs.GetStringValue()
                      : (e.User.Particular.Honorific == 3 ? Honorific.Ms.GetStringValue() : (e.User.Particular.Honorific == 4 ? Honorific.Dr.GetStringValue() : Honorific.Ir.GetStringValue())));

                    string check = "☒";
                    string pass = "☐";
                    string fail = "☐";
                    string absent = "☐";

                    if (examApplication.AssessmentResult == (int)ExamResult.Pass)
                    {
                        pass = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Failed)
                    {
                        fail = check;
                    }
                    else if (examApplication.AssessmentResult == (int)ExamResult.Absent)
                    {
                        absent = check;
                    }

                    attachmentData = new List<KeyValueModel>() {
                    // (0001) in M/CT/TSRC/S/XXXX (0001) is a serial number XXXX is the Course Code => Model.Ref  TODO: Check serial number
                    new KeyValueModel{Key="Model.Ref",Value="(0001) in M/CT/TSRC/S/" + e.Course.CourseCode },
                    new KeyValueModel{Key="Model.CourseName",Value=e.Course.CourseTrans.FirstOrDefault(x=>x.LanguageId == (int)LanguageCode.CN).CourseName},
                    new KeyValueModel{Key="Model.ExamYear",Value=examApplication.Exam.Date.Year.ToString()},
                    new KeyValueModel{Key="Model.ExamMonth",Value=examApplication.Exam.Date.Month.ToString()},
                    new KeyValueModel{Key="Model.ExamDay",Value=examApplication.Exam.Date.Day.ToString()},
                    new KeyValueModel{Key="Model.Pass",Value="☒"},
                    new KeyValueModel{Key="Model.Fail",Value="☐"},
                    new KeyValueModel{Key="Model.Absent",Value="☐"},
                    new KeyValueModel{Key="Model.EnquiryFirstPhone",Value=e.Course.Enquiries.FirstOrDefault()?.Phone},
                    new KeyValueModel{Key="Model.EnquiryFirstEmail",Value=e.Course.Enquiries.FirstOrDefault()?.Email},
                    new KeyValueModel{Key="Model.EnquiryFirstName",Value=e.Course.Enquiries.FirstOrDefault()?.ContactPersonCN},
                    new KeyValueModel{Key="Model.StudentHonorific",Value= honorific},
                    new KeyValueModel{Key="Model.StudentSurNameEnglish",Value=e.User.Particular.SurnameEN},
                    new KeyValueModel{Key="Model.StudentGivenNameEnglish",Value=e.User.Particular.GivenNameEN},
                    new KeyValueModel{Key="Model.StudentSurNameChinese",Value=e.User.Particular.SurnameCN},
                    new KeyValueModel{Key="Model.StudentGivenNameChinese",Value=e.User.Particular.GivenNameCN},
                    new KeyValueModel{Key="Model.StudentApplicationNumber",Value=e.ApplicationNumber},
                    new KeyValueModel{Key="Model.EmailSentYear",Value=dateSendEmail.Year.ToString()},
                    new KeyValueModel{Key="Model.EmailSentMonth",Value=dateSendEmail.Month.ToString()},
                    new KeyValueModel{Key="Model.EmailSentDay",Value=dateSendEmail.Day.ToString()},
                    };
                }

                var directory = ConfigHelper.GetByKey("ApplicationDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var pathDirectory = serPath + ApplicationTypeDocument.AssessmentDocument.ToString() + "\\" + e.Id + "_" + e.ApplicationNumber;

                string path = Common.Common.GenerateItem23Attachment(pathDirectory, attachmentData, isPass, isExam);
                #endregion

                bool isSuccess = MailHelper.SendMail(e.User.Email, subject, contentEmail, path: path);
                if (isSuccess)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
            }

            _unitOfWork.Commit();
            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> SendReExamDetail(IEnumerable<int> lstApplicationId, LanguageCode lang, int examType)
        {
            var result = new ResultModel<bool>();

            var listApplication = await _applicationRepository.GetMulti(x => lstApplicationId.Contains(x.Id), new string[] { "Course.CourseTrans", "Course.Enquiries",
                "User.UserDevices", "User.Particular", "AdminAssignedClassModel","ExamApplications.Exam.Class.ClassCommon", "ExamApplications.FromExam.Class.ClassCommon" });
            if (listApplication == null)
            {
                result.Message = "No application was found";
                return result;
            }

            var countReExam = listApplication[0].AdminAssignedClassModel.CountReExam;
            if (countReExam == null || countReExam == 0)
            {
                result.Message = "This class has no Re-Exam";
                return result;
            }
            if (examType == (int)ExamType.FirstReExam && countReExam < 1)
            {
                result.Message = "This class has no Re-Exam";
                return result;
            }
            if (examType == (int)ExamType.SecondReExam && countReExam < 2)
            {
                result.Message = "This class has no second Re-Exam";
                return result;
            }



            //if(exam)

            //var examTimeUnit = examDetail.FromTime.Split(',')[0];
            var courseName = listApplication[0].Course.CourseTrans.FirstOrDefault(x => x.LanguageId == (int)lang).CourseName;
            var itemName = lang == LanguageCode.EN ? "Item24EN.cshtml" : "Item24TC.cshtml";
            var subject = lang == LanguageCode.EN ? "Notification on Resit Examination Arrangement" : "補考通知書";

            var dateSendEmail = DateTime.Now;

            var server = System.Web.HttpContext.Current.Server;

            foreach (var e in listApplication)
            {
                if (e.User.UserDevices != null && e.User.UserDevices.Count() > 0)
                {
                    string notificationTitle = FileHelper.GetNotificationTitle("item24", "NotificationTitles", e.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                    Notification notification = new Notification()
                    {
                        Body = "",
                        DataId = e.Id,
                        Title = notificationTitle,
                        Type = (int)NotificationType.Application
                    };

                    foreach (var device in e.User.UserDevices)
                    {
                        NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                    }

                    notification.NotificationUsers.Add(new NotificationUser()
                    {
                        CreatedDate = DateTime.UtcNow,
                        IsFavourite = false,
                        IsRead = false,
                        IsRemove = false,
                        UserId = e.User.Id
                    });
                    _notificationRepository.Add(notification);
                }

                var displayName = lang == LanguageCode.EN ? (e.User.Particular.SurnameEN + " " + e.User.Particular.GivenNameEN) :
                    (e.User.Particular.SurnameCN + " " + e.User.Particular.GivenNameCN);
                var contentEmail = Common.Common.GenerateItem24Email(displayName, courseName, itemName);

                e.EmailStatus = string.IsNullOrEmpty(e.EmailStatus) ? "Re-Exam Details Email sent on " + dateSendEmail.ToShortDateString()
                  : e.EmailStatus + ", Re-Exam Details Email sent on " + dateSendEmail.ToShortDateString();
                _applicationRepository.Update(e);

                #region Attachment
                Exam reExamDetail = null;
                string reExamHourDateUnit = string.Empty;
                #region Get ReExam Detail
                var examApplicationDetail = e.ExamApplications.FirstOrDefault(x => (!x.IsMakeUp && x.Exam.Type == examType) || (x.IsMakeUp && x.FromExam.Type == examType));
                if (examApplicationDetail == null)
                {
                    result.Message = "Application: " + e.Id + " not found the exam";
                    return result;
                }

                if (!examApplicationDetail.IsMakeUp)
                {
                    reExamDetail = examApplicationDetail.Exam;
                }
                else
                {
                    reExamDetail = examApplicationDetail.FromExam;
                }

                if (reExamDetail == null)
                {
                    result.Message = "Application: " + e.Id + " not found the exam";
                    return result;
                }

                #endregion

                int reExamInt = 0;
                var isParse = int.TryParse(reExamDetail.FromTime.Split(':')[0], out reExamInt);
                if (reExamInt > 12)
                {
                    reExamHourDateUnit = "晚上";
                }
                else
                {
                    reExamHourDateUnit = "早上";
                }
                var passingMark = reExamDetail.Class.ExamPassingMask.HasValue ? reExamDetail.Class.ExamPassingMask.ToString() : null;
                var reExamFromTime = int.Parse(reExamDetail.FromTime.Split(':')[0]) * 60 + int.Parse(reExamDetail.FromTime.Split(':')[1]);
                var reExamToTime = int.Parse(reExamDetail.ToTime.Split(':')[0]) * 60 + int.Parse(reExamDetail.ToTime.Split(':')[1]);
                double totalTime = Math.Round((double)(reExamToTime - reExamFromTime) / 60, 1);
                var honorific = ((Honorific)(e.User.Particular.Honorific)).GetStringValue();
                var studentNameCN = e.User.Particular.SurnameCN + e.User.Particular.GivenNameCN + honorific;

                var attachmentData = new List<KeyValueModel>() {
                // (0001) in M/CT/TSRC/S/XXXX (0001) is a serial number XXXX is the Course Code => Model.Ref  TODO: Check serial number
                new KeyValueModel{Key="Model.Ref",Value="(0001) in M/CT/TSRC/S/" + e.Course.CourseCode },
                new KeyValueModel{Key="Model.CourseName",Value=e.Course.CourseTrans.FirstOrDefault(x=>x.LanguageId == (int)LanguageCode.CN).CourseName},
                new KeyValueModel{Key="Model.ReExamVenue",Value=reExamDetail.ExamVenue},
                new KeyValueModel{Key="Model.ReExamDay",Value=reExamDetail.Date.Day.ToString()},
                new KeyValueModel{Key="Model.ReExamMonth",Value=reExamDetail.Date.Month.ToString()},
                new KeyValueModel{Key="Model.ReExamYear",Value=reExamDetail.Date.Year.ToString()},
                new KeyValueModel{Key="Model.AMPM",Value=reExamHourDateUnit},
                new KeyValueModel{Key="Model.DateOfWeek",Value=reExamDetail.Date.DayOfWeek.ToString()},
                new KeyValueModel{Key="Model.PassRQ",Value=passingMark},
                new KeyValueModel{Key="Model.PassUnitRQ",Value=reExamDetail.Class.ClassCommon.TypeName},
                new KeyValueModel{Key="Model.TimeFrom",Value=reExamDetail.FromTime},
                new KeyValueModel{Key="Model.TimeTo",Value=reExamDetail.ToTime},
                new KeyValueModel{Key="Model.TotalTime",Value=totalTime.ToString()},
                new KeyValueModel{Key="Model.EnquiryFirstPhone",Value=e.Course.Enquiries.FirstOrDefault()?.Phone},
                new KeyValueModel{Key="Model.EnquiryFirstEmail",Value=e.Course.Enquiries.FirstOrDefault()?.Email},
                new KeyValueModel{Key="Model.EnquiryFirstName",Value=e.Course.Enquiries.FirstOrDefault()?.ContactPersonCN},
                new KeyValueModel{Key="Model.StNameCn",Value=studentNameCN},
                new KeyValueModel{Key="Model.ApplicationNumber",Value=e.ApplicationNumber},
                new KeyValueModel{Key="Model.EmailSentYear",Value=dateSendEmail.Year.ToString()},
                new KeyValueModel{Key="Model.EmailSentMonth",Value=dateSendEmail.Month.ToString()},
                new KeyValueModel{Key="Model.EmailSentDay",Value=dateSendEmail.Day.ToString()},
                };

                var directory = ConfigHelper.GetByKey("ApplicationDocumentDirectory");
                var serPath = server.MapPath(directory);
                var pathDirectory = serPath + ApplicationTypeDocument.AssessmentDocument.ToString() + "\\" + e.Id + "_" + e.ApplicationNumber;

                string path = Common.Common.GenerateItem24Attachment(pathDirectory, attachmentData, server);
                #endregion

                bool isSuccess = MailHelper.SendMail(e.User.Email, subject, contentEmail, path: path);
                if (isSuccess)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
            }

            _unitOfWork.Commit();
            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<ResultModel<bool>> AssignReExamTimeslot(AssignReExamTimeslotBindingModel model)
        {
            var result = new ResultModel<bool>();

            var lstApplication = await _applicationRepository.GetMulti(x => model.ListApplicationId.Contains(x.Id)
               , new string[] { "Invoices", "ExamApplications" });

            if (lstApplication.Any(x => x.EligibleForResitExam))
            {
                result.Message = "Student(s) being ineligible for resit exam. Please check again.";
                return result;
            }

            var examOrigin = (await _examRepository.GetMulti(x => x.ClassId == model.OriginalClassId && x.Type != (int)ExamType.Exam && x.Date > DateTime.Now
               , new string[] { "Class" })).FirstOrDefault();

            if (examOrigin.Id != model.ExamOriginalId)
            {
                result.Message = "Original exam is not available";
                return result;
            }

            var examDestination = await _examRepository.GetSingleByCondition(x => x.Id == model.ExamDestinationId);

            if (examDestination.Class.CourseId != examOrigin.Class.CourseId)
            {
                result.Message = "This exam is not the same course";
                return result;
            }

            if (examDestination != null)
            {
                result.Message = "The Re-Exam to was not found";
                return result;
            }

            if (examDestination.Date <= DateTime.Now)
            {
                result.Message = "This exam was out of date";
                return result;
            }

            var allOfUserHaveNotPaidYet = false;

            if (examOrigin.Type == (int)ExamType.FirstReExam)
            {
                foreach (var application in lstApplication)
                {
                    var invoiceStatus = application.Invoices.FirstOrDefault(x => x.TypeReExam == (int)ExamType.FirstReExam)?.Status;
                    var listInvoiceStatusAccepted = new List<int>() { (int)Common.Enums.InvoiceStatus.Settled, (int)Common.Enums.InvoiceStatus.Overpaid };
                    if (!listInvoiceStatusAccepted.Contains(invoiceStatus ?? 0))
                    {
                        allOfUserHaveNotPaidYet = true;
                        break;
                    }
                    var examApplication = application.ExamApplications.FirstOrDefault(x => x.Exam.Type == (int)ExamType.FirstReExam);
                    examApplication.IsMakeUp = true;
                    examApplication.FromExamId = examApplication.ExamId;
                    examApplication.ExamId = model.ExamDestinationId;
                    _applicationRepository.Update(application);
                }
            }
            else if (examOrigin.Type == (int)ExamType.SecondReExam)
            {
                foreach (var application in lstApplication)
                {
                    var invoiceStatus = application.Invoices.FirstOrDefault(x => x.TypeReExam == (int)ExamType.FirstReExam)?.Status;
                    var listInvoiceStatusAccepted = new List<int>() { (int)Common.Enums.InvoiceStatus.Settled, (int)Common.Enums.InvoiceStatus.Overpaid };
                    if (!listInvoiceStatusAccepted.Contains(invoiceStatus ?? 0))
                    {
                        allOfUserHaveNotPaidYet = true;
                        break;
                    }
                    var examApplication = application.ExamApplications.FirstOrDefault(x => x.Exam.Type == (int)ExamType.FirstReExam);
                    examApplication.IsMakeUp = true;
                    examApplication.FromExamId = examApplication.ExamId;
                    examApplication.ExamId = model.ExamDestinationId;
                    _applicationRepository.Update(application);
                }
            }

            if (allOfUserHaveNotPaidYet)
            {
                result.Message = "Not all of student pay the invoice.";
                return result;
            }

            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }

        public async Task<List<ReExamFromViewModel>> GetAssignReExamFrom(int classId)
        {
            var listExam = (await _classRepository.GetSingleByCondition(x => x.Id == classId, new string[] { "Exams" })).ToReExamFromViewModel();

            return listExam;
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadInvoiceAttachment(int invoiceId, int id)
        {
            var result = new ResultModel<FileReturnViewModel>();

            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.Id == invoiceId, new string[] { "Application", "Application.Course.CourseTrans",
                "Application.User.Particular", "InvoiceItems", "PaymentTransactions.TransactionType1", "PaymentTransactions.AcceptedBank" });

            var invoiceType = invoice.InvoiceItems.FirstOrDefault(x => x.InvoiceItemTypeId != (int)Common.Enums.InvoiceItemType.Discount)?.InvoiceItemTypeId;
            var adminUser = _userRepository.GetSingleById(id);

            var userReceived = "";
            var quantity = "";
            var note1 = "";
            var note2 = "";
            var note3 = "";
            if (invoice.Status == (int)Common.Enums.InvoiceStatus.SettledByBatch && invoiceType == (int)SPDC.Common.Enums.InvoiceItemType.CourseFee)
            {

                var listBatchPayment = (await _batchPaymentRepository.GetMulti(x => x.IsSettled)).ToList();
                for (int i = 0; i < listBatchPayment.Count(); i++)
                {
                    if (string.IsNullOrEmpty(listBatchPayment[i].ListApplication))
                    {
                        continue;
                    }
                    var listInvoiceId = listBatchPayment[i].ListApplication.Split(',');
                    if (listInvoiceId.Contains(invoice.Id.ToString()))
                    {
                        userReceived = listBatchPayment[i].Payee;
                        quantity = listBatchPayment[i].ListApplication.Split(',').Length.ToString();
                        break;
                    }
                }
            }
            else
            {
                userReceived = invoice.Application.User.Particular.SurnameCN + invoice.Application.User.Particular.GivenNameCN
                    + invoice.Application.User.Particular.SurnameEN.ToUpper() + ", " + invoice.Application.User.Particular.GivenNameCN;
                note1 = "付款人以" + invoice.PaymentTransactions.LastOrDefault()?.TransactionType1.NameCN + "作為付款:";
                note2 = invoice.PaymentTransactions.LastOrDefault()?.TransactionType1.NameEN + " is the payment of for the following persons:";
                note3 = userReceived;
            }

            var path = GenerateInvoiceAttachment((SPDC.Common.Enums.InvoiceItemType)invoiceType, invoice, adminUser.UserName, userReceived, quantity, note1, note2, note3);

            if (File.Exists(path))
            {
                var stream = new MemoryStream(File.ReadAllBytes(path));
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = stream,
                    FileType = "application/pdf",
                    FileName = "Invoice_Attachment.pdf"
                };
                File.Delete(path);
                return result;
            }
            result.Message = "File is not found";
            return result;
        }
    }
}
