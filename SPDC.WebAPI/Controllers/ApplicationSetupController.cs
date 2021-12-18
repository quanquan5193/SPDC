using SPDC.Model.BindingModels;
using SPDC.Service.Services;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SPDC.Common;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Service;
using SPDC.WebAPI.Helpers;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/ApplicationSetup")]
    [EnableCors("*", "*", "*")]
    public class ApplicationSetupController : ApiControllerBase
    {
        private readonly IApplicationSetupService _applicationSetupService;
        private readonly ICourseService _courseService;

        public ApplicationSetupController(IApplicationSetupService applicationSetupService, ICourseService courseService)
        {
            _applicationSetupService = applicationSetupService;
            _courseService = courseService;

        }

        [HttpGet]
        [Route("GetApplicationByCourseId/{courseId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetApplicationByCourseId(int courseId)
        {
            Log.Info($"Start Get Application - Course Id: {courseId}");
            var result = await _applicationSetupService.GetApplicationByCourseId(courseId);
            var objViewModel = Mapper.Map<ApplicationSetupViewModel>(result.Data);
            Log.Info($"Completed Get Application - Course Id: {courseId}");

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, objViewModel));
        }

        [HttpGet]
        [Route("GetApplicationById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetApplicationById(int id)
        {
            Log.Info($"Start Get Application - Application Id: {id}");
            var result = await _applicationSetupService.GetApplicationById(id);
            //var objBindingModel = Mapper.Map<Ap>(result.Data);
            var objViewModel = Mapper.Map<ApplicationSetupViewModel>(result.Data);
            Log.Info($"Completed Get Application - Application Id: {id}");

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, objViewModel));
        }


        [HttpPost]
        [Route("SaveApplicationSetup")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SaveApplicationSetup(ApplicationSetupViewModel applicationSetupViewModel)
        {
            Log.Info($"Start Save Application Setup - Application Id: {applicationSetupViewModel.Id}");
            ApplicationSetups resultApplicationSetups;
            var objMapApplication = Mapper.Map<ApplicationSetups>(applicationSetupViewModel);
            if (applicationSetupViewModel.Id == 0)
                //update
                resultApplicationSetups = _applicationSetupService.CreateApplicationSetup(objMapApplication);
            else
                //add
                resultApplicationSetups = await _applicationSetupService.UpdateApplicationSetup(objMapApplication);

            var applicationSetupView = Mapper.Map<ApplicationSetupViewModel>(resultApplicationSetups);
            Log.Info($"Completed Save Application Setup - Application Id: {applicationSetupViewModel.Id}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, applicationSetupView));
        }

        [HttpGet]
        [Route("GetInvoicesByApplication")]
        public async Task<IHttpActionResult> GetInvoicesByApplicationId(int applicationId)
        {
            var result = await _applicationSetupService.GetInvoicesByApplicationId(applicationId);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("CheckCourseApplyAvailable/{courseId}")]
        public async Task<IHttpActionResult> CheckCourseApplyAvailable(int courseId)
        {
            var result = await _applicationSetupService.CheckCourseApplyAvailable(courseId);

            int userId = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out userId);

            if (!success)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("error", "Common", GetLanguageCode().ToString()), false, false));
            }

            if (!result)
            {
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("course_apply_msg", "CourseSearchDetail", GetLanguageCode().ToString()), false, false));
            }

            bool canApply = await _courseService.CanUserApplyCourse(courseId, userId);

            if (!canApply)
            {
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("can_not_create_application", "CourseSearchDetail", GetLanguageCode().ToString()), false, false));
            }

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }
    }
}