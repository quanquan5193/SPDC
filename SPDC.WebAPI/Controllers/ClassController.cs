using SPDC.Model.BindingModels;
using SPDC.Service.Services;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SPDC.Common;
using SPDC.Data;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.WebAPI.Helpers;
using System.Web.Script.Serialization;
using SPDC.Model.BindingModels.AdditionalClassApproval;
using SPDC.Model.ViewModels.AdditionalClassApproval;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Class")]
    [EnableCorsAttribute("*", "*", "*")]
    public class ClassController : ApiControllerBase
    {
        private readonly IClassService _classService;
        private ILanguageService _languageService;
        private IUserService _userService;
        public ClassController(IClassService classService, ILanguageService languageService, IUserService userService)
        {
            _classService = classService;
            _languageService = languageService;
            _userService = userService;

        }

        [HttpPost]
        [Route("CreateAdmin")]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult CreateClassByAdminPortal(CreateClassAdminBindingModel model)
        {
            Log.Info($"Start Create Admin - Id: {model.Id}");
            var result = _classService.CreateClassAdmin(model);
            Log.Info($"Completed Create Admin - Id: {model.Id}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, new { id = result.Id }));
        }

        ///// <summary>
        ///// get all class
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("GetAllClass")]
        ////[Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        //public async Task<IHttpActionResult> GetAllClass(int? targetClassId = null)
        //{
        //    Log.Info($"Start Get All Class - Id: {targetClassId}");
        //    var result = await _classService.GetAllClass(targetClassId);
        //    var listBindingModel = Mapper.Map<IEnumerable<ClassBindingModel>>(result);
        //    var listViewModel = Mapper.Map<IEnumerable<ClassViewModel>>(listBindingModel);
        //    Log.Info($"Completed Get All Class - Id: {targetClassId}");

        //    return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, listViewModel));
        //}

        /// <summary>
        /// Create record class 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveClass")]
        public async Task<IHttpActionResult> SaveClass(ClassBindingModel model)
        {
            Log.Info($"Start Save Class - Class Id: {model.Id}");
            var isCreateUpdateClass = _classService.CheckCreateUpdateClass(model.CourseId);

            if (!isCreateUpdateClass)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("The Course Setup is not approved"));
            }

            Class resultClass;
            //mapper binding model to model
            var objClass = Mapper.Map<Class>(model);
            var userId = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            //var userId = 1;
            resultClass = model.Id > 0 ? await _classService.UpdateClass(objClass, userId) : _classService.CreateClass(objClass, userId);
            //map model to binding model
            var classBindingModel = Mapper.Map<ClassBindingModel>(resultClass);

            //save list lesson 
            //if (classBindingModel != null)
            //    foreach (var item in classBindingModel.Lessons)
            //    {
            //        if (item.ListFileUpload != null)
            //            item.ListDocumentImageUrl = SaveImage(item.ListFileUpload, item.Id);
            //    }
            //mapper
            var classViewModel = Mapper.Map<ClassViewDetailModel>(classBindingModel);

            Log.Info($"End Save Class - Class Id: {model.Id}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, classViewModel));
        }

        private List<string> SaveImage(HttpFileCollection files, int lessonId)
        {
            var listDocumentImageUrl = new List<string>();
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", "xlsx", ".ppt", ".pptx", ".csv", ".txt" };
            //if (file == null) return BadRequest("Null file");
            //if (file.ContentLength > 10 * 1024 * 1024) return BadRequest("Max file size exceeded.");
            //if (file.ContentLength == 0) return BadRequest("Empty file");
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[0];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var uploadsFolderPath = Path.Combine(ConfigHelper.GetByKey("LessonUpload"), "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(uploadsFolderPath);

                var ext = Path.GetExtension(file.FileName);
                //if (!allowedExtensions.Contains(ext)) return BadRequest("Invalid Image type.");

                var directory = ConfigHelper.GetByKey("LessonUpload");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var path = serPath + "\\" + "uploads";
                if (!Directory.Exists(path))
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(path);
                var pathFile = Common.Common.GenFileNameDuplicate(path + "\\" + fileName);
                file.SaveAs(pathFile);

                var url = "uploads" + "\\" + fileName;
                //assign doc binding model
                var documentBindingModel = new DocumentBindingModel()
                {
                    Url = url,
                    FileName = fileName,
                    ContentType = file.ContentType
                };
                //mapping document
                var objDocument = Mapper.Map<Document>(documentBindingModel);
                //save document
                var documentImageId = _classService.CreateDocument(objDocument);

                var courseDocBindingModel = new CourseDocumentViewModel()
                {
                    LessonId = lessonId,
                    DocumentId = documentImageId,
                    CourseId = 5,
                };
                var objCourseDocument = Mapper.Map<CourseDocument>(courseDocBindingModel);

                //save CourseDocument
                var courseDocumentId = _classService.CreateCourseDocument(objCourseDocument);
                listDocumentImageUrl.Add(ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Content/GetImage?id=" + documentImageId);
            }

            return listDocumentImageUrl;
        }

        /// <summary>
        /// Create record class 
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteClass")]
        public async Task<IHttpActionResult> DeleteClass(int classId)
        {
            Log.Info($"Start Delete Class - Class Id: {classId}");
            //delete
            var result = await _classService.DeleteClass(classId);
            //map model to binding model
            //var classBindingModel = Mapper.Map<ClassBindingModel>(result);
            //var classViewModel = Mapper.Map<ClassViewDetailModel>(classBindingModel);
            Log.Info($"Completed Delete Class - Class Id: {classId}");
            return result ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), result)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Can not remove all the class"));
        }

        /// <summary>
        /// Check Exist Class
        /// </summary>
        /// <param name="classCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckExistClassCode")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CheckExistClassCode(string classCode)
        {
            Log.Info($"Start Check Exist Class Code - Code: {classCode}");
            var result = await _classService.CheckExistClassCode(classCode);
            Log.Info($"Completed Check Exist Class Code - Code: {classCode}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClassById/{classId}")]
        public async Task<IHttpActionResult> GetClassById(int classId, int courseId = 0)
        {
            Log.Info($"Start Get Class - Class Id: {classId}");
            var result = await _classService.GetClassById(classId, courseId);
            //var objBindingModel = Mapper.Map<ClassBindingModel>(result.Data);
            var objViewModel = result.Data;
            Log.Info($"Completed Get Class - Class Id: {classId}");
            return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, objViewModel));
        }

        [HttpGet]
        [Route("CountClassCreated")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CountClassCreated(int userId)
        {
            Log.Info($"Start Count Class Created - User Id: {userId}");
            var result = await _classService.CountClassCreated(userId);
            Log.Info($"Completed Count Class Created - User Id: {userId}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        /// <summary>
        /// Create record class 
        /// </summary>
        /// <param name="listModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveAllClass")]
        public async Task<IHttpActionResult> SaveAllClass(List<ClassBindingModel> listModels)
        {
            Log.Info($"Start Save All Class");
            var resultClass = new List<Class>();
            var userId = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            //var userId = 1;

            foreach (var item in listModels)
            {
                //mapper binding model to model
                var objClass = Mapper.Map<Class>(item);
                if (item != null && item.Id > 0)
                    resultClass.Add(await _classService.UpdateClass(objClass, userId));
                else
                    resultClass.Add(_classService.CreateClass(objClass, userId));
            }

            //map model to binding model
            var listClassBindingModel = Mapper.Map<List<ClassBindingModel>>(resultClass);

            //mapper
            var listClassViewModel = Mapper.Map<List<ClassViewDetailModel>>(listClassBindingModel);
            Log.Info($"Completed Save All Class");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, listClassViewModel));
        }

        [HttpGet]
        [Route("GetAllClassCode")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllClassCode(string year)
        {
            Log.Info($"Get all class code in year: {year}");
            var listClassCode = await _classService.GetAllClassCode(year);
            Log.Info($"Completed get all class code in year: {year}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, listClassCode));
        }

        #region Class Common
        [HttpGet]
        [Route("GetAllClassCommon/{typeId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllClassCommon(int? typeId = null)
        {
            Log.Info($"Start Get All Class Common - Type Id: {typeId}");

            if (typeId == 1 || typeId == 3)
            {
                var result = await _classService.GetAllClassCommon(typeId);
                Log.Info($"Completed Get All Class Common - Type Id: {typeId}");
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }

            if (typeId == 2)
            {
                List<string> years = new List<string>();
                for (int i = DateTime.UtcNow.Year; i <= 2099; i++)
                {
                    years.Add(i.ToString());
                }
                Log.Info($"Completed Get All Class Common - Type Id: {typeId}");
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, years));
            }

            //var result = await _classService.GetAllClassCommon(typeId);
            Log.Info($"Completed Get All Class Common - Type Id: {typeId}");
            return BadRequest();
            //return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, result));
        }


        #endregion

        #region Target Class
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetClassView"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateTargetClass")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateTargetClass(TargetClassViewModel targetClassView)
        {
            Log.Info($"Start Create Target Class");
            var userId = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            //var userId = 1;
            var objTargetClassBindingModel = Mapper.Map<TargetClassBindingModel>(targetClassView);
            var objTargetClasses = Mapper.Map<TargetClasses>(objTargetClassBindingModel);
            //Create target
            var resultTargetClass = await _classService.CreateTargetClass(targetClassView, userId);
            ////map model to binding model
            //var targetClassBindingModels = Mapper.Map<TargetClassBindingModel>(resultTargetClass.Data);
            ////mapper
            //var targetClassViewModel = Mapper.Map<TargetClassViewModel>(targetClassBindingModels);
            Log.Info($"Completed Create Target Class");
            return Content(HttpStatusCode.OK, new ActionResultModel(resultTargetClass.Message, true, resultTargetClass.Data));
        }

        [HttpGet]
        [Route("GetTargetClass")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetTargetClass(int courseId)
        {
            Log.Info("Start Get Target Class");
            var resultTargetClass = await _classService.GetTargetClass(courseId);
            if (resultTargetClass.Data == null) return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, null));
            ////map model to binding model
            //var targetClassBindingModels = Mapper.Map<TargetClassBindingModel>(resultTargetClass);
            ////mapper
            //var targetClassViewModel = Mapper.Map<TargetClassViewModel>(targetClassBindingModels);
            Log.Info("Completed Get Target Class");

            var result = await _classService.ChangeStatusSubClasses(resultTargetClass.Data);

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        [HttpGet]
        [Route("CloneClass/{currentClassId}/{cloneClassId}")]
        public async Task<IHttpActionResult> CloneClassFromExistingClass(int currentClassId, int cloneClassId)
        {
            if (currentClassId == 0 || cloneClassId == 0)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("Class Id can not be null"));
            }

            int userId = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out userId);
            if (!success)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("cannot_get_user_id")));
            }

            var result = await _classService.CloneClassFromExistingClass(currentClassId, cloneClassId, userId);

            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess, result.Data)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));

        }

        #endregion

        #region Lesson
        //Create lesson
        [HttpPost]
        [Route("SaveLesson")]
        public async Task<IHttpActionResult> SaveLesson()
        {
            Log.Info($"Start Create Lesson in Class");
            var listFileDelete = new List<int>();
            var formData = HttpUtility.UrlDecode(HttpContext.Current.Request.Params["LessonViewModel"]);
            var lessonViewModel = JsonConvert.DeserializeObject<LessonViewModel>(formData);
            var objLessonBindingModel = Mapper.Map<LessonBindingModel>(lessonViewModel);
            var getFileDelete = HttpContext.Current.Request.Form.Get("ListFileToDelete");
            if (getFileDelete != null)
                listFileDelete = JsonConvert.DeserializeObject<List<int>>(getFileDelete);

            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out var id);
            //var success = true;
            //var id = 1;
            if (!success)
            {
                Log.Error("Failed Create Lesson");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var file = HttpContext.Current.Request.Files;
            var result = objLessonBindingModel.Id == 0 ? await _classService.CreateLesson(objLessonBindingModel, id, file) : await _classService.UpdateLesson(objLessonBindingModel, id, file, listFileDelete);
            if (result.IsSuccess)
            {
                Log.Info("Completed Create Lesson");
            }
            else
            {
                Log.Error("Failed Create Lesson");
            }
            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed", false, result));
        }

        [HttpPost]
        [Route("DeleteLesson")]
        public IHttpActionResult DeleteLesson(int lessonId)
        {
            Log.Info($"Start Delete Lesson - Lesson Id: {lessonId}");
            //delete
            var result = _classService.DeleteLesson(lessonId);
            ////map model to binding model
            //var examBindingModel = Mapper.Map<ExamBindingModel>(result);
            //var examViewModel = Mapper.Map<ExamViewModel>(examBindingModel);
            Log.Info($"Completed Delete Lesson - Lesson Id: {lessonId}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }
        #endregion

        #region Exam and Re-Exam
        //Save exam
        [HttpPost]
        [Route("SaveExam")]
        [AllowAnonymous]
        public IHttpActionResult SaveExam(ExamViewModel examViewModel)
        {
            Log.Info($"Start Save list exam");
            ExamBindingModel resultExamBindig;
            //mapper binding model to model
            var userId = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            var examBindingModel = Mapper.Map<ExamBindingModel>(examViewModel);
            if (examBindingModel.Id > 0)
                resultExamBindig = _classService.UpdateExam(examBindingModel, userId);
            else
                resultExamBindig = _classService.CreateExam(examBindingModel, userId);
            //mapper 
            var examView = Mapper.Map<ExamViewModel>(resultExamBindig);
            Log.Info($"Completed Save list exam");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, examView));
        }

        [HttpPost]
        [Route("DeleteExam/{examId}")]
        [AllowAnonymous]
        public IHttpActionResult DeleteExam(int examId)
        {
            Log.Info($"Start Delete Exam - Exam Id: {examId}");
            //delete
            var result = _classService.DeleteExam(examId);
            //mapper
            var examViewModel = Mapper.Map<ExamViewModel>(result);
            Log.Info($"Completed Delete Exam - Exam Id: {examId}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, examViewModel));
        }
        #endregion

        #region Modules
        [HttpGet]
        [Route("GetAllModules/{courseId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllModules(int courseId)
        {
            Log.Info($"Start get All modules");
            var lang = await _languageService.GetLanguageByCode(GetLanguageCode().ToString());
            var result = await _classService.GetAllModules(lang.Id, courseId);
            var modulTranBinding = Mapper.Map<List<SelectModuleItem>>(result);
            Log.Info($"End get All modules");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, modulTranBinding));
        }

        #endregion

        #region Approval for addtional class

        [HttpPost]
        [Route("AdditionalClassApproval/Paging")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetAdditionalClassApproval(AdditionalClassSearchModel model)
        {
            Log.Info($"Get Additional Class Approval");
            IEnumerable<AdditionalClassesApproval> list = await _classService.GetAdditionalClassApprovalPaging(model.CourseId, model.SortBy, model.IsDescending, model.Index, model.Size);
            int total = await _classService.GetAdditionalClassApprovalTotal(model.CourseId);
            var rtnlist = Mapper.Map<IEnumerable<AdditionalClassesApprovalViewModel>>(list);

            int userId = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out userId);
            if (!success)
            {
                return BadRequest(FileHelper.GetServerMessage("cannot_get_user_id"));
            }

            var user = await _userService.GetUserByID(userId);

            foreach (var item in rtnlist)
            {
                item.ApprovalUpdatedBy = user.DisplayName;
            }

            Log.Info($"Completed get Additional Class Approval");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, new PaginationSet<AdditionalClassesApprovalViewModel> { Items = rtnlist, Page = model.Index, TotalCount = total }));
        }

        [HttpPost]
        [Route("AdditionalClassApproval/Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> SaveAdditionalClassApproval()
        {
            Log.Info($"Get Additional Class Approval");
            int userId = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out userId);
            if (!success)
            {
                return BadRequest(FileHelper.GetServerMessage("cannot_get_user_id"));
            }

            var model = new JavaScriptSerializer().Deserialize<AdditionalClassBindingModel>(HttpContext.Current.Request.Params["AdditionalClassApprovalModel"]);

            var files = HttpContext.Current.Request.Files;

            AdditionalClassesApproval result = await _classService.CreateAdditionalClassApproval(model.CourseId, userId, model.NewNumber, model.ApprovalRemark, model.StatusTo, files);
            if (result == null)
            {
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("update_failure"), false, false));
            }
            Log.Info($"Completed get Additional Class Approval");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("update_success"), true, result != null));
        }


        #endregion
    }
}
