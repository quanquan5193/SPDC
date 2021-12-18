using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AutoEmail
{
    public class PaymentDueDateReminderViewModel
    {
        public string Email { get; set; }

        public string InvoiceNumber { get; set; }

        public string DisplayName { get; set; }

        public int CommunicationLanguage { get; set; }

        public string CourseName { get; set; }                                                          
    }
}
