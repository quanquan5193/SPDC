using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchPayment
{
    public class BatchPaymentViewModel
    {
        public int Id { get; set; }

        public DateTime BatchPaymentDate { get; set; }

        public decimal BatchPaymentAmount { get; set; }

        public string ReferenceNumber { get; set; }

        public string Remarks { get; set; }
    }
}
