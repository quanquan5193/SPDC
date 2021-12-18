using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPDC.Model.Models.CIC
{
    public class CreatePaymentRequestModel
    {
        public string ApplicationID { get; set; }

        public string CreatedBy { get; set; }

        public List<PaymentItem> Payment_Items { get; set; }

        public List<PaymentMethod> Payment_Methods { get; set; }
    }
}