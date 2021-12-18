using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class RefundTransactionHistoryDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RefundTransactionApprovedStatusHistoryId { get; set; }

        public int DocumentId { get; set; }

        public virtual RefundTransactionApprovedStatusHistory RefundTransactionApprovedStatusHistory { get; set; }

        public virtual Document Document { get; set; }
    }
}
