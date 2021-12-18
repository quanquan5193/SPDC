using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Transaction
{
    public class TransactionBindingModel
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int TypeOfPayment { get; set; }

        public decimal Amount { get; set; }

        public int BankCodeAndBankName { get; set; }

        [Required]
        public string RefNo { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Remarks { get; set; }

        public string ReasonForRefund { get; set; }

        public List<int> ListFileToDelete { get; set; }
    }
}
