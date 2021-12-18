using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class ContentViewModel
    {
        public int ContentID { get; set; }

        public DateTime? DateCreated { get; set; }

        public string Title { get; set; }

        public string ContentType { get; set; }
        public DateTime? AnnoucementDate { get; set; }

        public DateTime? LastPublishDate { get; set; }

        public string ApprovalStatus { get; set; }

        public string PublishStatus { get; set; }
        public string UrlImage { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public int CmsStatus { get; set; }
    }

    public class ContentTypeViewModel
    {
        public int ContentTypeID { get; set; }

        public string ContentName { get; set; }
    }
}
