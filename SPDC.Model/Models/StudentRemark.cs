using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("StudentRemarks")]
    public class StudentRemark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public bool IsMissingQualifications { get; set; }

        public bool IsMissingWorkExperience { get; set; }

        public bool IsMissingSupportingDocuments { get; set; }

        public bool IsReject { get; set; }

        [StringLength(256)]
        public string Other { get; set; }

        public virtual Course Course { get; set; }
    }
}
