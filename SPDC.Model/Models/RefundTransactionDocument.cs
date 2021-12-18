using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class RefundTransactionDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RefundTransactionId { get; set; }

        public int DocumentId { get; set; }

        public Document Document { get; set; }

        public RefundTransaction RefundTransaction { get; set; }
    }
}
