using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("QualificationTemp")]
    public class QualificationTemp
    {
        public QualificationTemp()
        {
            QualificationTempTrans = new HashSet<QualificationTempTran>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DateObtained { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<QualificationTempTran> QualificationTempTrans { get; set; }
    }
}
