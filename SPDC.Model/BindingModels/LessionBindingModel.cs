using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;

namespace SPDC.Model.BindingModels
{
    public class LessonBindingModel
    {
        public LessonBindingModel()
        {
            Documents = new List<DocumentBindingModel>();
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

        public List<DocumentBindingModel> Documents { get; set; }
    }

}
