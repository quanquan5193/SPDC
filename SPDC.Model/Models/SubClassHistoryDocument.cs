using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class SubClassHistoryDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SubClassHistoryId { get; set; }

        public int DocumentId { get; set; }

        public virtual Document Document { get; set; }

        public virtual SubClassApprovedStatusHistory SubClassApprovedStatusHistory { get; set; }
    }
}
