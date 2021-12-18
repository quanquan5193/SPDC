using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchApplication
{
    public class BatchApplicationApplicant
    {
        public int Id { get; set; }

        public string CICNumber { get; set; }

        public string SurnameEN { get; set; }

        public string SurnameCN { get; set; }

        public string GivenNameEN { get; set; }

        public string GivenNameCN { get; set; }

    }
}
