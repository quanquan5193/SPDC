using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.StudentAccount
{
    public class StudentAccountBindingModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string Hkid { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassportExpiredDate { get; set; }
        public string SurNameCN { get; set; }
        public string GivenNameCN { get; set; }
        [Required]
        public string SurNameEN { get; set; }
        [Required]
        public string GivenNameEN { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public int? CommunicationLanguage { get; set; }
    }
}
