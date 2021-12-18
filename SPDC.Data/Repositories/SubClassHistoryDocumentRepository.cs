using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ISubClassHistoryDocumentRepository : IRepository<SubClassHistoryDocument>
    {
    }
    public class SubClassHistoryDocumentRepository : RepositoryBase<SubClassHistoryDocument>, ISubClassHistoryDocumentRepository
    {
        public SubClassHistoryDocumentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
