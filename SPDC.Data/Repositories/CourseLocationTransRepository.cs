using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICourseLocationTransRepository : IRepository<CourseLocationTran>
    {
    }

    public class CourseLocationTransRepository : RepositoryBase<CourseLocationTran>, ICourseLocationTransRepository
    {
        public CourseLocationTransRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
