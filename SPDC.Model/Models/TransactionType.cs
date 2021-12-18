using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("TransactionTypes")]
    public  class TransactionType
    {
        public TransactionType()
        {
            PaymentTransactions = new HashSet<PaymentTransaction>();
            RefundTransactions = new HashSet<RefundTransaction>();
            BatchPayments = new HashSet<BatchPayment>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public int LanguageId { get; set; }
        [Required]
        [StringLength(256)]
        public string NameEN { get; set; }

        [Required]
        [StringLength(256)]
        public string NameCN { get; set; }

        [Required]
        [StringLength(256)]
        public string NameHK { get; set; }

        //public virtual Language Language { get; set; }

        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }

        public virtual ICollection<RefundTransaction> RefundTransactions { get; set; }

        public virtual ICollection<BatchPayment> BatchPayments { get; set; }
    }

}
