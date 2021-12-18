using SPDC.Data.Infrastructure;
using SPDC.Model.BindingModels.Assessment;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.Assessment;
using SPDC.Model.ViewModels.BatchApplication;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPDC.Common.StaticConfig;

namespace SPDC.Data.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        IEnumerable<CoursePortalAdminViewModel> GetCoursesPaging(int langId, IEnumerable<int> coursecategories, string coursecode, string coursenameEN, string coursenameTC, bool? displaycourse, int index, string sortBy, bool isDescending, int size, string[] includes, out int count);

        IEnumerable<BatchApplicationCourse> GetAllCourseCode();

        Task<IEnumerable<CourseAssessmentViewModel>> GetCourseAssessment(CourseAssessmentFilter filter);
    }
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<BatchApplicationCourse> GetAllCourseCode()
        {
            return dbSet.Include("TargetClasses")
                .Where(x => true)
                .Select(x => new BatchApplicationCourse()
                {
                    Id = x.Id,
                    CourseCode = x.CourseCode,
                    IsHaveTargetClass = x.TargetClasses != null,
                    CourseNameEN = x.CourseTrans.Where(y => y.LanguageId == (int)LanguageCode.EN).FirstOrDefault().CourseName,
                    CourseNameTC = x.CourseTrans.Where(y => y.LanguageId == (int)LanguageCode.HK).FirstOrDefault().CourseName
                }).ToArray();
        }

        public IEnumerable<CoursePortalAdminViewModel> GetCoursesPaging(int langId, IEnumerable<int> coursecategories, string coursecode, string coursenameEN, string coursenameTC, bool? displaycourse, int index, string sortBy, bool isDescending, int size, string[] includes, out int count)
        {
            var query = dbSet.Include(includes.First());
            foreach (var include in includes.Skip(1))
                query = query.Include(include);

            if (coursecategories.Count() > 0)
            {
                query = query.Where(x => coursecategories.Contains(x.CategoryId));
            }

            if (!string.IsNullOrWhiteSpace(coursecode))
            {
                query = query.Where(x => x.CourseCode.Contains(coursecode));
            }

            if (displaycourse.HasValue)
            {
                query = query.Where(x => x.InvisibleOnWebsite == displaycourse);
            }

            if (!string.IsNullOrWhiteSpace(coursenameEN))
            {
                query = query.Where(x => x.CourseTrans.Where(y => y.LanguageId == (int)LanguageCode.EN).FirstOrDefault().CourseName.Contains(coursenameEN));
            }

            if (!string.IsNullOrWhiteSpace(coursenameTC))
            {
                query = query.Where(x => x.CourseTrans.Where(y => y.LanguageId == (int)LanguageCode.HK).FirstOrDefault().CourseName.Contains(coursenameTC));
            }

            var resultNoOrder = query.ToArray();
            count = resultNoOrder.Length;
            var resultWithOrder = resultNoOrder
                                .Select(i => new CoursePortalAdminViewModel()
                                {
                                    CourseID = i.Id,
                                    CourseCategory = i.CourseCategory == null ? string.Empty : (i.CourseCategory.CourseCategorieTrans.Count == 0 ? string.Empty : i.CourseCategory.CourseCategorieTrans.Where(x => x.LanguageId == langId).FirstOrDefault().Name),
                                    CourseCode = i.CourseCode,
                                    CourseNameEN = i.CourseTrans.Count == 0 ? string.Empty : (i.CourseTrans.Where(x => x.LanguageId == (int)LanguageCode.EN).FirstOrDefault() != null ? i.CourseTrans.Where(x => x.LanguageId == (int)LanguageCode.EN).FirstOrDefault().CourseName : string.Empty),
                                    CourseNameCN = i.CourseTrans.Count == 0 ? string.Empty : (i.CourseTrans.Where(x => x.LanguageId == (int)LanguageCode.HK).FirstOrDefault() != null ? i.CourseTrans.Where(x => x.LanguageId == (int)LanguageCode.HK).FirstOrDefault().CourseName : string.Empty),
                                    Duration = i.DurationTotal,
                                    CourseFee = i.CourseFee,
                                    TargetClass = i.TargetClassSize,
                                    WithExam = i.CanApplyForReExam,//TODO không có giá trị WithExam trong bảng course
                                    WithModule = i.ByModule,
                                    DisplayCourseInformation = i.InvisibleOnWebsite ?? false,
                                    CourseApprovedStatus = i.CourseApprovedStatus
                                })
                                .OrderByCustom($"{sortBy} {(isDescending ? "DESC" : "ASC")}")
                                .Skip((index - 1) * size).Take(size);
            return resultWithOrder;
        }

        public async Task<IEnumerable<CourseAssessmentViewModel>> GetCourseAssessment(CourseAssessmentFilter filter)
        {
            var query = dbSet.Include("CourseTrans");
            if (!string.IsNullOrWhiteSpace(filter.CourseNameEN))
            {
                query = query.Where(x => x.CourseTrans.Any(c => c.CourseName.ToLower().Contains(filter.CourseNameEN) && c.LanguageId == (int)LanguageCode.EN));
            }
            if (!string.IsNullOrWhiteSpace(filter.CourseNameCN))
            {
                query = query.Where(x => x.CourseTrans.Any(c => c.CourseName.ToLower().Contains(filter.CourseNameCN) && (c.LanguageId == (int)LanguageCode.CN || c.LanguageId == (int)LanguageCode.HK)));
            }
            if (!string.IsNullOrWhiteSpace(filter.AcademicYear))
            {
                query = query.Where(x => x.TargetClasses.AcademicYear.Equals(filter.AcademicYear));
            }

            var result = (await query.ToArrayAsync()).Select(x => new CourseAssessmentViewModel()
            {
                CourseId = x.Id,
                CourseCode = x.CourseCode
            });

            return result;
        }
    }
}
