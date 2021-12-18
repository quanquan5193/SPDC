using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using static SPDC.Common.StaticConfig;

namespace SPDC.WebAPI.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {

        public LanguageCode GetLanguageCode()
        {
            var code = "EN";

            var a = Request.Headers.AcceptLanguage;
            if (a.Count > 0)
            {
                code = a.First().Value;
            }

            switch (code)
            {
                case "EN":
                case "en":
                    return LanguageCode.EN;
                case "CN":
                case "cn":
                    return LanguageCode.CN;
                case "HK":
                case "hk":
                    return LanguageCode.HK;
                default:
                    return LanguageCode.EN;
            }
        }

    }
}