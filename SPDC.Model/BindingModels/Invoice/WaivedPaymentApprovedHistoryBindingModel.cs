using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Invoice
{
    public class WaivedPaymentApprovedHistoryBindingModel
    {
        public int InvoiceId { get; set; }

        public int ApprovedStatus { get; set; }

        public string Remarks { get; set; }
    }
}
