using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.MakeupClass
{
    public class AssignToMakeupClassBindingModel
    {
        public int MakeupClassId { get; set; }
        public List<MakeUpAttendenceBindingModel> Data { get; set; }
    }
    public class MakeUpAttendenceBindingModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int FromLessonId { get; set; }
        public bool IsDiplayOnStudentPortal { get; set; }

    }
}
