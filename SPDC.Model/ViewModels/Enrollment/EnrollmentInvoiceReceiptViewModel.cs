using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Enrollment
{
    public class EnrollmentInvoiceReceiptViewModel
    {
        public string InvoiceType { get; set; }

        public string InvoiceNumber { get; set; }

        public string InvoiceDate { get; set; }

        public string InvoiceUrl { get; set; }

        public string ReceiptNumber { get; set; }

        public string ReceiptDate { get; set; }

        public string ReceiptUrl { get; set; }
    }
}
