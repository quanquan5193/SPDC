using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CmsContentTypeViewModel
    {
        public CmsContentTypeViewModel()
        {
            BannerBackground = new List<SubListCmsContentTypeViewModel>();
            Announcement = new List<SubListCmsContentTypeViewModel>();
            NewsAndEvents = new List<SubListCmsContentTypeViewModel>();
            PromotionalItems = new List<SubListCmsContentTypeViewModel>();
            OtherInerPages = new List<SubListCmsContentTypeViewModel>();
            InclementWeatherArrangements = new List<SubListCmsContentTypeViewModel>();
            WelcomeMessages = new List<SubListCmsContentTypeViewModel>();
        }
        public List<SubListCmsContentTypeViewModel> BannerBackground { get; set; }
        public List<SubListCmsContentTypeViewModel> Announcement { get; set; }
        public List<SubListCmsContentTypeViewModel> NewsAndEvents { get; set; }
        public List<SubListCmsContentTypeViewModel> PromotionalItems { get; set; }
        public List<SubListCmsContentTypeViewModel> OtherInerPages { get; set; }
        public List<SubListCmsContentTypeViewModel> InclementWeatherArrangements { get; set; }
        public List<SubListCmsContentTypeViewModel> WelcomeMessages { get; set; }

    }
    public class SubListCmsContentTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CmsType { get; set; }
    }
}
