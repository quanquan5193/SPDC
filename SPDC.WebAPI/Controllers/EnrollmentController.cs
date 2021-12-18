using Microsoft.AspNet.Identity;
using SPDC.Common;
using SPDC.Model.BindingModels.Enrollment;
using SPDC.Model.ViewModels.Enrollment;
using SPDC.Service;
using SPDC.WebAPI.Helpers;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Enrollment")]
    [EnableCors("*", "*", "*")]
    public class EnrollmentController : ApiControllerBase
    {
        private readonly IEnrollmentService _service;

        public EnrollmentController(IEnrollmentService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("GetMyCourse")]
        //[AllowAnonymous]
        public async Task<IHttpActionResult> GetMyCourse(GetMyCourseBingdingModel model)
        {
            var id = 0;

            if (model.UserId == 0)
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);

                if (!success)
                {
                    Log.Error($"Failed Get Calendar");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "Failed Get Calendar", Data = null });
                }
            }
            model.UserId = id;

            try
            {
                var result = await _service.GetClasses(model.UserId, model.Index, model.PageSize);

                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message, false));
            }
        }

        [HttpGet]
        [Route("GetEnrollmentDetail")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetEnrollmentDetail(int applicationId)
        {
            try
            {
                var result = await _service.GetEnrollmentDetail(applicationId);

                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message, false));
            }
        }

        [HttpPost]
        [Route("GetClassDetail")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetClassDetail(GetClassDetailBingdingModel model)
        {
            try
            {
                var result = await _service.GetClassDetail(model.ClassId, model.Index, model.PageSize);

                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message, false));
            }
        }

        [HttpPost]
        [Route("GetCalendar")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCalendar(GetCalendarBindingModel model)
        {
            int id = 0;
            if (model.UserId == 0)
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);

                if (!success)
                {
                    Log.Error($"Failed Get Calendar");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "Failed Get Calendar", Data = null });
                }

                model.UserId = id;
            }

            try
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (string.IsNullOrEmpty(model.From) || string.IsNullOrEmpty(model.To))
                {

                }
                else
                {
                    fromDate = DateTime.ParseExact(model.From, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(model.To, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                var result = await _service.GetCalendar(model.UserId, fromDate, toDate);

                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message, false));
            }
        }

        [HttpPost]
        [Route("GetExamList")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetExamList(GetExamListBindingModel model)
        {
            try
            {
                var result = await _service.GetExams(model.ApplicationId, model.Index, model.PageSize);

                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message, false));
            }
        }

        [HttpGet]
        [Route("GetInvoices/{applicationId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetInvoices(int applicationId)
        {
            try
            {
                var result = await _service.GetInvoices(applicationId);

                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message, false));
            }
        }

        //[HttpGet]
        //[Route("GetMyCourses")]
        //[AllowAnonymous]
        //public async Task<IHttpActionResult> GetMyCourses()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex.Message);
        //    }

        //    return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        //}

        //[HttpGet]
        //[Route("GetInvoiceList")]
        //[AllowAnonymous]
        //public async Task<IHttpActionResult> GetInvoiceList()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex.Message);
        //    }

        //    return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        //}

    }
}