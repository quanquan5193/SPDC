using SPDC.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using SPDC.WebAPI.Models;
using System.IO;
using System.Web;
using SPDC.Common;
using Microsoft.AspNet.Identity;
using SPDC.Data;
using Microsoft.AspNet.Identity.Owin;
using SPDC.Model.ViewModels;
using SPDC.Model.BindingModels;
using SPDC.WebAPI.Helpers;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/UploadFile")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UploadFileController : ApiControllerBase
    {

        private IUploadFileService _uploadFileService;
        private ApplicationUserManager _userManager;
        private IClientService _clientService;

        public UploadFileController(IUploadFileService uploadFileService, IClientService clientService)
        {
            _clientService = clientService;
            _uploadFileService = uploadFileService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [Route("file")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UploadFile(int userId = 0)
        {

            Log.Info($"Start Upload File");
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
                    Log.Error($"Failed Upload File");
                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Upload File");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var lstFilUpload = HttpContext.Current.Request.Files;
            for (int i = 0; i < lstFilUpload.Count; i++)
            {

                var file = lstFilUpload[i];
                if (file.ContentLength > StaticConfig.MaximumFileLength)
                {
                    Log.Error($"Failed Upload File");
                    Content(HttpStatusCode.BadRequest, new ActionResultModel($"File {Path.GetFileName(file.FileName)} is too large"));
                }
            }

            var isUpload = await _uploadFileService.UploadFile(lstFilUpload, id);
            if (isUpload)
            {
                Log.Info($"Completed Upload File");
            }
            else
            {
                Log.Error($"Failed Upload File");
            }

            return isUpload ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to upload"));

        }

        [HttpPost]
        [Route("delete-file")]
        //[AllowAnonymous]
        public async Task<IHttpActionResult> DeleteFile(DocToDeleteBindingModel model)
        {
            Log.Info($"Start Delete File - Id: {model.DocID}");
            var isUpload = await _uploadFileService.DeleteFile(model.DocID);
            Log.Info($"Completed Delete File");

            return isUpload ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to delete file"));
        }

        [HttpPost]
        [Route("delete-file-temp")]
        //[AllowAnonymous]
        public async Task<IHttpActionResult> DeleteFileTemp(DocToDeleteBindingModel model)
        {
            Log.Info($"Start Delete File - Id: {model.DocID}");
            var isUpload = await _uploadFileService.DeleteFileTemp(model.DocID);
            Log.Info($"Completed Delete File");

            return isUpload ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to delete file"));
        }



        [HttpGet]
        [Route("get-file")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllFile(int userId = 0)
        {

            Log.Info($"Start Get All File - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
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
                    Log.Error($"Start Get All File - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
            }


            var result = await _uploadFileService.GetAllFile(id);

            List<FileViewModel> fileReturnViewModels = new List<FileViewModel>();

            if (result != null)
            {
                foreach (var item in result)
                {
                    fileReturnViewModels.Add(new FileViewModel()
                    {
                        DocId = item.Document.Id,
                        FileName = item.Document.FileName,
                        ModifiedDate = item.Document.ModifiedDate
                    });
                }
            }
            Log.Info($"Completed Get All File - User Id: {HttpContext.Current.User.Identity.GetUserId()}");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, fileReturnViewModels));
        }


        [HttpGet]
        [Route("get-file-temp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllFileTemp(int userId = 0)
        {

            Log.Info($"Start Get All File - User Id Temp: {HttpContext.Current.User.Identity.GetUserId()}");
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
                    Log.Error($"Start Get All File  Temp- User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
            }
            List<FileViewModel> fileReturnViewModels = new List<FileViewModel>();

            var result = await _uploadFileService.GetAllFileTemp(id);
            string apiPath = await _clientService.GetClientUrlByNameAsync("ApiPortal");

            if (result?.Any() != true)
            {
                var result2 = await _uploadFileService.GetAllFile(id);
                if (result2?.Any() == true)
                {
                    foreach (var item in result2)
                    {
                        fileReturnViewModels.Add(new FileViewModel()
                        {
                            DocId = item.Document.Id,
                            FileName = item.Document.FileName,
                            ModifiedDate = item.Document.ModifiedDate,
                            Url = $"{ConfigHelper.GetByKey("DMZAPI")}api/Proxy?url=UploadFile/download-document/{item.Document.Id}"

                        });
                    }
                }
            }
            else
            {
                foreach (var item in result)
                {
                    fileReturnViewModels.Add(new FileViewModel()
                    {
                        DocId = item.Document.Id,
                        FileName = item.Document.FileName,
                        ModifiedDate = item.Document.ModifiedDate,
                        Url = $"{ConfigHelper.GetByKey("DMZAPI")}api/Proxy?url=UploadFile/download-document/{item.Document.Id}"
                    });
                }
            }
            Log.Info($"Completed Get All File Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, fileReturnViewModels));
        }

        [HttpGet]
        [Route("download-document/{docId}")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> DownloadCourseDocument(int docId)
        {
            Log.Info($"Start Download Document - Id: {docId}");
            if (docId == 0)
            {
                Log.Error($"Failed Download Document - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel("Document Id can not be 0"));
            }

            var result = await _uploadFileService.DownloadCourseDocument(docId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download Document - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download Document Temp - Id: {docId}");
            return httpResponseMessage;
        }


        [HttpGet]
        [Route("download-document-temp")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> DownloadCourseDocumentTemp(int docId)
        {
            Log.Info($"Start Download Document Temp - Id: {docId}");
            if (docId == 0)
            {
                Log.Error($"Failed Download Document Temp - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel("Document Id can not be 0"));
            }

            var result = await _uploadFileService.DownloadCourseDocumentTemp(docId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download Document Temp - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download Document Temp - Id: {docId}");
            return httpResponseMessage;
        }

    }
}
