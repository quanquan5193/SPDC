using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service
{
    public class ResultModel<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;
        public T Data { get; set; }

    }
}
