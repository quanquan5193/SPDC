using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class SubjectOfficerModel
    {
        [Required]
        public string Centre_Code { get; set; }

        [Required]
        public string Officer_User_ID { get; set; }

        public string[] Officer_User_ID_Secondary { get; set; }

        [Required]
        public string Instructor_ID { get; set; }

    }
}