using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Assessment
{

    public class AssessmentTableViewModel
    {
        public AssessmentTableViewModel()
        {
            ListAssessmentRecord = new List<AssessmentRecordViewModel>();
        }

        public int CurrentClassStatus { get; set; }

        public int AttendanceRequirement { get; set; }

        public int CommonId { get; set; }

        public int ExamPassingRequirement { get; set; }

        public byte? MaxReExamCount { get; set; }

        public int? ExamPassingRequirementSuffix { get; set; }

        public List<AssessmentRecordViewModel> ListAssessmentRecord { get; set; }
    }

    public class AssessmentRecordViewModel
    {
        public AssessmentRecordViewModel()
        {
            ListSupportingDocument = new List<FileViewModel>();
        }
        public int ApplicationId { get; set; }

        public string StudentNo { get; set; }

        public string ChineseName { get; set; }

        public string EnglishName { get; set; }

        public string CICno { get; set; }

        public int AttendancePercentage { get; set; }

        public int AttendanceLesson { get; set; }

        public double AttendanceHours { get; set; }

        public bool EligibleForAttendance { get; set; }

        public DateTime? AttendanceCertificateIssueDate { get; set; }

        public int? ExamAssessmentMarks { get; set; }

        public int? ExamAssessmentResult { get; set; }

        public int? FirstExamAssessmentMarks { get; set; }

        public int? FirstExamAssessmentResult { get; set; }

        public int? SecondExamAssessmentMarks { get; set; }

        public int? SecondExamAssessmentResult { get; set; }

        public int? EnrollmentStatus { get; set; }

        public bool EligibleForCourseCompletion { get; set; }

        public DateTime? CourseCompletionCertificateIssueDate { get; set; }

        public float Credit { get; set; }

        public int CreditAcquired { get; set; }

        public string Remarks { get; set; }

        public bool EligibleForResitExam { get; set; }

        public List<FileViewModel> ListSupportingDocument { get; set; }

        public int? FirstReExamInvoiceStatus { get; set; }

        public int? SecondReExamInvoiceStatus { get; set; }

        public string[] EmailStatus { get; set; }

        public DateTime? FirstReExamDate { get; set; }

        public DateTime? SecondReExamDate { get; set; }
    }
}
