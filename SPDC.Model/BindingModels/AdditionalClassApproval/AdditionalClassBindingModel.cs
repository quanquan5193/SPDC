using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.AdditionalClassApproval
{
    public class AdditionalClassBindingModel
    {
        public int CourseId { get; set; }

        public int NewNumber { get; set; }

        public string ApprovalRemark { get; set; }

        public int StatusTo { get; set; }

    }
}
