using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("WorkExperienceTempTran")]
    public class WorkExperienceTempTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LanguageId { get; set; }

        [Required]
        [StringLength(256)]
        public string Location { get; set; }

        [StringLength(256)]
        public string JobNature { get; set; }

        [StringLength(256)]
        public string Position { get; set; }

        public virtual WorkExperienceTemp WorkExperienceTemp { get; set; }
    }

}
