using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Application
{
    public class CourseCodeViewModel
    {
        public int Id { get; set; }

        public string CourseCode { get; set; }

        public string CourseNameEN { get; set; }
        public string CourseNameCN { get; set; }
        public string AcademicYear { get; set; }
        public string[] AcademicYears { get; set; }
    }
}
