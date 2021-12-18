using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IPaymentTransactionDocumentRepository : IRepository<PaymentTransactionDocument>
    {
    }
    public class PaymentTransactionDocumentRepository: RepositoryBase<PaymentTransactionDocument>, IPaymentTransactionDocumentRepository
    {
        public PaymentTransactionDocumentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
