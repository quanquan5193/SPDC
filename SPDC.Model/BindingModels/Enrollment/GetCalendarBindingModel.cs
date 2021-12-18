using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Enrollment
{
    public class GetCalendarBindingModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public int UserId { get; set; } = 0;
    }
}
