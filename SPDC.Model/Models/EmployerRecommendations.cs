using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("EmployerRecommendations")]
    public class EmployerRecommendation
    {
        public EmployerRecommendation()
        {
            EmployerRecommendationTrans = new HashSet<EmployerRecommendationTran>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Tel { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<EmployerRecommendationTran> EmployerRecommendationTrans { get; set; }
    }

}
