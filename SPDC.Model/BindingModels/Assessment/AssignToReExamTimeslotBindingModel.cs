using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Assessment
{
    public class AssignToReExamTimeslotBindingModel
    {
        public int ReExamTimeslotId { get; set; }
        public List<ReExamTimeslotApplicationBindingModel> Data { get; set; }
    }

    public class ReExamTimeslotApplicationBindingModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int FromReExamId { get; set; }

    }
}
