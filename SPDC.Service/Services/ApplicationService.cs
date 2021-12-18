using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels;
using SPDC.Model.BindingModels.Assessment;
using SPDC.Model.BindingModels.MakeupClass;
using SPDC.Model.BindingModels.MyApplication;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.Application;
using SPDC.Model.ViewModels.Assessment;
using SPDC.Model.ViewModels.Enrollment;
using SPDC.Model.ViewModels.MakeupClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPDC.Common.StaticConfig;
using InvoiceStatus = SPDC.Common.Enums.InvoiceStatus;

namespace SPDC.Service.Services
{

    public interface IApplicationService
    {
        Task<ResultModel<int>> CreateApplication(ApplicationCreateBindingModel lstModel, int userId, int languageID);

        Task<ResultModel<int>> UpdateApplication(ApplicationCreateBindingModel lstModel, int userId);

        Task<bool> CheckExistApplication(int? applicationId);

        Task<Application> GetApplication(int appID);

        Task<Class> GetClassByID(int classID);

        Task<IEnumerable<Module>> GetModuleByID(string moduleID);

        //Task<EnrollmentMyCourseViewModel> GetCourseEnrollment(int userId, int langCode);

        Task<ApplicationViewModel> GetApplicationByCourseAndUser(int applicationId);
        Task<PaginationSet<MyCourseApplicationViewModel>> GetMyCourseApplication(MyCourseApplicationFilter filter, int id, int langCode);
        Task<IEnumerable<InvoiceBindingModel>> ListApplicationInvoiceAndRecipt(int appId, int id);
        Task<bool> WithdrawalApplication(int appId);
        Task<ResultModel<FileReturnViewModel>> DownloadCourseInoivce(int invoiceId);
        Task<IEnumerable<StudentSearchViewModel>> GetApplicationForMakeupClass(string no, string namecn, string nameen);
        Task<IEnumerable<AssignLessonViewModel>> GetAssignLesson(int appId);
        Task<IEnumerable<StudentSearchViewModel>> GetAssignApplications(int[] ids);
        Task<bool> SendIneligibleForExamEmail(int courseId, int[] ids);
        int CreateEditResitExam(ResitExamBindingModel model);
        Task<IEnumerable<ResitExamViewModel>> GetResitExames(ResitExamSearchModel model);
        Task<ResitExamViewModel> GetResitExamesById(int id);
        Task<bool> AssignToReExamTimeslot(AssignToReExamTimeslotBindingModel models);
        Task<Application> GetApplicationByStudentNo(string studentNo);

        Task<IEnumerable<Application>> GetListApplicationByCommencementDate(int day);

        Task<IEnumerable<InvoiceReminderViewModel>> GetListInvoiceByStatus(InvoiceStatus status, int day);
    }

    public class ApplicationService : IApplicationService
    {
        IParticularService _particularService;
        private IUnitOfWork _unitOfWork;
        private IApplicationRepository _applicationRepository;
        private IEnrollmentStatusRepository _enrollmentStatusRepository;
        private IApplicationTransRepository _applicationTransRepository;
        private IClassRepository _classRepository;
        private IModuleRepository _moduleRepository;
        private IInvoiceRepository _invoiceRepository;
        private ICourseRepository _courseRepository;
        private INotificationRepository _notificationRepository;
        private ICommonDataService _commonDataService;
        private IResitExamRepository _resitExamRepository;
        private IExamRepository _examRepository;
        private IResitExamApplicationRepository _resitExamApplicationRepository;

