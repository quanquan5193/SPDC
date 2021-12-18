using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmsContentType = SPDC.Common.Enums.CmsContentType;

namespace SPDC.Data.Repositories
{

    public interface IContentRepository : IRepository<CmsContent>
    {
        IEnumerable<CmsContent> GetMultiWithPaging(string contentmanagement, int contenttype, int index, string sortBy, bool isDescending, int size, int typeCode,out int count/*, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both*/);
    }

    public class ContentRepository : RepositoryBase<CmsContent>, IContentRepository
    {
        public ContentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<CmsContent> GetMultiWithPaging(string contentmanagement, int contenttype, int index, string sortBy, bool isDescending, int size, int typeCode ,out int count/*, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both*/)
        {
            string typeCodeToSearch = typeCode > 0 && typeCode < 8 ? StaticEnum.GetStringValue((CmsContentType)typeCode) : null;

            var result = dbSet.Include("CmsContentType").Include("CmsImages").Where(x => x.Title.Contains(contentmanagement) 
                                    && (contenttype != 0 ? x.ContentTypeId == contenttype : true) 
                                    && (typeCodeToSearch != null ? x.CmsContentType.CmsType.Equals(typeCodeToSearch) : true));

            if (sortBy.Equals("ContentType"))
            {
                result = isDescending ? result.OrderByDescending(x => x.CmsContentType.Name) : result.OrderBy(x => x.CmsContentType.Name);
            }
            else
            {
                result = result.OrderByCustom(sortBy + " " + (isDescending ? "DESC" : "ASC"));
            }

            count = result.Count();
            return result.Skip(size * (index - 1)).Take(size).ToArray();
        }
    }
}
