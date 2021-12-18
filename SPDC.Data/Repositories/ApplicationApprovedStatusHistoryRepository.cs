using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IApplicationApprovedStatusHistoryRepository : IRepository<ApplicationApprovedStatusHistory>
    {
    }
    public class ApplicationApprovedStatusHistoryRepository : RepositoryBase<ApplicationApprovedStatusHistory>, IApplicationApprovedStatusHistoryRepository
    {
        public ApplicationApprovedStatusHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
