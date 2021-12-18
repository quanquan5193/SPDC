using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Repositories;
using SPDC.Model.ViewModels.Enrollment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPDC.Service.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IClassRepository _classRepo;
        private readonly IUserRepository _userRepo;
        private readonly IApplicationRepository _applicationRepo;
        private readonly ILessionRepository _lessionRepo;
        private readonly IExamApplicationRepository _examApplicationRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IClientRepository _clientRepo;

        public EnrollmentService(ICourseRepository courseRepository
            , IClassRepository classRepository
            , IUserRepository userRepository
            , IApplicationRepository applicationRepository
            , ILessionRepository lessionRepository
            , IExamApplicationRepository examApplicationRepository
            , IInvoiceRepository invoiceRepository
            , IClientRepository clientRepository)
        {
            _courseRepo = courseRepository;
            _classRepo = classRepository;
            _userRepo = userRepository;
            _applicationRepo = applicationRepository;
            _lessionRepo = lessionRepository;
            _examApplicationRepo = examApplicationRepository;
            _invoiceRepo = invoiceRepository;
            _clientRepo = clientRepository;
        }

        public async Task<IList<EnrollmentCalendarViewModel>> GetCalendar(int userId, DateTime? from, DateTime? to)
        {
            var query = await _applicationRepo.GetMulti(i => i.UserId == userId && (i.EnrollmentStatusStorages.Count > 0 && (i.EnrollmentStatusStorages.OrderByDescending(j => j.Id).FirstOrDefault().Status == (int)EnrollmentStatus.Enrolled) || i.EnrollmentStatusStorages.OrderByDescending(j => j.Id).FirstOrDefault().Status == (int)EnrollmentStatus.Resit)
            , new string[] {
                "LessonAttendances","LessonAttendances.Lesson", "LessonAttendances.Lesson.Class", "LessonAttendances.Lesson.Class.Course",
                "MakeUpAttendences", "MakeUpAttendences.MakeUpClass", "MakeUpAttendences.Lesson","MakeUpAttendences.Lesson.Class.Course",
                "Course.CourseTrans","EnrollmentStatusStorages", "EnrollmentStatusStorages.EnrollmentStatus" });

            List<EnrollmentCalendarViewModel> lstLession = new List<EnrollmentCalendarViewModel>();
            var a = query.SelectMany(x => x.LessonAttendances.Select(y => y.Lesson));


            lstLession.AddRange(
                query.SelectMany(i => i.LessonAttendances.Select(y => y.Lesson))
                .Where(i => (!from.HasValue || i.Date.CompareTo(from) >= 0) && (!to.HasValue || i.Date.CompareTo(to) <= 0))
                .Select(i => new EnrollmentCalendarViewModel()
                {
                    ClassCode = i.Class.ClassCode,
                    CourseName = i.Class.Course.CourseTrans.FirstOrDefault(j => j.LanguageId == 1).CourseName,
                    Date = i.Date,
                    FromTime = $"{i.TimeFromHrs.ToString("00")}:{i.TimeFromMin.ToString("00")}",
                    ToTime = $"{i.TimeToHrs.ToString("00")}:{i.TimeToMin.ToString("00")}",
                    Venue = i.Venue
                }));

            lstLession.AddRange(query.SelectMany(i => i.MakeUpAttendences.Where(x => x.IsDisplayToStudentPortal).Select(y => y.MakeUpClass))
                .Where(i => (!from.HasValue || i.Date.CompareTo(from) >= 0) && (!to.HasValue || i.Date.CompareTo(to) <= 0))
                .Select(i => new EnrollmentCalendarViewModel()
                {
                    ClassCode = i.Name,
                    CourseName = i.MakeUpAttendences.First().Lesson.Class.Course.CourseTrans.FirstOrDefault(j => j.LanguageId == 1).CourseName,
                    Date = i.Date,
                    FromTime = $"{i.TimeFromHrs.ToString("00")}:{i.TimeFromMin.ToString("00")}",
                    ToTime = $"{i.TimeToHrs.ToString("00")}:{i.TimeToMin.ToString("00")}",
                    Venue = i.Venue
                }).ToList());

            return lstLession.ToList();
        }

        public async Task<PaginationSet<EnrollmentClassDetailViewModel>> GetClassDetail(int classId, int index, int pageSize)
        {
            var query = await _lessionRepo.GetMultiPaging(i => i.ClassId == classId, "Id", false, index, pageSize, new string[] { "Class.Course.CourseDocuments", "Class.Course.CourseDocuments.Document" });
            var totalCount = await _lessionRepo.Count(i => i.ClassId == classId);

            var apiUrl = (await _clientRepo.GetSingleByCondition(x => x.ClientName.Equals("ApiPortal"))).ClientUrl;


            var result = query.Select(i => new EnrollmentClassDetailViewModel()
            {
                LessionId = i.Id,
                LessionCode = i.No.ToString("00"),
                TimeFrom = $"{i.TimeFromHrs.ToString("00")}:{i.TimeFromMin.ToString("00")}",
                TimeTo = $"{i.TimeToHrs.ToString("00")}:{i.TimeToMin.ToString("00")}",
                Venue = i.Venue,
                Date = i.Date.ToString(SPDCConstant.DateFormat),
                Documents = i.Class.Course.CourseDocuments.Where(j => !j.LessonId.HasValue || j.LessonId.Value == i.Id).Select(j => new EnrollmentCoureMaterial()
                {
                    Id = j.DocumentId,
                    FileName = j.Document.FileName,
                    Url = $"{ConfigHelper.GetByKey("DMZAPI")}/api/Proxy?url=Courses/download-document?docId={j.DocumentId}"

                }).ToList()
            });

            return new PaginationSet<EnrollmentClassDetailViewModel>() { Items = result, Page = index, TotalCount = totalCount };
        }

        public async Task<PaginationSet<EnrollmentMyClassViewModel>> GetClasses(int userId, int index, int pageSize)
        {
            var query = await _applicationRepo.GetMulti(i => i.AdminAssignedClass.HasValue && i.UserId == userId, new string[] { "AdminAssignedClassModel", "Course", "Course.CourseTrans", "EnrollmentStatusStorages", "EnrollmentStatusStorages.EnrollmentStatus" });
            var totalCount = await _applicationRepo.Count(i => i.AdminAssignedClass.HasValue && i.UserId == userId);

            var result = query
                            .Select(i => new EnrollmentMyClassViewModel()
                            {
                                CourseCode = i.Course.CourseCode,
                                CourseName = i.Course.CourseTrans.FirstOrDefault(j => j.LanguageId == 1).CourseName,
                                ClassCommencementDate = i.AdminAssignedClassModel.CommencementDate,
                                ClassCompletionDate = i.AdminAssignedClassModel.CompletionDate,
                                ApplicationId = i.Id,
                                Duration = i.Course.DurationTotal,
                                CreditsAccumulated = i.CreditAcquired,
                                EnrollmentStatus = i.EnrollmentStatusStorages.Count > 0 ? i.EnrollmentStatusStorages.OrderByDescending(j => j.LastModifiedDate).FirstOrDefault().EnrollmentStatus.NameEN : string.Empty
                            });

            return new PaginationSet<EnrollmentMyClassViewModel>() { Items = result, TotalCount = totalCount, Page = index };
        }

        public async Task<EnrollmentDetailViewModel> GetEnrollmentDetail(int appplicationId)
        {
            var query = await _applicationRepo.GetMulti(i => i.Id == appplicationId, new string[] { "Course", "Course.CourseTrans", "EnrollmentStatusStorages", "EnrollmentStatusStorages.EnrollmentStatus", "Course.TargetClasses" });

            var result = query.Select(i => new EnrollmentDetailViewModel()
            {
                AcademicYear = i.Course.TargetClasses != null ? i.Course.TargetClasses.AcademicYear : string.Empty
                ,
                CourseCode = i.Course.CourseCode
                ,
                CourseName = i.Course.CourseTrans.FirstOrDefault(j => j.LanguageId == 1).CourseName
                ,
                CourseName_Chi = i.Course.CourseTrans.FirstOrDefault(j => j.LanguageId == 2).CourseName,
                CreditsAccquired = i.CreditAcquired,
                EnrollmentStatus = i.EnrollmentStatusStorages.Count > 0 ? i.EnrollmentStatusStorages.OrderByDescending(j => j.LastModifiedDate).FirstOrDefault().EnrollmentStatus.NameEN : string.Empty,
                ClassId = i.AdminAssignedClass
            });

            return result.FirstOrDefault();
        }

        public async Task<PaginationSet<EnrollmentExamDetailViewModel>> GetExams(int applicationId, int index, int pageSize)
        {
            var query = await _examApplicationRepo.GetMultiPaging(i => i.ApplicationId == applicationId, "Id", false, index, pageSize, new string[] { "Exam" });

            var result = new List<EnrollmentExamDetailViewModel>();

            var totalCount = await _examApplicationRepo.Count(i => i.ApplicationId == applicationId);

            for (int i = 0; i < query.Count(); i++)
            {
                var currentEl = query.ElementAt(i);

                var item = new EnrollmentExamDetailViewModel();
                item.Score = currentEl.AssessmentMark;
                item.ExamStatus = currentEl.AssessmentResult;
                item.TimeFrom = currentEl.Exam.FromTime;
                item.TimeTo = currentEl.Exam.ToTime;
                item.Venue = currentEl.Exam.ExamVenueText;
                item.Date = currentEl.Exam.Date.ToString(SPDCConstant.DateFormat);

                if (i == 0)
                {
                    item.ExamType = "Exam";
                }
                else
                {
                    item.ExamType = $"{i.ToOrdinal()} Re-exam";
                }

                result.Add(item);
            }

            return new PaginationSet<EnrollmentExamDetailViewModel>() { Items = result, TotalCount = totalCount, Page = index };
        }

        public async Task<IList<EnrollmentInvoiceReceiptViewModel>> GetInvoices(int applicationId)
        {
            var query = await _invoiceRepo.GetMulti(i => i.ApplicationId == applicationId, new string[] { "InvoiceItems" });

            var apiUrl = (await _clientRepo.GetSingleByCondition(x => x.ClientName.Equals("ApiPortal"))).ClientUrl;

            var result = query.Select(i => new EnrollmentInvoiceReceiptViewModel()
            {
                InvoiceDate = i.PaymentDueDate.ToString(SPDCConstant.DateFormat),
                InvoiceNumber = i.InvoiceNumber,
                InvoiceType = i.InvoiceItems.FirstOrDefault(j => j.InvoiceItemTypeId != (int)InvoiceItemType.Discount).InvoiceItemTypeId.ToString(),
                InvoiceUrl = $"{ConfigHelper.GetByKey("DMZAPI")}/api/Proxy?url=Application/DownloadInvoice?invoiceId={i.Id}",
                ReceiptDate = "",
                ReceiptNumber = "",
                ReceiptUrl = ""
            });

            return result.ToList();
        }
    }
}