        public ApplicationService(IUnitOfWork unitOfWork, IApplicationRepository applicationRepository, IInvoiceRepository invoiceRepository,
            IEnrollmentStatusRepository enrollmentStatusRepository, IApplicationTransRepository applicationTransRepository,
            IClassRepository classRepository, IModuleRepository moduleRepository, ICourseRepository courseRepository,
            INotificationRepository notificationRepository, ICommonDataService commonDataService, IResitExamRepository resitExamRepository,
            IExamRepository examRepository, IResitExamApplicationRepository resitExamApplicationRepository)
        {
            _unitOfWork = unitOfWork;
            _applicationRepository = applicationRepository;
            _invoiceRepository = invoiceRepository;
            _enrollmentStatusRepository = enrollmentStatusRepository;
            _applicationTransRepository = applicationTransRepository;
            _classRepository = classRepository;
            _moduleRepository = moduleRepository;
            _courseRepository = courseRepository;
            _notificationRepository = notificationRepository;
            _commonDataService = commonDataService;
            _resitExamRepository = resitExamRepository;
            _examRepository = examRepository;
            _resitExamApplicationRepository = resitExamApplicationRepository;
        }

        public async Task<ResultModel<int>> CreateApplication(ApplicationCreateBindingModel lstModel, int userId, int languageID)
        {
            var result = new ResultModel<int>();
            try
            {
                Application application = new Application();
                application.CourseId = lstModel.CourseId;
                application.UserId = userId;
                application.StudentPreferredClass = lstModel.ClassId;
                application.ModuleIds = lstModel.ModuleID.Length == 0 ? null : string.Join(",", lstModel.ModuleID);
                application.IHaveApplyFor = lstModel.IHaveApplyFor;
                application.IHaveApplyForText = lstModel.IHaveApplyForText;
                application.IsRequiredRecipt = lstModel.IsRequiredRecipt;

                //var dataApplicationNumber = (await _applicationRepository.GetAll())?.OrderByDescending(x => Convert.ToInt32(x.ApplicationNumber)).FirstOrDefault();
                var dataApplicationNumber = (await _applicationRepository.GetMulti(x => x.CourseId == lstModel.CourseId)).LastOrDefault();
                var course = await _courseRepository.GetSingleByCondition(x => x.Id == lstModel.CourseId, new string[] { "TargetClasses", "Classes" });

                string studentNoFormat = new string('0', Math.Max(course.Classes.Sum(c => c.Capacity).ToString().Length, 2));
                int.TryParse(dataApplicationNumber?.ApplicationNumber?.Split('-').LastOrDefault(), out int startNumber);
                application.ApplicationNumber = $"{course.CourseCode}-{course.TargetClasses.AcademicYear}-{(startNumber + 1).ToString(studentNoFormat)}";

                //application.Status = (int)Common.Enums.ApplicationStatus.Created;
                application.LastModifiedBy = userId;
                application.LastModifiedDate = DateTime.Now;
                application.Status = /*lstModel.IsSubmit ? (int)Common.Enums.ApplicationStatus.Submitted : */(int)Common.Enums.ApplicationStatus.Created;

                //application.ApplicationStatusStorages.Add(new ApplicationStatusStorage()
                //{
                //    Status = (int)Common.Enums.ApplicationStatus.Created,
                //    LastModifiedBy = userId,
                //    LastModifiedDate = DateTime.Now
                //});

                var createaApplication = _applicationRepository.Add(application);

                _unitOfWork.Commit();

                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = application.Id;
                return result;

            }
            catch (Exception ex)
            {
                return result;
                throw;
            }
        }

        public async Task<ResultModel<int>> UpdateApplication(ApplicationCreateBindingModel lstModel, int userId)
        {
            var result = new ResultModel<int>();
            var application = await _applicationRepository.GetSingleByCondition(x => x.Id == lstModel.ApplicationId, new string[] { "ApplicationStatusStorages" });
            var listStatusToEdit = new List<int>() { (int)Common.Enums.ApplicationStatus.Created, (int)Common.Enums.ApplicationStatus.SupplementaryInformation };
            if (!listStatusToEdit.Contains(application.Status))
            {
                result.Message = "This application was submited";
                return result;
            }
            else if (lstModel.IsSubmit && lstModel.ClassId != application.StudentPreferredClass)
            {
                result.Message = "An error occurred while processing your request";
                return result;
            }

            application.CourseId = lstModel.CourseId;
            application.StudentPreferredClass = lstModel.ClassId;
            application.ModuleIds = (lstModel.ModuleID == null || lstModel.ModuleID.Length == 0) ? null : string.Join(",", lstModel.ModuleID);

            application.LastModifiedBy = userId;
            application.LastModifiedDate = DateTime.Now;
            application.Status = lstModel.IsSubmit ? (int)Common.Enums.ApplicationStatus.Submitted : application.Status;
            application.ApplicationLastSubmissionDate = lstModel.IsSubmit ? DateTime.Now : application.ApplicationLastSubmissionDate;
            application.IHaveApplyFor = lstModel.IHaveApplyFor;
            application.IHaveApplyForText = lstModel.IHaveApplyForText;
            application.IsRequiredRecipt = lstModel.IsRequiredRecipt;

            //application.ApplicationStatusStorages.FirstOrDefault().LastModifiedBy = userId;
            //application.ApplicationStatusStorages.FirstOrDefault().LastModifiedDate = DateTime.Now;

            _applicationRepository.Update(application);

            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = application.Id;
            return result;
        }

