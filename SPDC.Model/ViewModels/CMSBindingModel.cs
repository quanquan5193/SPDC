using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CMSBindingModel
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
        public bool PublishStatus { get; set; }
        public bool ShowOnLandingPage { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ModifiedByName { get; set; }
        public bool ApproveStatus { get; set; }
        public DateTime? LastPublishDate { get; set; }
        public int CmsImageId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ImagePath { get; set; }
        public int CmsStatus { get; set; }
        public int ApplyingFor { get; set; }
        public string CorrespondingTitle { get; set; }
        public int? MatchedItemId { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}