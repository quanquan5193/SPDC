using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("InvoiceStatus")]
    public class InvoiceStatus
    {
        public InvoiceStatus()
        {
            Invoices = new HashSet<Invoice>();
            InvoiceStatusStorages = new HashSet<InvoiceStatusStorage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public int LanguageId { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<InvoiceStatusStorage> InvoiceStatusStorages { get; set; }
    }
}
