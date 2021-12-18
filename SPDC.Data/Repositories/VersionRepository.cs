using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IVersionRepository : IRepository<VersionManagement>
    {
    }

    public class VersionRepository : RepositoryBase<VersionManagement>, IVersionRepository
    {
        public VersionRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
