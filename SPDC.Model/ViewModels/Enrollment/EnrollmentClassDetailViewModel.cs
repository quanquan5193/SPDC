using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Enrollment
{
    public class EnrollmentClassDetailViewModel
    {
        public int LessionId { get; set; }

        public string LessionCode { get; set; }

        public string Date { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }


        public string Venue { get; set; }


        public List<EnrollmentCoureMaterial> Documents { get; set; }
    }
}
