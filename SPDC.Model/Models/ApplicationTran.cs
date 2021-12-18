using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ApplicationTrans")]
    public class ApplicationTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public int ApplicationId { get; set; }

        [StringLength(256)]
        public string Others { get; set; }

        [StringLength(256)]
        public string Remarks { get; set; }

        public virtual Application Application { get; set; }

        public virtual Language Language { get; set; }
    }
}
