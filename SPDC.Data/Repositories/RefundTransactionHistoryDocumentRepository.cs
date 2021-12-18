using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IRefundTransactionHistoryDocumentRepository : IRepository<RefundTransactionHistoryDocument> 
    {
    }
    public class RefundTransactionHistoryDocumentRepository:RepositoryBase<RefundTransactionHistoryDocument>, IRefundTransactionHistoryDocumentRepository
    {
        public RefundTransactionHistoryDocumentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
