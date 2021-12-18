using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IAdminPermissionRepository : IRepository<AdminPermission>
    {

    }

    public class AdminPermissionRepository: RepositoryBase<AdminPermission>, IAdminPermissionRepository
    {
        public AdminPermissionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
