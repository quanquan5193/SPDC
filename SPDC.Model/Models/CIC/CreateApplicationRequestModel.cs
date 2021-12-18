using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPDC.Model.Models.CIC
{
    public class CreateApplicationRequestModel
    {
        [Required]
        public string ApplicationID { get; set; }

        [Required]
        public DateTime Apply_Date { get; set; }

        [Required]
        public string Apply_Method { get; set; }

        [Required]
        public string CardStatus { get; set; }

        [Required]
        public string Course_Code { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public PersonalInfo Personal_Info { get; set; }

        [Required]
        public string Venue_Code { get; set; }

        public Dictionary<string, string> Additional_Questions { get; set; }

        public string Application_Remarks { get; set; }

        public string Assign_Class_Later { get; set; }

        public Education Education { get; set; }

        public string[] Module_Course_Codes { get; set; }

        public List<PaymentItem> Payment_Items { get; set; }

        public List<PaymentMethod> Payment_Methods { get; set; }

        public string Previous_Student_No { get; set; }

        public List<Qualification> Qualifications { get; set; }

        public List<WorkExperience> Work_Experiences { get; set; }

    }
}