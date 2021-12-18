using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class ApplicationHistoryDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ApplicationHistoryId { get; set; }

        public int DocumentId { get; set; }

        public virtual ApplicationApprovedStatusHistory ApplicationApprovedStatusHistory { get; set; }

        public virtual Document Document { get; set; }
    }
}
