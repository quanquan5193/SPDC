using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class SearchModel
    {
        public string Id { get; set; }

        public int DataId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Data { get; set; }

        public string DataType { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsVisible { get; set; }

        public string Url { get; set; }

        public string Path { get; set; }

        public string Content { get; set; }

        public Attachment Attachment { get; set; }

    }
}
