using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.MakeupClass
{
    public class MakeupClassBindingModel
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "model_error_name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "model_error_date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "model_error_timeFromMin")]
        public int TimeFromMin { get; set; }

        [Required(ErrorMessage = "model_error_timeFromHrs")]
        public int TimeFromHrs { get; set; }

        [Required(ErrorMessage = "model_error_timeToMin")]
        public int TimeToMin { get; set; }

        [Required(ErrorMessage = "model_error_timeToHrs")]
        public int TimeToHrs { get; set; }

        [Required(ErrorMessage = "model_error_venue")]
        public string Venue { get; set; }
    }
}
