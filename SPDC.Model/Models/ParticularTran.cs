using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ParticularTrans")]
    public partial class ParticularTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ParticularId { get; set; }

        public int LanguageId { get; set; }

        [StringLength(256)]
        public string RelatedQualifications1Text { get; set; }

        [StringLength(256)]
        public string RelatedQualifications2Text { get; set; }

        [StringLength(256)]
        public string PresentEmployer { get; set; }

        [StringLength(256)]
        public string Position { get; set; }


        public virtual Language Language { get; set; }

        public virtual Particular Particular { get; set; }
    }
}
