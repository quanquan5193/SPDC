using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AutoEmail
{
    public class ClassCommencementDateReminderViewModel
    {
        public string CourseCode { get; set; }

        public string ClassCode { get; set; }

        public DateTime CommencementDate { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public int CommunicationLanguage { get; set; }
    }
}
