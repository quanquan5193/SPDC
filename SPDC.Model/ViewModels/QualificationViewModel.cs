using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class QualificationViewModel
    {
        public QualificationViewModel()
        {
            ListSubQualifications = new List<SubQualificationViewModel>();
        }
        [Required]
        public int Id { get; set; }
        public bool? RelatedQualifications1Check { get; set; }

        public bool? RelatedQualifications2Check { get; set; }

        public string RelatedQualifications2Year { get; set; }

        public bool? RelatedQualifications3Check { get; set; }

        public string RelatedQualifications1Text { get; set; }

        public string RelatedQualifications2Text { get; set; }
        public int ApplicationId { get; set; }
        public List<SubQualificationViewModel> ListSubQualifications { get; set; }
    }

    public class SubQualificationViewModel
    {
        public int? Id { get; set; }

        [Required]
        public DateTime DateObtained { get; set; }

        [Required]
        public string IssuingAuthority { get; set; }

        [Required]
        public string LevelAttained { get; set; }

    }
}
