using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Approval
{
    public class CancelPostponedSubClassBindingModel
    {
        public int ClassId { get; set; }

        public string NewClassCode { get; set; }

        public int? NewAttendanceRequirement { get; set; }

        public int? NewAttendanceRequirementTypeId { get; set; }

        public DateTime? NewClassCommencementDate { get; set; }

        public DateTime? NewClassCompletionDate { get; set; }

        public int? NewClassCapacity { get; set; }

        public int NewClassStatus { get; set; }

        public int ApprovedStatus { get; set; }

        public string Remarks { get; set; }
    }
}
