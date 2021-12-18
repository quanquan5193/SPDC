using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SPDC.Common;
using SPDC.Common.Enums;
using Document = SPDC.Model.Models.Document;
using SPDC.Model.ViewModels.Assessment;
using SPDC.Model.ViewModels.MakeupClass;

namespace SPDC.Service.Services
{
    public interface IClassService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Class CreateClassAdmin(CreateClassAdminBindingModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //Task<IEnumerable<Class>> GetAllClass(int? targetClassId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Class CreateClass(Class model, int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Class> UpdateClass(Class model, int userId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="classCode"></param>
        /// <returns></returns>
        Task<bool> CheckExistClassCode(string classCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<ResultModel<ClassDetailViewModel>> GetClassById(int classId, int courseId = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<bool> DeleteClass(int classId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listDocument"></param>
        /// <returns></returns>
        int CreateDocument(Document document);

        int CreateCourseDocument(CourseDocument courseDocument);

        Task<int> CountClassCreated(int userId);

        Task<IEnumerable<ClassCommon>> GetAllClassCommon(int? typeId);

        Task<ResultModel<TargetClassViewModel>> CreateTargetClass(TargetClassViewModel targetClassView, int userId);

        Task<ResultModel<TargetClassViewModel>> GetTargetClass(int courseId);

        Task<IEnumerable<SelectItemClass>> GetAllClassCode(string year);

        Task<ResultModel<LessonBindingModel>> CreateLesson(LessonBindingModel model, int userId, HttpFileCollection file = null);

        Task<ResultModel<LessonBindingModel>> UpdateLesson(LessonBindingModel model, int userId, HttpFileCollection file = null, List<int> listFileDelete = null);
        bool CheckCreateUpdateClass(int courseId);
        Task<TargetClassViewModel> ChangeStatusSubClasses(TargetClassViewModel model);

        bool DeleteLesson(int lessonId);

        ExamBindingModel CreateExam(ExamBindingModel model, int userId);

        ExamBindingModel UpdateExam(ExamBindingModel model, int userId);

        ExamBindingModel DeleteExam(int examId);
        Task<List<ModuleTran>> GetAllModules(int langId, int courseId);

        Task<IEnumerable<AdditionalClassesApproval>> GetAdditionalClassApprovalPaging(int courseId, string sortBy, bool isDescending, int index, int size);

        Task<int> GetAdditionalClassApprovalTotal(int courseId);

        Task<AdditionalClassesApproval> CreateAdditionalClassApproval(int courseId, int userId, int newNumber, string approvalRemark, int statusTo, HttpFileCollection files);
        Task<ResultModel<bool>> CloneClassFromExistingClass(int currentClassId, int cloneClassId, int userId);
        Task<IEnumerable<ClassAssessmentViewModel>> GetClassCodeAssessment(int courseId);

        Task<IEnumerable<AssignLessonViewModel>> GetAssignLesson(string classCode);
    }

    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassRepository _classRepository;
        private readonly ILessionRepository _lessionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ICourseDocumentsRepository _courseDocumentsRepository;
        private readonly IClassCommonRepository _classCommonRepository;
        private readonly ITargetClassRepository _targetClassRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseLocationRepository _courseLocationRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IAdditionalClassesApprovalRepository _additionalClassesApprovalRepository;
        private readonly IClientRepository _clientRepo;
        private readonly IClientRepository _clientRepository;
        private readonly ISystemPrivilegeRepository _systemPrivilegeRepository;
        private readonly IUserRepository _userRepository;
        ICommonDataService _commonDataService;

        private const string PrefixChar = "M";

        public ClassService(IUnitOfWork unitOfWork, IClassRepository classRepository, ILessionRepository lessionRepository,
            IExamRepository examRepository, IDocumentRepository documentRepository, ICourseDocumentsRepository courseDocumentsRepository,
            IClassCommonRepository classCommonRepository, ITargetClassRepository targetClassRepository, ICourseRepository courseRepository,
            ICourseLocationRepository courseLocationRepository, IApplicationRepository applicationRepository, IModuleRepository moduleRepository,
            IAdditionalClassesApprovalRepository additionalClassesApprovalRepository, IClientRepository clientRepo, IClientRepository clientRepository,
            ISystemPrivilegeRepository systemPrivilegeRepository, IUserRepository userRepository, ICommonDataService commonDataService
)
        {
            _unitOfWork = unitOfWork;
            _classRepository = classRepository;
            _lessionRepository = lessionRepository;
            _examRepository = examRepository;
            _documentRepository = documentRepository;
            _courseDocumentsRepository = courseDocumentsRepository;
            _classCommonRepository = classCommonRepository;
            _targetClassRepository = targetClassRepository;
            _courseRepository = courseRepository;
            _courseLocationRepository = courseLocationRepository;
            _applicationRepository = applicationRepository;
            _moduleRepository = moduleRepository;
            _additionalClassesApprovalRepository = additionalClassesApprovalRepository;
            _clientRepo = clientRepo;
            _clientRepository = clientRepository;
            _systemPrivilegeRepository = systemPrivilegeRepository;
            _userRepository = userRepository;
            _commonDataService = commonDataService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Class CreateClassAdmin(CreateClassAdminBindingModel model)
        {
            Class classes = new Class()
            {
                ClassCode = model.ClassCode,
                CourseId = model.CourseId,
                CommencementDate = model.CommencementDate,
                CompletionDate = model.CompletionDate,
                IsExam = model.IsExam,
                IsReExam = model.IsReExam,
                ExamPassingMask = model.ExamPassingMask,
                ReExamFees = model.ReExamFees,
                //AcademicYear = model.AcademicYear,
                InvisibleOnWebsite = model.InvisibleOnWebsite
            };

            var result = _classRepository.Add(classes);
            _unitOfWork.Commit();

            foreach (var data in model.Lessons)
            {
                Model.Models.Lesson lesson = new Model.Models.Lesson();
            }


            return result;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IEnumerable<Class>> GetAllClass(int? targetClassId)
        //{
        //    var result = (await _classRepository.GetMulti(x => true && x.DeleteDate == null && (targetClassId != null ? x.TargetClassId == targetClassId : x.TargetClassId != null), null)).Select(i => new Class()
        //    {
        //        Id = i.Id,
        //        ClassCode = i.ClassCode,
        //        AttendanceRequirement = i.AttendanceRequirement,
        //        ClassCommonId = i.ClassCommonId,
        //        CommencementDate = i.CommencementDate,
        //        CompletionDate = i.CompletionDate,
        //        Capacity = i.Capacity,
        //        EnrollmentNumber = i.EnrollmentNumber,
        //        InvisibleOnWebsite = i.InvisibleOnWebsite,
        //    }).ToArray();

        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Class CreateClass(Class model, int userId)
        {
            model.CreateBy = userId;
            model.CreateDate = DateTime.Now;
            var result = _classRepository.Add(model);
            _unitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteClass(int classId)
        {
            var classModel = await _classRepository.GetSingleByCondition(x => x.Id == classId, new string[] { "Course", "Course.Classes" });
            if (classModel.Course.Classes.Count >= 2)
            {
                //delte application
                _applicationRepository.DeleteMulti(x => x.StudentPreferredClass != null && x.StudentPreferredClass == classId || x.AdminAssignedClass != null && x.AdminAssignedClass == classId);
                //delete ẽam
                _examRepository.DeleteMulti(x => x.ClassId == classId);
                //delete lesson
                _lessionRepository.DeleteMulti(x => x.ClassId == classId);
                //delete class
                _classRepository.Delete(classId);
                _unitOfWork.Commit();
                return true;
            }

            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Class> UpdateClass(Class model, int userId)
        {
            //find objClassMap
            var modelClass = await _classRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Lessons" });
            //check model class
            if (modelClass != null)
            {
                //convert data from cus model to model
                ConvertCustomModelToModel(modelClass, model, userId);
                //update
                _classRepository.Update(modelClass);
            }
            _unitOfWork.Commit();
            return modelClass;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelClass"></param>
        /// <param name="objClassMap"></param>
        /// <param name="userId"></param>
        private static void ConvertCustomModelToModel(Class modelClass, Class objClassMap, int userId)
        {
            modelClass.ClassCode = objClassMap.ClassCode;
            modelClass.CourseId = objClassMap.CourseId;
            modelClass.CommencementDate = objClassMap.CommencementDate;
            modelClass.CompletionDate = objClassMap.CompletionDate;
            modelClass.IsExam = objClassMap.IsExam;
            modelClass.IsReExam = objClassMap.IsReExam;
            modelClass.ExamPassingMask = objClassMap.ExamPassingMask;
            modelClass.ReExamFees = objClassMap.ReExamFees;
            //modelClass.AcademicYear = objClassMap.AcademicYear;
            modelClass.InvisibleOnWebsite = objClassMap.InvisibleOnWebsite;
            //modelClass.Exams = objClassMap.Exams;
            //modelClass.Lessons = objClassMap.Lessons;
            modelClass.CreateDate = objClassMap.CreateDate;
            modelClass.CreateBy = objClassMap.CreateBy;
            modelClass.LastModifiedDate = DateTime.Now;
            modelClass.LastModifiedBy = userId;
            modelClass.Capacity = objClassMap.Capacity;
            modelClass.AttendanceRequirement = objClassMap.AttendanceRequirement;
            modelClass.Capacity = objClassMap.Capacity;
            modelClass.EnrollmentNumber = objClassMap.EnrollmentNumber;
            modelClass.ClassCommonId = objClassMap.ClassCommonId;
            modelClass.CountReExam = objClassMap.CountReExam;
        }


        /// <summary>
        /// Check exist class code in database
        /// </summary>
        /// <param name="classCode"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistClassCode(string classCode)
        {
            return await _classRepository.CheckContains(x => x.ClassCode.Equals(classCode));
        }

        /// <summary>
        /// Get object Class by Id
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<ResultModel<ClassDetailViewModel>> GetClassById(int classId, int courseId = 0)
        {
            //init
            var result = new ResultModel<ClassDetailViewModel>();
            //find
            var objClass = _classRepository.GetSingleById(classId);
            if (objClass == null)
            {
                result.Message = "There is no class not match.";
                result.Data = null;
                return result;
            }
            var cls = new ClassDetailViewModel()
            {
                Id = objClass.Id,
                AttendanceRequirement = objClass.AttendanceRequirement,
                Capacity = objClass.Capacity,
                ClassCode = objClass.ClassCode,
                ClassCommonId = objClass.ClassCommonId,
                ClassNumber = objClass.ClassNumber,
                CommencementDate = objClass.CommencementDate,
                CompletionDate = objClass.CompletionDate,
                CountReExam = objClass.CountReExam,
                CourseId = objClass.CourseId,
                CreateBy = objClass.CreateBy,
                CreateDate = objClass.CreateDate,
                DeleteBy = objClass.DeleteBy,
                DeleteDate = objClass.DeleteDate,
                EnrollmentNumber = objClass.EnrollmentNumber,
                ExamPassingMask = objClass.ExamPassingMask,
                InvisibleOnWebsite = objClass.InvisibleOnWebsite,
                IsExam = objClass.IsExam,
                IsReExam = objClass.IsReExam,
                LastModifiedBy = objClass.LastModifiedBy,
                LastModifiedDate = objClass.LastModifiedDate,
                ReExamFees = objClass.ReExamFees,
                SubClassApprovedStatus = objClass.SubClassApprovedStatus,
                SubClassApprovedStatusHistories = objClass.SubClassApprovedStatusHistories,
                SubClassStatus = objClass.SubClassStatus
            };

            //get list child
            var classExams = (await _examRepository.GetMulti(x => x.ClassId == objClass.Id && x.DeleteBy == null && x.DeleteDate == null)).Select(i => new ExamViewModel()
            {
                ClassCommonId = i.ClassCommonId,
                ClassId = i.ClassId,
                Date = i.Date,
                Dateline = i.Dateline,
                ExamVenue = i.ExamVenue,
                ExamVenueText = i.ExamVenueText,
                FromTime = i.FromTime,
                Id = i.Id,
                IsReExam = i.IsReExam,
                Marks = i.Marks,
                ModuleId = i.ModuleId,
                ToTime = i.ToTime,
                Type = i.Type
            }).ToList();

            cls.Exams = classExams.Where(x => x.Type == (int)ExamType.Exam).ToList();
            cls.FirstExams = classExams.Where(x => x.Type == (int)ExamType.FirstReExam).ToList();
            cls.SecondExams = classExams.Where(x => x.Type == (int)ExamType.SecondReExam).ToList();

            var lstLesson = await _lessionRepository.GetMulti(x => x.ClassId == objClass.Id && x.DeleteBy == null && x.DeleteDate == null);

            if (lstLesson.Count > 0)
            {
                var apiUrl = (await _clientRepo.GetSingleByCondition(x => x.ClientName.Equals("ApiPortal"))).ClientUrl;

                foreach (var item in lstLesson)
                {
                    var lesson = new LessonViewModel()
                    {
                        ClassId = item.ClassId,
                        Date = item.Date,
                        Id = item.Id,
                        LocationId = item.LocationId,
                        No = item.No,
                        TimeFromHrs = item.TimeFromHrs,
                        TimeFromMin = item.TimeFromMin,
                        TimeToHrs = item.TimeToHrs,
                        TimeToMin = item.TimeToMin,
                        Venue = item.Venue
                    };

                    var lstDoc = await _courseDocumentsRepository.GetMulti(i => i.LessonId == item.Id && i.Document != null, new string[] { "Document" });

                    foreach (var doc in lstDoc)
                    {
                        lesson.Documents.Add(new DocumentViewModel()
                        {
                            ContentType = doc.Document.ContentType,
                            FileName = doc.Document.FileName,
                            Id = doc.DocumentId,
                            Url = $"{ConfigHelper.GetByKey("DMZAPI")}api/Proxy?url=Courses/download-document?docId={doc.DocumentId}"
                        });
                    }


                    cls.Lessons.Add(lesson);
                }
            }
            //assign data

            if (courseId != 0)
            {
                var course = await _courseRepository.GetSingleByCondition(x => x.Id == courseId, new string[] { "TargetClasses", "Classes" });
                course.TargetClasses.TargetNumberClass += 1;
                var aClass = CloneClass(objClass);
            }

            result.Message = Common.Common.Success;
            result.IsSuccess = true;
            result.Data = cls;
            return result;
        }

        private Class CloneClass(Class objClass)
        {
            var result = new Class();

            return result;
        }


        //public int UploadDocument(string url, string filename, string type)
        //{
        //    CmsImage image = new CmsImage()
        //    {
        //        Url = url,
        //        ContentType = type,
        //        FileName = filename
        //    };
        //    var result = _cMSImageRepository.Add(image);
        //    _unitOfWork.Commit();
        //    return result.Id;
        //}

        public int CreateDocument(Document document)
        {
            var result = _documentRepository.Add(document);
            _unitOfWork.Commit();
            return result.Id;
        }

        public int CreateCourseDocument(CourseDocument document)
        {
            var result = _courseDocumentsRepository.Add(document);
            _unitOfWork.Commit();
            return result.Id;
        }


        public async Task<int> CountClassCreated(int userId)
        {
            var result = await _classRepository.Count(x => x.DeleteBy == null && x.DeleteDate == null);
            return result;
        }

        public async Task<IEnumerable<ClassCommon>> GetAllClassCommon(int? typeId)
        {
            IEnumerable<ClassCommon> result;
            if (typeId != null)
                result = await _classCommonRepository.GetMulti(x => x.TypeCommon == typeId);
            else
                result = await _classCommonRepository.GetAll();
            return result.OrderBy(x => x.Id);
        }

        public async Task<ResultModel<TargetClassViewModel>> CreateTargetClass(TargetClassViewModel targetClass, int userId)
        {
            var result = new ResultModel<TargetClassViewModel>();

            #region Create Target Class and Sub Class
            var course = await _courseRepository.GetSingleByCondition(x => x.Id == targetClass.CourseId, new string[] { "TargetClasses" });

            if (course == null)
            {
                result.Message = "Course was not found";
                return result;
            }

            var modelTargetClass = new TargetClasses();
            //modelTargetClass.ClassCommonId = targetClass.ClassCommonId;
            modelTargetClass.TargetNumberClass = targetClass.TargetNumberClass;
            modelTargetClass.Id = targetClass.Id;
            modelTargetClass.AcademicYear = targetClass.AcademicYear;
            course.TargetClasses = modelTargetClass;

            //var classCommon = _classCommonRepository.GetSingleById(targetClass.ClassCommonId);

            for (int i = 0; i < targetClass.TargetNumberClass; i++)
            {
                var classCode = GenerateClassCode(PrefixChar, course.CourseVenueId, course.CourseCode, modelTargetClass.AcademicYear, i + 1);
                var objClass = new Class()
                {
                    ClassCode = classCode,
                    CommencementDate = DateTime.Now,
                    CompletionDate = DateTime.Now,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                };
                course.Classes.Add(objClass);
            }

            _courseRepository.Update(course);
            _unitOfWork.Commit();
            #endregion

            #region Return View Model

            var viewModel = (await _courseRepository.GetSingleByCondition(x => x.Id == targetClass.CourseId, new string[] { "TargetClasses", "Classes.ClassCommon" })).ToTargetClassViewModel();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = viewModel;
            return result;

            #endregion
        }

        private string GenerateClassCode(string prefixChar, int? courseVenueId, string courseCode, string year, int defineCode)
        {
            //find venue code
            var venueCode = courseVenueId != null ? _courseLocationRepository.GetSingleById((int)courseVenueId)?.VenueCode ?? string.Empty : string.Empty;
            var strCode = prefixChar + venueCode + "-" + courseCode + "-" + (year.Length > 2 ? year.Substring(2) : string.Empty) + defineCode.ToString("000");
            return strCode;
        }

        public async Task<TargetClassViewModel> ChangeStatusSubClasses(TargetClassViewModel model)
        {
            var course = await _courseRepository.GetSingleByCondition(x => x.Id == model.CourseId, new string[] { "Classes" });

            var isOpenApplicationSetup = false;
            var isAnyClassApproving = false;
            if (course.ClassApprovedStatus == (int)CourseApprovedStatus.ThirdApproved)
            {
                isOpenApplicationSetup = true;
                foreach (var item in course.Classes)
                {
                    if (item.SubClassApprovedStatus != (int)SubClassApprovedStatus.Created || item.SubClassApprovedStatus != (int)SubClassApprovedStatus.ThirdApproved ||
                        item.SubClassApprovedStatus != (int)SubClassApprovedStatus.FirstReject || item.SubClassApprovedStatus != (int)SubClassApprovedStatus.SecondReject ||
                            item.SubClassApprovedStatus != (int)SubClassApprovedStatus.ThirdReject || item.SubClassApprovedStatus != (int)SubClassApprovedStatus.Cancel)
                    {
                        isAnyClassApproving = true;
                        break;
                    }
                }
            }

            if (isOpenApplicationSetup && !isAnyClassApproving)
            {
                model.IsOpenApplicationSetup = true;
            }

            UpdateStatusSubClass(course);

            model.ClassApprovedStatus = course.ClassApprovedStatus;

            return model;

        }

        public async Task<ResultModel<TargetClassViewModel>> GetTargetClass(int courseId)
        {
            var result = new ResultModel<TargetClassViewModel>();

            var record = await _courseRepository.GetSingleByCondition(x => x.Id == courseId, new string[] { "TargetClasses", "Classes.ClassCommon", "Classes.Exams", "Classes.Lessons" });

            TargetClassViewModel targetClassViewModel = record.TargetClasses != null ? record.ToTargetClassViewModel() : null;

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = targetClassViewModel;
            return result;
        }

        private void UpdateStatusSubClass(Course course)
        {
            if (course == null) return;
            if (course.ClassApprovedStatus == (int)ClassApprovedStatus.ThirdApproved)
            {
                var listSubClassStatusForNotChange = new List<int>() { (int)SubClassStatus.Cancelled, (int)SubClassStatus.Postponed };
                foreach (var item in course.Classes)
                {
                    if (item.CommencementDate.ToString("ddMMyyyy").Equals(DateTime.Now.ToString("ddMMyyyy")) && !listSubClassStatusForNotChange.Contains(item.SubClassStatus))
                    {
                        item.SubClassStatus = (int)SubClassStatus.Openned;
                    }
                }
            }
            _courseRepository.Update(course);
            _unitOfWork.Commit();
        }

        public async Task<IEnumerable<SelectItemClass>> GetAllClassCode(string year)
        {
            var result = (await _classRepository.GetMulti(x => x.Course.TargetClasses.AcademicYear.Equals(year.Trim()))).Select(c =>
            new SelectItemClass
            {
                Id = c.Id,
                ClassCode = c.ClassCode
            });
            //var result = (await _classRepository.GetMulti(x => x.AcademicYear.Equals(year))).Select(x =>
            //      new SelectItemClass
            //      {
            //          Id = x.Id,
            //          ClassCode = x.ClassCode
            //      });
            //return result;

            //Fix after re-design database
            return result;
        }

        private string CombineFolder(int modelClassId, string lesson, int modelId)
        {
            return modelClassId + lesson + modelClassId;
        }


        #region LESSON
        public async Task<ResultModel<LessonBindingModel>> CreateLesson(LessonBindingModel model, int userId, HttpFileCollection filesCollection = null)
        {
            var result = new ResultModel<LessonBindingModel>();
            var listDocuments = new List<Document>();
            try
            {
                var apiUrl = (await _clientRepo.GetSingleByCondition(x => x.ClientName.Equals("ApiPortal"))).ClientUrl;

                var objLesson = ConvertLessonBindingToLesson(model, userId);
                var objClass = _classRepository.GetSingleById(model.ClassId);

                _lessionRepository.Add(objLesson);
                _unitOfWork.Commit();


                if (filesCollection != null)
                {
                    var folder = CombineFolder(model.ClassId, "Lesson", model.Id);
                    var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                    var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                    var path = serPath + "\\" + folder;
                    if (!Directory.Exists(path))
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(path);



                    for (var i = 0; i < filesCollection.Count; i++)
                    {
                        if (filesCollection.AllKeys[i].Equals("listFileUploadLesson"))
                        {
                            var file = filesCollection[i];
                            if (file.ContentLength > StaticConfig.MaximumFileLength)
                            {
                                result.Message = "A file is too large";
                                result.IsSuccess = false;
                            }
                            var pathFile = Common.Common.GenFileNameDuplicate(path + "\\" + file.FileName);
                            file.SaveAs(pathFile);
                            var fileNameToSaveDb = Path.GetFileName(pathFile);
                            var pathToSaveDb = folder + "\\" + fileNameToSaveDb;

                            var doc = _documentRepository.Add(EntityHelpers.ToDoccumentForCourse(pathToSaveDb, file.ContentType, fileNameToSaveDb, objClass.CourseId, (int)DistinguishDocType.CourseBrochure, objLesson.Id));

                            listDocuments.Add(doc);
                        }
                    }
                }

                _unitOfWork.Commit();

                //listDocuments.Add(new DocumentBindingModel() { ContentType = doc.ContentType, FileName = doc.FileName, Id = doc.Id, Url = $"{ConfigHelper.GetByKey("DMZAPI")}api/Proxy?url=Courses/download-document?docId={doc.Id}" });

                model.Id = objLesson.Id;

                foreach (var doc in listDocuments)
                {
                    model.Documents.Add(new DocumentBindingModel()
                    {
                        ContentType = doc.ContentType
                        ,
                        FileName = doc.FileName
                        ,
                        Id = doc.Id
                        ,
                        Url = $"{ConfigHelper.GetByKey("DMZAPI")}/api/Proxy?url=Courses/download-document?docId={doc.Id}"
                    });
                }


                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = model;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public async Task<ResultModel<LessonBindingModel>> UpdateLesson(LessonBindingModel model, int userId, HttpFileCollection filesCollection = null, List<int> fileDelete = null)
        {
            var result = new ResultModel<LessonBindingModel>();
            var listDocuments = new List<Document>();
            try
            {
                var newdata = ConvertLessonBindingToLesson(model, userId);

                var objLesson = await _lessionRepository.GetSingleByCondition(x => x.Id == model.Id);

                //objLesson.FromTime = newdata.FromTime;
                objLesson.No = newdata.No;
                objLesson.Date = newdata.Date;
                objLesson.TimeFromMin = newdata.TimeFromMin;
                objLesson.TimeFromHrs = newdata.TimeFromHrs;
                objLesson.TimeToMin = newdata.TimeToMin;
                objLesson.TimeToHrs = newdata.TimeToHrs;
                //objLesson.FromTime = newdata.FromTime;
                //objLesson.ToTime = newdata.ToTime;
                objLesson.Venue = newdata.Venue;

                var objClass = _classRepository.GetSingleById(model.ClassId);

                var listCourDocuments = await _documentRepository.GetMulti(x => x.CourseDocuments.Any(n => n.LessonId == objLesson.Id), new string[] { "CourseDocuments" });
                var folder = CombineFolder(model.ClassId, "Lesson", model.Id);
                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var path = serPath + "\\" + folder;
                var apiUrl = (await _clientRepo.GetSingleByCondition(x => x.ClientName.Equals("ApiPortal"))).ClientUrl;

                if (fileDelete != null)
                {
                    for (int i = 0; i < fileDelete.Count; i++)
                    {
                        var doc = listCourDocuments.FirstOrDefault(x => x.Id == fileDelete[i]);
                        if (doc != null)
                        {
                            var courseDoc = doc.CourseDocuments.FirstOrDefault(x => x.DocumentId == doc.Id);
                            _courseDocumentsRepository.Delete(courseDoc);

                            var tempUrl = serPath + doc.Url;
                            if (File.Exists(tempUrl))
                            {
                                File.Delete(tempUrl);
                            }
                            _documentRepository.Delete(doc);
                        }
                    }
                }

                if (filesCollection != null)
                {
                    if (!Directory.Exists(path))
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(path);

                    for (int i = 0; i < filesCollection.Count; i++)
                    {
                        if (filesCollection.AllKeys[i].Equals("listFileUploadLesson"))
                        {
                            var file = filesCollection[i];
                            if (file.ContentLength > StaticConfig.MaximumFileLength)
                            {
                                result.Message = "A file is too large";
                                result.IsSuccess = false;
                            }
                            var pathFile = Common.Common.GenFileNameDuplicate(path + "\\" + file.FileName);
                            file.SaveAs(pathFile);
                            var fileNameToSaveDb = Path.GetFileName(pathFile);
                            var pathToSaveDb = folder + "\\" + fileNameToSaveDb;

                            var doc = _documentRepository.Add(EntityHelpers.ToDoccumentForCourse(pathToSaveDb, file.ContentType, fileNameToSaveDb, objClass.CourseId, (int)DistinguishDocType.CourseBrochure, objLesson.Id));

                            listDocuments.Add(doc);
                        }
                    }
                }

                _lessionRepository.Update(objLesson);
                _unitOfWork.Commit();

                foreach (var doc in listDocuments)
                {
                    model.Documents.Add(new DocumentBindingModel()
                    {
                        ContentType = doc.ContentType
                        ,
                        FileName = doc.FileName
                        ,
                        Id = doc.Id
                        ,
                        Url = $"{ConfigHelper.GetByKey("DMZAPI")}/api/Proxy?url=Courses/download-document?docId={doc.Id}"
                    });
                }

                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = model;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        public bool DeleteLesson(int lessonId)
        {
            var modelLesson = _lessionRepository.GetSingleById(lessonId);
            _lessionRepository.Delete(modelLesson);
            _unitOfWork.Commit();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lessonBinding"></param>
        /// <param name="userId"></param>
        /// <param name="lesson"></param>
        private static Lesson ConvertLessonBindingToLesson(LessonBindingModel lessonBinding, int userId, Lesson lesson = null)
        {
            if (lesson == null)
                lesson = new Lesson { CreateBy = userId, CreateDate = DateTime.Now };

            lesson.LastModifiedBy = userId;
            lesson.LastModifiedDate = DateTime.Now;
            lesson.No = lessonBinding.No;
            //lesson.FromTime = lessonBinding.FromTime;
            lesson.Venue = lessonBinding.Venue;
            //lesson.ToTime = lessonBinding.ToTime;
            lesson.LocationId = lessonBinding.LocationId;
            lesson.Date = lessonBinding.Date;
            lesson.ClassId = lessonBinding.ClassId;

            lesson.TimeFromMin = lessonBinding.TimeFromMin;
            lesson.TimeFromHrs = lessonBinding.TimeFromHrs;
            lesson.TimeToMin = lessonBinding.TimeToMin;
            lesson.TimeToHrs = lessonBinding.TimeToHrs;

            return lesson;
        }

        #endregion

        #region EXAM
        public ExamBindingModel CreateExam(ExamBindingModel bindingExam, int userId)
        {
            var modelExam = ConvertBindingExamToExam(bindingExam, userId);
            _examRepository.Add(modelExam);
            _unitOfWork.Commit();
            var convertExam = ConvertExamToBindingExam(modelExam);
            return convertExam;
        }

        public ExamBindingModel UpdateExam(ExamBindingModel objExam, int userId)
        {
            //find 
            var modelExam = _examRepository.GetSingleById(objExam.Id);
            //check model class
            if (modelExam != null)
            {
                //convert data from cus model to model
                ConvertBindingExamToExam(objExam, userId, modelExam);
                //update
                _examRepository.Update(modelExam);
            }
            _unitOfWork.Commit();
            var bindingExam = ConvertExamToBindingExam(modelExam);
            return bindingExam;
        }

        public ExamBindingModel DeleteExam(int examId)
        {
            var modelExam = _examRepository.GetSingleById(examId);
            _examRepository.Delete(modelExam);
            _unitOfWork.Commit();
            var examBindingModel = ConvertExamToBindingExam(modelExam);
            return examBindingModel;
        }

        private static Exam ConvertBindingExamToExam(ExamBindingModel objExam, int userId, Exam modelExam = null)
        {
            if (modelExam == null)
                modelExam = new Exam { CreateBy = userId, CreateDate = DateTime.Now };

            modelExam.ClassId = objExam.ClassId;
            modelExam.ExamVenue = objExam.ExamVenue;
            modelExam.Type = objExam.Type;
            modelExam.Date = objExam.Date;
            modelExam.FromTime = objExam.FromTime;
            modelExam.ToTime = objExam.ToTime;
            modelExam.ClassCommonId = objExam.ClassCommonId;
            modelExam.ExamVenueText = objExam.ExamVenueText;
            modelExam.IsReExam = objExam.IsReExam;
            modelExam.ModuleId = objExam.ModuleId;
            modelExam.Dateline = objExam.Dateline;
            modelExam.Marks = objExam.Marks;
            modelExam.LastModifiedBy = userId;
            modelExam.LastModifiedDate = DateTime.Now;
            return modelExam;
        }

        private ExamBindingModel ConvertExamToBindingExam(Exam model)
        {
            var examBinding = new ExamBindingModel()
            {
                Id = model.Id,
                ClassId = model.ClassId,
                ExamVenue = model.ExamVenue,
                Type = model.Type,
                Date = model.Date,
                FromTime = model.FromTime,
                ToTime = model.ToTime,
                Dateline = model.Dateline,
                ClassCommonId = model.ClassCommonId,
                ModuleId = model.ModuleId,
                ExamVenueText = model.ExamVenueText,
                IsReExam = model.IsReExam,
                Marks = model.Marks
            };

            return examBinding;
        }
        #endregion

        public async Task<List<ModuleTran>> GetAllModules(int langId, int courseId)
        {
            var listModule = await _moduleRepository.GetMulti(x => x.CourseId == courseId, new[] { "ModuleTrans" });
            var listModuleTrans = new List<ModuleTran>();
            foreach (var item in listModule)
            {
                listModuleTrans.Add(item.ModuleTrans.FirstOrDefault(x => x.LanguageId == langId));
            }
            return listModuleTrans;

        }

        public bool CheckCreateUpdateClass(int courseId)
        {
            var isCreateUpdate = _courseRepository.GetSingleById(courseId).CourseApprovedStatus == (int)Common.Enums.CourseApprovedStatus.ThirdApproved;
            return isCreateUpdate;
        }

        #region Additional Classes Approval

        public async Task<AdditionalClassesApproval> CreateAdditionalClassApproval(int courseId, int userId, int newNumber, string approvalRemark, int statusTo, HttpFileCollection files)
        {
            var targetClasses = await _targetClassRepository.GetSingleByCondition(x => x.Id == courseId, new string[] { "Course", "Course.Classes" });
            if (newNumber < targetClasses.TargetNumberClass && statusTo == (int)AdditionalClassApprovedStatus.Sumitted)
                return null;

            var course = _courseRepository.GetSingleById(courseId);
            if (course == null || course.ClassApprovedStatus != (int)ClassApprovedStatus.ThirdApproved)
            {
                return null;
            }

            AdditionalClassesApproval recentRecord = (await _additionalClassesApprovalRepository.GetMulti(x => x.CourseId == courseId)).LastOrDefault();
            if (recentRecord != null)
            {
                if (newNumber == 0) newNumber = recentRecord.NewTargetNumber;
                else if (statusTo != (int)AdditionalClassApprovedStatus.Sumitted && newNumber != recentRecord.NewTargetNumber) return null;
            }

            AdditionalClassesApproval model = new AdditionalClassesApproval()
            {
                CourseId = courseId,
                UpdatedBy = userId,
                OriginalTargetNumber = targetClasses.TargetNumberClass,
                NewTargetNumber = newNumber,
                ApprovalRemark = approvalRemark,
                StatusFrom = (int)AdditionalClassApprovedStatus.Created,
                StatusTo = statusTo,
                Documents = new List<Document>()
            };

            if (recentRecord != null)
            {
                switch (recentRecord.StatusTo)
                {
                    case (int)AdditionalClassApprovedStatus.Sumitted:
                        if (!(statusTo == (int)AdditionalClassApprovedStatus.FirstApproved || statusTo == (int)AdditionalClassApprovedStatus.FirstReject || statusTo == (int)AdditionalClassApprovedStatus.Cancel))
                        {
                            return null;
                        }
                        break;

                    case (int)AdditionalClassApprovedStatus.FirstApproved:
                        if (!(statusTo == (int)AdditionalClassApprovedStatus.SecondApproved || statusTo == (int)AdditionalClassApprovedStatus.SecondReject))
                        {
                            return null;
                        }
                        break;

                    case (int)AdditionalClassApprovedStatus.SecondApproved:
                        if (!(statusTo == (int)AdditionalClassApprovedStatus.ThirdApproved || statusTo == (int)AdditionalClassApprovedStatus.ThirdReject))
                        {
                            return null;
                        }
                        break;
                    case (int)AdditionalClassApprovedStatus.Cancel:
                    case (int)ClassApprovedStatus.FirstReject:
                    case (int)ClassApprovedStatus.SecondReject:
                    case (int)ClassApprovedStatus.ThirdReject:
                        if (statusTo != (int)AdditionalClassApprovedStatus.Sumitted)
                        {
                            return null;
                        }
                        break;
                    default:
                        return null;
                }
                model.StatusFrom = recentRecord.StatusTo;
            }
            else
            {
                model.StatusTo = (int)AdditionalClassApprovedStatus.Sumitted;
            }

            if (files != null && files.Count > 0)
            {
                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                string pathDirectory = serPath + course.CourseCode + "\\" + DistinguishDocType.CancelledPostponed.ToString();

                if (!Directory.Exists(pathDirectory))
                {
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    string originalFileExtension = file.ContentType;
                    string pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + file.FileName);
                    string originalFileName = Path.GetFileName(pathFile);
                    string url = pathFile.Substring(pathFile.IndexOf(course.CourseCode));
                    file.SaveAs(pathFile);

                    model.Documents.Add(
                        new Document()
                        {
                            Url = url,
                            ContentType = originalFileExtension,
                            FileName = originalFileName,
                            ModifiedDate = DateTime.Now
                        }
                    );
                }
            }

            var result = _additionalClassesApprovalRepository.Add(model);
            IEnumerable<int> deleteClassId = null;
            if (statusTo == (int)AdditionalClassApprovedStatus.ThirdApproved)
            {
                //var classCommon = _classCommonRepository.GetSingleById(targetClasses.ClassCommonId);

                // Case 1 newNumber > targetClass
                if (newNumber > targetClasses.TargetNumberClass)
                {
                    var lastClass = course.Classes.LastOrDefault();
                    int startNumber = 1;
                    if (lastClass != null)
                    {
                        startNumber = Convert.ToInt32(lastClass.ClassCode.Substring(lastClass.ClassCode.Length - 3, 3));
                    }

                    for (var i = 0; i < newNumber - targetClasses.TargetNumberClass; i++)
                    {
                        startNumber++;
                        var classCode = GenerateClassCode(PrefixChar, course.CourseVenueId, course.CourseCode, targetClasses.AcademicYear, startNumber);
                        var objClass = new Class()
                        {
                            ClassCode = classCode,
                            CourseId = course.Id,
                            //TargetClassId = targetClasses.Id,
                            CommencementDate = DateTime.Now,
                            CompletionDate = DateTime.Now,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            //AcademicYear = classCommon.TypeName
                        };
                        var modelClass = CreateClass(objClass, userId);
                        course.Classes.Add(modelClass);
                    }
                }

                // Case 2 newNumber < targetClass
                if (newNumber < targetClasses.TargetNumberClass)
                {
                    var removeCount = targetClasses.TargetNumberClass - newNumber;
                    deleteClassId = course.Classes.OrderByDescending(x => x.Id).Take(removeCount).Select(x => x.Id);
                }

                targetClasses.TargetNumberClass = newNumber;
                _targetClassRepository.Update(targetClasses);
                //course.ClassApprovedStatus = (int)ClassApprovedStatus.Created;
                _courseRepository.Update(course);
            }
            if (deleteClassId != null)
            {
                foreach (int id in deleteClassId)
                {
                    _classRepository.Delete(id);
                }
            }
            _unitOfWork.Commit();
            await SendAdditionalClassEmailApproved(model.StatusTo, course);
            return result;
        }

        private async Task SendAdditionalClassEmailApproved(int approvedStatus, Course course)
        {
            #region Send Mail
            string callbackurl = (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals("AdminPortal"))).ClientUrl;

            var _submittedAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSubmitAndCancelClass == true)).Select(x => x.UserId);
            var _submittedAdminEmails = (await _userRepository.GetMulti(x => _submittedAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _firstAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsFirstApproveAndRejectClass == true)).Select(x => x.UserId);
            var _firstAdminEmails = (await _userRepository.GetMulti(x => _firstAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _secondAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsSecondpproveAndRejectClass == true)).Select(x => x.UserId);
            var _secondAdminEmails = (await _userRepository.GetMulti(x => _secondAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);

            var _thirdAdminIds = (await _systemPrivilegeRepository.GetMulti(x => x.CourseId == course.Id && x.IsThirdApproveAndRejectClass == true)).Select(x => x.UserId);
            var _thirdAdminEmails = (await _userRepository.GetMulti(x => _thirdAdminIds.Contains(x.Id))).Select(x => x.AdminEmail);
            switch (approvedStatus)
            {
                case (int)ClassApprovedStatus.Submitted:

                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item26", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem26Email(course.CourseCode, callbackurl + "/student-class-management/search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    break;
                case (int)ClassApprovedStatus.FirstApproved:
                    foreach (var e in _secondAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item28", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem28Email(course.CourseCode, callbackurl + "/student-class-management/search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/student-class-management/search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.FirstReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/student-class-management/search", "1st approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.SecondApproved:
                    foreach (var e in _thirdAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item29", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem29Email(course.CourseCode, callbackurl + "/student-class-management/search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/student-class-management/search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.SecondReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/student-class-management/search", "2nd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.ThirdApproved:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item30", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem30Email(course.CourseCode, callbackurl + "/student-class-management/search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.ThirdReject:

                    foreach (var e in _submittedAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item31", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem31Email(course.CourseCode, callbackurl + "/student-class-management/search", "3rd approver"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
                case (int)ClassApprovedStatus.Cancel:
                    foreach (var e in _firstAdminEmails)
                    {
                        string emailSubject = FileHelper.GetEmailSubject("item27", "EmailSubject", "EN");
                        bool isSuccess = MailHelper.SendMail(e, string.Format(emailSubject, course.CourseCode), Common.Common.GenerateItem27Email(course.CourseCode, callbackurl + "/student-class-management/search"));
                        if (isSuccess)
                        {
                            CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                            emailCommonData.ValueInt++;
                            _commonDataService.Update(emailCommonData);
                        }
                    }
                    break;
            }
            #endregion
        }

        public async Task<IEnumerable<AdditionalClassesApproval>> GetAdditionalClassApprovalPaging(int courseId, string sortBy, bool isDescending, int index, int size)
        {
            var result = await _additionalClassesApprovalRepository.GetMultiPaging(x => x.CourseId == courseId, sortBy, isDescending, index, size, new string[] { "Documents" });
            return result;
        }

        public async Task<int> GetAdditionalClassApprovalTotal(int courseId)
        {
            var count = await _additionalClassesApprovalRepository.Count(x => x.CourseId == courseId);
            return count;
        }

        public async Task<ResultModel<bool>> CloneClassFromExistingClass(int currentClassId, int cloneClassId, int userId)
        {
            var result = new ResultModel<bool>();
            var currentClass = await _classRepository.GetSingleByCondition(x => x.Id == currentClassId, new string[] { "Course", "Lessons", "Exams" });
            var listClassSetupStatusAvaiable = new List<int>() { (int)ClassApprovedStatus.Created, (int)ClassApprovedStatus.Cancel, (int)ClassApprovedStatus.FirstReject,
                (int)ClassApprovedStatus.SecondReject, (int)ClassApprovedStatus.ThirdReject};
            if (!listClassSetupStatusAvaiable.Contains(currentClass.Course.ClassApprovedStatus))
            {
                result.Message = "Class Setup was submitted";
                return result;
            }
            var cloneClass = await _classRepository.GetSingleByCondition(x => x.Id == cloneClassId, new string[] { "Lessons", "Exams" });

            result = CloneClass(currentClass, cloneClass, userId);

            return result;
        }

        private ResultModel<bool> CloneClass(Class currentClass, Class cloneClass, int userId)
        {
            var ressult = new ResultModel<bool>();
            try
            {
                currentClass.AttendanceRequirement = cloneClass.AttendanceRequirement;
                currentClass.CommencementDate = cloneClass.CommencementDate;
                currentClass.CompletionDate = cloneClass.CompletionDate;
                currentClass.Capacity = cloneClass.Capacity;
                currentClass.ClassCommonId = cloneClass.ClassCommonId;
                currentClass.CountReExam = cloneClass.CountReExam;
                //currentClass.Lessons = cloneClass.Lessons;
                //currentClass.Exams = cloneClass.Exams;

                _lessionRepository.DeleteMulti(x => x.ClassId == currentClass.Id);
                foreach (var lesson in cloneClass.Lessons)
                {
                    var newLesson = new Lesson();
                    newLesson.LastModifiedBy = userId;
                    newLesson.LastModifiedDate = DateTime.Now;
                    newLesson.No = lesson.No;
                    newLesson.TimeFromMin = lesson.TimeFromMin;
                    newLesson.TimeFromHrs = lesson.TimeFromHrs;
                    newLesson.TimeToMin = lesson.TimeToMin;
                    newLesson.TimeToHrs = lesson.TimeToHrs;
                    //newLesson.FromTime = lesson.FromTime;
                    //newLesson.ToTime = lesson.ToTime;
                    newLesson.Venue = lesson.Venue;
                    newLesson.LocationId = lesson.LocationId;
                    newLesson.Date = lesson.Date;
                    currentClass.Lessons.Add(newLesson);
                }
                foreach (var exam in currentClass.Exams.ToList())
                {
                    _examRepository.Delete(exam);
                }
                foreach (var exam in cloneClass.Exams)
                {
                    var newExam = new Exam();
                    newExam.ExamVenue = exam.ExamVenue;
                    newExam.Type = exam.Type;
                    newExam.Date = exam.Date;
                    newExam.FromTime = exam.FromTime;
                    newExam.ToTime = exam.ToTime;
                    newExam.ClassCommonId = exam.ClassCommonId;
                    newExam.ExamVenueText = exam.ExamVenueText;
                    newExam.IsReExam = exam.IsReExam;
                    //newExam.ModuleId = exam.ModuleId;
                    newExam.Dateline = exam.Dateline;
                    newExam.Marks = exam.Marks;
                    newExam.LastModifiedBy = userId;
                    newExam.LastModifiedDate = DateTime.Now;

                    currentClass.Exams.Add(newExam);
                }

                _classRepository.Update(currentClass);
                _unitOfWork.Commit();

                ressult.Message = "Success";
                ressult.IsSuccess = true;
                return ressult;
            }
            catch (Exception ex)
            {
                ressult.Message = "Failed to clone - " + ex.Message;
                return ressult;
            }
        }

        public async Task<IEnumerable<ClassAssessmentViewModel>> GetClassCodeAssessment(int courseId)
        {
            var result = (await _classRepository.GetMulti(x => x.Course.Id == courseId)).Select(x => new ClassAssessmentViewModel()
            {
                ClassId = x.Id,
                ClassCode = x.ClassCode
            });

            return result;
        }

        public async Task<IEnumerable<AssignLessonViewModel>> GetAssignLesson(string classCode)
        {
            var result = await _classRepository.GetSingleByCondition(x => x.ClassCode.Equals(classCode), new string[] { "Lessons" });
            return result.Lessons.Select(x => new AssignLessonViewModel()
            {
                Id = x.Id,
                Date = x.Date.ToString("dd/MM/yyyy"),
                ClassCode = result.ClassCode,
                TimeFromHrs = x.TimeFromHrs,
                TimeToHrs = x.TimeToHrs,
                TimeFromMin = x.TimeFromMin,
                TimeToMin = x.TimeToMin,
                Venue = x.Venue
            });
        }

        #endregion
    }
}
