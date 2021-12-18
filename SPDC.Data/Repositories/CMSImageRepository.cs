using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface ICMSImageRepository : IRepository<CmsImage>
    {
    }
    public class CMSImageRepository : RepositoryBase<CmsImage>, ICMSImageRepository
    {
        public CMSImageRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
