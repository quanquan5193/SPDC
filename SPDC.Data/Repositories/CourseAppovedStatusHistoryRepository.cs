using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICourseAppovedStatusHistoryRepository : IRepository<CourseAppovedStatusHistory>
    {
    }
    public class CourseAppovedStatusHistoryRepository : RepositoryBase<CourseAppovedStatusHistory>, ICourseAppovedStatusHistoryRepository
    {
        public CourseAppovedStatusHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
