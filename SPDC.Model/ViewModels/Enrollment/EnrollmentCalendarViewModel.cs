using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Enrollment
{
    public class EnrollmentCalendarViewModel
    {
        public string CourseName { get; set; }

        public string ClassCode { get; set; }

        public DateTime Date { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public string Venue { get; set; }
    }
}
