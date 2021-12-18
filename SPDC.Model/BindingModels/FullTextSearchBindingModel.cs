using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class FullTextSearchBindingModel
    {
        public string Keyword { get; set; }
        
        public int Page { get; set; }
        
        public int Records { get; set; }

        public string CMSType { get; set; }
    }
}
