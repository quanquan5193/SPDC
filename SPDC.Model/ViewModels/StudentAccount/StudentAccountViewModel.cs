using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.StudentAccount
{
    public class StudentAccountViewModel
    {
        public int Id { get; set; }
        public string StudentNameEN { get; set; }
        public string StudentNameCN { get; set; }
        public string HKID { get; set; }
        public string Passport { get; set; }
        public DateTime? PassportExpiredDate { get; set; }
        public string CICNumber { get; set; }
        public string LoginEmail { get; set; }
        public string SurNameCN { get; set; }
        public string GivenNameCN { get; set; }
        public string SurNameEN { get; set; }
        public string GivenNameEN { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? CommunicationLanguage { get; set; }
    }
}
