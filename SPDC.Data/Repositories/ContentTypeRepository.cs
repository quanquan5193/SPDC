using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface IContentTypeRepository : IRepository<CmsContentType>
    {
    }

    public class ContentTypeRepository : RepositoryBase<CmsContentType>, IContentTypeRepository
    {
        public ContentTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}