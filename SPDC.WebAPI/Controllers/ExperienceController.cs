using Microsoft.AspNet.Identity;
using SPDC.Common;
using SPDC.Model.BindingModels;
using SPDC.Model.ViewModels;
using SPDC.Service.Services;
using SPDC.WebAPI.Helpers;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Experience")]
    public class ExperienceController : ApiControllerBase
    {

        private IExperienceService _experienceService;
        private ILanguageService _languageService;
        private IWorkExperienceService _workExperienceService;

        public ExperienceController(IExperienceService experienceService, ILanguageService languageService, IWorkExperienceService workExperienceService)
        {
            _experienceService = experienceService;
            _languageService = languageService;
            _workExperienceService = workExperienceService;
        }

        [HttpGet]
        [Route("get-experience")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Getexperience()
        {
            Log.Info($"Start Get Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _experienceService.Getexperience(id);

            List<WorkExperienceBindingModel> subQualificationViewModels = new List<WorkExperienceBindingModel>();

            if (result != null)
            {
                foreach (var item in result)
                {
                    subQualificationViewModels.Add(new WorkExperienceBindingModel()
                    {
                        Id = item.Id,
                        FromYear = item.FromYear,
                        ToYear = item.ToYear,
                        Location = item.WorkExperienceTrans.Count > 0 ? item.WorkExperienceTrans.Where(x => x.LanguageId == lang.Id && x.WorkExperienceId == item.Id).FirstOrDefault().Location : "",
                        Position = item.WorkExperienceTrans.Count > 0 ? item.WorkExperienceTrans.Where(x => x.LanguageId == lang.Id && x.WorkExperienceId == item.Id).FirstOrDefault().Position : "",
                        BIMRelated = item.BIMRelated,
                        ClassifyWorkingExperience = item.ClassifyWorkingExperience,
                        JobNature = item.WorkExperienceTrans.Count > 0 ? item.WorkExperienceTrans.Where(x => x.LanguageId == lang.Id && x.WorkExperienceId == item.Id).FirstOrDefault().JobNature : "",
                        UserId = item.UserId
                    });
                }
            }

            Log.Info($"Completed Get Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, subQualificationViewModels));
        }

        [HttpPost]
        [Route("delete-experience")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> DeleteExperience(ExperienceDeleteBindingModel model)
        {
            Log.Info($"Start Get Experience - Id: {model.ExperienceID}");
            var success = await _experienceService.DeleteWorkExperience(model.ExperienceID);
            if (success)
            {
                Log.Info($"Completed Get Experience - Id: {model.ExperienceID}");
            }
            else
            {
                Log.Error($"Failed Get Experience - Id: {model.ExperienceID}");
            }
            return success ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to delete"));
        }

        [HttpPost]
        [Route("create-experience")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateExperience(WorkExperienceBindingModel model)
        {

            Log.Info($"Start Create Experience - Id: {model.Id}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Create Experience - Id: {model.Id}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Create Experience - Id: {model.Id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _experienceService.CreateExperience(model, lang.Id, id);
            if (result)
            {
                Log.Info($"Completed Create Experience - Id: {model.Id}");
            }
            else
            {
                Log.Info($"Failed Create Experience - Id: {model.Id}");
            }
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to create"));
        }

        [HttpPost]
        [Route("update-experience")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateExperience(WorkExperienceBindingModel model)
        {
            Log.Info($"Start Update Experience - Id: {model.Id}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update Experience - Id: {model.Id}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Experience - Id: {model.Id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _experienceService.UpdateExperience(model, lang.Id, id);
            if (result)
            {
                Log.Info($"Completed Update Experience - Id: {model.Id}");
            }
            else
            {
                Log.Error($"Failed Update Experience - Id: {model.Id}");
            }
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpGet]
        [Route("get-experience-by-id")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetExperienceById(int experienceID)
        {
            Log.Info($"Start Get Experience - Id: {experienceID}");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _experienceService.GetexperienceById(experienceID);

            WorkExperienceBindingModel workExperienceBindingModel = new WorkExperienceBindingModel();
            if (result != null)
            {
                workExperienceBindingModel.Id = result.Id;
                workExperienceBindingModel.FromYear = result.FromYear;
                workExperienceBindingModel.ToYear = result.ToYear;
                workExperienceBindingModel.Location = result.WorkExperienceTrans.Count > 0 ? result.WorkExperienceTrans.Where(x => x.LanguageId == lang.Id && x.WorkExperienceId == result.Id).FirstOrDefault().Location : "";
                workExperienceBindingModel.Position = result.WorkExperienceTrans.Count > 0 ? result.WorkExperienceTrans.Where(x => x.LanguageId == lang.Id && x.WorkExperienceId == result.Id).FirstOrDefault().Position : "";
                workExperienceBindingModel.BIMRelated = result.BIMRelated;
                workExperienceBindingModel.ClassifyWorkingExperience = result.ClassifyWorkingExperience;
                workExperienceBindingModel.JobNature = result.WorkExperienceTrans.Count > 0 ? result.WorkExperienceTrans.Where(x => x.LanguageId == lang.Id && x.WorkExperienceId == result.Id).FirstOrDefault().JobNature : "";
                workExperienceBindingModel.UserId = result.UserId;
            }

            Log.Info($"Completed Get Experience - Id: {experienceID}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, workExperienceBindingModel));
        }

        [HttpGet]
        [Route("export-experience")]
        public async Task<HttpResponseMessage> ExportWorkExperience(int type)
        {
            Log.Info($"Start Export Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Export Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("update_failure"), Data = null });
            }
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Export Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var result = await _workExperienceService.ExportWorkExperience(id, (int)GetLanguageCode(), type);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Export Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("export_was_failed")));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Export Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            return httpResponseMessage;
        }
    }
}
