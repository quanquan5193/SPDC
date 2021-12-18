using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class PrivilegesBindingModel
    {
        public string LDAPAccount { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int Index { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
