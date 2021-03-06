using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IAcceptedBankRepository : IRepository<AcceptedBank>
    {

    }
    public class AcceptedBankRepository: RepositoryBase<AcceptedBank>, IAcceptedBankRepository
    {
        public AcceptedBankRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
