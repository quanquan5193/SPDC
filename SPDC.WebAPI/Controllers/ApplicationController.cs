using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SPDC.Common;
using SPDC.Data;
using SPDC.Model.BindingModels;
using SPDC.Model.BindingModels.MyApplication;
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
    [RoutePrefix("api/Application")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ApplicationController : ApiControllerBase
    {
        private IApplicationService _applicationService;

        private ApplicationUserManager _userManager;

        private ILanguageService _languageService;

        private ICourseService _courseService;

        private IClassService _classService;
        private IParticularService _particularService;
        private IWorkExperienceService _workExperienceService;
        private IEmployerRecommendationService _employerRecommendationService;
        private IUploadFileService _uploadFileService;
        private IDocumentService _documentService;


        public ApplicationController(IApplicationService applicationService, ILanguageService languageService, ICourseService courseService, IClassService classService,
            IParticularService particularService, IWorkExperienceService workExperienceService, IEmployerRecommendationService employerRecommendationService,
            IUploadFileService uploadFileService, IDocumentService documentService)
        {
            _applicationService = applicationService;
            _languageService = languageService;
            _courseService = courseService;
            _classService = classService;
            _particularService = particularService;
            _workExperienceService = workExperienceService;
            _employerRecommendationService = employerRecommendationService;
            _uploadFileService = uploadFileService;
            _documentService = documentService;
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
        [Route("create-application")]
        public async Task<IHttpActionResult> CreateApplication(ApplicationCreateBindingModel model)
        {
            Log.Info("Start Create Application");
            int id = 0;
            bool isDelete = true;


            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                id = model.UserId;
            }
            else
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
                if (!success)
                {
                    Log.Error("Failed Create Application");
                                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
            }

            //if (!ModelState.IsValid)
            //{
            //    Log.Error("Failed Create Application");
            //    return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            //}

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error("Failed Create Application");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            // Allow user create multi-application per course
            //bool checkExistApplication = await _applicationService.CheckExistApplication(model.ApplicationId);

            var result = (model.ApplicationId == null || model.ApplicationId == 0) ? await _applicationService.CreateApplication(model, id, lang.Id) : await _applicationService.UpdateApplication(model, id);

            if (result.Data != 0)
            {
                Log.Info("Completed");

                if (model.IsSubmit == true)
                {

                    /***step 1: Personal Particulars & Education Level ***/
                    // 1.1. Get data Update particular from temp
                    var particularModel = await _particularService.GetParticularByUserTempId(id, (int)GetLanguageCode());
                    // 1.2. Update particular
                    var isUpdated = await _particularService.UpdateParticularByIdTempToTable(id);
                    if (isUpdated)
                    {
                        //1.3. Delete table temp
                        //var isDelete = await _particularService.deleteUpdateParticularTemp(particularModel);
                        Log.Info($"Completed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    }
                    else
                    {
                        Log.Error($"Failed Update Personal Particular - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    }

                    /***step 2: Revelant Memberships/Qualifications ***/
                    // 2.1. Get data  Update tQualification from temp
                    var qualification = await _particularService.GetQualificationByUserIdTemp(user.Id, (int)GetLanguageCode());

                    // 2.2. Update tQualification from temp
                    isUpdated = await _particularService.UpdateQualificationsTempToTable(qualification, (int)GetLanguageCode());
                    if (isUpdated)
                    {
                        //2.3. Delete table temp
                        isDelete = await _particularService.DeleterQualificationsTemp(qualification);
                        Log.Error($"Completed Update User Qualification - Id: {qualification.Id}");
                    }
                    else
                    {
                        Log.Error($"Failed Update User Qualification - Id: {qualification.Id}");
                    }

                    /***step 3: Relevant Working Experience ***/
                    //3.1. Get data Update Qualification from temp
                    var workExperience = await _workExperienceService.GetUserWorkExperienceTemp(user.Id, (int)GetLanguageCode());
                    if (workExperience.Count > 0)
                    {
                        //3.2. Update Qualification
                        isUpdated = await _workExperienceService.UpdateUserWorkExperienceTempToTable(workExperience, (int)GetLanguageCode(), id);
                        if (isUpdated)
                        {
                            //3. Delete table temp
                            isDelete = await _workExperienceService.DeleteUserWorkExperienceTemp(workExperience, (int)GetLanguageCode(), id);
                            Log.Info($"Completed Update User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                        }
                        else
                        {
                            Log.Error($"Failed Update User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                        }
                    }
                    else
                    {
                        Log.Error($"Failed Get User Work Experience : {HttpContext.Current.User.Identity.GetUserId()}");
                    }

                    /***step 4: Recommendation by Employer With Proof ***/
                    //4.1. Get data Recommendation by Employer With Proof  from temp
                    var emRecommendation = await _employerRecommendationService.GetEmployerRecommendationTemp(user.Id, (int)GetLanguageCode());

                    if (emRecommendation.Count > 0)
                    {
                        isUpdated = await _employerRecommendationService.UpdateEmployerRecommendationTempToTable(emRecommendation, (int)GetLanguageCode(), id);
                        if (isUpdated)
                        {
                            //3. Delete table temp
                            isDelete = await _employerRecommendationService.DeleteEmployerRecommendationTemp(emRecommendation, (int)GetLanguageCode(), id);

                            Log.Info($"Completed Update Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                        }
                        else
                        {
                            Log.Info($"Failed Update Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                        }
                    }

                    /***step 5: Supporting Documents ***/
                    //5.1. Get dataSupporting Documents from temp
                    var FileViewModel = await _uploadFileService.GetAllFileTemp(id);
                    if (FileViewModel?.Any() == true)
                    {
                        //5.1.  update Supporting Documents
                        List<UserDocumentTempViewModel> fileReturnViewModels = new List<UserDocumentTempViewModel>();

                        foreach (var item in FileViewModel)
                        {
                            fileReturnViewModels.Add(new UserDocumentTempViewModel()
                            {
                                Id = item.Id,
                                UserId = item.UserId,
                                Document = item.Document,
                                User = item.User
                            });
                        }
                        //5.2.  update Supporting Documents
                        isUpdated = await _documentService.UpdateUserDocumentTempToTable(fileReturnViewModels, id, user.CICNumber);
                        if (isUpdated)
                        {
                            //5.3.  Delete Supporting Documents Temp
                            isDelete = await _documentService.DeleteUserDocumentTemp(fileReturnViewModels, id, user.CICNumber);
                            Log.Info($"Completed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                        }
                        else
                        {
                            Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                        }
                    }
                    //1.3. Delete table temp
                    isDelete = await _particularService.deleteUpdateParticularTemp(particularModel);
                }
                else
                {

                }

            }
            else
            {
                Log.Error("Failed");
            }

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, true, new { id = result.Data })) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [Route("get-application")]
        public async Task<IHttpActionResult> GetApplication(int appID)
        {
            Log.Info($"Start Get Application - Id: {appID}");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _applicationService.GetApplication(appID);

            CourseApplicationStep6ViewModel courseApplicationStep6ViewModel = new CourseApplicationStep6ViewModel();

            var courseData = await _courseService.GetCourseApplicationForm(result.CourseId);

            if (courseData != null)
            {
                courseApplicationStep6ViewModel.CourseID = courseData.Id;
                courseApplicationStep6ViewModel.CourseCode = courseData.CourseCode;
                courseApplicationStep6ViewModel.CourseName = courseData.CourseTrans.Count > 0 ? courseData.CourseTrans.Where(x => x.LanguageId == lang.Id).FirstOrDefault().CourseName : "";

                if (lang.Id == 1)
                {
                    courseApplicationStep6ViewModel.ModeOfStudy = courseData.CourseType.NameEN;
                }
                if (lang.Id == 2)
                {
                    courseApplicationStep6ViewModel.ModeOfStudy = courseData.CourseType.NameCN;
                }
                if (lang.Id == 3)
                {
                    courseApplicationStep6ViewModel.ModeOfStudy = courseData.CourseType.NameHK;
                }
            }

            if (result.StudentPreferredClass.HasValue)
            {
                var classesData = await _applicationService.GetClassByID(result.StudentPreferredClass.Value);

                ClassApplicationViewModel classApplicationViewModel = new ClassApplicationViewModel();

                if (classesData != null)
                {
                    string classAvailabel = string.Format("{0} ({1} - {2})", classesData.ClassCode, classesData.CommencementDate.ToString("dd/MM/yyyy"), classesData.CompletionDate.ToString("dd/MM/yyyy"));
                    classApplicationViewModel.ClassId = classesData.Id;
                    classApplicationViewModel.ClassAvailable = classAvailabel;

                    courseApplicationStep6ViewModel.Classes = classApplicationViewModel;
                }
            }
            if (!string.IsNullOrWhiteSpace(result.ModuleIds))
            {
                var moduleData = await _applicationService.GetModuleByID(result.ModuleIds);

                List<ModuleApplicationViewModel> moduleApplicationViewModels = new List<ModuleApplicationViewModel>();

                if (moduleData.Count() > 0)
                {
                    foreach (var item in moduleData)
                    {
                        moduleApplicationViewModels.Add(new ModuleApplicationViewModel { ModuleId = item.Id, ModuleNo = item.ModuleNo });
                    }
                }
                courseApplicationStep6ViewModel.Modules = moduleApplicationViewModels;
            }

            courseApplicationStep6ViewModel.IHaveApplyFor = result.IHaveApplyFor;
            courseApplicationStep6ViewModel.IHaveApplyForText = result.IHaveApplyForText;
            courseApplicationStep6ViewModel.IsRequiredRecipt = result.IsRequiredRecipt;

            Log.Info($"Completed Get Application - Id: {appID}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new { id = courseApplicationStep6ViewModel }));
        }

        //[HttpGet]
        //[Route("get-enrollment")]
        //[AllowAnonymous]
        //public async Task<IHttpActionResult> GetMyEnrollment()
        //{
        //    var result = await _applicationService.GetCourseEnrollment(12, 1);

        //    return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        //}

        [HttpPost]
        [Route("MyCourseApplication")]
        public async Task<IHttpActionResult> GetMyCourseApplication(MyCourseApplicationFilter filter)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var result = await _applicationService.GetMyCourseApplication(filter, id, (int)GetLanguageCode());

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("GetInvoiceApplication")]
        public async Task<IHttpActionResult> ListApplicationInvoiceAndRecipt(int appId)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var result = await _applicationService.ListApplicationInvoiceAndRecipt(appId, id);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("Withdrawal")]
        public async Task<IHttpActionResult> WithdrawalApplication(int appId)
        {
            var result = await _applicationService.WithdrawalApplication(appId);

            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Action can not performed"));
        }

        [HttpGet]
        [Route("DownloadInvoice")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> DownloadCourseInvoice(int invoiceId)
        {
            var result = await _applicationService.DownloadCourseInoivce(invoiceId);

            if (!result.IsSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            return httpResponseMessage;
        }
    }
}
