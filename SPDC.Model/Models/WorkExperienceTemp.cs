using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("WorkExperienceTemp")]
    public class WorkExperienceTemp
    {
        public WorkExperienceTemp()
        {
            WorkExperienceTempTrans = new HashSet<WorkExperienceTempTran>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public DateTime FromYear { get; set; }

        [Required]
        public DateTime ToYear { get; set; }

        public bool BIMRelated { get; set; }

        [Required]
        public int ClassifyWorkingExperience { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<WorkExperienceTempTran> WorkExperienceTempTrans { get; set; }
    }

}
