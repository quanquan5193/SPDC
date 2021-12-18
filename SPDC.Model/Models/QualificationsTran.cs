using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("QualificationsTrans")]
    public partial class QualificationsTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string IssuingAuthority { get; set; }

        [Required]
        [StringLength(256)]
        public string LevelAttained { get; set; }

        public int QualificationId { get; set; }

        public int LanguageId { get; set; }

        public virtual Language Language { get; set; }

        public virtual Qualification Qualification { get; set; }
    }
}
