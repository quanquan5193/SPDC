using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IUserSubscriptionRepository : IRepository<UserSubscription>
    {
    }
    public class UserSubscriptionRepository : RepositoryBase<UserSubscription>, IUserSubscriptionRepository
    {
        public UserSubscriptionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
