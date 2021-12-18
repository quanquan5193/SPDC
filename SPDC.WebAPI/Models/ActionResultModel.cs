using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPDC.WebAPI.Models
{
    public class ActionResultModel
    {
        public string Message { get; set; }

        public object Data { get; set; }

        public bool Success { get; set; }

        public ActionResultModel() { }

        public ActionResultModel(string message, bool success = false, object data = null)
        {
            Message = message;
            Data = data;
            Success = success;
        }
    }

    
}