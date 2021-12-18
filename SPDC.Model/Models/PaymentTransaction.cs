using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("PaymentTransactions")]
    public class PaymentTransaction
    {
        public PaymentTransaction()
        {
            PaymentTransactionDocuments = new HashSet<PaymentTransactionDocument>();
            //PaymentTransactionTrans = new HashSet<PaymentTransactionTran>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int TransactionType { get; set; }

        //public int UserId { get; set; }

        //public int ApplicationId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(265)]
        public string RefNo { get; set; }

        //public virtual Application Application { get; set; }
        public int AcceptedBankId { get; set; }

        public string Remarks { get; set; }

        public string ReasonForRefund { get; set; }

        public virtual TransactionType TransactionType1 { get; set; }

        //public virtual ApplicationUser User { get; set; }

        //public virtual ICollection<PaymentTransactionTran> PaymentTransactionTrans { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual AcceptedBank AcceptedBank { get; set; }

        public virtual ICollection<PaymentTransactionDocument> PaymentTransactionDocuments { get; set; }
    }
}