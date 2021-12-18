using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IWorkExperienceRepository : IRepository<WorkExperience>
    {
    }
    public class WorkExperienceRepository : RepositoryBase<WorkExperience>, IWorkExperienceRepository
    {
        public WorkExperienceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
