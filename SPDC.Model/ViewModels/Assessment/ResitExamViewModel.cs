using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Assessment
{
    public class ResitExamViewModel
    {
        public int Id { get; set; } = 0;

        public string Name { get; set; }

        public string TypeText { get; set; }

        public int Type { get; set; }

        public DateTime ApplicationDateline { get; set; }

        public DateTime Date { get; set; }

        public int TimeFromMin { get; set; }

        public int TimeFromHrs { get; set; }

        public int TimeToMin { get; set; }

        public int TimeToHrs { get; set; }

        public string Venue { get; set; }
        public IEnumerable<ResitExamApplicationViewModel> ResitExamApplications { get; set; }
    }
    public class ResitExamApplicationViewModel
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public int ResitExamId { get; set; }

        public string StudentNo { get; set; }

        public string StudentSurnameCN { get; set; }

        public string StudentSurnameEN { get; set; }

        public string StudentGivenCN { get; set; }

        public string StudentGivenEN { get; set; }

        public string ClassCode { get; set; }

        public string Date { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }
    }
}
