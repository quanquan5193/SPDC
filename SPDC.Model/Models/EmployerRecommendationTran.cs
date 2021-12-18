using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("EmployerRecommendationTrans")]
    public class EmployerRecommendationTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(256)]
        public string ContactPerson { get; set; }

        public int LanguageId { get; set; }

        public int EmployerRecommendationId { get; set; }

        public string Position { get; set; }

        public string Email { get; set; }

        public virtual EmployerRecommendation EmployerRecommendation { get; set; }

        public virtual Language Language { get; set; }
    }

}
