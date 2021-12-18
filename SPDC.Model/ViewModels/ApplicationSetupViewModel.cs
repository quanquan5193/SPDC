using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Model.Models;

namespace SPDC.Model.ViewModels
{
    public class ApplicationSetupViewModel
    {

        public int Id { get; set; }

        public int CourseId { get; set; }

        public bool ShowEducationLevel { get; set; }

        public bool ShowRecommendation { get; set; }

        public bool ShowDocument { get; set; }

        public bool ShowCitf { get; set; }

        public bool ShowReceipt { get; set; }

        public string FundingSchema { get; set; }

        public RelevantMembershipViewModel RelevantMembership { get; set; }

        public RelevantWorkViewModel RelevantWork { get; set; }
    }
}
