using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.BatchApplication
{
    public class BatchApplicationViewModel
    {
        public BatchApplicationViewModel()
        {
            Messages = new List<string>();
        }

        public string FileName { get; set; }
        public bool IsError { get; set; }
        public List<string> Messages { get; set; }


    }
}
