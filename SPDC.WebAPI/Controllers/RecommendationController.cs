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
    [RoutePrefix("api/Recommendation")]
    public class RecommendationController : ApiControllerBase
    {

        private IEmployerRecommendationService _employerRecommendationService;
        private ILanguageService _languageService;

        public RecommendationController(IEmployerRecommendationService employerRecommendationService, ILanguageService languageService)
        {
            _employerRecommendationService = employerRecommendationService;
            _languageService = languageService;
        }

        [HttpGet]
        [Route("get-recommendation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetRecommendation(int userId = 0)
        {
            Log.Info($"Start Get Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            if (userId != 0)
            {
                id = userId;
            }
            else
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
                if (!success)
                {
                    Log.Error($"Failed Get Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _employerRecommendationService.GetRecommendation(id);

            List<RecommendationEmployerNewBindingModel> recommendationEmployerBindingModels = new List<RecommendationEmployerNewBindingModel>();

            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id).Count() > 0)
                    {
                        recommendationEmployerBindingModels.Add(new RecommendationEmployerNewBindingModel()
                        {
                            Id = item.Id,
                            ContactPersonTel = item.Tel,
                            CompanyName = item.EmployerRecommendationTrans.Count > 0 ? item.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == item.Id).FirstOrDefault().CompanyName : "",
                            ContactPersonName = item.EmployerRecommendationTrans.Count > 0 ? item.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == item.Id).FirstOrDefault().ContactPerson : "",
                            ContactPersonPosition = item.EmployerRecommendationTrans.Count > 0 ? item.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == item.Id).FirstOrDefault().Position : "",
                            ContactPersonEmail = item.EmployerRecommendationTrans.Count > 0 ? item.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == item.Id).FirstOrDefault().Email : "",
                            UserId = item.UserId
                        });
                    }

                }
            }

            Log.Info($"Completed Get Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, recommendationEmployerBindingModels));
        }


        [HttpPost]
        [Route("create-recommendation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateRecommendation(RecommendationEmployerNewBindingModel model)
        {
            Log.Info($"Start Create Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Create Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Create Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _employerRecommendationService.CreateRecommendation(model, lang.Id, id);
            if (result)
            {
                Log.Info($"Completed Create Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Create Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to create"));
        }

        [HttpPost]
        [Route("delete-recommendation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> DeleteRecommendation(RecommendationIdDeleteBindingModel model)
        {
            Log.Info($"Start Delete Recommendation - Id: {model.RecommendationId}");
            var success = await _employerRecommendationService.DeleteRecommendation(model.RecommendationId);
            if (success)
            {
                Log.Info($"Completed Delete Recommendation - Id: {model.RecommendationId}");
            }
            else
            {
                Log.Error($"Failed Delete Recommendation - Id: {model.RecommendationId}");
            }
            return success ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to delete"));
        }

        [HttpPost]
        [Route("update-recommendation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateRecommendation(RecommendationEmployerNewBindingModel model)
        {
            Log.Info($"Start Update Recommendation - Id: {model.Id}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update Recommendation - Id: {model.Id}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Recommendation - Id: {model.Id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _employerRecommendationService.UpdateRecommendation(model, lang.Id, id);
            if (result)
            {
                Log.Info($"Completed Update Recommendation - Id: {model.Id}");
            }
            else
            {
                Log.Error($"Failed Update Recommendation - Id: {model.Id}");
            }
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }


        [HttpGet]
        [Route("get-recommendation-by-id")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetRecommendationById(int recommendationId)
        {
            Log.Info($"Start Get Recommendation - Id: {recommendationId}");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _employerRecommendationService.GetRecommendationById(recommendationId);

            RecommendationEmployerNewBindingModel recommendationEmployerNewBindingModel = new RecommendationEmployerNewBindingModel();

            if (result != null)
            {

                recommendationEmployerNewBindingModel.Id = result.Id;
                recommendationEmployerNewBindingModel.ContactPersonTel = result.Tel;
                recommendationEmployerNewBindingModel.CompanyName = result.EmployerRecommendationTrans.Count > 0 ? result.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == result.Id).FirstOrDefault()?.CompanyName : "";
                recommendationEmployerNewBindingModel.ContactPersonName = result.EmployerRecommendationTrans.Count > 0 ? result.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == result.Id).FirstOrDefault()?.ContactPerson : "";
                recommendationEmployerNewBindingModel.ContactPersonPosition = result.EmployerRecommendationTrans.Count > 0 ? result.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == result.Id).FirstOrDefault()?.Position : "";
                recommendationEmployerNewBindingModel.ContactPersonEmail = result.EmployerRecommendationTrans.Count > 0 ? result.EmployerRecommendationTrans.Where(x => x.LanguageId == lang.Id && x.EmployerRecommendationId == result.Id).FirstOrDefault()?.Email : "";
                recommendationEmployerNewBindingModel.UserId = result.UserId;
            }
            Log.Info($"Completed Get Recommendation - Id: {recommendationId}");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, recommendationEmployerNewBindingModel));
        }
    }
}
