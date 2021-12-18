using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class ApplicationApprovedStatusHistory
    {
        public ApplicationApprovedStatusHistory()
        {
            ApplicationHistoryDocuments = new HashSet<ApplicationHistoryDocument>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public int ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        [StringLength(300)]
        public string ApprovalRemarks { get; set; }

        public virtual Application Application { get; set; }

        public virtual ICollection<ApplicationHistoryDocument> ApplicationHistoryDocuments { get; set; }

    }
}
