using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IWorkExperienceTransTempRepository : IRepository<WorkExperienceTempTran>
    {
    }
    public class WorkExperienceTransTempRepository : RepositoryBase<WorkExperienceTempTran>, IWorkExperienceTransTempRepository
    {
        public WorkExperienceTransTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
