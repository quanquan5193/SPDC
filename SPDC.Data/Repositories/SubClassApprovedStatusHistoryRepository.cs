using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ISubClassApprovedStatusHistoryRepository : IRepository<SubClassApprovedStatusHistory>
    {
    }
    public class SubClassApprovedStatusHistoryRepository : RepositoryBase<SubClassApprovedStatusHistory>, ISubClassApprovedStatusHistoryRepository
    {
        public SubClassApprovedStatusHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
