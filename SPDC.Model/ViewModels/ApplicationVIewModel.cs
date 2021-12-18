using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class ApplicationViewModel
    {
        public int ApplicationId { get; set; }
        public int? ClassReferenced { get; set; }
        public int[] ModuleReferenced { get; set; }
        public bool IHaveApplyFor { get; set; }
        public string IHaveApplyForText { get; set; }
        public bool IsRequiredRecipt { get; set; }
    }
}
