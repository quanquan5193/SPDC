using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class CreateAdminBindingModel
    {
        public string LdapAccount { get; set; }

        public string Email { get; set; }

        public IEnumerable<int> CourseIds { get; set; }
    }
}
