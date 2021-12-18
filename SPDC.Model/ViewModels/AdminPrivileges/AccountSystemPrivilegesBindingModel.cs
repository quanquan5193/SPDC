using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class AccountSystemPrivilegesBindingModel
    {
        [Required]
        public int UserId { get; set; }

        public bool IsCreateAdmin { get; set; }

        public bool IsSuspendAdmin { get; set; }

        public bool IsActiveAdmin { get; set; }

        public bool IsEditAdmin { get; set; }

        public bool IsAssignAdmin { get; set; }
    }
}
