using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class UserSubscriptionBindingModel
    {
        public string Company { get; set; }
        public int Honorific { get; set; }
        [Required]
        public string FirstNameEN { get; set; }
        [Required]
        public string LastNameEN { get; set; }
        public string FirstNameCN { get; set; }
        public string LastNameCN { get; set; }
        public string PrefixMobilePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Position { get; set; }
        [Required]
        public string Email { get; set; }
        public List<int> InterestedTypeOfCourse { get; set; }
        [Required]
        public int CommunicationLanguage { get; set; }
    }
}
