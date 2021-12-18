using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface INotificationUserRepository : IRepository<NotificationUser>
    {

    }

    public class NotificationUserRepository : RepositoryBase<NotificationUser>, INotificationUserRepository
    {
        public NotificationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
