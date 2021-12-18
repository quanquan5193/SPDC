using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ModuleTrans")]
    public partial class ModuleTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public int ModuleId { get; set; }

        [StringLength(256)]
        public string ModuleName { get; set; }

        [StringLength(256)]
        public string Hours { get; set; }

        public virtual Language Language { get; set; }

        public virtual Module Module { get; set; }
    }
}
