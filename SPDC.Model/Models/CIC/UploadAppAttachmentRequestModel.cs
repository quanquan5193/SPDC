using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPDC.Model.Models.CIC
{
    public class UploadAppAttachmentRequestModel
    {
        public string appID { get; set; }

        public string createdBy { get; set; }

        public HttpPostedFileBase file { get; set; }
    }
}