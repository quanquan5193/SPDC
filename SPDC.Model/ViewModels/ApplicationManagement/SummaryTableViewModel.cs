using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.ApplicationManagement
{
    public class SummaryTableViewModel
    {
        public SummaryTableViewModel()
        {
            SummaryCourseFee = new ClassifyStatusModel();
            SummaryReExamFee = new ClassifyStatusModel();
            SummaryOtherFee = new ClassifyStatusModel();
        }
        public string ClassCode { get; set; }

        public int Capacity { get; set; }

        public int StudentAssigned { get; set; }

        public ClassifyStatusModel SummaryCourseFee { get; set; }

        public ClassifyStatusModel SummaryReExamFee { get; set; }
        
        public ClassifyStatusModel SummaryOtherFee { get; set; }
    }

    public class ClassifyStatusModel
    {
        public int Created { get; set; }

        public int Offered { get; set; }

        public int Waived { get; set; }

        public int PaidPartially { get; set; }

        public int Settled { get; set; }

        public int SettledByBatch { get; set; }

        public int Revised { get; set; }

        public int Overdue { get; set; }
    }
}
