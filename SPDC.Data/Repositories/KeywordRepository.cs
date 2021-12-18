using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface IKeywordRepository : IRepository<Keyword>
    {

    }

    public class KeywordRepository : RepositoryBase<Keyword>, IKeywordRepository
    {
        public KeywordRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
