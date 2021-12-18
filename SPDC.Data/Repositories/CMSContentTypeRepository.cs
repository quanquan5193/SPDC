using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICMSContentTypeRepository : IRepository<CmsContentType>
    {
    }
    class CMSContentTypeRepository : RepositoryBase<CmsContentType>, ICMSContentTypeRepository
    {
        public CMSContentTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
