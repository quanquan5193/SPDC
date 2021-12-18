using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Approval
{
    public class CourseApprovedHistoryViewModel
    {
        public CourseApprovedHistoryViewModel()
        {
            ListDocuments = new List<CourseApprovedDocument>();
        }
        public int Id { get; set; }

        public string ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public string ApprovalRemarks { get; set; }

        public List<CourseApprovedDocument> ListDocuments { get; set; }
    }

    public class CourseApprovedDocument
    {
        public string FileName { get; set; }

        public string DownloadUrl { get; set; }
    }
}
