using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("InvoiceItemTrans")]
    public class InvoiceItemTran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int ItemId { get; set; }

        public int LanguageId { get; set; }

        [Required]
        [StringLength(256)]
        public string ItemName { get; set; }

        public virtual InvoiceItem InvoiceItem { get; set; }

        public virtual Language Language { get; set; }
    }
}
