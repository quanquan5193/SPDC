using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IWorkExperienceTempRepository : IRepository<WorkExperienceTemp>
    {
    }
    public class WorkExperienceTempRepository : RepositoryBase<WorkExperienceTemp>, IWorkExperienceTempRepository
    {
        public WorkExperienceTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
