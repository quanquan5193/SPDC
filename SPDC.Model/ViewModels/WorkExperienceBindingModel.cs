using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class WorkExperienceBindingModel
    {
        public int Id { get; set; }
        [Required]
        public DateTime FromYear { get; set; }
        [Required]
        public DateTime ToYear { get; set; }
        public int UserId { get; set; }
        public bool BIMRelated { get; set; }
        [Required]
        public int ClassifyWorkingExperience { get; set; }
        [Required]
        public string Location { get; set; }
        public string Position { get; set; }
        public string JobNature { get; set; }
    }
}
