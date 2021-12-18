using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class Education
    {
        public string Primar { get; set; }

        public string Senion { get; set; }

        public string Tertiary { get; set; }

        [StringLength(100)]
        public string Others { get; set; }

    }
}