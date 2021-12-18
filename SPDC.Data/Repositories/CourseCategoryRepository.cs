using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICourseCategoryRepository : IRepository<CourseCategory>
    {
    }
    public class CourseCategoryRepository : RepositoryBase<CourseCategory>, ICourseCategoryRepository
    {
        public CourseCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
