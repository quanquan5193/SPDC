using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class ClassAppovedStatusHistory
    {
        public ClassAppovedStatusHistory()
        {
            ClassHistoryDocuments = new HashSet<ClassHistoryDocument>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        [StringLength(300)]
        public string ApprovalRemarks { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<ClassHistoryDocument> ClassHistoryDocuments { get; set; }
    }
}
