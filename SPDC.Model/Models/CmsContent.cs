using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CmsContents")]
    public class CmsContent
    {
        public CmsContent()
        {
            CmsImages = new HashSet<CmsImage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ContentTypeId { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(256)]
        public string SEOUrlLink { get; set; }

        [StringLength(256)]
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

        public int? MatchedItemId { get; set; }

        public int? OrderNumber { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual CmsContentType CmsContentType { get; set; }

        public virtual ICollection<CmsImage> CmsImages { get; set; }
    }
}
