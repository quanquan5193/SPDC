using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CourseTrans")]
    public class CourseTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        public string CourseTitle { get; set; }

        public string Curriculum { get; set; }

        public string ConditionsOfCertificate { get; set; }

        public string Recognition { get; set; }
        
        public string AdmissionRequirements { get; set; }        

        public virtual Course Course { get; set; }

        public virtual Language Language { get; set; }
    }
}