        public async Task<bool> CheckExistApplication(int? applicationId)
        {
            bool result = false;

            if (applicationId != null)
            {
                result = (await _applicationRepository.GetSingleByCondition(x => x.Id == applicationId.Value)) != null ? true : false;
            }

            return result;
        }

        public async Task<Application> GetApplication(int appID)
        {
            var result = await _applicationRepository.GetSingleByCondition(x => x.Id == appID);

            return result;
        }

        public async Task<Class> GetClassByID(int classID)
        {
            var result = await _classRepository.GetSingleByCondition(x => x.Id == classID);

            return result;
        }

        public async Task<IEnumerable<Module>> GetModuleByID(string moduleID)
        {
            var arrayModuleId = moduleID.Split(',');
            var result = await _moduleRepository.GetMulti(x => arrayModuleId.Contains(x.Id.ToString()));

            return result;
        }

        //public async Task<EnrollmentMyCourseViewModel> GetCourseEnrollment(int userId, int langCode)
        //{
        //    EnrollmentMyCourseViewModel result = await Task.Run(() => _applicationRepository.GetCourseEnrollment(userId, langCode));

        //    return result;
        //}

        public async Task<ApplicationViewModel> GetApplicationByCourseAndUser(int applicationId)
        {
            var application = await _applicationRepository.GetSingleByCondition(x => x.Id == applicationId);
            if (application != null)
            {
                ApplicationViewModel result = new ApplicationViewModel();
                result.ApplicationId = application.Id;
                result.ClassReferenced = application.StudentPreferredClass;
                result.ModuleReferenced = application.ModuleIds != null ? Array.ConvertAll(application.ModuleIds.Split(','), int.Parse) : null;
                result.IHaveApplyFor = application.IHaveApplyFor;
                result.IHaveApplyForText = application.IHaveApplyForText;
                result.IsRequiredRecipt = application.IsRequiredRecipt;
                return result;
            }

            return null;
        }

        public async Task<PaginationSet<MyCourseApplicationViewModel>> GetMyCourseApplication(MyCourseApplicationFilter filter, int id, int langCode)
        {
            var lstApplication = (await _applicationRepository.GetMultiPaging(x => x.UserId == id, "Id", true, filter.Page, filter.Size,
                new string[] { "Course", "Course.CourseTrans", "Course.CourseType", "Invoices", "Invoices.InvoiceItems" }))
                .Select(c => c.ToMyCourseApplicationViewModel(langCode));

            var total = await _applicationRepository.Count(x => x.UserId == id);

            return new PaginationSet<MyCourseApplicationViewModel>()
            {
                Items = lstApplication,
                Page = filter.Page,
                TotalCount = total
            };
        }

        public async Task<IEnumerable<InvoiceBindingModel>> ListApplicationInvoiceAndRecipt(int appId, int id)
        {
            var application = (await _applicationRepository.GetSingleByCondition(x => x.Id == appId, new string[] { "Invoices" })).Invoices.Select(c => c.ToInvoiceBindingModel());

            return application;
        }

