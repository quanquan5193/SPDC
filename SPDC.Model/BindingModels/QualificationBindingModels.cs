using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class QualificationBindingModels
    {

        public int? Id { get; set; }

        [Required]
        public DateTime DateObtained { get; set; }

        [Required]
        public string IssuingAuthority { get; set; }

        [Required]
        public string LevelAttained { get; set; }


        public int UserId { get; set; }
    }
}
