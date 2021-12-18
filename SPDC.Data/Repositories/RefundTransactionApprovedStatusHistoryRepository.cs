using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IRefundTransactionApprovedStatusHistoryRepository : IRepository<RefundTransactionApprovedStatusHistory>
    {
    }
    public class RefundTransactionApprovedStatusHistoryRepository : RepositoryBase<RefundTransactionApprovedStatusHistory>, IRefundTransactionApprovedStatusHistoryRepository
    {
        public RefundTransactionApprovedStatusHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
