using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Invoice
{
    public class InvoiceCreateBindingModel
    {
        public int InvoiceId { get; set; }

        public int ApplicationId { get; set; }

        public int UserId { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime PaymentDueDate { get; set; }

        public bool RequiredHardCopy { get; set; }

        public IEnumerable<InvoiceItemBindingModel> ListInvoiceItems { get; set; }
    }

    public class InvoiceItemBindingModel
    {
        public int InvoiceItemId { get; set; }

        public int InvoiceItemTypeId { get; set; }

        public string NameEnglish { get; set; }

        public string NameChinese { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsDiscount { get; set; }
    }
}
