using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface IApplicationTransRepository : IRepository<ApplicationTran>
    {

    }

    public class ApplicationTransRepository : RepositoryBase<ApplicationTran>, IApplicationTransRepository
    {
        public ApplicationTransRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
