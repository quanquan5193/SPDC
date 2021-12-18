using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class AdmissionRequirementModel
    {
        [Required]
        public string Other_Desc { get; set; }

        public string Other_Desc_TC { get; set; }

        public string Other_Desc_SC { get; set; }
    }
}