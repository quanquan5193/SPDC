using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("BatchPayments")]
    public class BatchPayment
    {
        public BatchPayment()
        {
            Documents = new HashSet<Document>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TransactionTypeId { get; set; }

        public int AcceptedBankId { get; set; }

        public decimal BatchPaymentAmount { get; set; }
        
        [StringLength(265)]
        [Required]
        public string RefNo { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(265)]
        [Required]
        public string Payee { get; set; }

        public string Remarks { get; set; }

        public string ListApplication { get; set; }

        public int TotalCount { get; set; }

        public bool IsSettled { get; set; }

        public virtual TransactionType TransactionType { get; set; }

        public virtual AcceptedBank AcceptedBank { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
