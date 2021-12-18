using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.ApplicationManagement
{
    public class ApplicationHistoryFilter
    {
        public int ApplicationId { get; set; }

        public int Page { get; set; } = 1;

        public int Size { get; set; }
    }
}
