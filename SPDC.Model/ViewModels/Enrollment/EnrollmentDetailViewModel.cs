using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Enrollment
{
    public class EnrollmentDetailViewModel
    {
        public string AcademicYear { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public string CourseName_Chi { get; set; }

        public string EnrollmentStatus { get; set; }

        public int CreditsAccquired { get; set; }

        public int? ClassId { get; set; }
    }
}
