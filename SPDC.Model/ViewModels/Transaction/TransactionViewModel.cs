using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Transaction
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {
            ListDocuments = new List<TransactionDocument>();
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

        public List<TransactionDocument> ListDocuments { get; set; }
    }

    public class TransactionDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public string DownloadUrl { get; set; }
    }
}
