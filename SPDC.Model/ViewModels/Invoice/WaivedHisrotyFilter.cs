using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Invoice
{
    public class WaivedHisrotyFilter
    {
        public int InvoiceId { get; set; }

        public int Page { get; set; } = 1;

        public int Size { get; set; }
    }
}
