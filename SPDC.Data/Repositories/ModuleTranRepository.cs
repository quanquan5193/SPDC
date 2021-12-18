using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IModuleTranRepository : IRepository<ModuleTran>
    {
    }
    public class ModuleTranRepository : RepositoryBase<ModuleTran>, IModuleTranRepository
    {
        public ModuleTranRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
