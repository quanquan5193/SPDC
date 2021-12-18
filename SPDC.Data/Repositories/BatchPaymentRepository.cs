using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IBatchPaymentRepository : IRepository<BatchPayment> { };
    public class BatchPaymentRepository : RepositoryBase<BatchPayment>, IBatchPaymentRepository
    {
        public BatchPaymentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
