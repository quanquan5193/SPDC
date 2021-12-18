using SPDC.Common;
using SPDC.Model.BindingModels;
using SPDC.Service.Services;
using SPDC.WebAPI.Helpers;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserSubscription")]
    [EnableCorsAttribute("*", "*", "*")]
    public class UserSubscriptionController : ApiControllerBase
    {
        IUserSucscriptionService _userSucscriptionService;

        public UserSubscriptionController(IUserSucscriptionService userSucscriptionService)
        {
            _userSucscriptionService = userSucscriptionService;
        }

        [HttpPost]
        [Route("CreateSubscription")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateSubscription(UserSubscriptionBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            var result = await _userSucscriptionService.CreateSubscription(model);

            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Email is already existed", false));
        }

        [HttpGet]
        [Route("UnSubcribe/{id}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UnSubcribeWebsite(int id)
        {
            var result = await Task.Run(() => _userSucscriptionService.UnSubcribeWebsite(id));
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to unsubcribe", false));
        }
    }
}
