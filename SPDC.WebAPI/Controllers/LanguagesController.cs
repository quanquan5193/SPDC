using AutoMapper;
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
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;

namespace SPDC.WebAPI.Controllers
{

    [Authorize]
    [RoutePrefix("api/Languages")]
    [EnableCorsAttribute("*", "*", "*")]
    public class LanguagesController : ApiControllerBase
    {
        ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Code")]
        public async Task<IHttpActionResult> GetAll()
        {
            Log.Info("Start Get Languages");
            var lst = await _languageService.GetLanguages();
            Log.Info("Completed Get Languages");

            return Ok(lst);
        }

    }
}
