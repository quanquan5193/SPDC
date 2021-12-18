using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Documents")]
    public class Document
    {
        public Document()
        {
            CourseDocuments = new HashSet<CourseDocument>();
            UserDocuments = new HashSet<UserDocument>();
            UserDocumentsTemps = new HashSet<UserDocumentTemp>();
            CourseHistoryDocuments = new HashSet<CourseHistoryDocument>();
            ClassHistoryDocuments = new HashSet<ClassHistoryDocument>();
            SubClassHistoryDocuments = new HashSet<SubClassHistoryDocument>();
            ApplicationHistoryDocuments = new HashSet<ApplicationHistoryDocument>();
            WaivedHistoryDocuments = new HashSet<WaivedHistoryDocument>();
            PaymentTransactionDocuments = new HashSet<PaymentTransactionDocument>();
            ModifiedDate = DateTime.Now;
            AdditionalClassesApprovals = new HashSet<AdditionalClassesApproval>();
            RefundTransactionDocuments = new HashSet<RefundTransactionDocument>();
            RefundTransactionHistoryDocuments = new HashSet<RefundTransactionHistoryDocument>();
            BatchPayments = new HashSet<BatchPayment>();
            MakeUpClasses = new HashSet<MakeUpClass>();
            ApplicationAssessmentDocuments = new HashSet<ApplicationAssessmentDocument>();
            ApplicationAttendanceDocuments = new HashSet<Application>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Url { get; set; }

        [Required]
        [StringLength(256)]
        public string ContentType { get; set; }

        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CourseDocument> CourseDocuments { get; set; }

        public virtual ICollection<UserDocument> UserDocuments { get; set; }
        public virtual ICollection<UserDocumentTemp> UserDocumentsTemps { get; set; }
        public virtual ICollection<CourseHistoryDocument> CourseHistoryDocuments { get; set; }

        public virtual ICollection<ClassHistoryDocument> ClassHistoryDocuments { get; set; }

        public virtual ICollection<SubClassHistoryDocument> SubClassHistoryDocuments { get; set; }

        public virtual ICollection<ApplicationHistoryDocument> ApplicationHistoryDocuments { get; set; }

        public virtual ICollection<WaivedHistoryDocument> WaivedHistoryDocuments { get; set; }
     
        public virtual ICollection<PaymentTransactionDocument> PaymentTransactionDocuments { get; set; }
        
        public virtual ICollection<AdditionalClassesApproval> AdditionalClassesApprovals { get; set; }

        public virtual ICollection<RefundTransactionDocument> RefundTransactionDocuments { get; set; }

        public virtual ICollection<RefundTransactionHistoryDocument> RefundTransactionHistoryDocuments { get; set; }

        public virtual ICollection<BatchPayment> BatchPayments { get; set; }

        public virtual ICollection<MakeUpClass> MakeUpClasses { get; set; }

        public virtual ICollection<ApplicationAssessmentDocument> ApplicationAssessmentDocuments { get; set; }
        
        public virtual ICollection<Application> ApplicationAttendanceDocuments { get; set; }
    }

}
