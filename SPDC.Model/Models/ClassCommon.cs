using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ClassCommon")]
    public partial class ClassCommon
    {
        public ClassCommon()
        {
            //ClassCommon = new HashSet<ParticularTran>();
            AttendanceRequirementTypes = new HashSet<Class>();
            //NewAttendanceRequirementTypes = new HashSet<Class>();
            NewAttendanceRequirementTypes = new HashSet<SubClassDraft>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string TypeName { get; set; }

        public int? TypeCommon { get; set; }

        public ICollection<Class> AttendanceRequirementTypes { get; set; }

        //public ICollection<Class> NewAttendanceRequirementTypes { get; set; }

        public ICollection<SubClassDraft> NewAttendanceRequirementTypes { get; set; }

    }
}
