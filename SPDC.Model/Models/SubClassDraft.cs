using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class SubClassDraft
    {
        public SubClassDraft()
        {
            SubClassApprovedStatusHistories = new HashSet<SubClassApprovedStatusHistory>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClassId { get; set; }

        [StringLength(50)]
        public string NewClassCode { get; set; }

        public int? NewAttendanceRequirement { get; set; }

        public int? NewAttendanceRequirementTypeId { get; set; }

        public DateTime? NewClassCommencementDate { get; set; }

        public DateTime? NewClassCompletionDate { get; set; }

        public int? NewClassCapacity { get; set; }

        public int NewClassStatus { get; set; }

        public virtual Class Class { get; set; }

        public virtual ClassCommon NewAttendanceRequirementType { get; set; }

        public virtual ICollection<SubClassApprovedStatusHistory> SubClassApprovedStatusHistories { get; set; }
    }
}
