using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Invoices")]
    public class Invoice
    {
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
            InvoiceStatusStorages = new HashSet<InvoiceStatusStorage>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
            WaiverApprovedStatusHistories = new HashSet<WaiverApprovedStatusHistory>();
            RefundTransactions = new HashSet<RefundTransaction>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        //public int InvoiceType { get; set; }

        public int Status { get; set; }

        [Required]
        [StringLength(50)]
        [Index("INDEX_INVOICENUM",  IsUnique = true)]
        public string InvoiceNumber { get; set; }

        public decimal Fee { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public bool RequiresHardCopyReceipt { get; set; }

        public DateTime PaymentDueDate { get; set; }

        public int WaiverApprovedStatus { get; set; }

        public int? TypeReExam { get; set; }

        public virtual Application Application { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

        public virtual InvoiceStatus InvoiceStatus { get; set; }

        public virtual ICollection<InvoiceStatusStorage> InvoiceStatusStorages { get; set; }

        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }

        public virtual ICollection<WaiverApprovedStatusHistory> WaiverApprovedStatusHistories { get; set; }

        public virtual ICollection<RefundTransaction> RefundTransactions { get; set; }
    }
}
