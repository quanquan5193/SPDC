using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Application
{
    public class MyCourseApplicationViewModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string ApplicationNumber { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public float Duration { get; set; }
        public float DurationLesson { get; set; }
        public float DurationTotal { get; set; }
        public string StudyMode { get; set; }

        public float Credits { get; set; }

        public DateTime? LastSubmissionDate { get; set; }

        public int ApplicationStatus { get; set; }
        public string  ApplicationStatusName { get; set; }

        public int InvoiceStatusForCourseFee { get; set; }
        public string InvoiceStatusForCourseFeeName { get; set; }
    }
}
