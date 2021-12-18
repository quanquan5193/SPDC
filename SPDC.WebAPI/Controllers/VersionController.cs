using SPDC.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Version")]
    public class VersionController : ApiControllerBase
    {
        private IVersionService _versionService;
        public VersionController(IVersionService versionService)
        {
            _versionService = versionService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetVersion(string type)
        {
            return Ok(await _versionService.GetVersion(type));
        }

    }
}
