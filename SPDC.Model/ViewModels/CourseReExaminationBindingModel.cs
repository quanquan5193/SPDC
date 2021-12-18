using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CourseReExaminationBindingModel
    {
        public int Id { get; set; }
        public bool CanApplyForReExam { get; set; }
        public decimal? ReExamFee { get; set; }
        public string ReExamRemarks { get; set; }
    }
}
