using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CourseLocationTrans")]
    public class CourseLocationTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public int CourseLocationId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Desciption { get; set; }

        public virtual CourseLocation CourseLocation { get; set; }

        public virtual Language Language { get; set; }
    }
}
