using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Enrollment
{
    public class EnrollmentMyClassViewModel
    {
        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public DateTime? ClassCommencementDate { get; set; }

        public DateTime? ClassCompletionDate { get; set; }

        public float Duration { get; set; }

        public string EnrollmentStatus { get; set; }

        public int CreditsAccumulated { get; set; }

        public int ApplicationId { get; set; }

    }

    public class CourseInfomation
    {
        public int CourseId { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public DateTime? GraduationDate { get; set; }

        public string Duration { get; set; }
    }

    public class ScheduleLession
    {
        public string CourseCode { get; set; }
        public string ClassCode { get; set; }
        public DateTime AttendanceDate { get; set; }
    }
}
