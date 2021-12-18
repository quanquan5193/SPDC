using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class InvoiceReminderViewModel
    {
        public string CourseName { get; set; }

        public string InvoiceNo { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public int CommunicateLanguage { get; set; }
    }
}
