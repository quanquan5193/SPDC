using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.StudentAndClassManagement
{
    public class StudentClassManageFilterBindingModel
    {
        public string CourseCode { get; set; }
        public string AcademicYear { get; set; }
        public string CourseNameEnglish { get; set; }
        public string CourseNameChinese { get; set; }
        public DateTime? ClassCommencementDate { get; set; }
        public DateTime? ClassCompletionDate { get; set; }
        public int? CourseStatus { get; set; }
        public int? StudyMode { get; set; }
        public string StudentPreferredClassCode { get; set; }
        public string AdminAssignedClassCode { get; set; }
        public int? ApplicationStatus { get; set; }
        public string ApplicationNumber { get; set; }
        public string StudenNameEnglish { get; set; }
        public string StudentNameChinese { get; set; }
        public int? InvoiceStatus { get; set; }
        public int? EnrollmentStatus { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;

    }
}
