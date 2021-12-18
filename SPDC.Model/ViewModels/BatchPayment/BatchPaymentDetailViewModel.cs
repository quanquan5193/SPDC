using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchPayment
{
    public class BatchPaymentDetailViewModel
    {
        public BatchPaymentDetailViewModel()
        {
            ListBatchPaymentItem = new List<BatchPaymentItemViewModel>();
            BatchPaymentFiles = new List<BatchPaymentFile>();
        }
        public int Id { get; set; }

        public int TypeOfPaymentId { get; set; }

        public int BankCodeBankNameId { get; set; }

        public DateTime BatchPaymentDate { get; set; }

        public decimal BatchPaymentAmount { get; set; }

        public string ReferenceNumber { get; set; }

        public string Payee { get; set; }

        public string Remarks { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsSettled { get; set; }

        public List<BatchPaymentItemViewModel> ListBatchPaymentItem { get; set; }

        public List<BatchPaymentFile> BatchPaymentFiles { get; set; }
    }

    public class BatchPaymentFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Url { get; set; }
    }
}
