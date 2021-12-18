using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class AdminAccountBindingModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<int> CourseIds { get; set; }
    }
}
