using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class PaymentItem
    {
        [Required]
        public string Item_Code { get; set; }

        [Required]
        public float Unit_Price { get; set; }

        public int Qty { get; set; }

    }
}