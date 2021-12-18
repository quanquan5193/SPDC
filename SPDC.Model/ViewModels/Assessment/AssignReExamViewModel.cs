using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Assessment
{
    public class AssignReExamViewModel
    {
        public int ExamId { get; set; }
        public string ClassCode { get; set; }
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }
}
