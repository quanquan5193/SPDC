using SPDC.Common;
using SPDC.Data.Infrastructure;
using SPDC.Model.BindingModels.MakeupClass;
using SPDC.Model.BindingModels.StudentAndClassManagement;
using SPDC.Model.Models;
using SPDC.Model.ViewModels.BatchPayment;
using SPDC.Model.ViewModels.Enrollment;
using SPDC.Model.ViewModels.StudentAndClassManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPDC.Common.StaticConfig;

namespace SPDC.Data.Repositories
{

    public interface IApplicationRepository : IRepository<Application>
    {
        Task<PaginationSet<StudentClassManageViewModel>> SearchApplicationManagementByFilter(StudentClassManageFilterBindingModel filter, int classPreferId, int classAdminAssignId);
        //EnrollmentMyCourseViewModel GetCourseEnrollment(int userId, int langCode);
        BatchPaymentItemViewModel GetBatchPaymentItemOfferStatus(int userId, bool isChineseName, int? selectedApplicationId = null);
        BatchPaymentItemViewModel GetBatchPaymentItem(int userId, bool isChineseName, int? selectedApplicationId = null);

        IEnumerable<Application> SearchAllAttendance(AttendanceSearchBindingModel model);

        void Commit();
    }


    public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
    {
        public ApplicationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<PaginationSet<StudentClassManageViewModel>> SearchApplicationManagementByFilter(StudentClassManageFilterBindingModel filter, int classPreferId, int classAdminAssignId)
        {
            int skipCount = filter.Page < 0 ? 0 : (filter.Page - 1) * filter.Size;
            var tempCommencementDate = filter.ClassCommencementDate ?? DateTime.Now;
            var tempClassCompletionDate = filter.ClassCompletionDate ?? DateTime.Now;

            var query = dbSet.Include("StudentPreferredClassModel.Course")
                .Include("AdminAssignedClassModel")
                .Include("EnrollmentStatusStorages")
                //.Include("AdminAssignedClassModel.AdminAssignedApplicationModels")
                .Include("User.Particular")
                .Include("Invoices.InvoiceItems")
                .Include("Course")
                .Include("Course.Classes");

            #region filter code logic
            if (!string.IsNullOrWhiteSpace(filter.CourseCode))
            {
                query = query.Where(x => x.Course.CourseCode.Equals(filter.CourseCode));
            }
            if (!string.IsNullOrWhiteSpace(filter.AcademicYear))
            {
                query = query.Where(x => x.Course.TargetClasses.AcademicYear.Equals(filter.AcademicYear));
            }
            if (!string.IsNullOrWhiteSpace(filter.CourseNameEnglish))
            {
                query = query.Where(x => x.Course.CourseTrans.Any(c => c.CourseName.ToLower().Contains(filter.CourseNameEnglish.ToLower()) && c.LanguageId == (int)LanguageCode.EN));
            }
            if (!string.IsNullOrWhiteSpace(filter.CourseNameChinese))
            {
                query = query.Where(x => x.Course.CourseTrans.Any(c => c.CourseName.ToLower().Contains(filter.CourseNameChinese.ToLower()) && (c.LanguageId == (int)LanguageCode.CN || c.LanguageId == (int)LanguageCode.HK)));
            }
            if (filter.ClassCommencementDate.HasValue)
            {
                query = query.Where(x => x.AdminAssignedClassModel.CommencementDate.Day == tempCommencementDate.Day && x.Course.CommencementDate.Month == tempCommencementDate.Month && x.Course.CommencementDate.Year == tempCommencementDate.Year);
            }
            if (filter.ClassCompletionDate.HasValue)
            {
                query = query.Where(x => x.AdminAssignedClassModel.CompletionDate.Day == tempClassCompletionDate.Day && x.Course.CompletionDate.Month == tempClassCompletionDate.Month && x.Course.CompletionDate.Year == tempClassCompletionDate.Year);
            }
            if (filter.CourseStatus != null)
            {
                query = query.Where(x => x.Course.Status == filter.CourseStatus);
            }
            if (filter.StudyMode != null)
            {
                query = query.Where(x => x.Course.CourseTypeId == filter.StudyMode);
            }
            if (!string.IsNullOrWhiteSpace(filter.StudentPreferredClassCode) && classPreferId != 0)
            {
                query = query.Where(x => x.StudentPreferredClass == classPreferId);
            }
            if (!string.IsNullOrWhiteSpace(filter.AdminAssignedClassCode))
            {
                query = query.Where(x => x.AdminAssignedClass == classAdminAssignId);
            }

            if (filter.ApplicationStatus != null)
            {
                query = query.Where(x => x.Status == filter.ApplicationStatus);
            }
            if (!string.IsNullOrWhiteSpace(filter.ApplicationNumber))
            {
                query = query.Where(x => x.ApplicationNumber.Equals(filter.ApplicationNumber));
            }

            if (!string.IsNullOrWhiteSpace(filter.StudenNameEnglish))
            {
                query = query.Where(x => (x.User.Particular.SurnameEN + x.User.Particular.GivenNameEN).Contains(filter.StudenNameEnglish));
            }
            if (!string.IsNullOrWhiteSpace(filter.StudentNameChinese))
            {
                query = query.Where(x => (x.User.Particular.SurnameCN + x.User.Particular.GivenNameCN).Contains(filter.StudentNameChinese));
            }
            if (filter.InvoiceStatus != null)
            {
                query = query.Where(x => x.Invoices.Count > 0 && x.Invoices.OrderByDescending(i => i.Id).FirstOrDefault().Status == filter.InvoiceStatus);
            }
            #endregion

            var sortExpression = "ApplicationLastSubmissionDate DESC";
            var listApplicationManagement = (await query.OrderByCustom(sortExpression).Skip(skipCount).Take(filter.Size).ToListAsync())
                                            .Select(v => v.ToStudentClassManageViewModel());

            var total = query.Count();

            return new PaginationSet<StudentClassManageViewModel>()
            {
                Items = listApplicationManagement,
                Page = filter.Page,
                TotalCount = total
            };
        }

        //public EnrollmentMyCourseViewModel GetCourseEnrollment(int userId, int langCode)
        //{
        //    EnrollmentMyCourseViewModel result = new EnrollmentMyCourseViewModel();
        //    var query = dbSet.Include("Course").Include("ApplicationStatusStorages").Include("StudentPreferredClassModel").Include("StudentPreferredClassModel.Lessons");

        //    result.CourseEnrolled = query.Where(x => x.EnrollmentStatusStorages.Any(c => c.Status == (int)Common.Enums.EnrollmentStatus.Enrolled)).Select(v => new CourseInfomation()
        //    {
        //        CourseId = v.CourseId,
        //        CourseCode = v.Course.CourseCode,
        //        CourseName = v.Course.CourseTrans.FirstOrDefault(b => b.LanguageId == langCode).CourseName,
        //        Duration = v.Course.DurationHrs + " hours",
        //        GraduationDate = v.GraduationDate
        //    }).ToList();

        //    result.CourseCompleted = query.Where(x => x.EnrollmentStatusStorages.Any(c => c.Status == (int)Common.Enums.EnrollmentStatus.Completed)).Select(v => new CourseInfomation()
        //    {
        //        CourseId = v.CourseId,
        //        CourseCode = v.Course.CourseCode,
        //        CourseName = v.Course.CourseTrans.FirstOrDefault(b => b.LanguageId == langCode).CourseName,
        //        Duration = v.Course.DurationHrs + " hours",
        //        GraduationDate = v.GraduationDate
        //    }).ToList();

        //    result.CourseGraduated = query.Where(x => x.EnrollmentStatusStorages.Any(c => c.Status == (int)Common.Enums.EnrollmentStatus.Graduated)).Select(v => new CourseInfomation()
        //    {
        //        CourseId = v.CourseId,
        //        CourseCode = v.Course.CourseCode,
        //        CourseName = v.Course.CourseTrans.FirstOrDefault(b => b.LanguageId == langCode).CourseName,
        //        Duration = v.Course.DurationHrs + " hours",
        //        GraduationDate = v.GraduationDate
        //    }).ToList();

        //    result.CourseResit = query.Where(x => x.EnrollmentStatusStorages.Any(c => c.Status == (int)Common.Enums.EnrollmentStatus.Resit)).Select(v => new CourseInfomation()
        //    {
        //        CourseId = v.CourseId,
        //        CourseCode = v.Course.CourseCode,
        //        CourseName = v.Course.CourseTrans.FirstOrDefault(b => b.LanguageId == langCode).CourseName,
        //        Duration = v.Course.DurationHrs + " hours",
        //        GraduationDate = v.GraduationDate
        //    }).ToList();

        //    result.CourseDropped = query.Where(x => x.EnrollmentStatusStorages.Any(c => c.Status == (int)Common.Enums.EnrollmentStatus.Dropped)).Select(v => new CourseInfomation()
        //    {
        //        CourseId = v.CourseId,
        //        CourseCode = v.Course.CourseCode,
        //        CourseName = v.Course.CourseTrans.FirstOrDefault(b => b.LanguageId == langCode).CourseName,
        //        Duration = v.Course.DurationHrs + " hours",
        //        GraduationDate = v.GraduationDate
        //    }).ToList();

        //    result.ListScheduleEachLession = query.Where(x => x.EnrollmentStatusStorages.Any(c => c.Status == (int)Common.Enums.EnrollmentStatus.Enrolled)).SelectMany(v =>
        //    v.StudentPreferredClassModel.Lessons.Select(b => new ScheduleLession()
        //    {
        //        CourseCode = v.Course.CourseCode,
        //        ClassCode = v.StudentPreferredClassModel.ClassCode,
        //        AttendanceDate = b.Date
        //    })).ToList();

        //    return result;
        //}

        public BatchPaymentItemViewModel GetBatchPaymentItemOfferStatus(int userId, bool isChineseName, int? selectedApplicationId = null)
        {
            var query = dbSet.Include("Invoices.PaymentTransactions").Include("User.Particular").Include("Course.TargetClasses");

            query = query.Where(x => x.UserId == userId
                                    && ((x.Invoices.OrderByDescending(y => y.Id).FirstOrDefault().Status == (int)Common.Enums.InvoiceStatus.Offered)
                                    && (x.Invoices.OrderByDescending(y => y.Id).FirstOrDefault().InvoiceItems.Any(z => z.InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee)))
                                    || (selectedApplicationId.HasValue ? x.Id == selectedApplicationId : false)
                                    );

            var result1 = query.ToList().GroupBy(x => x.User).FirstOrDefault();
            var result = result1.ApplicationToBatchPaymentItemViewModel(isChineseName, selectedApplicationId);

            return result;
        }

        public BatchPaymentItemViewModel GetBatchPaymentItem(int userId, bool isChineseName, int? selectedApplicationId = null)
        {
            var query = dbSet.Include("Invoices.PaymentTransactions").Include("User.Particular").Include("Course.TargetClasses");

            query = query.Where(x => x.UserId == userId
                                    && ((x.Invoices.OrderByDescending(y => y.Id).FirstOrDefault().Status == (int)Common.Enums.InvoiceStatus.Offered
                                    || x.Invoices.OrderByDescending(y => y.Id).FirstOrDefault().Status == (int)Common.Enums.InvoiceStatus.PaidPartially)
                                    && (x.Invoices.OrderByDescending(y => y.Id).FirstOrDefault().InvoiceItems.Any(z => z.InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee)))
                                    || (selectedApplicationId.HasValue ? x.Id == selectedApplicationId : false)
                                    );

            var result1 = query.ToList().GroupBy(x => x.User).FirstOrDefault();
            var result = result1.ApplicationToBatchPaymentItemViewModel(isChineseName, selectedApplicationId);

            return result;
        }

        public IEnumerable<Application> SearchAllAttendance(AttendanceSearchBindingModel model)
        {
            IQueryable<Application> query = dbSet
                .Include("User").Include("User.Particular")
                .Include("AdminAssignedClassModel")
                .Include("EnrollmentStatusStorages")
                .Include("LessonAttendances").Include("LessonAttendances.Lesson")
                .Include("ApplicationAttendanceDocuments")
                .Include("MakeUpAttendences")
                .Include("MakeUpAttendences.MakeUpClass");

            int enrolledStatus = (int)Common.Enums.EnrollmentStatus.Enrolled;
            query = query.Where(x => x.CourseId == model.CourseId
                                     && x.AdminAssignedClass == model.ClassId
                                     && x.EnrollmentStatusStorages.Count(s => s.Status == enrolledStatus) > 0);

            #region Add IQueryable filter
            if (!string.IsNullOrWhiteSpace(model.AcademicYear))
            {
                query = query.Where(x => x.Course.TargetClasses.AcademicYear == model.AcademicYear);
            }
            if (!string.IsNullOrWhiteSpace(model.CourseNameEN))
            {
                query = query.Where(x => x.Course.CourseTrans.FirstOrDefault(y => y.LanguageId == (int)LanguageCode.EN).CourseName == model.CourseNameEN);
            }
            if (!string.IsNullOrWhiteSpace(model.CourseNameCN))
            {
                query = query.Where(x => x.Course.CourseTrans.FirstOrDefault(y => y.LanguageId == (int)LanguageCode.HK).CourseName == model.CourseNameCN);
            }
            if (!string.IsNullOrWhiteSpace(model.StudentNameEN))
            {
                query = query.Where(x => (x.User.Particular.SurnameEN + " " + x.User.Particular.GivenNameEN).Contains(model.StudentNameEN));
            }
            if (!string.IsNullOrWhiteSpace(model.StudentNameCN))
            {
                query = query.Where(x => (x.User.Particular.SurnameCN + " " + x.User.Particular.GivenNameCN).Contains(model.StudentNameCN));
            }
            if (!string.IsNullOrWhiteSpace(model.StudentNo))
            {
                query = query.Where(x => x.ApplicationNumber.Contains(model.StudentNo));
            }
            if (!string.IsNullOrWhiteSpace(model.CICNo))
            {
                query = query.Where(x => x.User.CICNumber.Equals(model.CICNo));
            }

            string hkidNoEncrypted = EncryptUtilities.EncryptAes256ToString(model.HKIDNo);
            if (!string.IsNullOrWhiteSpace(hkidNoEncrypted))
            {
                query = query.Where(x => x.User.Particular.HKIDNoEncrypted.Equals(hkidNoEncrypted));
            }

            string passportNoEncrypted = EncryptUtilities.EncryptAes256ToString(model.PassportNo);
            if (!string.IsNullOrWhiteSpace(passportNoEncrypted))
            {
                query = query.Where(x => x.User.Particular.PassportNoEncrypted.Equals(passportNoEncrypted));
            }
            #endregion


            var result = query.ToArray().AsEnumerable();
            return result;
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
