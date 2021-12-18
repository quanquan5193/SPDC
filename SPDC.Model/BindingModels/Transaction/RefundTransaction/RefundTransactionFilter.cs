using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Transaction.RefundTransaction
{
    public class RefundTransactionFilter
    {
        public int InvoiceId { get; set; }

        public int Size { get; set; }

        public int Page { get; set; } = 1;
    }
}
