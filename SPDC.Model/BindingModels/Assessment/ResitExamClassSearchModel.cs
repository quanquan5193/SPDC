using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Assessment
{
    public class ResitExamSearchModel
    {
        public string Name { get; set; }
        public string Venue { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Type { get; set; }
    }
}
