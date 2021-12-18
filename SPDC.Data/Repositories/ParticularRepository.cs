using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IParticularRepository : IRepository<Particular>
    {
    }
    public class ParticularRepository : RepositoryBase<Particular>, IParticularRepository
    {
        public ParticularRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
