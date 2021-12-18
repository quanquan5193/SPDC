using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class ContentBindingModel
    {
        public string ContentManagement { get; set; } = "";

        public int ContentType { get; set; } = 0;

        public int index { get; set; } = 1;

        public string sortBy { get; set; } = "Id";

        public bool isDescending { get; set; } = false;

        public int size { get; set; } = 20;
        public int typeCode { get; set; }
        public bool? BulkStatus { get; set; }
        public int ApplyingFor { get; set; }
    }

    public class GetMatchContentBindingModel
    {
        public string Keyword { get; set; } = "";

        public int ContentType { get; set; } = 0;

        public int Page { get; set; } = 1;

        public string SortBy { get; set; } = "Id";

        public bool IsDescending { get; set; } = false;

        public int Size { get; set; } = 20;

    }
    public class MatchContentBindingModel
    {
        public int Id { get; set; }

        public int MatchId { get; set; }
    }
}
