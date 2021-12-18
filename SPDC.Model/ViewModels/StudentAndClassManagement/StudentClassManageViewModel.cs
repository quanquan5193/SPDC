using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.StudentAndClassManagement
{
    public class StudentClassManageViewModel
    {
        public StudentClassManageViewModel()
        {
            ListClasAvaiable = new List<ClassAvaiable>();
        }
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public string CourseCode { get; set; }

        public string StudentPreferredClass { get; set; }

        public int AdminAssignedClass { get; set; }

        public AttendanceOnCapacity AssignedPerClass { get; set; }

        public DateTime SubmissionDate { get; set; }

        public string ApplicationNumber { get; set; }

        public string CICNumber { get; set; }

        public string StudentNameChinese { get; set; }

        public string StudentNameEnglish { get; set; }

        public int ApplicationStatus { get; set; }

        public int? InvoiceStatus { get; set; }

        public int? EnrollmentStatus { get; set; }

        public bool PaymentReminder { get; set; }

        public bool EnrollmentEmailNotification { get; set; }

        public List<ClassAvaiable> ListClasAvaiable { get; set; }
    }

    public class ClassAvaiable
    {
        public int Id { get; set; }

        public string ClassCode { get; set; }
    }

    public class AttendanceOnCapacity
    {
        public int AssignedStudents { get; set; }
        public int Capacity { get; set; }
    }
}
