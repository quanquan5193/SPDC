using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class RefundTransaction
    {
        public RefundTransaction()
        {
            RefundTransactionDocuments = new HashSet<RefundTransactionDocument>();
            RefundTransactionApprovedStatusHistories = new HashSet<RefundTransactionApprovedStatusHistory>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int TransactionTypeId { get; set; }

        public int AcceptedBankId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(265)]
        public string RefNo { get; set; }

        public string Remarks { get; set; }

        public string ReasonForRefund { get; set; }

        public int RefundApprovedStatus { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual TransactionType TransactionType { get; set; }

        public virtual AcceptedBank AcceptedBank { get; set; }

        public virtual ICollection<RefundTransactionDocument> RefundTransactionDocuments { get; set; }

        public virtual ICollection<RefundTransactionApprovedStatusHistory> RefundTransactionApprovedStatusHistories { get; set; }
    }
}
