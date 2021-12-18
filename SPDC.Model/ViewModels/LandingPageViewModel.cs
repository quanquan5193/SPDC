using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class LandingPageViewModel
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public string Title { get; set; }
        public string SEOUrlLink { get; set; }
    }

    public class LandingPageAnnouncementViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public string SEOUrlLink { get; set; }
    }
}
