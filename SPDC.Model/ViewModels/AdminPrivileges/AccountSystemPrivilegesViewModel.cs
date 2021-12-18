using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class AccountSystemPrivilegesViewModel
    {
        public int Id { get; set; }
        public string LDAPAccount { get; set; }
        public string Email { get; set; }
        public bool IsCreateAdmin { get; set; }
        public bool IsSuspendAdmin { get; set; }
        public bool IsActiveAdmin { get; set; }
        public bool IsEditAdmin { get; set; }
        public bool IsAssignAdmin { get; set; }
    }
}
