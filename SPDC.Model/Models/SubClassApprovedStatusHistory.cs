using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class SubClassApprovedStatusHistory
    {
        public SubClassApprovedStatusHistory()
        {
            SubClassHistoryDocuments = new HashSet<SubClassHistoryDocument>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClassId { get; set; }

        public int ApprovalUpdatedBy { get; set; }

        public int ApprovalStatusFrom { get; set; }
        
        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        public int SubClassDraftId { get; set; }

        public virtual SubClassDraft SubClassDraft { get; set; }

        public virtual Class Class { get; set; }

        public virtual ICollection<SubClassHistoryDocument> SubClassHistoryDocuments { get; set; }
    }
}
