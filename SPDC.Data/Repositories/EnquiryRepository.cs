using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IEnquiryRepository : IRepository<Enquiry>
    {
    }
    public class EnquiryRepository : RepositoryBase<Enquiry>, IEnquiryRepository
    {
        public EnquiryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
