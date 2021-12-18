using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class RawCMSViewModel
    {
        public int Id { get; set; }

        public int ContentTypeId { get; set; }

        public string Title { get; set; }

        public string SEOUrlLink { get; set; }

        public string ImageSEO { get; set; }

        public DateTime? AnnoucementDate { get; set; }

        public string Description { get; set; }

        public string FullDescription { get; set; }

        public string ShortDescription { get; set; }

        public int CmsStatus { get; set; }

        public bool ApproveStatus { get; set; }

        public bool PublishStatus { get; set; }

        public DateTime? LastPublishDate { get; set; }

        public bool ShowOnLandingPage { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int LastModifiedBy { get; set; }

        public int ApplyingFor { get; set; }

        public RawCmsContentType CmsContentType { get; set; }

        public ICollection<RawCmsImage> CmsImages { get; set; }
    }

    public class RawCmsContentType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NameTC { get; set; }

        public string NameSC { get; set; }

        public string CmsType { get; set; }
    }

    public class RawCmsImage
    {
        public int Id { get; set; }

        public int? CmsId { get; set; }

        public string Url { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
