using AutoMapper;
using Microsoft.AspNet.Identity;
using SPDC.Common;
using SPDC.Model.BindingModels;
using SPDC.Model.Models;
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
    [RoutePrefix("api/Particular")]
    [EnableCorsAttribute("*", "*", "*")]
    public class ParticularController : ApiControllerBase
    {
        IParticularService _particularService;
        public ParticularController(IParticularService particularService)
        {
            _particularService = particularService;

        }

        [HttpGet]
        [Route("personal-particular")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetParticularUserByEmail(int userId = 0)
        {
            Log.Info($"Start Get Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
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
                    Log.Error($"Failed Get Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
            }

            var particularModel = await _particularService.GetParticularByUserId(id, (int)GetLanguageCode());
            if (particularModel != null)
            {
                Log.Info($"Completed Get Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Get Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return (particularModel != null) ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, Mapper.Map<ParticularBindingModel>(particularModel)))
                : Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }


        [HttpGet]
        [Route("personal-particular-temp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetParticularUserByEmailTemp(int userId = 0)
        {
            Log.Info($"Start Get Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
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
                    Log.Error($"Failed Get Personal Particulartemp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
            }

            var particularModel = await _particularService.GetParticularByUserTempId(id, (int)GetLanguageCode());
            if (particularModel != null)
            {
                Log.Info($"Completed Get Personal Particular temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                particularModel = await _particularService.GetParticularByUserId(id, (int)GetLanguageCode());
                if (particularModel != null)
                {
                    Log.Info($"Completed Get Personal Particular temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                }
                else
                {
                    Log.Error($"Failed Get Personal Particular temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                }
            }
            return (particularModel != null) ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, Mapper.Map<ParticularBindingModel>(particularModel)))
                : Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }

        [HttpPost]
        [Route("update-particular")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateParticular(ParticularBindingModel model)
        {
            Log.Info($"Start Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }
            DateTime? passportDateToAsign = null;

            model.HKIDNo = string.IsNullOrWhiteSpace(model.HKIDNo) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.HKIDNo));
            model.PassportNo = string.IsNullOrWhiteSpace(model.PassportNo) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.PassportNo));
            model.MobileNumber = string.IsNullOrWhiteSpace(model.MobileNumber) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.MobileNumber));

            //var hasPassportExiredDate = false;
            if (model.HKIDNo == null && /*!String.IsNullOrEmpty(model.PassportExpiredDate)*/model.PassportExpiredDate != null)
            {
                var expiredDateConvert = DateTime.Now;
                //hasPassportExiredDate = DateTime.TryParse(model.PassportExpiredDate, out expiredDateConvert);
                //passportDateToAsign = expiredDateConvert;
                passportDateToAsign = model.PassportExpiredDate;
            }
            if (string.IsNullOrEmpty(model.HKIDNo) && (String.IsNullOrEmpty(model.PassportNo) || !model.PassportExpiredDate.HasValue /* || !hasPassportExiredDate*/))
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("provide_hkid_or_passport_and_expired_date"));
            }

            var hkId = !String.IsNullOrEmpty(model.HKIDNo) ? EncryptUtilities.EncryptAes256(model.HKIDNo) : null;
            var stringHkId = hkId != null ? EncryptUtilities.GetEncryptedString(hkId) : null;

            var passPort = !String.IsNullOrEmpty(model.PassportNo) ? EncryptUtilities.EncryptAes256(model.PassportNo) : null;
            var stringPassportNo = passPort != null ? EncryptUtilities.GetEncryptedString(passPort) : null;


            var mobileNumber = !String.IsNullOrEmpty(model.MobileNumber) ? EncryptUtilities.EncryptAes256(model.MobileNumber) : null;
            var stringMobileNumber = mobileNumber != null ? EncryptUtilities.GetEncryptedString(mobileNumber) : null;

            bool isExistHKidOrPassportNo = await _particularService.IsExistHKidOrPassportNo(model.Id, stringHkId, stringPassportNo);
            bool isExistMobileNumber = await _particularService.IsExistMobileNumber(stringMobileNumber, model.Id);

            if (isExistHKidOrPassportNo || isExistMobileNumber)
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("cic_hkid_passport_no_or_mobile_number_is_existed")));
            }

            var isUpdate = await _particularService.UpdateParticularById(model, (int)GetLanguageCode(), stringHkId, stringPassportNo, stringMobileNumber);
            if (isUpdate)
            {
                Log.Info($"Completed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdate == true ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("update_success", "ServerMessages", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure", "ServerMessages", GetLanguageCode().ToString())));
        }


        [HttpPost]
        [Route("update-particular-temp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateParticularTemp(ParticularBindingModel model)
        {
            Log.Info($"Start Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }
            DateTime? passportDateToAsign = null;

            // Case user don't click async button in E-Application Form step 1
            if (model.Id == 0)
            {
                int id = 0;
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
                if (!success)
                {
                    Log.Error($"Failed Get Personal Particulartemp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
                model.Id = id;
            }

            model.HKIDNo = string.IsNullOrWhiteSpace(model.HKIDNo) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.HKIDNo));
            model.PassportNo = string.IsNullOrWhiteSpace(model.PassportNo) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.PassportNo));
            model.MobileNumber = string.IsNullOrWhiteSpace(model.MobileNumber) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.MobileNumber));

            //var hasPassportExiredDate = false;
            if (model.HKIDNo == null && /*!String.IsNullOrEmpty(model.PassportExpiredDate)*/model.PassportExpiredDate != null)
            {
                var expiredDateConvert = DateTime.Now;
                //hasPassportExiredDate = DateTime.TryParse(model.PassportExpiredDate, out expiredDateConvert);
                //passportDateToAsign = expiredDateConvert;
                passportDateToAsign = model.PassportExpiredDate;
            }
            if (string.IsNullOrEmpty(model.HKIDNo) && (String.IsNullOrEmpty(model.PassportNo) || !model.PassportExpiredDate.HasValue /* || !hasPassportExiredDate*/))
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("You have to provide HKID or Passport and Expired Date"));
            }

            var hkId = !String.IsNullOrEmpty(model.HKIDNo) ? EncryptUtilities.EncryptAes256(model.HKIDNo) : null;
            var stringHkId = hkId != null ? EncryptUtilities.GetEncryptedString(hkId) : null;

            var passPort = !String.IsNullOrEmpty(model.PassportNo) ? EncryptUtilities.EncryptAes256(model.PassportNo) : null;
            var stringPassportNo = passPort != null ? EncryptUtilities.GetEncryptedString(passPort) : null;


            var mobileNumber = !String.IsNullOrEmpty(model.MobileNumber) ? EncryptUtilities.EncryptAes256(model.MobileNumber) : null;
            var stringMobileNumber = mobileNumber != null ? EncryptUtilities.GetEncryptedString(mobileNumber) : null;

            bool isExistHKidOrPassportNo = await _particularService.IsExistHKidOrPassportNo(model.Id, stringHkId, stringPassportNo);
            bool isExistMobileNumber = await _particularService.IsExistMobileNumber(stringMobileNumber, model.Id);

            if (isExistHKidOrPassportNo || isExistMobileNumber)
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("CICNumber, HKid, PassportNo  or Mobile Number is existed"));
            }

            var isUpdate = await _particularService.UpdateParticularByIdTemp(model, (int)GetLanguageCode(), stringHkId, stringPassportNo, stringMobileNumber);
            if (isUpdate)
            {
                Log.Info($"Completed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdate == true ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("update_success", "ServerMessages", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure", "ServerMessages", GetLanguageCode().ToString())));
        }


        [HttpPost]
        [Route("particular-hkid-passportid")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetParticularByHKIdOrPassportId(ApplicationBindingModel model)
        {
            string logInfo = string.IsNullOrEmpty(model.HKIdNo) ? $"Passport Id: {model.PassportID}" : $"HKID: {model.HKIdNo}";
            Log.Info("Start Get Particular - " + logInfo);
            var result = await _particularService.GetParticularByHKIDOrPassportId(model, (int)GetLanguageCode());
            if (result != null)
            {
                Log.Info("Completed Get Particular - " + logInfo);
            }
            else
            {
                Log.Error("Failed Get Particular - " + logInfo);
            }

            return (result != null) ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, Mapper.Map<ParticularBindingModel>(result)))
               : Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }
    }
}
