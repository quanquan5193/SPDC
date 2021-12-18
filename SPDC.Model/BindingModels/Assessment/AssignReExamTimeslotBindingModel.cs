using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Assessment
{
    public class AssignReExamTimeslotBindingModel
    {
        public List<int> ListApplicationId { get; set; }

        public int OriginalClassId { get; set; }

        public int ExamOriginalId { get; set; }

        public int ExamDestinationId { get; set; }
    }
}
