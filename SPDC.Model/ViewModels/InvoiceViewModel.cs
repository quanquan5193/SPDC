using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            InvoiceItems = new List<InvoiceItemViewModel>();
        }
        public int InvoiceId { get; set; }

        public string CourseCode { get; set; }

        public string CourseNameEN { get; set; }
        
        public string CourseNameCN { get; set; }

        public string StudentNameEN { get; set; }

        public string StudentNameCN { get; set; }

        public string InvoiceNumber { get; set; }

        public decimal Fee { get; set; }

        public DateTime PaymentDueDate { get; set; }

        public bool RequiredhardCopy { get; set; }

        public int Status { get; set; }

        public int WaiverApprovedStatus { get; set; }

        public int? CommunicationLanguage { get; set; }

        public List<InvoiceItemViewModel> InvoiceItems { get; set; }
    }

    public class InvoiceItemViewModel
    {
        public int Id { get; set; }
        public string NameEN { get; set; }
        public string NameCN { get; set; }
        public decimal Price { get; set; }
        public bool IsDiscount { get; set; }
        public int InvoiceItemTypeId { get; set; }
    }
}
