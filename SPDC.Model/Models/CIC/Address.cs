using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class Address
    {
        public string Region_Code { get; set; }

        public string District_Code { get; set; }

        public string Sub_District { get; set; }

        public string FlatUnit { get; set; }

        [StringLength(10)]
        public string Floor { get; set; }

        [StringLength(10)]
        public string Flat_No { get; set; }

        [StringLength(200)]
        public string Building_Name { get; set; }

        [StringLength(200)]
        public string Estate_Name { get; set; }

        [StringLength(200)]
        public string Street_Name { get; set; }

        public string Street_No_Start { get; set; }

        public string Street_No_End { get; set; }

    }
}