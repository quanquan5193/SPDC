using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;

namespace SPDC.Data.Repositories
{

    public interface ITargetClassRepository : IRepository<TargetClasses>
    {

    }

    public class TargetClassRepository : RepositoryBase<TargetClasses>, ITargetClassRepository
    {
        public TargetClassRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
