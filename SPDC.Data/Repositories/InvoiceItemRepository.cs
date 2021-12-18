using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IInvoiceItemRepository : IRepository<InvoiceItem> { }
    public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
