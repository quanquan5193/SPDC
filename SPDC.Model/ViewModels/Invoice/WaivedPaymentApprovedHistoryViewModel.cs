using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Invoice
{
    public class WaivedPaymentApprovedHistoryViewModel
    {
        public WaivedPaymentApprovedHistoryViewModel()
        {
            ListDocuments = new List<WaivedApprovedDocument>();
        }
        public int Id { get; set; }

        public string ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public string ApprovalRemarks { get; set; }

        public List<WaivedApprovedDocument> ListDocuments { get; set; }
    }

    public class WaivedApprovedDocument
    {
        public string FileName { get; set; }

        public string DownloadUrl { get; set; }
    }
}
