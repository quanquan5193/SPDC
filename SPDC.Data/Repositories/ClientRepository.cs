using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IClientRepository : IRepository<ClientApplication>
    {

    }
    public class ClientRepository : RepositoryBase<ClientApplication>, IClientRepository
    {
        public ClientRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
