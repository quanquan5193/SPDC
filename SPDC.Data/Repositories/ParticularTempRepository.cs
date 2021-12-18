using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IParticularTempRepository : IRepository<ParticularTemp>
    {
    }
    public class ParticularTempRepository : RepositoryBase<ParticularTemp>, IParticularTempRepository
    {
        public ParticularTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
