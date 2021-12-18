using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.BatchApplication
{
    public class BatchApplicationBindingModel
    {
        public int UserId { get; set; }
        public string CICNumber { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string SurnameEN { get; set; }
        public string GivenNameEN { get; set; }
        public string SurnameCN { get; set; }
        public string GivenNameCN { get; set; }

        public string CourseNameEN { get; set; }
        public string CourseNameTC { get; set; }
        public string Url { get; set; }
    }
}
