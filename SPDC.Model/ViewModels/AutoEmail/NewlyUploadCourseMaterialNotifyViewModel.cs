using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AutoEmail
{
    public class NewlyUploadCourseMaterialNotifyViewModel
    {
        public int CommunicationLanguage { get; set; }

        public string ClassCode { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string CourseName { get; set; }
    }
}
