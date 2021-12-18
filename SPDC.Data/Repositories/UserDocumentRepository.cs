using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IUserDocumentTempRepository : IRepository<UserDocumentTemp>
    {
    }
    class UserDocumentTempRepository : RepositoryBase<UserDocumentTemp>, IUserDocumentTempRepository
    {
        public UserDocumentTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
