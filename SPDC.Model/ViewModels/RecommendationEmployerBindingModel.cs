using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class RecommendationEmployerBindingModel
    {
        public int Id { get; set; }
        [Required]
        public string Tel { get; set; }
        public string CompanyName { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPosition { get; set; }
        public string ContactPersonEmail { get; set; }
        public int UserId { get; set; }
    }


    public class RecommendationEmployerNewBindingModel
    {
        public int? Id { get; set; }
        [Required]
        public string ContactPersonTel { get; set; }

        public string CompanyName { get; set; }

        public string ContactPersonName { get; set; }

        public string ContactPersonPosition { get; set; }
        public string ContactPersonEmail { get; set; }

        public int UserId { get; set; }
    }
}
