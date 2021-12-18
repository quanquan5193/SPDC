using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchPayment
{
    public class UserForSettledViewModel
    {
        public int Id { get; set; }

        public string ApplicantName { get; set; }

        public string CICNumber { get; set; }

        public bool IsChineseName { get; set; }
    }
}
