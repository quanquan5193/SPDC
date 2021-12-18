using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.Assessment
{
    public class ResitExamBindingModel
    {
        public int Id { get; set; } = 0;

        public string Name { get; set; }

        public int Type { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateLine { get; set; }

        public int TimeFromMin { get; set; }

        public int TimeFromHrs { get; set; }

        public int TimeToMin { get; set; }

        public int TimeToHrs { get; set; }

        public string Venue { get; set; }
    }
}
