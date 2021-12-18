using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace SPDC.WebAPI.Helpers
{
    public class BindingModelHelper
    {
        public static string GetModelStateErrorMessage(ModelStateDictionary ModelState, string formName, string langCode)
        {
            string errorStr = "";
            foreach (ModelState modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    //errorStr = errorStr + error.ErrorMessage + " ";                    
                    var path = System.Web.HttpContext.Current.Server.MapPath($"~/MultiLanguage/{formName + langCode}.json");
                    try
                    {
                        JObject o1 = JObject.Parse(File.ReadAllText(path));
                        errorStr = errorStr + o1.Property(string.IsNullOrWhiteSpace(error.ErrorMessage) ? "entity_validation_message" : error.ErrorMessage).Value.ToString() + " ";
                    }
                    catch
                    {
                        errorStr = errorStr + "File multi language is not found or message have not setted yet ";
                    }
                }
            }
            return errorStr;
        }
    }
}