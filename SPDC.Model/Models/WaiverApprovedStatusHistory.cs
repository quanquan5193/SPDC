using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class WaiverApprovedStatusHistory
    {
        public WaiverApprovedStatusHistory()
        {
            WaivedHistoryDocuments = new HashSet<WaivedHistoryDocument>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int ApprovalUpdatedBy { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public int ApprovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public string ApprovalRemarks { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual ICollection<WaivedHistoryDocument> WaivedHistoryDocuments { get; set; }
    }
}
