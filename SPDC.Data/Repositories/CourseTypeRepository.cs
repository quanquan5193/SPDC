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

    public interface ICourserTypeRepository : IRepository<CourseType>
    {
    }
    public class CourseTypeRepository : RepositoryBase<CourseType>, ICourserTypeRepository
    {

        public CourseTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
