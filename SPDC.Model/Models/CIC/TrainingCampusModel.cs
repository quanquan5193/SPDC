using System.ComponentModel.DataAnnotations;

namespace SPDC.Model.Models.CIC
{
    public class TrainingCampusModel
    {
        [Required]
        public string Venue_Code { get; set; }

        [Required]
        public string Centre_Code { get; set; }

    }
}