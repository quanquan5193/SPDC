using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IUserDocumentRepository : IRepository<UserDocument>
    {
    }
    class UserDocumentRepository : RepositoryBase<UserDocument>, IUserDocumentRepository
    {
        public UserDocumentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
