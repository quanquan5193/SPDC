using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IRefundTransactionDocumentRepository : IRepository<RefundTransactionDocument>
    {
    }
    public class RefundTransactionDocumentRepository : RepositoryBase<RefundTransactionDocument>, IRefundTransactionDocumentRepository
    {
        public RefundTransactionDocumentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
