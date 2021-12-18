using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using SPDC.Common;
using SPDC.Data;
using SPDC.Model.ViewModels;
using SPDC.Service.Services;
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
using SPDC.Model.Models;
using System.IO;
using SPDC.WebAPI.Helpers;
using Nest;
using Elasticsearch.Net;
using SPDC.Model.BindingModels;
using SPDC.Common.Enums;
using System.Drawing.Imaging;
using System.Net.Mail;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Content")]
    [EnableCorsAttribute("*", "*", "*")]
    public class CMSContentController : ApiControllerBase
    {
        private ICMSContentService _iCMSContentService;
        private ApplicationUserManager _userManager;
        private readonly IUserService _userService;
        private IClientService _clientService;
        private INotificationService _notificationService;
        private ICommonDataService _commonDataService;

        //public CMSContentController(ApplicationUserManager userManager)
        //{
        //    UserManager = userManager;
        //}

        public CMSContentController(ICMSContentService cMSContentService, IUserService userService, IClientService clientService, INotificationService notificationService, ICommonDataService commonDataService)
        {
            _iCMSContentService = cMSContentService;
            _userService = userService;
            _clientService = clientService;
            _notificationService = notificationService;
            _commonDataService = commonDataService;
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

        [Route("GetCmsContent")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetListCmsContentTypes()
        {
            Log.Info("Start Get CMS Content");
            var result = await _iCMSContentService.GetListCmsContentType((int)GetLanguageCode());
            if (result.IsSuccess)
            {
                Log.Info("Completed Get CMS Content");
            }
            else
            {
                Log.Error("Failed Get CMS Content");
            }
            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed"));
        }

        [Route("GetContent")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetContentById(int id)
        {
            Log.Info("Start Get Content");
            var result = await _iCMSContentService.GetCMSContentById(id);
            if (result.IsSuccess)
            {
                Log.Info("Completed Get Content");
            }
            else
            {
                Log.Error("Failed Get Content");
            }
            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed"));
        }

        [Route("CreateUpdate")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateCmsContent()
        {
            Log.Info("Start Create CMS Content");
            var a = HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CMSBindingModel"]);
            var model = JsonConvert.DeserializeObject<CMSBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CMSBindingModel"]));
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error("Failed Create CMS Content");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error("Failed Create CMS Content");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "No user matched", Data = null });
            }

            HttpPostedFile file = null;
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                file = HttpContext.Current.Request.Files[0];
                if (!StaticConfig.imageMimeTypeExtensions.Contains(file.ContentType))
                {
                    Log.Error("Failed Create CMS Content");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "File upload is invalid" });
                }
            }

            var result = model.Id == 0 ? await _iCMSContentService.CreateCMSContent(model, user.Id, file) : await _iCMSContentService.UpdateCMSContent(model, user.Id, file);
            if (result.IsSuccess)
            {
                Log.Info("Completed Create CMS Content");

                bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);
            }
            else
            {
                Log.Error("Failed Create CMS Content");
            }

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, false, result.IsSuccess));

        }

        [Route("GetImage")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetImageById(int id)
        {
            Log.Info("Start Get Image");
            var result = await _iCMSContentService.GetImageById(id);
            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info("Completed Get Image");
            return httpResponseMessage;
        }

        [Route("Delete/{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteCmsContent(int id)
        {
            Log.Info("Start Delete Image");
            var result = await _iCMSContentService.DeleteCmsContent(id);
            if (result.IsSuccess)
            {
                Log.Info("Completed Delete Image");
            }
            else
            {
                Log.Error("Failed Delete Image");
            }
            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("Background")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetBackgroundContent(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get Background");
            var result = await _iCMSContentService.GetBackground(applyingFor);
            int imgId = 0;
            if (result.Count() > 0)
            {
                var images = result.First().CmsImages;
                if (images.Count > 0)
                {
                    var image = images.First();
                    imgId = image.Id;
                }
            }
            var imgResult = await _iCMSContentService.GetImageById(imgId);
            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(imgResult.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = imgResult.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imgResult.Data.FileType);
            Log.Info("Completed Get Background");
            return httpResponseMessage;
        }

        [HttpGet]
        [Route("SideNav")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetSideNavContent(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get SideNav");
            var result = await _iCMSContentService.GetSideNav(applyingFor);
            int imgId = 0;
            if (result.Count() > 0)
            {
                var images = result.First().CmsImages;
                if (images.Count > 0)
                {
                    var image = images.First();
                    imgId = image.Id;
                }
            }
            var imgResult = await _iCMSContentService.GetImageById(imgId);
            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(imgResult.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = imgResult.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imgResult.Data.FileType);
            Log.Info("Completed Get SideNav");
            return httpResponseMessage;
        }

        [HttpGet]
        [Route("Banner")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBannerContent(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get Banner");
            var result = await _iCMSContentService.GetBanner((int)GetLanguageCode(), applyingFor);
            List<string> urls = new List<string>();
            foreach (var item in result)
            {
                urls.Add(ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Content/GetImage?id=" + (item.CmsImages.Count > 0 ? item.CmsImages.First().Id : 0));
            }
            if (urls.Count == 0)
            {
                urls.Add(ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Content/GetImage?id=" + 0);
            }

            Log.Info("Completed Get Banner");
            return Content(HttpStatusCode.OK, urls);
        }

        [HttpGet]
        [Route("LandingPageAnnoucement")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetLandingPageAnnouncement(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get LandingPageAnnoucement");
            var result = await _iCMSContentService.GetLandingPageAnnouncement((int)GetLanguageCode(), applyingFor);
            Log.Info("Completed Get LandingPageAnnoucement");

            return Content(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<CMSBindingModel> CmsBindingModel(CmsContent result)
        {
            var userCreate = await _userService.GetUserByID(result.CreateBy);
            var userModified = await _userService.GetUserByID(result.LastModifiedBy);
            var urlImage = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Content/GetImage?id=";
            var cmsBindingModel = result.ToCMSBindingModel(userCreate?.DisplayName ?? "Not Available",
                userModified?.DisplayName ?? "Not Available", urlImage);
            return cmsBindingModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="langId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAboutUs")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAboutUs(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get AboutUs");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetAboutUs((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);
            Log.Info("Completed Get AboutUs");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="langId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetContactUs")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetContactUs(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get ContactUs");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetContactUs((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);

            Log.Info("Completed Get ContactUs");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="langId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsefulLinks")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUsefulLinks(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get GetUsefulLinks");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetUsefulLinks((int)GetLanguageCode(), applyingFor);
            if (result != null)
            {
                cmsBindingModel = await CmsBindingModel(result);
            }
            Log.Info("Completed Get GetUsefulLinks");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="langId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPrivacyPolicy")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPrivacyPolicy(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get PrivacyPolicy");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetPrivacyPolicy((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);
            Log.Info("Completed Get PrivacyPolicy");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="langId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDisclaimer")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetDisclaimer(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get Disclaimer");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetDisclaimer((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);
            Log.Info("Completed Get Disclaimer");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        [HttpGet]
        [Route("LandingPageNewsAndEvents")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetLandingPageNewAndEvent(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get NewsAndEvents");
            var result = await _iCMSContentService.GetLandingPageNewAndEvent((int)GetLanguageCode(), applyingFor);
            Log.Info("Completed Get NewsAndEvents");

            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("LandingPageHotCourse")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetLandingPageHotCourse(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get HotCourse");
            var result = await _iCMSContentService.GetLandingPageHotCourse((int)GetLanguageCode(), applyingFor);
            Log.Info("Completed Get HotCourse");

            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("LandingPageStemAlliance")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetLandingPageStemAlliance(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get StemAlliance");
            var result = await _iCMSContentService.GetLandingPageStemAlliance((int)GetLanguageCode(), applyingFor);
            Log.Info("Completed Get StemAlliance");

            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("LandingPageCareerProgression")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetLandingPageCareerProgression(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get CareerProgression");
            var result = await _iCMSContentService.GetLandingPageCareerProgression((int)GetLanguageCode(), applyingFor);
            Log.Info("Completed Get CareerProgression");

            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("FAQ")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetFAQ(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get FAQ");
            var result = await _iCMSContentService.GetFAQ((int)GetLanguageCode(), applyingFor);
            Log.Info("Completed Get FAQ");

            return Content(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("BulkActions")]
        public async Task<IHttpActionResult> PublishAndUnPublishListContent()
        {
            Log.Info("Start Change Status Bulk Action");
            var lstIdString = string.Empty;
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error("Failed Change Status Bulk Action");
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            try
            {
                lstIdString = HttpContext.Current.Request.Params["ListCotnent"];
                var listContent = JsonConvert.DeserializeObject<HandleContentBindingModel>(HttpUtility.UrlDecode(lstIdString));

                var result = await _iCMSContentService.PublishAndUnpublishContent(listContent, id);
                if (result.IsSuccess)
                {
                    Log.Info("Completed Change Status Bulk Action");
                    foreach (var cmsId in listContent.ListIdContent)
                    {
                        bool cloneElasticResult = await CloneDataToElasticAsync(id);
                    }
                }
                else
                {
                    Log.Error("Failed Change Status Bulk Action");
                }

                return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, result.Message));

            }
            catch (Exception ex)
            {
                Log.Info($"Failed Change Status Bulk Action - Message: {ex.Message}");
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetLanguageForm/{formName}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetMultiLanguage(string formName)
        {
            Log.Info($"Start Get Language Form: {formName}");
            try
            {
                var path = System.Web.HttpContext.Current.Server.MapPath($"~/MultiLanguage/{formName + GetLanguageCode().ToString()}.json");
                var result = await Task.Run(() => _iCMSContentService.GetLanguageForm(path));
                if (result.IsSuccess)
                {
                    Log.Info($"Completed Get Language Form: {formName}");
                }
                else
                {
                    Log.Error($"Failed Get Language Form: {formName}");
                }
                return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed", result.IsSuccess, result.Message));
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Get Language Form: {formName} - Message: {ex.Message}");
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("CkEditorUploads2")]

        // Don't change "IFormFile upload" text below.
        public async Task<IHttpActionResult> UploadImage()
        {
            Log.Info("Start CkEditor Uploads2");
            var lstFileInsert = HttpContext.Current.Request.Files;

            var file = lstFileInsert[0];
            var allowedExtensions = new[] { ".Jpg", ".png", ".PNG", ".JPG", "JPEG", ".jpg", ".jpeg", ".Heif", ".tiff" };
            if (file == null) return BadRequest("Null file");
            if (file.ContentLength > 10 * 1024 * 1024) return BadRequest("Max file size exceeded.");
            if (file.ContentLength == 0) return BadRequest("Empty file");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var uploadsFolderPath = Path.Combine(ConfigHelper.GetByKey("ContentUpload"), "uploads");

            if (!Directory.Exists(uploadsFolderPath))
                Common.Common.CreateDirectoryAndGrantFullControlPermission(uploadsFolderPath);

            var ext = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(ext)) return BadRequest("Invalid Image type.");


            var directory = ConfigHelper.GetByKey("ContentUpload");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
            var path = serPath + "\\" + "uploads";
            if (!Directory.Exists(path))
            {
                Common.Common.CreateDirectoryAndGrantFullControlPermission(path);
            }
            var pathFile = Common.Common.GenFileNameDuplicate(path + "\\" + fileName);

            file.SaveAs(pathFile);

            var cmsImageId = _iCMSContentService.UploadDesctiprionImg("uploads" + "\\" + fileName, fileName, file.ContentType);

            var cmsImageUrl = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Content/GetImage?id=" + cmsImageId;

            Log.Info("Completed CkEditor Uploads2");
            return Ok(new
            {

                uploaded = true,
                url = cmsImageUrl

            });
        }

        [HttpGet]
        [Route("ListEvent")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetListEvent(int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info($"Start Get List Event - Index: {index} - Size: {size}");
            var result = await _iCMSContentService.GetListEvent((int)GetLanguageCode(), index, size, applyingFor);
            Log.Info("Copmpleted Get List Event - Index: {index} - Size: {size}");

            return Content(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCampusInfo")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCampusInfo(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get CampusInfo");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetCampusInformation((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);
            Log.Info("Copmpleted Get CampusInfo");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSitemap")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSitemap(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get Sitemap");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetSitemap((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);
            Log.Info("Copmpleted Get Sitemap");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        [HttpGet]
        [Route("ListInclementWeatherArrangements")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetListInclementWeatherArrangements(int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info($"Start Get List Inclement Weather Arrangements - Index: {index} - Size: {size}");
            var result = await _iCMSContentService.GetListInclementWeatherArrangements((int)GetLanguageCode(), index, size, applyingFor);
            Log.Info($"Copmpleted Get List Inclement Weather Arrangements - Index: {index} - Size: {size}");

            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("GetInclementWeatherArrangement")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetInclementWeatherArrangement(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get InclementWeatherArrangement");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetInclementWeatherArrangement((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);
            Log.Info("Copmpleted Get InclementWeatherArrangement");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }


        [HttpGet]
        [Route("ListWelcomeMessages")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetListWelcomeMessages(int index, int size, int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info($"Start Get List Welcome Messages - Index: {index} - Size: {size}");
            var result = await _iCMSContentService.GetListWelcomeMessages((int)GetLanguageCode(), index, size, applyingFor);
            Log.Info($"Copmpleted Get List Welcome Messages - Index: {index} - Size: {size}");

            return Content(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("GetWelcomeMessage")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetWelcomeMessage(int applyingFor = (int)Common.Enums.ContentApplyingFor.Both)
        {
            Log.Info("Start Get Welcome Message");
            var cmsBindingModel = new CMSBindingModel();
            var result = await _iCMSContentService.GetWelcomeMessage((int)GetLanguageCode(), applyingFor);
            if (result != null)
                cmsBindingModel = await CmsBindingModel(result);
            Log.Info("Copmpleted Get Welcome Message");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, cmsBindingModel));
        }

        [HttpPost]
        [Route("GetMatchContents")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetMatchContents(GetMatchContentBindingModel model)
        {
            Log.Info("Start Get List Content");
            var result = await _iCMSContentService.GetUnMatchContents(model.Keyword, model.ContentType, model.Page, model.SortBy, model.IsDescending, model.Size);

            Log.Info("Completed Get List Content");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("MatchContents")]
        [AllowAnonymous]
        public IHttpActionResult MatchContents(MatchContentBindingModel model)
        {
            Log.Info("Start Get List Content");
            var result = _iCMSContentService.MatchContents(model.Id, model.MatchId);

            Log.Info("Completed Get List Content");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("SendNotificationEmails")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SendNotificationEmails(NotificationEmailBindingModel model)
        {
            Log.Info("Start Get Send Notification Emails");

            string url = await _clientService.GetClientUrlByNameAsync("ApplicantPortal");
            string urlEN = String.Empty;
            string urlTC = String.Empty;
            string emailSubjectEN = "";
            string emailSubjectTC = "";
            var cms = await _iCMSContentService.GetCMSContentById(model.cmsId);
            var cmsMatched = await _iCMSContentService.GetCMSContentById(model.cmsMatchedId);
            Notification notificationEN = new Notification();
            Notification notificationTC = new Notification();

            if (cms == null || cmsMatched == null)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Failure", false, "content did not match"));
            }

            // Create or Get QRCode
            string directory = ConfigHelper.GetByKey("ContentUpload");
            string serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
            string path = serPath + "\\" + "QRCode" + "\\" + "Content" + "\\" + cms.Data.Id;
            LinkedResource linkedResourceEN = null;
            LinkedResource linkedResourceTC = null;

            if (!Directory.Exists(path))
            {
                Common.Common.CreateDirectoryAndGrantFullControlPermission(path);
            }

            if (SearchHelper.CmsUrlhelper(cms.Data.ContentTypeId, true).Equals("en"))
            {
                emailSubjectEN = cms.Data.Title;
                urlEN = url + SearchHelper.CmsUrlhelper(cms.Data.ContentTypeId, true) + "/" + SearchHelper.CmsUrlhelper(cms.Data.ContentTypeId, false) + "/" + cms.Data.Id + "/" + cms.Data.SEOUrlLink;
                string fileDirect = path + "\\" + "QRCodeEN.png";
                linkedResourceEN = MailHelper.GenerateEmailImage(GetQRCodeStream(fileDirect, urlEN), "image/png", "QRCode");

                notificationEN.Title = cms.Data.Title;
                notificationEN.Body = "";
                notificationEN.DataId = cms.Data.Id;
                notificationEN.Type = (int)NotificationType.CMS;
            }
            else
            {
                emailSubjectTC = cms.Data.Title;
                urlTC = url + SearchHelper.CmsUrlhelper(cms.Data.ContentTypeId, true) + "/" + SearchHelper.CmsUrlhelper(cms.Data.ContentTypeId, false) + "/" + cms.Data.Id + "/" + cms.Data.SEOUrlLink;
                string fileDirect = path + "\\" + "QRCodeTC.png";
                linkedResourceTC = MailHelper.GenerateEmailImage(GetQRCodeStream(fileDirect, urlTC), "image/png", "QRCode");

                notificationTC.Title = cms.Data.Title;
                notificationTC.Body = "";
                notificationTC.DataId = cms.Data.Id;
                notificationTC.Type = (int)NotificationType.CMS;
            }
            if (SearchHelper.CmsUrlhelper(cmsMatched.Data.ContentTypeId, true).Equals("en"))
            {
                emailSubjectEN = cms.Data.Title;
                urlEN = url + SearchHelper.CmsUrlhelper(cmsMatched.Data.ContentTypeId, true) + "/" + SearchHelper.CmsUrlhelper(cmsMatched.Data.ContentTypeId, false) + "/" + cmsMatched.Data.Id + "/" + cmsMatched.Data.SEOUrlLink;
                string fileDirect = path + "\\" + "QRCodeEN.png";
                linkedResourceEN = MailHelper.GenerateEmailImage(GetQRCodeStream(fileDirect, urlEN), "image/png", "QRCode");

                notificationEN.Title = cms.Data.Title;
                notificationEN.Body = "";
                notificationEN.DataId = cms.Data.Id;
                notificationEN.Type = (int)NotificationType.CMS;
            }
            else
            {
                emailSubjectTC = cms.Data.Title;
                urlTC = url + SearchHelper.CmsUrlhelper(cmsMatched.Data.ContentTypeId, true) + "/" + SearchHelper.CmsUrlhelper(cmsMatched.Data.ContentTypeId, false) + "/" + cmsMatched.Data.Id + "/" + cmsMatched.Data.SEOUrlLink;
                string fileDirect = path + "\\" + "QRCodeTC.png";
                linkedResourceTC = MailHelper.GenerateEmailImage(GetQRCodeStream(fileDirect, urlTC), "image/png", "QRCode");

                notificationTC.Title = cms.Data.Title;
                notificationTC.Body = "";
                notificationTC.DataId = cms.Data.Id;
                notificationTC.Type = (int)NotificationType.CMS;
            }

            List<SendMailModel> list = new List<SendMailModel>();
            foreach (var type in model.UserType)
            {
                if (type == UserNotificationType.NonRegisteredUsers)
                {
                    var users = await _userService.GetUsersForNofitication(type);
                    foreach (var user in users)
                    {
                        foreach (var id in model.CourseTypes)
                        {
                            if (user.InterestedTypeOfCourse.Contains(id))
                            {
                                if (user.CommunicationLanguage == (int)CommunicationLanguageType.English)
                                {
                                    user.CmsUrl = urlEN;
                                    user.EmailContent = Common.Common.GenerateMatchNewEventEmailContent(user, user.CmsUrl, "Item33EN.cshtml");
                                    user.ImageResourse = linkedResourceEN;
                                    user.EmailSubject = emailSubjectEN;
                                }
                                else
                                {
                                    user.CmsUrl = urlTC;
                                    user.EmailContent = Common.Common.GenerateMatchNewEventEmailContent(user, user.CmsUrl, "Item33TC.cshtml");
                                    user.ImageResourse = linkedResourceTC;
                                    user.EmailSubject = emailSubjectTC;
                                }

                                if (!list.Any(x => x.Email.Equals(user.Email)))
                                {
                                    list.Add(user);
                                }
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    var users = await _userService.GetUsersForNofitication(type);
                    foreach (var user in users)
                    {
                        if (user.CommunicationLanguage == (int)CommunicationLanguageType.English)
                        {
                            user.CmsUrl = urlEN;
                            user.EmailContent = Common.Common.GenerateMatchNewEventEmailContent(user, user.CmsUrl, "Item33EN");
                            user.ImageResourse = linkedResourceEN;
                            user.EmailSubject = emailSubjectEN;
                        }
                        else
                        {
                            user.CmsUrl = urlTC;
                            user.EmailContent = Common.Common.GenerateMatchNewEventEmailContent(user, user.CmsUrl, "Item33TC");
                            user.ImageResourse = linkedResourceTC;
                            user.EmailSubject = emailSubjectTC;
                        }

                        if (!list.Any(x => x.Email.Equals(user.Email)))
                        {
                            list.Add(user);
                        }
                    }
                }
            }

            int count = 1;
            foreach (var user in list)
            {
                bool isSuccesss = MailHelper.SendMail(user.Email, user.EmailSubject, user.EmailContent, new List<LinkedResource>() { user.ImageResourse });
                if (isSuccesss)                {

                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);
                }
                count++;
                if (count % 20 == 0)
                {
                    DateTime now = DateTime.Now;
                    while (DateTime.Now.Subtract(now).Minutes < 3)
                    {
                        // wait for 3 minutes for Google limit, Change the number when you change FromEmailAddress
                    }
                }

                if (user.DeviceToken != null && user.DeviceToken.Length > 0)
                {
                    if (user.CommunicationLanguage == (int)CommunicationLanguageType.English)
                    {
                        foreach (string token in user.DeviceToken)
                        {
                            NotificationHelper.PushNotification(notificationEN.Body, notificationEN.Title, token);
                        }
                        notificationEN.NotificationUsers.Add(new NotificationUser()
                        {
                            CreatedDate = DateTime.UtcNow,
                            IsFavourite = false,
                            IsRead = false,
                            IsRemove = false,
                            UserId = user.UserId
                        });
                    }
                    else
                    {
                        foreach (string token in user.DeviceToken)
                        {
                            NotificationHelper.PushNotification(notificationTC.Body, notificationTC.Title, token);
                        }

                        notificationTC.NotificationUsers.Add(new NotificationUser()
                        {
                            CreatedDate = DateTime.UtcNow,
                            IsFavourite = false,
                            IsRead = false,
                            IsRemove = false,
                            UserId = user.UserId
                        });
                    }

                }
            }

            // Storage notification
            _notificationService.CreateNotification(notificationEN);
            _notificationService.CreateNotification(notificationTC);

            Log.Info("Completed Get Send Mail");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, "Success"));
        }

        [HttpPost]
        [Route("TestNotification")]
        [AllowAnonymous]
        public IHttpActionResult MatchContents(TestNoti model)
        {

            NotificationHelper.PushNotification(model.Content, model.Title, model.Token);
            return Ok();
        }

        [HttpGet]
        [Route("AutoUpdateCMSStatus")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateCMSPublishStatus()
        {
            var result = await _iCMSContentService.UpdateCMSPublishStatus();
            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess)) :
                    Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess));

        }

        private MemoryStream GetQRCodeStream(string fileDirect, string url)
        {
            if (!File.Exists(fileDirect))
            {
                var bitmap = FileHelper.GenerateQRCode(url, fileDirect);
            }
            //var imageData = Convert.FromBase64String("/9j/4AAQSkZJRgABAgEASABIAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEB");
            var stream = new MemoryStream(File.ReadAllBytes(fileDirect));
            return stream;
        }


        private async Task<bool> CloneDataToElasticAsync(int cmsId)
        {
            Log.Info($"Start clone data to elastic - CMS Id: {cmsId}");

            var defaultIndex = "searchdata";
            var siteUrl = await _clientService.GetClientUrlByNameAsync("ApplicantPortal");

            var content = await _iCMSContentService.GetRawCMSContentById(cmsId);

            if (content == null)
            {
                Log.Info($"Failue to clone data to elastic - CMS Id: {cmsId}");
                return false;
            }

            List<CmsContent> contents = new List<CmsContent>() { content };
            List<SearchModel> dataList = new List<SearchModel>(contents.ToElasticSearchList(siteUrl));

            ElasticClient client = ElasticSearchClient.GetInstance();
            bool responseChecking = false;

            foreach (var item in dataList)
            {

                if (item.DataType.Equals("document"))
                {
                    var res = await client.IndexAsync(item, i => i
                                                      .Pipeline("attachments")
                                                      .Refresh(Refresh.WaitFor));
                    responseChecking = res.ApiCall.Success;
                }
                else
                {
                    var res = await client.IndexDocumentAsync(item);
                    responseChecking = res.ApiCall.Success;
                }

                if (!responseChecking)
                {
                    break;
                }
            }

            Log.Info($"Completed clone data to elastic - CMS Id: {cmsId}");

            return responseChecking;
        }

    }

    public class TestNoti
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Token { get; set; }
    }
}