using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.MakeupClass
{
    public class StudentSearchViewModel
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public string ApplicationNumber { get; set; }
        public string StudentSurnameCN { get; set; }
        public string StudentSurnameEN { get; set; }
        public string StudentGivenCN { get; set; }
        public string StudentGivenEN { get; set; }
    }
}
