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
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPDC.WebAPI.Controllers
{

    [Authorize]
    [RoutePrefix("api/Content")]
    [EnableCorsAttribute("*", "*", "*")]
    public class ContentController : ApiControllerBase
    {
        private IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost]
        [Route("List")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetContents(ContentBindingModel model)
        {
            Log.Info("Start Get List Content");
            var result = _contentService.GetContents(model.ContentManagement, model.ContentType, model.index, model.sortBy, model.isDescending, model.size, model.typeCode, model.BulkStatus, out int count/*, model.ApplyingFor*/);

            Log.Info("Completed Get List Content");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new PaginationSet<ContentViewModel>()
            {
                Items = result,
                Page = model.index,
                TotalCount = count
            }));
        }

        [HttpGet]
        [Route("ContentType")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetContentType()
        {
            Log.Info("Start Get Content Type");
            var result = await _contentService.GetContentType();
            Log.Info("Completed Get Content Type");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }
    }
}
