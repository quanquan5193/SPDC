using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Enrollment
{
    public class EnrollmentExamDetailViewModel
    {
        public string ExamType { get; set; }

        public string Date { get; set; }

        public string Venue { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }

        public int? ExamStatus { get; set; }

        public int? Score { get; set; }

        public string ReExamApplicationDeadline { get; set; }
    }
}
