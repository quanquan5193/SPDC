using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{

    public interface ICMSContentRepository : IRepository<CmsContent>
    {
        Task<IEnumerable<CmsContent>> GetAllContentAndSort(int type, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both);
    }

    public class CMSContentRepository : RepositoryBase<CmsContent>, ICMSContentRepository
    {
        public CMSContentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<IEnumerable<CmsContent>> GetAllContentAndSort(int type, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            var today = DateTime.Now;
            int bothContentApply = (int)Common.Enums.ContentApplyingFor.Both;
            var result = dbSet
                            .Where(x =>
                                    x.CmsContentType.Id == type
                                    && x.CmsStatus == (int)Common.Enums.CMSPublishType.Publish
                                    && (x.ApplyingFor == applyingFor || x.ApplyingFor == bothContentApply)
                                    && (x.ReleaseDate < DateTime.Now && x.EndDate > DateTime.Now)
                            ).OrderByDescending(c => c.LastPublishDate);
            return await result.ToListAsync();
        }
    }
}
