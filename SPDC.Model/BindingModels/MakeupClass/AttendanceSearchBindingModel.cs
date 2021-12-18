using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.MakeupClass
{
    public class AttendanceSearchBindingModel
    {
        public int CourseId { get; set; }
        public int ClassId { get; set; }
        public string AcademicYear { get; set; }
        public string CourseNameEN { get; set; }
        public string CourseNameCN { get; set; }
        public string StudentNameEN { get; set; }
        public string StudentNameCN { get; set; }
        public string StudentNo { get; set; }
        public string CICNo { get; set; }
        public string HKIDNo { get; set; }
        public string PassportNo { get; set; }
    }
}
