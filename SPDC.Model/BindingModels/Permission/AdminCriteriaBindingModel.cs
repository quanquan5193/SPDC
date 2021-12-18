using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Permission
{
    public class AdminCriteriaBindingModel
    {
        public string LDAPAccount { get; set; }
        public int CourseId { get; set; }
    }
}
