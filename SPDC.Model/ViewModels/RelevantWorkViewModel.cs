using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class RelevantWorkViewModel
    {
        public int Id { get; set; }

        public bool ShowTwoYears { get; set; }

        public bool ShowThreeYears { get; set; }

        public bool ShowFourYears { get; set; }

        public bool ShowFiveYears { get; set; }

        public bool ShowThreeYearsLeak { get; set; }

        public bool ShowWorkingExperience { get; set; }

        public bool ShowLetterTemplate { get; set; }

        public string Specify { get; set; }
    }
}
