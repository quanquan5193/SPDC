
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class SendMailModel
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public int CommunicationLanguage { get; set; }

        public List<int> InterestedTypeOfCourse { get; set; }

        public string FirstNameEN { get; set; }

        public string LastNameEN { get; set; }

        public string FirstNameCN { get; set; }

        public string LastNameCN { get; set; }

        public string EmailContent { get; set; }

        public string CmsUrl { get; set; }

        public LinkedResource ImageResourse { get; set; }

        public string EmailSubject { get; set; }

        public string[] DeviceToken { get; set; }

    }
}
