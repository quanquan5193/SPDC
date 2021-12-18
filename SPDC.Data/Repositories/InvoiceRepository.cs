using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Invoice GetLastInvoice();
    }
    public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public Invoice GetLastInvoice()
        {
            return dbSet.OrderByDescending(i => i.Id).FirstOrDefault();
        }
    }
}
