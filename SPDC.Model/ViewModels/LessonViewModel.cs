using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPDC.Model.ViewModels
{
    public class LessonViewModel
    {
        public LessonViewModel()
        {
            Documents = new List<DocumentViewModel>();
        }

        public int Id { get; set; } = 0;

        public int ClassId { get; set; }

        public int No { get; set; }

        public DateTime Date { get; set; }

        public int TimeFromMin { get; set; }

        public int TimeFromHrs { get; set; }

        public int TimeToMin { get; set; }

        public int TimeToHrs { get; set; }

        //public string FromTime { get; set; }

        public string Venue { get; set; }

        //public string ToTime { get; set; }

        public int LocationId { get; set; }

        public IList<DocumentViewModel> Documents { get; set; }

    }
}
