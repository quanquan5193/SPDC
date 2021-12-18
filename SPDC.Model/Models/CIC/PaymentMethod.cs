using System;
using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class PaymentMethod
    {
        [Required]
        public string Payment_Method { get; set; }

        [Required]
        public string Payment_Amt { get; set; }

        [Required]
        public DateTime Payment_Date { get; set; }

        public string Bank { get; set; }

        [Required]
        [StringLength(20)]
        public string Receipt_No { get; set; }

        [StringLength(20)]
        public string Reference_No { get; set; }
    }
}