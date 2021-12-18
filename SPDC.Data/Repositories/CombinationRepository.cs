using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICombinationRepository : IRepository<ModuleCombination>
    {
    }
    public class CombinationRepository : RepositoryBase<ModuleCombination>, ICombinationRepository
    {
        public CombinationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }

}
