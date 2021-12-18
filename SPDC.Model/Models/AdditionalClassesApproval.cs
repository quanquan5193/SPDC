using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("AdditionalClassesApprovals")]
    public class AdditionalClassesApproval
    {
        public AdditionalClassesApproval()
        {
            Documents = new HashSet<Document>();
            UpdatedTime = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int OriginalTargetNumber { get; set; }

        public int NewTargetNumber { get; set; }

        public int UpdatedBy { get; set; }

        public int StatusFrom { get; set; }

        public int StatusTo { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string ApprovalRemark { get; set; }

        public Course Course { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
