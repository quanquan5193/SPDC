using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Application
{
    public class InvoiceBindingModel
    {
        public string InvoiceNumber { get; set; }

        public DateTime? InvoiceCreatedDate { get; set; }

        public string ReciptNumber { get; set; }

        public DateTime? ReciptCreatedDate { get; set; }

        public string LinkInvoicePdf { get; set; }

        public string LinkReciptPdf { get; set; }
    }
}
