using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Approval
{
    public class ClassApprovedHistoryViewModel
    {
        public ClassApprovedHistoryViewModel()
        {
            ListDocuments = new List<ClassApprovedDocument>();
        }
        public int Id { get; set; }

        public string ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public string ApprovalRemarks { get; set; }

        public List<ClassApprovedDocument> ListDocuments { get; set; }
    }

    public class ClassApprovedDocument
    {
        public string FileName { get; set; }

        public string DownloadUrl { get; set; }
    }
}
