using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("EnrollmentStatus")]
    public class EnrollmentStatus
    {
        public EnrollmentStatus()
        {
            EnrollmentStatusStorages = new HashSet<EnrollmentStatusStorage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string NameEN { get; set; }

        [Required]
        [StringLength(256)]
        public string NameTC { get; set; }

        [Required]
        [StringLength(256)]
        public string NameSC { get; set; }

        public virtual ICollection<EnrollmentStatusStorage> EnrollmentStatusStorages { get; set; }
    }
}
