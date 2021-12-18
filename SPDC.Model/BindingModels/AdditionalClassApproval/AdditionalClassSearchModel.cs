using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels.AdditionalClassApproval
{
    public class AdditionalClassSearchModel
    {
        public int CourseId { get; set; }
        public string SortBy { get; set; } = "Id";
        public bool IsDescending { get; set; } = false;
        public int Index { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
