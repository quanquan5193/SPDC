using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("InvoiceItems")]
    public partial class InvoiceItem
    {
        public InvoiceItem()
        {
            InvoiceItemTrans = new HashSet<InvoiceItemTran>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public decimal Price { get; set; }

        public string EnglishName { get; set; }

        public string ChineseName { get; set; }

        public bool IsDiscount { get; set; }

        public int InvoiceItemTypeId { get; set; }

        public virtual InvoiceItemType InvoiceItemType { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual ICollection<InvoiceItemTran> InvoiceItemTrans { get; set; }
    }

}
