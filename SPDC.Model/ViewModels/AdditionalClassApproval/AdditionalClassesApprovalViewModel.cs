using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdditionalClassApproval
{
    public class AdditionalClassesApprovalViewModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int OriginalTargetNumber { get; set; }

        public int NewTargetNumber { get; set; }

        public string ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public string ApprovalRemarks { get; set; }
        public virtual ICollection<AdditionalClassesApprovalDocumentViewModel> ListDocuments { get; set; }
    }

    public class AdditionalClassesApprovalDocumentViewModel
    {
        public string DownloadUrl { get; set; }
        public string FileName { get; set; }
    }
}
