using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Approval
{
    public class CancelApprovalSubClassBindingModel
    {
        public int ClassId { get; set; }

        public int ApprovedStatus { get; set; }

        public string Remarks { get; set; }
    }
}
