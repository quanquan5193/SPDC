using SPDC.WebAPI.Helpers;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace SPDC.WebAPI.Providers
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode status = HttpStatusCode.BadRequest;
            String message = String.Empty;
            var exceptionType = actionExecutedContext.Exception.GetType();

            Log.Error("Exception: " + exceptionType);
            Log.Error("Exception: " + actionExecutedContext.Exception.Message);
            Log.Error("Exception: " + actionExecutedContext.Exception.StackTrace);

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Access to the Web API is not authorized.";
                status = HttpStatusCode.Unauthorized;
            }
            else
            {
                message = "Internal Server Error.";
                status = HttpStatusCode.InternalServerError;
            }

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new ObjectContent<ActionResultModel>(new ActionResultModel(message, false, GetInnerExceptionString(actionExecutedContext.Exception)), GlobalConfiguration.Configuration.Formatters.JsonFormatter),
                StatusCode = status
            };


            base.OnException(actionExecutedContext);
        }

        private string GetInnerExceptionString(Exception ex)
        {
            if (ex is DbEntityValidationException)
            {
                DbEntityValidationException dbex = ex as DbEntityValidationException;
                var a = dbex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x=>x.ErrorMessage);
                return String.Join(Environment.NewLine, dbex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
            }
            if (ex.InnerException == null)
            {
                return String.Join(Environment.NewLine, ex.Message);
            }
            else
            {
                return GetInnerExceptionString(ex.InnerException);
            }
        }
    }
}