using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Approval
{
    public class SubClassApprovedHistoryViewModel
    {
        public SubClassApprovedHistoryViewModel()
        {
            ListDocuments = new List<SubClassApprovedDocument>();
        }
        public int Id { get; set; }

        public string NewClassCode { get; set; }

        public int? NewAttendanceRequirement { get; set; }

        public int? NewAttendanceRequirementTypeId { get; set; }

        public DateTime? NewClassCommencementDate { get; set; }

        public DateTime? NewClassCompletionDate { get; set; }

        public int? NewClassCapacity { get; set; }

        public int NewClassStatus { get; set; }

        public int ApprovedStatus { get; set; }

        public string Remarks { get; set; }

        public string ApprovedBy { get; set; }

        public int ApprovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public List<SubClassApprovedDocument> ListDocuments { get; set; }
    }

    public class SubClassApprovedDocument
    {
        public string FileName { get; set; }

        public string DownloadUrl { get; set; }
    }
}
