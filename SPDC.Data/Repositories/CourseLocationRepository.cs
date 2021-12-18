using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface ICourseLocationRepository : IRepository<CourseLocation>
    {
    }

    public class CourseLocationRepository : RepositoryBase<CourseLocation>, ICourseLocationRepository
    {
        public CourseLocationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
