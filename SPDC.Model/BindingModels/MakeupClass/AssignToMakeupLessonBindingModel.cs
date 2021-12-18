using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.MakeupClass
{
    public class AssignToMakeupLessonBindingModel
    {
        public int[] AppIds { get; set; }
        public MakeupLessonRecord[] Data { get; set; }
    }

    public class MakeupLessonRecord
    {
        public int LessonId { get; set; }
        public int FromLessonId { get; set; }
    }

}
