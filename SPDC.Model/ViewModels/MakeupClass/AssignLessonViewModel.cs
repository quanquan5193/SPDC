using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.MakeupClass
{
    public class AssignLessonViewModel
    {
        public int Id { get; set; }
        public string ClassCode { get; set; }
        public string Date { get; set; }
        public int TimeFromHrs { get; set; }
        public int TimeToHrs { get; set; }
        public int TimeFromMin { get; set; }
        public int TimeToMin { get; set; }
        public string Venue { get; set; }
    }
}
