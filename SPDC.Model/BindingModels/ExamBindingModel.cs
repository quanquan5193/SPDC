using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class ExamBindingModel
    {
        public int Id { get; set; }

        public int ClassId { get; set; }

        public string ExamVenue { get; set; }

        public int Type { get; set; }

        public int? ClassCommonId { get; set; }

        public DateTime Date { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public DateTime Dateline { get; set; }

        public string Marks { get; set; }

        public bool IsReExam { get; set; }

        public string ExamVenueText { get; set; }

        public int? ModuleId { get; set; }

    }
}
