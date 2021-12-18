using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface ISystemPrivilegeRepository : IRepository<SystemPrivilege>
    {
    }
    public class SystemPrivilegeRepository : RepositoryBase<SystemPrivilege>, ISystemPrivilegeRepository
    {
        public SystemPrivilegeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
