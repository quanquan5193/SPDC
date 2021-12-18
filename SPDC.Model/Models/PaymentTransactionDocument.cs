using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class PaymentTransactionDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PaymentTransactionId { get; set; }

        public int DocumentId { get; set; }

        public int TypeOfDocument { get; set; }

        public virtual Document Document { get; set; }

        public virtual PaymentTransaction PaymentTransaction { get; set; }
    }
}
