using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IParticularTempTranRepository : IRepository<ParticularTempTran>
    {
    }
    public class ParticularTempTranRepository : RepositoryBase<ParticularTempTran>, IParticularTempTranRepository
    {
        public ParticularTempTranRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
