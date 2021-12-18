using System;
using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class Qualification
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]        
        public DateTime Obtained_Date { get; set; }

        [StringLength(20)]
        public string Certificate_No { get; set; }
    }
}