        public async Task<bool> WithdrawalApplication(int appId)
        {
            var application = await _applicationRepository.GetSingleByCondition(x => x.Id == appId);

            if (application.Invoices.Any(x
                => (x.Status == (int)Common.Enums.InvoiceStatus.PaidPartially
                    || x.Status == (int)Common.Enums.InvoiceStatus.Waived
                    || x.Status == (int)Common.Enums.InvoiceStatus.Overpaid
                    || x.Status == (int)Common.Enums.InvoiceStatus.Settled
                    || x.Status == (int)Common.Enums.InvoiceStatus.SettledByBatch)
                && x.InvoiceItems.Any(c => c.InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee)))
            {
                return false;
            }

            application.Status = (int)Common.Enums.ApplicationStatus.Withdrawal;
            _applicationRepository.Update(application);
            _unitOfWork.Commit();
            return true;
        }

        public async Task<ResultModel<FileReturnViewModel>> DownloadCourseInoivce(int invoiceId)
        {
            var result = new ResultModel<FileReturnViewModel>();

            var invoiceTemplatePath = ConfigHelper.GetByKey("InvoiceTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(invoiceTemplatePath) + "invoice.html";

            if (!File.Exists(serPath))
            {
                result.Message = "Template was not found";
                return result;
            }

            var invoice = await _invoiceRepository.GetSingleByCondition(x => x.Id == invoiceId, new string[] { /*"Application", */"Application.Course" });

            if (invoice == null)
            {
                result.Message = "Invoice was not found";
                return result;
            }

            string text = File.ReadAllText(serPath);
            text = text.Replace("@courseCode", invoice.Application.Course.CourseCode);
            text = text.Replace("@numberCourseFee", invoice.Fee.ToString());

            var tempDirPdf = System.Web.HttpContext.Current.Server.MapPath(invoiceTemplatePath) + $"{invoice.Id}.html";
            tempDirPdf = Common.Common.GenFileNameDuplicate(tempDirPdf);
            File.WriteAllText(tempDirPdf, text);
            var stream = Common.Common.GeneratePdf(tempDirPdf);
            File.Delete(tempDirPdf);

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = new FileReturnViewModel()
            {
                Stream = stream,
                FileType = "application/pdf",
                FileName = "Invoice.pdf"
            };

            return result;
        }

        public async Task<IEnumerable<StudentSearchViewModel>> GetApplicationForMakeupClass(string no, string namecn, string nameen)
        {
            List<Application> result = new List<Application>();

            // TODO: Steve - Add Eligible For Make-Up Class
            if (!string.IsNullOrWhiteSpace(no))
            {
                //result = await _applicationRepository.GetMulti(x => x.ApplicationNumber.Contains(no) && x.EligibleForMakeUpClass
                result = await _applicationRepository.GetMulti(x => x.ApplicationNumber.Contains(no)
                                                    , new string[] { "User.Particular" });
            }
            if (!string.IsNullOrWhiteSpace(namecn))
            {
                //result = await _applicationRepository.GetMulti(x => (x.User.Particular.SurnameCN + " " + x.User.Particular.GivenNameCN).Contains(namecn) && x.EligibleForMakeUpClass
                result = await _applicationRepository.GetMulti(x => (x.User.Particular.SurnameCN + " " + x.User.Particular.GivenNameCN).Contains(namecn)
                                                                    , new string[] { "User.Particular" });
            }
            if (!string.IsNullOrWhiteSpace(nameen))
            {
                result = await _applicationRepository.GetMulti(x => (x.User.Particular.SurnameEN + " " + x.User.Particular.GivenNameEN).Contains(nameen)
                //result = await _applicationRepository.GetMulti(x => (x.User.Particular.SurnameEN + " " + x.User.Particular.GivenNameEN).Contains(nameen) && x.EligibleForMakeUpClass
                                                                    , new string[] { "User.Particular" });
            }

            return result.Select(x => x.ToStudentSearchViewModel());
        }

        public async Task<IEnumerable<AssignLessonViewModel>> GetAssignLesson(int appId)
        {
            var result = await _applicationRepository.GetSingleByCondition(x => x.Id == appId, new string[] { "AdminAssignedClassModel", "LessonAttendances", "LessonAttendances.Lesson" });

            return result.LessonAttendances.Select(x => new AssignLessonViewModel()
            {
                Id = x.Id,
                Date = x.Lesson.Date.ToString("dd/MM/yyyy"),
                ClassCode = result.AdminAssignedClassModel.ClassCode,
                TimeFromHrs = x.Lesson.TimeFromHrs,
                TimeToHrs = x.Lesson.TimeToHrs,
                TimeFromMin = x.Lesson.TimeFromMin,
                TimeToMin = x.Lesson.TimeToMin,
                Venue = x.Lesson.Venue
            });
        }

        public async Task<IEnumerable<StudentSearchViewModel>> GetAssignApplications(int[] ids)
        {
            var result = await _applicationRepository.GetMulti(x => ids.Contains(x.Id), new string[] { "User.Particular" });

            return result.Select(x => x.ToStudentSearchViewModel());
        }

        public async Task<bool> SendIneligibleForExamEmail(int courseId, int[] ids)
        {
            List<Application> apps = await _applicationRepository.GetMulti(x => ids.Contains(x.Id)
                                        , new string[] { "User", "User.UserDevices", "User.Particular", "AdminAssignedClassModel", "AdminAssignedClassModel.Exams" });
            Course course = await _courseRepository.GetSingleByCondition(x => x.Id == courseId, new string[] { "CourseTrans", "Enquiries" });

            foreach (Application app in apps)
            {
                string courseName = app.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? course.CourseTrans.FirstOrDefault(x => x.LanguageId == 1).CourseName : course.CourseTrans.FirstOrDefault(x => x.LanguageId == 3).CourseName;
                string emailSubject = FileHelper.GetEmailSubject("item20", "EmailSubject", app.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");
                string emailTemplate = app.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item20EN.cshtml" : "Item20TC.cshtml";

                CommonData emailSerialNumber = await _commonDataService.GetByKey("EmailSerialNumber");
                string serialNumber = emailSerialNumber.ValueInt > 9999 ? emailSerialNumber.ValueInt.ToString("0000") : emailSerialNumber.ValueInt.ToString();

                // Add attachment
                var attachmentData = new List<KeyValueModel>() {
                new KeyValueModel{Key="Model.Ref",Value=$"({serialNumber}) in M/CT/TSRC/S/{course.CourseCode}"},
                new KeyValueModel{Key="Model.Name",Value=app.User.Particular.SurnameCN + app.User.Particular.GivenNameCN + (((Honorific)app.User.Particular.Honorific).GetStringValue())},
                new KeyValueModel{Key="Model.CourseHeader",Value= $"{course.CourseTrans.FirstOrDefault(x=>x.LanguageId ==  (int)LanguageCode.HK)?.CourseName}{course.CourseTrans.FirstOrDefault(x=>x.LanguageId == (int)LanguageCode.EN)?.CourseName}({app.AdminAssignedClassModel.ClassCode})" },
                new KeyValueModel{Key="Model.YearSend",Value=DateTime.Now.Year.ToString() },
                new KeyValueModel{Key="Model.MonthSend",Value=DateTime.Now.Month.ToString() },
                new KeyValueModel{Key="Model.DaySend",Value=DateTime.Now.Day.ToString() },
                new KeyValueModel{Key="Model.UntickedCaculate1",Value=(int)app.AdminAssignedClassModel.ClassCommonId == (int)AttendanceRequirementType.Lesson ? app.LessonAttendances.Count(x=>x.IsTakeAttendance == false).ToString() : app.LessonAttendances.Where(x => x.IsTakeAttendance == false).Sum(x => (x.Lesson.TimeFromHrs * 60 + x.Lesson.TimeFromMin)/60).ToString()},
                new KeyValueModel{Key="Model.UntickedUnit",Value="堂" },
                new KeyValueModel{Key="Model.AttendanceRequirementPercentValue",Value=app.AdminAssignedClassModel.AttendanceRequirement.ToString() },
                new KeyValueModel{
                    Key="Model.AttendanceRequirementPercentUnit",
                    Value = (int)app.AdminAssignedClassModel.ClassCommonId == (int)AttendanceRequirementType.Lesson ? "堂" :  (int)app.AdminAssignedClassModel.ClassCommonId == (int)AttendanceRequirementType.Hrs ? "Hrs" : "%"},
                // 2 => Model.AttendanceRequirement
                //new KeyValueModel{Key="Model.AttendanceRequirementOrigin",Value="2" },
                new KeyValueModel{Key="Model.AttendanceRequirementUnit",Value = (int)app.AdminAssignedClassModel.ClassCommonId == (int)AttendanceRequirementType.Lesson ? "堂" :  (int)app.AdminAssignedClassModel.ClassCommonId == (int)AttendanceRequirementType.Hrs ? "Hrs" : "%"},
                // 3 => Model.UntickedCaculate2 - If Attendance Requirement Unit is “Lesson(s)”, calculate the total number of unticked lesson columns. -If Attendance Requirement Unit is “Hrs”, calculate the total hours from the unticked lesson columns.
                new KeyValueModel{Key="Model.UntickedCaculate2",Value=(int)app.AdminAssignedClassModel.ClassCommonId == (int)AttendanceRequirementType.Percent ? (Math.Ceiling((double)app.AdminAssignedClassModel.AttendanceRequirement / 100 * app.LessonAttendances.Count)).ToString() :  app.AdminAssignedClassModel.AttendanceRequirement.HasValue ? ((int)app.AdminAssignedClassModel.AttendanceRequirement).ToString() : ""},
                new KeyValueModel{Key="Model.YearExam",Value=app.AdminAssignedClassModel.Exams.FirstOrDefault()?.Date.ToString("yyyy") },
                new KeyValueModel{Key="Model.MonthExam",Value=app.AdminAssignedClassModel.Exams.FirstOrDefault()?.Date.ToString("MM") },
                new KeyValueModel{Key="Model.DayExam",Value=app.AdminAssignedClassModel.Exams.FirstOrDefault()?.Date.ToString("dd") },
                new KeyValueModel{Key="Model.EnquiryPhone",Value=course.Enquiries.FirstOrDefault()?.Phone },
                new KeyValueModel{Key="Model.EnquiryEmail",Value=course.Enquiries.FirstOrDefault()?.Email },
                new KeyValueModel{Key="Model.EnquiryName",Value=course.Enquiries.FirstOrDefault()?.ContactPersonHK }
                };

                var directory = ConfigHelper.GetByKey("ApplicationDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var pathDirectory = serPath + ApplicationTypeDocument.AttendanceDocument.ToString() + "\\" + app.Id + "_" + app.ApplicationNumber;

                string path = Common.Common.GenerateItem20Attachment(pathDirectory, attachmentData);

                bool isSuccesss = MailHelper.SendMail(app.User.Email, emailSubject, Common.Common.GenerateItem20Email(emailTemplate, app.AdminAssignedClassModel.ClassCode, app.User.DisplayName, courseName, app.AdminAssignedClassModel.Exams.FirstOrDefault().Date.ToString("dd/MM/yyyy")), path: path);
                if (isSuccesss)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }


                if (app.User.UserDevices != null && app.User.UserDevices.Count() > 0)
                {
                    string notificationTitle = FileHelper.GetNotificationTitle("item20", "NotificationTitles", app.User.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");

                    Notification notification = new Notification()
                    {
                        Body = "",
                        DataId = app.Id,
                        Title = notificationTitle,
                        Type = (int)NotificationType.Application
                    };

                    foreach (var device in app.User.UserDevices)
                    {
                        NotificationHelper.PushNotification(notification.Body, notification.Title, device.DeviceToken);
                    }

                    notification.NotificationUsers.Add(new NotificationUser()
                    {
                        CreatedDate = DateTime.Now,
                        IsFavourite = false,
                        IsRead = false,
                        IsRemove = false,
                        UserId = app.UserId
                    });
                    _notificationRepository.Add(notification);
                }
            }
            _unitOfWork.Commit();
            return true;
        }

        public int CreateEditResitExam(ResitExamBindingModel model)
        {
            ResitExam resitExam;
            if (model.Id == 0)
            {
                resitExam = new ResitExam()
                {
                    Date = model.Date,
                    Name = model.Name,
                    ResitExamApplicationDeadline = model.DateLine,
                    TimeFromHrs = model.TimeFromHrs,
                    TimeFromMin = model.TimeFromMin,
                    TimeToHrs = model.TimeToHrs,
                    TimeToMin = model.TimeToMin,
                    TypeOfReExam = model.Type,
                    Venue = model.Venue
                };

                _resitExamRepository.Add(resitExam);
            }
            else
            {
                resitExam = _resitExamRepository.GetSingleById(model.Id);
                resitExam.Date = model.Date;
                resitExam.Name = model.Name;
                resitExam.ResitExamApplicationDeadline = model.DateLine;
                resitExam.TimeFromHrs = model.TimeFromHrs;
                resitExam.TimeFromMin = model.TimeFromMin;
                resitExam.TimeToHrs = model.TimeToHrs;
                resitExam.TimeToMin = model.TimeToMin;
                resitExam.TypeOfReExam = model.Type;
                resitExam.Venue = model.Venue;

                _resitExamRepository.Update(resitExam);
            }
            _unitOfWork.Commit();
            return resitExam.Id;
        }

        public async Task<IEnumerable<ResitExamViewModel>> GetResitExames(ResitExamSearchModel model)
        {
            var result = await _resitExamRepository.GetMulti(x => x.Name.Contains(model.Name)
                                                                    && x.Venue.Contains(model.Venue)
                                                                    && x.TypeOfReExam == model.Type
                                                                    && (!model.DateFrom.HasValue || model.DateFrom.HasValue && x.Date >= model.DateFrom)
                                                                    && (!model.DateTo.HasValue || model.DateTo.HasValue && x.Date <= model.DateTo.Value));
            return result.Select(x => new ResitExamViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Id,
                TypeText = x.Id == (int)ExamType.FirstReExam ? ExamType.FirstReExam.GetStringValue() : ExamType.SecondReExam.GetStringValue(),
                ApplicationDateline = x.Date,
                Date = x.Date,
                TimeFromHrs = x.TimeFromHrs,
                TimeFromMin = x.TimeFromMin,
                TimeToHrs = x.TimeToHrs,
                TimeToMin = x.TimeToMin,
                Venue = x.Venue
            });
        }

        public async Task<ResitExamViewModel> GetResitExamesById(int id)
        {
            var result = await _resitExamRepository.GetSingleByCondition(x => x.Id == id
                                                    , new string[] { "ResitExamApplications"
                                                                    , "ResitExamApplications.Application"
                                                                    , "ResitExamApplications.Application.User"
                                                                    , "ResitExamApplications.Application.User.Particular"
                                                                    , "ResitExamApplications.Exam"
                                                                    , "ResitExamApplications.Exam.Class"});
            return new ResitExamViewModel()
            {
                Id = result.Id,
                Name = result.Name,
                Type = result.TypeOfReExam,
                TypeText = result.TypeOfReExam == (int)ExamType.FirstReExam ? ExamType.FirstReExam.GetStringValue() : ExamType.SecondReExam.GetStringValue(),
                ApplicationDateline = result.Date,
                Date = result.Date,
                TimeFromHrs = result.TimeFromHrs,
                TimeFromMin = result.TimeFromMin,
                TimeToHrs = result.TimeToHrs,
                TimeToMin = result.TimeToMin,
                Venue = result.Venue,
                ResitExamApplications = result.ResitExamApplications.Select(x => new ResitExamApplicationViewModel()
                {
                    Id = x.Id,
                    ResitExamId = x.ResitExamId,
                    ApplicationId = x.ApplicationId,
                    StudentNo = x.Application.ApplicationNumber,
                    StudentGivenCN = x.Application.User.Particular.GivenNameCN,
                    StudentSurnameCN = x.Application.User.Particular.SurnameCN,
                    StudentGivenEN = x.Application.User.Particular.GivenNameEN,
                    StudentSurnameEN = x.Application.User.Particular.SurnameEN,
                    ClassCode = x.Exam.Class.ClassCode,
                    Date = x.Exam.Date.ToString("dd/MM/yyyy"),
                    TimeFrom = x.Exam.FromTime,
                    TimeTo = x.Exam.ToTime
                })
            };
        }

        public async Task<bool> AssignToReExamTimeslot(AssignToReExamTimeslotBindingModel models)
        {
            var reExam = await _resitExamRepository.GetSingleByCondition(x => x.Id == models.ReExamTimeslotId);
            var dbResordIds = reExam.ResitExamApplications.Select(x => x.Id);

            int[] updateIds = models.Data.Where(x => x.Id != 0).Select(x => x.Id).ToArray();
            int[] deleteIds = dbResordIds.Where(x => !updateIds.Contains(x)).ToArray();

            foreach (var item in models.Data)
            {
                var fromExam = await _examRepository.GetSingleByCondition(x => x.Id == item.FromReExamId);
                if (item.Id != 0)
                {
                    var updateItem = reExam.ResitExamApplications.SingleOrDefault();
                    if (updateItem == null)
                    {
                        return false;
                    }
                    updateItem.ApplicationId = item.AppId;
                    updateItem.Exam_Id = fromExam.Id;
                    updateItem.ResitExamId = models.ReExamTimeslotId;
                }
                else
                {
                    reExam.ResitExamApplications.Add(new ResitExamApplication()
                    {
                        ApplicationId = item.AppId,
                        ResitExamId = models.ReExamTimeslotId,
                        Exam_Id = fromExam.Id,
                        AssessmentMark = null,
                        AssessmentResult = null
                    });
                }
            }

            _resitExamRepository.Update(reExam);
            _resitExamApplicationRepository.DeleteMulti(x => deleteIds.Contains(x.Id));

            _unitOfWork.Commit();

            return true;
        }

        public async Task<Application> GetApplicationByStudentNo(string studentNo)
        {
            var result = await _applicationRepository.GetSingleByCondition(x => x.ApplicationNumber.Equals(studentNo)
                                                        , new string[] { "ExamApplications", "ExamApplications.Exam", "ExamApplications.Exam.Class" });
            return result;
        }

        public async Task<IEnumerable<Application>>  GetListApplicationByCommencementDate(int day)
        {
            try
            {
                var compareDate = DateTime.Now.AddDays(day);
                var query = await _applicationRepository.GetMulti(i => i.AdminAssignedClassModel != null && DbFunctions.DiffDays(i.AdminAssignedClassModel.CommencementDate, compareDate) == 0, new string[] { "AdminAssignedClassModel", "User", "Course" });

                return query;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IEnumerable<InvoiceReminderViewModel>> GetListInvoiceByStatus(InvoiceStatus status, int day)
        {
            var compareDate = DateTime.Now.AddDays(day);

            var query = await _invoiceRepository.GetMulti(i => i.Status == (int)status && DbFunctions.DiffDays(i.PaymentDueDate, compareDate) == 0, new string[] { "Application", "Application.User", "Application.Course" });

            return query.Select(i => new InvoiceReminderViewModel()
            {
                CommunicateLanguage = i.Application.User.CommunicationLanguage.HasValue ? i.Application.User.CommunicationLanguage.Value : 1
                ,
                CourseName = i.Application.Course.CourseTrans.FirstOrDefault(j => j.LanguageId == (i.Application.User.CommunicationLanguage.HasValue ? i.Application.User.CommunicationLanguage.Value : 1)).CourseName
            ,
                DisplayName = i.Application.User.DisplayName
            ,
                Email = i.Application.User.Email
            ,
                InvoiceNo = i.InvoiceNumber
            });
        }
    }
}
