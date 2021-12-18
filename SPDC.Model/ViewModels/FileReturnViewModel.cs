using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class FileReturnViewModel
    {
        public MemoryStream Stream { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
    }

    public class FileViewModel
    {
        public int DocId { get; set; }

        public string FileName { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Url { get; set; }
    }
}
