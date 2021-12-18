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
using System.Web.Http.Cors;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Qualification")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QualificationController : ApiControllerBase
    {
        private IQualificationService _qualificationService;
        private ILanguageService _languageService;

        public QualificationController(IQualificationService qualificationService, ILanguageService languageService)
        {
            _qualificationService = qualificationService;
            _languageService = languageService;
        }

        [HttpGet]
        [Route("get-qualifications")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetQualification()
        {
            Log.Info($"Start Get Qualification - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Qualification - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _qualificationService.GetQualification(id);

            List<SubQualificationViewModel> subQualificationViewModels = new List<SubQualificationViewModel>();

            if (result != null)
            {
                foreach (var item in result)
                {
                    var issuingAuthority = item.QualificationsTrans.FirstOrDefault(n => n.LanguageId == lang.Id)?.IssuingAuthority;
                    var levelAttained = item.QualificationsTrans.FirstOrDefault(n => n.LanguageId == lang.Id)?.LevelAttained;
                    if (!string.IsNullOrEmpty(issuingAuthority) && !string.IsNullOrEmpty(levelAttained))
                    {
                        subQualificationViewModels.Add(new SubQualificationViewModel()
                        {
                            Id = item.Id,
                            DateObtained = item.DateObtained,
                            IssuingAuthority = issuingAuthority,
                            LevelAttained = levelAttained,
                        });
                    }
                    
                }
            }
            Log.Info($"Completed Get Qualification - User Id: {HttpContext.Current.User.Identity.GetUserId()}");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, subQualificationViewModels));
        }

        [HttpPost]
        [Route("delete-qualifications")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> DeleteQualification(QualificationToDeleteBindingModel model)
        {
            Log.Info($"Start Delete Qualification - Id: {model.QualificationID}");
            var success = await _qualificationService.DeleteQualifications(model.QualificationID);
            if (success)
            {
                Log.Info($"Completed Delete Qualification - Id: {model.QualificationID}");
            }
            else
            {
                Log.Error($"Failed Delete Qualification - Id: {model.QualificationID}");

            }
            return success ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to delete"));
        }

        [HttpPost]
        [Route("create-qualifications")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateQualification(QualificationBindingModels model)
        {

            Log.Info($"Start Create Qualification - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Create Qualification - Id: {HttpContext.Current.User.Identity.GetUserId()}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Create Qualification - Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _qualificationService.CreateQualification(model, lang.Id, id);
            if (result)
            {
                Log.Info($"Completed Create Qualification - Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Create Qualification - Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to create"));
        }

        [HttpPost]
        [Route("update-qualifications")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateQualification(QualificationBindingModels model)
        {
            Log.Info($"Start Update Qualification - Id: {model.Id}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update Qualification - Id: {model.Id}");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Qualification - Id: {model.Id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _qualificationService.UpdateQualification(model, lang.Id, id);
            if (result)
            {
                Log.Info($"Completed Update Qualification - Id: {model.Id}");
            }
            else
            {
                Log.Error($"Failed Update Qualification - Id: {model.Id}");
            }
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpGet]
        [Route("get-qualifications-by-id")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetQualificationsById(int qualificationId)
        {
            Log.Info($"Start Get Qualification - Id: {qualificationId}");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _qualificationService.GetQualificationsById(qualificationId);

            SubQualificationViewModel subQualificationViewModel = new SubQualificationViewModel();
            if (result != null)
            {
                subQualificationViewModel.Id = result.Id;
                subQualificationViewModel.DateObtained = result.DateObtained;
                subQualificationViewModel.IssuingAuthority = result.QualificationsTrans.Count > 0 ? result.QualificationsTrans.Where(x => x.LanguageId == lang.Id && x.QualificationId == result.Id).FirstOrDefault()?.IssuingAuthority : "";
                subQualificationViewModel.LevelAttained = result.QualificationsTrans.Count > 0 ? result.QualificationsTrans.Where(x => x.LanguageId == lang.Id && x.QualificationId == result.Id).FirstOrDefault()?.LevelAttained : "";
            }

            Log.Info($"Completed Get Qualification - Id: {qualificationId}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, subQualificationViewModel));
        }
    }
}