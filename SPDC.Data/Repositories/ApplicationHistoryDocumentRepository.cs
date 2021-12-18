using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IApplicationHistoryDocumentRepository : IRepository<ApplicationHistoryDocument>
    {
    }
    public class ApplicationHistoryDocumentRepository : RepositoryBase<ApplicationHistoryDocument>, IApplicationHistoryDocumentRepository
    {
        public ApplicationHistoryDocumentRepository(IDbFactory dbFactory): base(dbFactory)
        {
        }
    }
}
