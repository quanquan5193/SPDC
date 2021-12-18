using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;

namespace SPDC.Data.Repositories
{
    public interface IApplicationSetupRepository : IRepository<ApplicationSetups>
    {

    }
    public class ApplicationSetupRepository : RepositoryBase<ApplicationSetups>, IApplicationSetupRepository
    {
        public ApplicationSetupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
