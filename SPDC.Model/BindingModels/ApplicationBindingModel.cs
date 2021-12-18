using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class ApplicationBindingModel
    {
        public string PassportID { get; set; }

        public string HKIdNo { get; set; }
    }

    public class ApplicationCreateBindingModel
    {

        public int? ApplicationId { get; set; }

        public int CourseId { get; set; }

        public int? ClassId { get; set; }

        public bool IsSubmit  { get; set; }

        public int[] ModuleID { get; set; } = new int[] { };

        public bool IHaveApplyFor { get; set; }

        public string IHaveApplyForText { get; set; }

        public bool IsRequiredRecipt { get; set; }

        public int UserId { get; set; }
    }

    public class ExperienceDeleteBindingModel
    {
        public int ExperienceID { get; set; }
    }

    public class QualificationToDeleteBindingModel
    {
        public int QualificationID { get; set; }
    }

    public class RecommendationIdDeleteBindingModel
    {
        public int RecommendationId { get; set; }
    }

    public class DocToDeleteBindingModel
    {
        public int DocID { get; set; }
    }
    
}
