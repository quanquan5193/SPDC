using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPDC.Service.Services
{
    public interface IContentService
    {
        IEnumerable<ContentViewModel> GetContents(string contentmanagement, int contenttype, int index, string sortBy, bool isDescending, int size, int typeCode, bool? bulkStatus, out int count/*, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both*/);

        Task<IEnumerable<ContentTypeViewModel>> GetContentType();
    }

    public class ContentService : IContentService
    {
        private IUnitOfWork _unitOfWork;

        private IContentRepository _contentRepository;

        private IContentTypeRepository _contentTypeRepository;

        public ContentService(IUnitOfWork unitOfWork, IContentRepository contentRepository, IContentTypeRepository contentTypeRepository)
        {
            _unitOfWork = unitOfWork;
            _contentRepository = contentRepository;
            _contentTypeRepository = contentTypeRepository;
        }

        public IEnumerable<ContentViewModel> GetContents(string contentmanagement, int contenttype, int index, string sortBy, bool isDescending, int size, int typeCode, bool? bulkStatus, out int count/*, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both*/)
        {
            string typeCodeToSearch = typeCode > 0 && typeCode < 8 ? StaticEnum.GetStringValue((CmsContentType)typeCode) : null;

            var urlImage = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Content/GetImage?id="; 

            var resultcourselocation = _contentRepository.GetMultiWithPaging(contentmanagement,contenttype,index,sortBy,isDescending,size,typeCode,out count/*, applyingFor*/).Select(
                i => new ContentViewModel()
                {
                    ContentID = i.Id,
                    Title = i.Title,
                    DateCreated = i.CreateDate,
                    ContentType = i.CmsContentType != null ? i.CmsContentType.Name : "",
                    AnnoucementDate = i.AnnoucementDate,
                    LastPublishDate = i.LastPublishDate,
                    ApprovalStatus = i.ApproveStatus ? "Approved" : "Unapproved",
                    PublishStatus = i.PublishStatus ? "Published" : "Unpublished",
                    UrlImage = i.CmsImages.FirstOrDefault() != null ? urlImage + i.CmsImages.FirstOrDefault().Id : null,
                    Description = i.Description,
                    ShortDescription = i.ShortDescription,
                    CmsStatus = i.CmsStatus
                }); ;
            return resultcourselocation;
        }

        public async Task<IEnumerable<ContentTypeViewModel>> GetContentType()
        {
            var result = (await _contentTypeRepository.GetMulti(x => true,
               new string[] { })).Select(
                i => new ContentTypeViewModel()
                {
                    ContentTypeID = i.Id,
                    ContentName = i.Name
                });

            return result;
        }
    }
}
