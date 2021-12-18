using AutoMapper;
using Elasticsearch.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Nest;
using Newtonsoft.Json;
using SPDC.Common;
using SPDC.Data;
using SPDC.Model.BindingModels;
using SPDC.Model.BindingModels.Assessment;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.Application;
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
using static SPDC.Common.StaticConfig;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Courses")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CoursesController : ApiControllerBase
    {
        private ICourseService _courseService;
        private ILanguageService _languageService;
        private ILevelofApprovalService _levelofApprovalService;
        private ICourseMasterDataService _courseMasterDataService;
        private IApplicationService _applicationService;
        private IClientService _clientService;
        private IDocumentService _documentService;
        private IClassService _classService;

        public CoursesController(ICourseService courseService, ILanguageService languageService, ILevelofApprovalService levelofApprovalService,
            ICourseMasterDataService courseMasterDataService, IApplicationService applicationService, IClientService clientService, IDocumentService documentService,
            IClassService classService)
        {
            _courseService = courseService;
            _languageService = languageService;
            _levelofApprovalService = levelofApprovalService;
            _courseMasterDataService = courseMasterDataService;
            _applicationService = applicationService;
            _clientService = clientService;
            _documentService = documentService;
            _classService = classService;
        }

        [HttpPost]
        [Route("Categories")]
        [AllowAnonymous]
        public IHttpActionResult CreateCategory(CourseCategoryBindingModel model)
        {
            Log.Info("Start Create Course Categories");
            CourseCategory category = new CourseCategory()
            {
                ParentId = model.ParentId,
                Status = model.Status,
                CourseCategorieTrans = new List<CourseCategorieTran>()
            };

            foreach (var value in model.values)
            {

                category.CourseCategorieTrans.Add(new CourseCategorieTran()
                {
                    LanguageId = value.LanguageId,
                    Name = value.Name,
                    Title = value.Title
                });
            }

            var result = _courseService.AddCategory(category);
            Log.Info("Completed Create Course Categories");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, Mapper.Map<CourseCategoryViewModel>(result)));
        }

        [HttpGet]
        [Route("Categories")]
        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetCategoriesAsync()
        {
            Log.Info("Start Get Course Categories");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _courseService.GetCategoriesAsync(lang.Id);

            Log.Info("Completed Get Course Categories");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, Mapper.Map<IEnumerable<CourseCategoryViewModel>>(result)));
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> CreateCourseAsync(CreateCourseBindingModel model)
        {
            Log.Info("Start Create Course");

            Course course;
            if (model.Id == 0)
            {
                course = await _courseService.CreateAsync(model);
            }
            else
            {
                course = await _courseService.UpdateAsync(model);
            }
            if (course == null)
            {
                Log.Error("Course code is existed");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel()
                {
                    Message = FileHelper.GetServerMessage("CMSCourseSetup", "course_code_exist_error_msg", GetLanguageCode().ToString()),
                    Data = null
                });
            }

            Log.Info("Completed Create Course");

            bool cloneElasticResult = await CloneDataToElasticAsync(course.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new { id = course.Id }));
        }

        [HttpPost]
        [Route("Fee")]
        public async Task<IHttpActionResult> UpdateFeeAsync(CourseFeeBindingModel model)
        {
            Log.Info("Start Update Course Fee");
            //var model = JsonConvert.DeserializeObject<CourseFeeBindingModel>(System.Web.HttpContext.Current.Request.Params["fee"]);
            var result = await _courseService.UpdateFeeAsync(model);

            if (!result.IsSuccess)
            {
                Log.Error("Failed Update Course Fee");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }
            Log.Info("Completed Update Course Fee");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpPost]
        [Route("Allowance")]
        public async Task<IHttpActionResult> UpdateAllowanceAsync(CourseAllowanceBindingModel model)
        {
            Log.Info("Start Update Allowance");
            var result = _courseService.UpdateAllowance(model);

            if (!result.IsSuccess)
            {
                Log.Error("Failed Update Allowance");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }
            Log.Info("Completed Update Allowance");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpPost]
        [Route("ProgrammeLeadership")]
        public async Task<IHttpActionResult> UpdateProgrammeLeadership(CourseProgrammeLeadershipBindingModel model)
        {
            Log.Info("Start Update ProgrammeLeadership");
            var result = await _courseService.UpdateProgrammeLeadershipAsync(model);

            if (!result.IsSuccess)
            {
                Log.Error("Failed Update ProgrammeLeadership");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }
            Log.Info("Completed Update ProgrammeLeadership");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpPost]
        [Route("CourseInformation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCourseInformations(CourseSearchBindingModel model)
        {
            Log.Info("Start Get Course Information");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            if (String.IsNullOrEmpty(model.sortBy))
            {
                model.sortBy = "Id";
            }

            var result = await _courseService.GetCourseInformation(lang.Id, model.coursecategories, model.coursemode, model.courselocations, model.search, model.index, model.sortBy, model.isDescending, model.size);

            var count = await _courseService.CountTotalSearch(lang.Id, model.coursecategories, model.coursemode, model.courselocations, model.search);

            List<CourseInformationViewModel> courseInformationModel = new List<CourseInformationViewModel>();

            foreach (var item in result)
            {
                var rtnItem = new CourseInformationViewModel()
                {
                    CourseID = item.Id,
                    CourseCode = item.CourseCode,
                    CourseFee = item.CourseFee,
                    Duration = item.DurationTotal,
                    CourseName = item.CourseTrans.Count == 0 ? "" : item.CourseTrans.Where(x => x.LanguageId == lang.Id).FirstOrDefault().CourseName,
                    CourseCategories = item.CourseCategory == null ? "" : (item.CourseCategory.CourseCategorieTrans.Count == 0 ? "" : item.CourseCategory.CourseCategorieTrans.Where(x => x.LanguageId == lang.Id).FirstOrDefault().Name),
                };

                if (lang.Id == 1)
                {
                    rtnItem.ModeOfStudy = item.CourseType.NameEN;
                }
                if (lang.Id == 2)
                {
                    rtnItem.ModeOfStudy = item.CourseType.NameCN;
                }
                if (lang.Id == 3)
                {
                    rtnItem.ModeOfStudy = item.CourseType.NameHK;
                }

                courseInformationModel.Add(rtnItem);
            }

            if (model.sortBy == "CourseCategories")
            {
                if (model.isDescending == false)
                {
                    courseInformationModel = courseInformationModel.OrderBy(x => x.CourseCategories).ThenBy(u => u.CourseCategories == null).ToList();
                }
                else
                {
                    courseInformationModel = courseInformationModel.OrderByDescending(x => x.CourseCategories == null).ThenBy(u => u.CourseCategories).ToList();
                }
            }
            if (model.sortBy == "ModeOfStudy")
            {
                if (model.isDescending == false)
                {
                    courseInformationModel = courseInformationModel.OrderBy(x => x.ModeOfStudy == null).ThenBy(u => u.ModeOfStudy).ToList();
                }
                else
                {
                    courseInformationModel = courseInformationModel.OrderByDescending(x => x.ModeOfStudy == null).ThenBy(u => u.ModeOfStudy).ToList();
                }
            }
            if (model.sortBy == "CourseName")
            {
                if (model.isDescending == false)
                {
                    courseInformationModel = courseInformationModel.OrderBy(x => x.CourseName).ThenBy(u => u.CourseName == null).ToList();
                }
                else
                {
                    courseInformationModel = courseInformationModel.OrderByDescending(x => x.CourseName).ThenBy(u => u.CourseName == null).ToList();
                }
            }


            Log.Info("Completed Get Course Information");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new PaginationSet<CourseInformationViewModel>()
            {
                Items = courseInformationModel,
                Page = model.index,
                TotalCount = count
            }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CourseMode")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetCourseMode()
        {
            Log.Info("Start Get Course Mode");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());
            List<CourseModeViewModel> rtnList = new List<CourseModeViewModel>();
            var result = await _courseService.GetCourseMode();

            foreach (var item in result)
            {
                CourseModeViewModel rtnItem = new CourseModeViewModel();
                rtnItem.CourseModeID = item.Id;
                if (lang.Id == 1)
                {
                    rtnItem.CourseModeName = item.NameEN;
                }
                if (lang.Id == 2)
                {
                    rtnItem.CourseModeName = item.NameCN;
                }
                if (lang.Id == 3)
                {
                    rtnItem.CourseModeName = item.NameHK;
                }
                rtnList.Add(rtnItem);
            }

            Log.Info("Completed Get Course Mode");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, rtnList));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CourseLocation")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetCourseLocation()
        {
            Log.Info("Start Get Course Location");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _courseService.GetCourseLocation(lang.Id);

            List<CourseLocationViewModel> returnModel = new List<CourseLocationViewModel>();
            foreach (var item in result)
            {
                returnModel.Add(new CourseLocationViewModel()
                {
                    CourseLocationID = item.Id,
                    CourseLocationName = item.CourseLocationTrans.Count != 0 ? item.CourseLocationTrans.FirstOrDefault().Name : ""
                });
            }
            Log.Info("Completed Get Course Location");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, returnModel));
        }

        [HttpPost]
        [Route("Curriculum")]
        public async Task<IHttpActionResult> UpdateCurriculumAsync(CurriculumBindingModel model)
        {
            Log.Info($"Start Update Curriculum - Id: {model.Id}");

            var result = await _courseService.UpdateCurriculumAsync(model);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Update Curriculum - Id: {model.Id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            Log.Info($"Completed Update Curriculum - Id: {model.Id}");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpPost]
        [Route("Recognition")]
        public async Task<IHttpActionResult> UpdateRecognitionAsync(RecognitionBindingModel model)
        {

            Log.Info($"Start Update Recognition - Id: {model.Id}");
            var result = await _courseService.UpdateRecognitionAsync(model);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Update Recognition - Id: {model.Id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            Log.Info($"Completed Update Recognition - Id: {model.Id}");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpPost]
        [Route("AdmissionRequirements")]
        public async Task<IHttpActionResult> UpdateAdmissionRequirementsAsync(AdmissionRequirementsBindingModel model)
        {

            Log.Info($"Start Update Admission Requirements  - Id: {model.Id}");
            var result = await _courseService.UpdateAdmissionRequirementsAsync(model);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Update Admission Requirements - Id: {model.Id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            Log.Info($"Completed Update Admission Requirements - Id: {model.Id}");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpPost]
        [Route("CertificateConditions")]
        public async Task<IHttpActionResult> UpdateCertificateConditionsAsync(CertificateConditionsBindingModel model)
        {

            Log.Info($"Start Update Admission Certificate Conditions - Id: {model.Id}");
            var result = await _courseService.UpdateCertificateConditionsAsync(model);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Update Admission Certificate Conditions - Id: {model.Id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }
            Log.Info($"Completed Update Admission Certificate Conditions - Id: {model.Id}");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpPost]
        [Route("Enquiry")]
        public async Task<IHttpActionResult> UpdateEnquiryAsync(EnquirysBindingModel model)
        {

            Log.Info($"Start Update Enquiry - Id: {model.Id}");
            var result = await _courseService.UpdateEnquirysAsync(model);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Update Enquiry - Id: {model.Id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            Log.Info($"Completed Update Enquiry - Id: {model.Id}");

            bool cloneElasticResult = await CloneDataToElasticAsync(result.Data.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data.Id));
        }

        [HttpGet]
        [Route("Get-ReExamination")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetReExamination(int courseId)
        {
            Log.Info($"Start Get ReExamination - Course Id: {courseId}");
            var result = await _courseService.GetReExamination(courseId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Get ReExamination - Course Id: {courseId}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }
            Log.Info($"Completed Get ReExamination - Course Id: {courseId}");

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data));
        }

        [HttpPost]
        [Route("Re-Examination")]
        public async Task<IHttpActionResult> UpdateReExamination(CourseReExaminationBindingModel model)
        {

            Log.Info($"Start Update ReExamination - Course Id: {model.Id}");
            var result = await _courseService.UpdateReExamination(model);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Update ReExamination - Course Id: {model.Id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            Log.Info($"Completed Update ReExamination - Course Id: {model.Id}");

            bool cloneElasticResult = await CloneDataToElasticAsync(model.Id);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data));
        }

        [HttpGet]
        [Route("Get-CourseKeyword")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCourseKeyword(int courseId)
        {
            Log.Info($"Start Get Course Keyword - Course Id: {courseId}");
            var result = await _courseService.GetCourseKeyword(courseId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Get Course Keyword - Course Id: {courseId}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }
            Log.Info($"Start Get Course Keyword - Course Id: {courseId}");

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data));
        }

        [HttpPost]
        [Route("Course-Keyword/{courseId}")]
        public async Task<IHttpActionResult> UpdateCourseKeyword(List<KeywordBindingModel> model, int courseId)
        {
            Log.Info($"Start Get Course Keyword");
            var result = await _courseService.UpdateCourseKeyword(model, courseId);

            if (!result.IsSuccess)
            {
                Log.Info($"Failed Get Course Keyword");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(result.Message, result.IsSuccess, null));
            }
            Log.Info($"Completed Get Course Keyword");

            bool cloneElasticResult = await CloneDataToElasticAsync(courseId);

            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CourseInformationById/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetCourseInformationById(int id)
        {
            Log.Info($"Start Get Course Information - Id: {id}");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _courseService.GetCourseInformationById(id);

            CourseDetailInfoViewModel courseDetailInfo = new CourseDetailInfoViewModel();

            if (result != null)
            {
                courseDetailInfo.CourseID = result.Id;
                courseDetailInfo.CourseCode = result.CourseCode;
                courseDetailInfo.CommencementDate = result.CommencementDate;
                courseDetailInfo.CompletionDate = result.CompletionDate;
                courseDetailInfo.Duration = result.DurationTotal;
                courseDetailInfo.Credits = result.Credits;
                courseDetailInfo.StudyLocation = result.CourseVenueId != null ? await _courseService.GetLocationByID(lang.Id, result.CourseVenueId ?? 1) : null;
                courseDetailInfo.ObjectiveEN = result.ObjectiveEN;
                courseDetailInfo.ObjectiveTC = result.ObjectiveTC;
                courseDetailInfo.ObjectiveSC = result.ObjectiveSC;
                courseDetailInfo.WaitingTimeEN = result.WaitingTimeEN;
                courseDetailInfo.WaitingTimeSC = result.WaitingTimeSC;
                courseDetailInfo.WaitingTimeTC = result.WaitingTimeTC;
                courseDetailInfo.ByModule = result.ByModule;
                courseDetailInfo.DurationHrs = result.DurationHrs;
                courseDetailInfo.DurationLesson = result.DurationLesson;
                courseDetailInfo.DurationTotal = result.DurationTotal;

                CourseFeeViewModel courseFeeViewModel = new CourseFeeViewModel();

                if (result.Modules.Count > 0)
                {
                    List<CourseFeeModuleViewModel> courseFeeModuleViewModels = new List<CourseFeeModuleViewModel>();

                    foreach (var item in result.Modules)
                    {
                        courseFeeModuleViewModels.Add(new CourseFeeModuleViewModel() { ModuleNo = item.ModuleNo, CourseFee = item.Fee });
                    }

                    courseFeeViewModel.CourseFeeModule = courseFeeModuleViewModels;

                }
                else if (result.Modules.Count == 0 && result.DiscountFee != null)
                {
                    courseFeeViewModel.DiscountFee = result.DiscountFee.Value;
                    courseFeeViewModel.CourseFee = result.CourseFee;
                }

                courseDetailInfo.CourseFee = courseFeeViewModel;

                if (lang.Id == (int)LanguageCode.EN)
                {
                    courseDetailInfo.ProgrammeLeader = result.ProgrammeLeader == null ? "" : result.ProgrammeLeader.NameEN;
                    courseDetailInfo.Lecture = result.Lecturer == null ? "" : result.Lecturer.NameEN;
                    courseDetailInfo.MediumOfInstruction = result.MediumOfInstruction == null ? "" : result.MediumOfInstruction.NameEN;
                    courseDetailInfo.ModeOfStudy = result.CourseType.NameEN;
                }
                if (lang.Id == (int)LanguageCode.CN)
                {
                    courseDetailInfo.ProgrammeLeader = result.ProgrammeLeader == null ? "" : result.ProgrammeLeader.NameCN;
                    courseDetailInfo.Lecture = result.Lecturer == null ? "" : result.Lecturer.NameCN;
                    courseDetailInfo.MediumOfInstruction = result.MediumOfInstruction == null ? "" : result.MediumOfInstruction.NameCN;
                    courseDetailInfo.ModeOfStudy = result.CourseType.NameCN;
                }
                if (lang.Id == (int)LanguageCode.HK)
                {
                    courseDetailInfo.ProgrammeLeader = result.ProgrammeLeader == null ? "" : result.ProgrammeLeader.NameHK;
                    courseDetailInfo.Lecture = result.Lecturer == null ? "" : result.Lecturer.NameHK;
                    courseDetailInfo.MediumOfInstruction = result.MediumOfInstruction == null ? "" : result.MediumOfInstruction.NameHK;
                    courseDetailInfo.ModeOfStudy = result.CourseType.NameHK;
                }

                if (result.CourseDocuments.Count > 0)
                {
                    var siteUrl = ConfigHelper.GetByKey("DMZAPI");
                    if (result.CourseDocuments.FirstOrDefault(x => x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.ApplicationForm) != null)
                        courseDetailInfo.ApplicationFormUrl = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + result.CourseDocuments.FirstOrDefault(x => x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.ApplicationForm).DocumentId;

                    if (result.CourseDocuments.FirstOrDefault(x => x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.CourseBrochure) != null)
                        courseDetailInfo.CourseBrochureUrl = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + result.CourseDocuments.FirstOrDefault(x => x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.CourseBrochure).DocumentId;

                    var courseLogos = result.CourseDocuments.Where(x => x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.CourseImage);
                    if (courseLogos.Count() > 0)
                    {
                        foreach (var item in courseLogos)
                        {
                            courseDetailInfo.CourseLogoImages.Add(ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + item.DocumentId);
                        }
                    }
                }


                //List<string> curciculum = new List<string>();

                //List<string> recognition = new List<string>();

                //List<string> admissionRequirements = new List<string>();

                //List<string> conditionsofCertificateAward = new List<string>();


                //var data = result.CourseTrans.Where(x => x.LanguageId == lang.Id).ToList();
                //if (data.Count > 0)
                //{
                //    foreach (var item in result.CourseTrans)
                //    {
                //        curciculum.Add(item.Curriculum);
                //        recognition.Add(item.Recognition);
                //        admissionRequirements.Add(item.AdmissionRequirements);
                //        conditionsofCertificateAward.Add(item.ConditionsOfCertificate);
                //    }
                //}
                var transData = result.CourseTrans.FirstOrDefault(x => x.LanguageId == lang.Id);
                courseDetailInfo.Curriculum = transData == null ? "" : transData.Curriculum;
                courseDetailInfo.Recognition = transData == null ? "" : transData.Recognition;
                courseDetailInfo.AdmissionRequirements = transData == null ? "" : transData.AdmissionRequirements;
                courseDetailInfo.ConditionsofCertificateAward = transData == null ? "" : transData.ConditionsOfCertificate;
                courseDetailInfo.CourseName = transData == null ? "" : transData.CourseName;

                List<EnquiryViewModel> enquiryViewModel = new List<EnquiryViewModel>();

                foreach (var item in result.Enquiries)
                {
                    enquiryViewModel.Add(new EnquiryViewModel()
                    {
                        ContactPerson = (lang.Id == 1 ? item.ContactPersonEN : (lang.Id == 2 ? item.ContactPersonCN : item.ContactPersonHK)),
                        Email = item.Email,
                        Phone = item.Phone,
                        Fax = item.Fax
                    });
                }

                courseDetailInfo.Enquiry = enquiryViewModel;

            }
            else
            {
                courseDetailInfo = null;
            }

            Log.Info($"Completed Get Course Information - Id: {id}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, courseDetailInfo));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CourseInformationByAdminPortal")]
        public async Task<IHttpActionResult> GetCourseInformationByAdminPortal(CourseSearchAdminPortalBindingModel model)
        {
            Log.Info($"Start Get Course Information (Admin)");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());
            int count = 0;
            var result = _courseService.GetCourseInformationByAdminPortal(lang.Id, model.coursecategories, model.coursecode, model.coursenameEN, model.coursenameCN, model.displaycourse, model.index, model.sortBy, model.isDescending, model.size, out count);

            Log.Info($"Completed Get Course Information (Admin)");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new PaginationSet<CoursePortalAdminViewModel>()
            {
                Items = result,
                Page = model.index,
                TotalCount = count
            }));

        }

        //[HttpPost]
        //[Route("CreateAdmin")]
        //public IHttpActionResult CreateClassByAdminPortal(CreateClassAdminBindingModel model)
        //{
        //    Log.Info($"Start Create Class (Admin) - Course Id: {model.Id}");
        //    var result = _courseService.CreateClassAdmin(model);
        //    Log.Info($"Completed Create Class (Admin) - Course Id: {model.Id}");
        //    return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, new { id = result.Id }));
        //}

        [Route("get-document")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCourseDocumentation(int courseId)
        {
            Log.Info($"Start Get Course Document (Admin) - Course Id: {courseId}");
            if (courseId == 0)
            {
                Log.Error($"Failed Get Course Document (Admin) - Course Id: {courseId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Course can not be 0"));
            }

            var result = await _courseService.GetCourseDocumentation(courseId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Get Course Document (Admin) - Course Id: {courseId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            Log.Info($"Completed Get Course Document (Admin) - Course Id: {courseId}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result.Data));
        }

        [HttpPost]
        [Route("update-document")]
        public async Task<IHttpActionResult> UpdateCourseDocument(int courseId)
        {
            Log.Info($"Start Update Course Document (Admin) - Course Id: {courseId}");
            if (courseId == 0)
            {
                Log.Error($"Failed Update Course Document (Admin) - Course Id: {courseId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Course can not be 0"));
            }

            var course = _courseService.GetCourseById(courseId);
            if (course == null)
            {
                Log.Error($"Failed Update Course Document (Admin) - Course Id: {courseId}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel("Course is not found"));
            }

            var lstFileToDelete = JsonConvert.DeserializeObject<List<int>>(HttpContext.Current.Request.Form.Get("ListFileToDelete"));
            var lstFileUpdates = HttpContext.Current.Request.Files;

            var result = await _courseService.UpdateCourseDocumentation(courseId, lstFileToDelete, lstFileUpdates, course.CourseCode);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Update Course Document (Admin) - Course Id: {courseId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            Log.Info($"Completed Update Course Document (Admin) - Course Id: {courseId}");

            var dataDocument = await _documentService.GetCourseDocuments();
            bool cloneElasticResult = await CloneDataToElasticAsync(courseId, dataDocument);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result.Data));
        }

        [HttpGet]
        [Route("download-document")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> DownloadCourseDocument(int docId)
        {
            Log.Info($"Start Download Course Document (Admin) - Id: {docId}");
            if (docId == 0)
            {
                Log.Error($"Failed Download Course Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel("Document Id can not be 0"));
            }

            var result = await _courseService.DownloadCourseDocument(docId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download Course Document (Admin) - Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(result.Message, result.IsSuccess, null));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download Course Document (Admin) - Id: {docId}");
            return httpResponseMessage;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CourseApplication")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetCourseApplicationForm(int courseId, int applicationId = 0)
        {

            Log.Info($"Start Get Course Application - Coourse Id: {courseId}");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _courseService.GetCourseApplicationForm(courseId);

            CourseApplicationViewModel courseApplicationViewModel = new CourseApplicationViewModel();

            if (result != null)
            {
                courseApplicationViewModel.CourseID = result.Id;
                courseApplicationViewModel.CourseCode = result.CourseCode;
                courseApplicationViewModel.CourseName = result.CourseTrans.Count > 0 ? result.CourseTrans.Where(x => x.LanguageId == lang.Id).FirstOrDefault().CourseName : "";

                if (lang.Id == 1)
                {
                    courseApplicationViewModel.ModeOfStudy = result.CourseType.NameEN;
                }
                if (lang.Id == 2)
                {
                    courseApplicationViewModel.ModeOfStudy = result.CourseType.NameCN;
                }
                if (lang.Id == 3)
                {
                    courseApplicationViewModel.ModeOfStudy = result.CourseType.NameHK;
                }

                List<ClassApplicationViewModel> classApplicationViewModels = new List<ClassApplicationViewModel>();
                foreach (var classes in result.Classes)
                {
                    if (classes.InvisibleOnWebsite == true)
                    {
                        string classAvailabel = string.Format("{0} ({1} - {2})", classes.ClassCode, classes.CommencementDate.ToString("dd/MM/yyyy"), classes.CompletionDate.ToString("dd/MM/yyyy"));
                        classApplicationViewModels.Add(new ClassApplicationViewModel { ClassId = classes.Id, ClassAvailable = classAvailabel });
                    }
                }

                courseApplicationViewModel.Classes = classApplicationViewModels;

                List<ModuleApplicationViewModel> moduleApplicationViewModels = new List<ModuleApplicationViewModel>();

                foreach (var module in result.Modules)
                {
                    moduleApplicationViewModels.Add(new ModuleApplicationViewModel { ModuleId = module.Id, ModuleNo = module.ModuleNo });
                }

                courseApplicationViewModel.Modules = moduleApplicationViewModels;
            }

            //int id = 0;
            //var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);

            if (applicationId != 0)
            {
                var application = await _applicationService.GetApplicationByCourseAndUser(applicationId);
                if (application != null)
                {
                    courseApplicationViewModel.ApplicationInfo = application;
                }
            }

            Log.Info($"Completed Get Course Application - Coourse Id: {courseId}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, courseApplicationViewModel));

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("IsReExam")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> ShowReExam()
        {
            Log.Info($"Start Get IsReExam");
            var result = _courseService.ShowReExam();
            Log.Info($"Completed Get IsReExam");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Lecturer")]
        public async Task<IHttpActionResult> GetLecturer()
        {
            Log.Info($"Start Get Lecturer");
            var result = await _courseMasterDataService.GetLecturersAsync();
            Log.Info($"Completed Get Lecturer");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("LevelofApproval")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetLevelofApproval()
        {
            Log.Info($"Start Get Level of Approval");
            var result = await _courseMasterDataService.GetLevelofApprovalsAsync();
            Log.Info($"Completed Get Level of Approval");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("MediumOfInstruction")]
        public async Task<IHttpActionResult> GetMediumOfInstruction()
        {
            Log.Info($"Start Get Medium Of Instruction");
            var result = await _courseMasterDataService.GetMediumOfInstructionsAsync();
            Log.Info($"Completed Get Medium Of Instruction");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ProgrammeLeader")]
        public async Task<IHttpActionResult> GetProgrammeLeader()
        {
            Log.Info($"Start Get Programme Leader");
            var result = await _courseMasterDataService.GetProgrammeLeadersAsync();
            Log.Info($"Completed Get Programme Leader");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CourseById")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetRawCourseById(int id)
        {
            Log.Info($"Start Get Course Information - Id: {id}");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());

            var result = await _courseService.GetCourseInformationById(id);

            var mappingModel = Mapper.Map<RawCourseViewModel>(result);
            mappingModel.LogoImages = await GetLogoUrl(mappingModel);
            foreach (var doc in mappingModel.CourseDocuments)
            {
                doc.Document.Url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + doc.Id;
            }

            Log.Info($"Completed Get Course Information - Id: {id}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, mappingModel));
        }

        private async Task<IEnumerable<RawDocument>> GetLogoUrl(RawCourseViewModel mappingModel)
        {
            var siteUrl = await _clientService.GetClientUrlByNameAsync("ApiPortal");
            return mappingModel.CourseDocuments.Where(x => x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.CourseImage)
                .Select(c => new RawDocument()
                {
                    Id = c.DocumentId,
                    Url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + c.DocumentId,
                    ContentType = c.Document.ContentType,
                    FileName = c.Document.FileName,
                    ModifiedDate = c.Document.ModifiedDate
                });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("CourseList")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> CourseList()
        {
            Log.Info($"Start Get Course List");
            var lang = (int)GetLanguageCode();

            var result = await _courseService.GetCourses();

            Log.Info($"Completed Get Course List");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result.Select(x => new { Id = x.Id, CourseCode = x.CourseCode, CourseName = x.CourseTrans.FirstOrDefault(y => y.LanguageId == lang)?.CourseName })));
        }

        [HttpGet]
        [Route("GetCourseCalender")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCourseCalender(int id)
        {
            var result = await _courseService.GetCourseCalendar(id);

            var resultMapped = Mapper.Map<List<CourseCalendarViewModel>>(result);

            return Ok(new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, resultMapped));
        }

        [HttpPost]
        [Route("CreateCourseSetup")]
        public async Task<IHttpActionResult> CreateCourseSetup()
        {
            #region Deserialize Model
            var createCourseModel = JsonConvert.DeserializeObject<CreateCourseBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CreateCourseModel"]));
            var admissionRequirementsModel = JsonConvert.DeserializeObject<AdmissionRequirementsBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["AdmissionRequirementsModel"]));
            var certificateConditionsModel = JsonConvert.DeserializeObject<CertificateConditionsBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CertificateConditionsModel"]));
            var curriculumModel = JsonConvert.DeserializeObject<CurriculumBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CurriculumModel"]));
            var enquirysgModel = JsonConvert.DeserializeObject<EnquirysBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["EnquirysgModel"]));
            var courseProgrammeLeadershipModel = JsonConvert.DeserializeObject<CourseProgrammeLeadershipBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CourseProgrammeLeadershipModel"]));
            var courseReExaminationModel = JsonConvert.DeserializeObject<CourseReExaminationBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CourseReExaminationModel"]));
            var recognitionModel = JsonConvert.DeserializeObject<RecognitionBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["RecognitionModel"]));
            var courseFeeModel = JsonConvert.DeserializeObject<CourseFeeBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CourseFeeModel"]));
            var courseAllowanceModel = JsonConvert.DeserializeObject<CourseAllowanceBindingModel>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["CourseAllowanceModel"]));
            var listKeywordModel = JsonConvert.DeserializeObject<List<KeywordBindingModel>>(HttpUtility.UrlDecode(HttpContext.Current.Request.Params["ListKeywordModel"]));

            var lstFileToDelete = JsonConvert.DeserializeObject<List<int>>(HttpContext.Current.Request.Form.Get("ListFileToDelete"));
            var lstFileUpdates = HttpContext.Current.Request.Files;
            #endregion
            
            Course course;
            #region Create Course
            if (createCourseModel.Id == 0)
            {
                course = await _courseService.CreateAsync(createCourseModel);
            }
            else
            {
                course = await _courseService.UpdateAsync(createCourseModel);
            }

            if (course == null)
            {
                Log.Error("Course code is existed");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel()
                {
                    Message = FileHelper.GetServerMessage("course_code_exist_error_msg", "CMSCourseSetup", GetLanguageCode().ToString()),
                    Data = null
                });
            }
            #endregion

            if (course.Id > 0)
            {
                admissionRequirementsModel.Id = course.Id;
                certificateConditionsModel.Id = course.Id;
                curriculumModel.Id = course.Id;
                enquirysgModel.Id = course.Id;
                courseProgrammeLeadershipModel.Id = course.Id;
                courseReExaminationModel.Id = course.Id;
                recognitionModel.Id = course.Id;
                courseFeeModel.Id = course.Id;
                courseAllowanceModel.Id = course.Id;
                if (createCourseModel.Id == 0)
                {
                    foreach (var item in listKeywordModel)
                    {
                        item.CourseId = course.Id;
                    }
                }

            }

            #region Admission Requirement
            var result1 = await _courseService.UpdateAdmissionRequirementsAsync(admissionRequirementsModel);
            #endregion
            #region Certificate Condition
            var result2 = await _courseService.UpdateCertificateConditionsAsync(certificateConditionsModel);
            #endregion
            #region Cirriculum
            var result3 = await _courseService.UpdateCurriculumAsync(curriculumModel);
            #endregion
            #region Enquiry
            var result4 = await _courseService.UpdateEnquirysAsync(enquirysgModel);
            #endregion
            #region Programme Leadership
            var result5 = await _courseService.UpdateProgrammeLeadershipAsync(courseProgrammeLeadershipModel);
            #endregion
            #region Re-Examination
            var result6 = await _courseService.UpdateReExamination(courseReExaminationModel);
            #endregion
            #region Recognition
            var result7 = await _courseService.UpdateRecognitionAsync(recognitionModel);
            #endregion
            #region Fee
            var result8 = await _courseService.UpdateFeeAsync(courseFeeModel);
            #endregion
            #region Allowance
            var result9 = _courseService.UpdateAllowance(courseAllowanceModel);
            #endregion
            #region Course-Keyword
            var result10 = await _courseService.UpdateCourseKeyword(listKeywordModel, course.Id);
            #endregion
            #region Update-Course-Document
            var result11 = await _courseService.UpdateCourseDocumentation(course.Id, lstFileToDelete, lstFileUpdates, course.CourseCode);
            #endregion

            if (result1.IsSuccess
                && result2.IsSuccess
                && result3.IsSuccess
                && result4.IsSuccess
                && result5.IsSuccess
                && result6.IsSuccess
                && result7.IsSuccess
                && result8.IsSuccess
                && result9.IsSuccess
                && result10.IsSuccess
                && result11.IsSuccess)
            {
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, course.Id));
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed"));
            }

        }

        [HttpPost]
        [Route("GetCourseCodeByFilter")]
        public async Task<IHttpActionResult> GetCourseCodeByFilter(CourseCodeFilter filter)
        {
            if (filter.CourseCategoryId == 0 || filter.CourseTypeId == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed"));
            }
            var result = await _courseService.GetCourseCodeByFilter(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        private async Task<bool> CloneDataToElasticAsync(int courseId, List<CourseDocument> docs = null)
        {
            Log.Info($"Start clone data to elastic - Course Id: {courseId}");

            var defaultIndex = "searchdata";
            var siteUrl = await _clientService.GetClientUrlByNameAsync("ApplicantPortal");

            Course course = await _courseService.GetCourseInformationById(courseId);

            if (course == null)
            {
                Log.Info($"Failue to clone data to elastic - Course Id: {courseId}");
                return false;
            }

            List<Course> courses = new List<Course>() { course };
            List<SearchModel> dataList = new List<SearchModel>(courses.ToElasticSearchList(siteUrl));

            if (docs != null)
            {
                dataList.AddRange(docs.ToElasticSearchList(siteUrl));
            }

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

            Log.Info($"Completed clone data to elastic - Course Id: {courseId}");

            return responseChecking;
        }

        #region Assessment
        [HttpPost]
        [Route("GetCourseNameAssessment")]
        public async Task<IHttpActionResult> GetCourseNameByString(CourseAssessmentFilter filter)
        {
            var result = await _courseService.GetCourseNameByString(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpPost]
        [Route("GetCourseCodeAssessment")]
        public async Task<IHttpActionResult> GetCourseCodeAssessment(CourseAssessmentFilter filter)
        {
            var result = await _courseService.GetCourseCodeAssessment(filter);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("GetClassCodeAssessment")]
        public async Task<IHttpActionResult> GetCourseCodeAssessment(int courseId)
        {
            var result = await _classService.GetClassCodeAssessment(courseId);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }
        #endregion
    }
}
