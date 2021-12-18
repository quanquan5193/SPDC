using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.ApplicationManagement
{
    public class ApplicationApprovedHistoryViewModel
    {
        public ApplicationApprovedHistoryViewModel()
        {
            ListDocuments = new List<ApplicationApprovedDocument>();
        }
        public int Id { get; set; }

        public string ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public string ApprovalRemarks { get; set; }

        public List<ApplicationApprovedDocument> ListDocuments { get; set; }
    }

    public class ApplicationApprovedDocument
    {
        public string FileName { get; set; }

        public string DownloadUrl { get; set; }
    }
}
