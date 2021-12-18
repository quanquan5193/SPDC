using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IProgrammeLeaderRepository : IRepository<ProgrammeLeader>
    {
    }

    public class ProgrammeLeaderRepository : RepositoryBase<ProgrammeLeader>, IProgrammeLeaderRepository
    {
        public ProgrammeLeaderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
