using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;

namespace SPDC.Data.Repositories
{
    public interface IRelevantMembershipRepository : IRepository<RelevantMemberships>
    {

    }
    public class RelevantMembershipRepository : RepositoryBase<RelevantMemberships>, IRelevantMembershipRepository
    {
        public RelevantMembershipRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
