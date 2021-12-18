using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class DocumentBindingModel
    {
        public int Id { get; set; } = 0;

        public string Url { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
