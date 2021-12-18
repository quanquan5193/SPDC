using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.BatchPayment
{
    public class BatchPaymentDetailBindingModel
    {
        public int Id { get; set; }

        [Required]
        public int TypeOfPaymentId { get; set; }

        [Required]
        public int BankCodeBankNameId { get; set; }

        [Required]
        public DateTime BatchPaymentDate { get; set; }

        [Required]
        public decimal BatchPaymentAmount { get; set; }

        [Required]
        public string ReferenceNumber { get; set; }

        [Required]
        public string Payee { get; set; }

        public string Remarks { get; set; }

        public List<int> ListFileToDelete { get; set; }

        public List<ApplicationForBatchPayment> ListApplication { get; set; }
    }

    public class ApplicationForBatchPayment
    {
        public int UserId { get; set; }

        public int ApplicationId { get; set; }

        public bool IsChineseName { get; set; }
    }
}
