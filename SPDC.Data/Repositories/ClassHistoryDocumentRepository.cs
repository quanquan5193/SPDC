using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IClassHistoryDocumentRepository : IRepository<ClassHistoryDocument>
    {
    }
    public class ClassHistoryDocumentRepository : RepositoryBase<ClassHistoryDocument>, IClassHistoryDocumentRepository
    {
        public ClassHistoryDocumentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
