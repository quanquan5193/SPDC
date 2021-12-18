using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Enrollment
{
    public class GetMyCourseBingdingModel
    {
        public int UserId { get; set; } = 0;
        public int Index { get; set; }
        public int PageSize { get; set; }
    }
}
