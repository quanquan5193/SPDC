using System;
using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class WorkExperience
    {
        [Required]
        public DateTime Work_From_Date { get; set; }

        [Required]
        [StringLength(200)]
        public string Position { get; set; }

        [Required]
        [StringLength(200)]
        public string Employer_Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Work_Nature { get; set; }

        public DateTime Work_To_Date { get; set; }

        public string BIM_Related { get; set; }
    }
}