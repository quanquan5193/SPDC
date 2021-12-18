using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchApplication
{
    public class BatchApplicationCourse
    {
        public int Id { get; set; }

        public string CourseCode { get; set; }
        public string CourseNameEN { get; set; }
        public string CourseNameTC { get; set; }

        public bool IsHaveTargetClass { get; set; }

    }
}
