using System;
using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class PersonalInfo
    {
        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname_Chi { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname_Eng { get; set; }

        [Required]
        [StringLength(100)]
        public string Given_Name_Chi { get; set; }

        [Required]
        [StringLength(100)]
        public string Given_Name_Eng { get; set; }

        [Required]
        [StringLength(8)]
        public string Contact_No { get; set; }

        [Required]
        public DateTime Date_Of_Birth { get; set; }

        [Required]
        public string PassportNo { get; set; }

        [Required]
        public string HKID { get; set; }

        public string Is_DOB_Year_Only { get; set; }

        public int Year_Of_Birth { get; set; }

        public string Origin { get; set; }

        public string Email { get; set; }

        [StringLength(8)]
        public string Mobile_No { get; set; }

        public Address Res_Address_Eng { get; set; }

        public Address Res_Address_Chi { get; set; }

        public Address Corr_Address_Eng { get; set; }

        public Address Corr_Address_Chi { get; set; }

    }
}