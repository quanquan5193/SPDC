using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Transaction.RefundTransaction
{
    public class RefundTransactionApprovedHistoryViewModel
    {
        public RefundTransactionApprovedHistoryViewModel()
        {
            ListRefundDocuments = new List<RefundApprovedDocument>();
            ListApprovedDocuments = new List<RefundApprovedDocument>();
        }
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int TypeOfPayment { get; set; }

        public decimal Amount { get; set; }

        public int BankCodeAndBankName { get; set; }

        public string RefNo { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Remarks { get; set; }

        public string ReasonForRefund { get; set; }

        public int RefundApprovedStatus { get; set; }

        public List<RefundApprovedDocument> ListRefundDocuments { get; set; }

        public int ApprovedId { get; set; }

        public string ApprovalUpdatedBy { get; set; }

        public int AppovalStatusFrom { get; set; }

        public int ApprovalStatusTo { get; set; }

        public DateTime ApprovalUpdatedTime { get; set; }

        public string ApprovalRemarks { get; set; }

        public List<RefundApprovedDocument> ListApprovedDocuments { get; set; }
    }
    public class RefundApprovedDocument
    {
        public int DocumentId { get; set; }

        public string FileName { get; set; }

        public string DownloadUrl { get; set; }
    }
}
