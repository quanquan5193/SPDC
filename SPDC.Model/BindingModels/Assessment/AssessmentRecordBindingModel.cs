using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Assessment
{
    public class AssessmentRecordBindingModel
    {
        public int Id { get; set; }

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

        public int CreditAcquired { get; set; }

        public string Remarks { get; set; }

        public bool EligibleForResitExam { get; set; }

        public List<FileToDelete> ListFileToDelete { get; set; }
    }

    public class FileToDelete
    {
        public int ApplicationId { get; set; }
        
        public int DocumentId { get; set; }
    }
}
