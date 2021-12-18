using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Enquiries")]
    public class Enquiry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? CourseId { get; set; }

        public int EnquiryNo { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(256)]
        public string Phone { get; set; }

        [StringLength(256)]
        public string Fax { get; set; }

        [StringLength(256)]
        public string ContactPersonEN { get; set; }

        [StringLength(256)]
        public string ContactPersonCN { get; set; }

        [StringLength(256)]
        public string ContactPersonHK { get; set; }

        public virtual Course Course { get; set; }
    }
}
