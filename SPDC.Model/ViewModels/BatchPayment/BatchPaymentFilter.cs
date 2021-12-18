using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchPayment
{
    public class BatchPaymentFilter
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Page { get; set; } = 1;

        public int Size { get; set; }
    }
}
