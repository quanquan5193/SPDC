using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("EnrollmentStatusStorage")]
    public class EnrollmentStatusStorage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public int Status { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public virtual Application Application { get; set; }

        public virtual EnrollmentStatus EnrollmentStatus { get; set; }
    }
}
