using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class UserSubscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Company { get; set; }

        [Required]
        public int Honorific { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstNameEN { get; set; }

        [Required]
        [StringLength(256)]
        public string LastNameEN { get; set; }

        [StringLength(256)]
        public string FirstNameCN { get; set; }

        [StringLength(256)]
        public string LastNameCN { get; set; }

        [StringLength(10)]
        public string PrefixMobilePhone { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [StringLength(256)]
        public string Position { get; set; }

        [Required]
        [Index("INDEX_Email", IsUnique = true)]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string InterestedTypeOfCourse { get; set; }

        [Required]
        public int CommunicationLanguage { get; set; }

        [Required]
        public bool IsSubscribe { get; set; }
    }
}
