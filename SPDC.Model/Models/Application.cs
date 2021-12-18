using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Applications")]
    public class Application
    {
        public Application()
        {
            ApplicationStatusStorages = new HashSet<ApplicationStatusStorage>();
            ApplicationTrans = new HashSet<ApplicationTran>();
            EnrollmentStatusStorages = new HashSet<EnrollmentStatusStorage>();
            Invoices = new HashSet<Invoice>();
            LessonAttendances = new HashSet<LessonAttendance>();
            //PaymentTransactions = new HashSet<PaymentTransaction>();
            DownloadDocumentTrackings = new HashSet<DownloadDocumentTracking>();
            ApplicationApprovedStatusHistories = new HashSet<ApplicationApprovedStatusHistory>();
            MakeUpAttendences = new HashSet<MakeUpAttendence>();
            ExamApplications = new HashSet<ExamApplication>();
            ResitExamApplications = new HashSet<ResitExamApplication>();
            ApplicationAssessmentDocuments = new HashSet<ApplicationAssessmentDocument>();
            ApplicationAttendanceDocuments = new HashSet<Document>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int UserId { get; set; }

        public string ModuleIds { get; set; }

        public int? ApplicationStatusId { get; set; }

        public int? EnrollmentStatusId { get; set; }

        public int? StudentPreferredClass { get; set; }

        public int? AdminAssignedClass { get; set; }

        public bool? IsAvailable { get; set; }

        public DateTime? GraduationDate { get; set; }

        [StringLength(256)]
        [Index("INDEX_APPNUM", IsUnique = true)]
        public string ApplicationNumber { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public DateTime? ApplicationFirstSubmissionDate { get; set; }

        public DateTime? ApplicationLastSubmissionDate { get; set; }

        public int Status { get; set; }

        public bool IHaveApplyFor { get; set; }

        public string IHaveApplyForText { get; set; }

        public bool IsRequiredRecipt { get; set; }

        public string RemarksAttendance { get; set; }

        public bool EligibleForExam { get; set; }

        public bool EligibleForMakeUpClass { get; set; }

        public bool EligibleForAttendanceCertification { get; set; }

        public DateTime? AttendanceCertificateIssueDate { get; set; }

        public bool EligibleForCourseCompletionCertification { get; set; }

        public DateTime? CourseCompletionCertificateIssueDate { get; set; }

        public int CreditAcquired { get; set; }

        public string RemarksExam { get; set; }

        public bool EligibleForResitExam { get; set; }

        public int FirstExamStatus { get; set; }

        public int SecondExamStatus { get; set; }

        public string EmailStatus { get; set; }

        public int? AttendanceMarks { get; set; }

        public virtual Class StudentPreferredClassModel { get; set; }

        public virtual Class AdminAssignedClassModel { get; set; }

        public virtual Course Course { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<MakeUpAttendence> MakeUpAttendences { get; set; }

        public virtual ICollection<ApplicationStatusStorage> ApplicationStatusStorages { get; set; }

        public virtual ICollection<ApplicationTran> ApplicationTrans { get; set; }

        public virtual ICollection<EnrollmentStatusStorage> EnrollmentStatusStorages { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<LessonAttendance> LessonAttendances { get; set; }

        //public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }

        public virtual ICollection<DownloadDocumentTracking> DownloadDocumentTrackings { get; set; }

        public virtual ICollection<ApplicationApprovedStatusHistory> ApplicationApprovedStatusHistories { get; set; }

        public virtual ICollection<ExamApplication> ExamApplications { get; set; }

        public virtual ICollection<ResitExamApplication> ResitExamApplications { get; set; }

        public virtual ICollection<ApplicationAssessmentDocument> ApplicationAssessmentDocuments { get; set; }

        public virtual ICollection<Document> ApplicationAttendanceDocuments { get; set; }
    }
}
