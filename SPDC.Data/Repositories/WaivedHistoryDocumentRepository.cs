using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IWaivedHistoryDocumentRepository : IRepository<WaivedHistoryDocument>
    {
    }
    public class WaivedHistoryDocumentRepository : RepositoryBase<WaivedHistoryDocument>, IWaivedHistoryDocumentRepository
    {
        public WaivedHistoryDocumentRepository(IDbFactory dbFactory) : base (dbFactory)
        {
        }
    }
}
