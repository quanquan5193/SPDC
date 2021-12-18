using Microsoft.AspNet.Identity;
using SPDC.Common;
using SPDC.Model.BindingModels.Approval;
using SPDC.Model.ViewModels.Approval;
using SPDC.Service.Services;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace SPDC.WebAPI.Controllers
{
    [RoutePrefix("api/Approval")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SettingUpController : ApiControllerBase
    {
        private ISettingUpService _settingUpService;
        public SettingUpController(ISettingUpService settingUpService)
        {
            _settingUpService = settingUpService;
        }

        [HttpPost]
        [Route("ListCourseHistories")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetListCourseApprovedHistoryByCourseId(CourseHistoryFilter filter)
        {
            var result = await _settingUpService.GetListCourseApprovedHistoryByCourseId(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("ChangeApprovedStatusCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ChangeApprovedStatusCourseSetup()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<CancelSubmitCourseBindingModel>(HttpContext.Current.Request.Params["CancelSubmitModel"]);

            var files = HttpContext.Current.Request.Files;
            var result = await _settingUpService.ChangeApprovedStatusCourseSetup(id, model.CourseId, model.ApprovedStatus, model.Remarks, files, GetLanguageCode().ToString());

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("ListClassHistories")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetListClassApprovedHistoryByCourseId(ClassHistoryFilter filter)
        {
            var result = await _settingUpService.GetListClassApprovedHistoryByCourseId(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("ChangeApprovedStatusClass")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ChangeApprovedStatusClassSetup()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<CancelSubmitClassBindingModel>(HttpContext.Current.Request.Params["CancelSubmitModel"]);

            var files = HttpContext.Current.Request.Files;
            var result = await _settingUpService.ChangeApprovedStatusClassSetup(id, model.CourseId, model.ApprovedStatus, model.Remarks, files);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("ListSubClassHistories")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetListSubClassApprovalHistories(SubClassHistoryFilter filter)
        {
            var result = await _settingUpService.GetListSubClassApprovalHistories(filter);
         
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("CancelledOrPostponedClass")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> SubmitCancelledOrPostponedSubClassSetup()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<CancelPostponedSubClassBindingModel>(HttpContext.Current.Request.Params["CancelledPostponedModel"]);

            var files = HttpContext.Current.Request.Files;
            var result = await _settingUpService.SubmitCancelledOrPostponedSubClassSetup(model, id, files);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpPost]
        [Route("ApprovedSubClass")]
        public async Task<IHttpActionResult> ChangeApprovedStatusSubClass()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var model = new JavaScriptSerializer().Deserialize<CancelApprovalSubClassBindingModel>(HttpContext.Current.Request.Params["CancelApprovalModel"]);

            var files = HttpContext.Current.Request.Files;
            var result = await _settingUpService.ChangeApprovedStatusSubClass(model, id, files);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }
    }
}
