using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;

namespace SPDC.Data.Repositories
{
    public interface IRelevantWorkRepository : IRepository<RelevantWorks>
    {

    }
    public class RelevantWorkRepository : RepositoryBase<RelevantWorks>, IRelevantWorkRepository
    {
        public RelevantWorkRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
