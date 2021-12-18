using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IPaymentTransactionRepository : IRepository<PaymentTransaction>
    {
    }
    public class PaymentTransactionRepository : RepositoryBase<PaymentTransaction>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
