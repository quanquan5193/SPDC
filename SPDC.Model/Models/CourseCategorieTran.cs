using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CourseCategorieTrans")]
    public class CourseCategorieTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LanguageId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Title { get; set; }

        public int CategoryId { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }

        public virtual Language Language { get; set; }
    }

}
