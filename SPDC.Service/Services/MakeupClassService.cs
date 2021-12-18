using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels;
using SPDC.Model.BindingModels.MakeupClass;
using SPDC.Model.Models;
using SPDC.Model.ViewModels.MakeupClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPDC.Service.Services
{
    public interface IMakeupClassService
    {
        int Create(MakeupClassBindingModel model, IList<HttpPostedFile> files);

        Task<int> UpdateAsync(MakeupClassBindingModel model, IList<HttpPostedFile> files, int[] deleteFileIds);

        Task<List<MakeUpClass>> GetMakeUpClassesAsync(MakeupClassSearchModel model);

        Task<MakeUpClass> GetMakeUpClassesByIdAsync(int id);

        Task<bool> AssignStudentToMakeupClass(AssignToMakeupClassBindingModel models, int userId);

        Task<bool> AssignStudentsToMakeupLesson(AssignToMakeupLessonBindingModel models, int userId);

        Task<AttendanceContainerViewModel> SearchAllAttendance(AttendanceSearchBindingModel models, string apiPath);

        Task<bool> SaveAllAttendance(IEnumerable<AttendanceViewModel> models, HttpFileCollection files, int[] deleteFileIds);

        Task<bool> AssignStudentValidation(AssignToMakeupLessonBindingModel models);
    }
    public class MakeupClassService : IMakeupClassService
    {
        private IMakeupClassRepository _makeupClassRepository;
        private IMakeUpAttendenceRepository _makeUpAttendenceRepository;
        private ILessonAttendanceRepository _lessonAttendanceRepository;
        private IApplicationRepository _applicationRepository;
        private ICourseRepository _courseRepository;
        private IClassRepository _classRepository;
        private ILessionRepository _lessionRepository;
        private IDocumentRepository _documentRepository;
        private IUnitOfWork _unitOfWork;

        public MakeupClassService(IMakeupClassRepository makeupClassRepository, IMakeUpAttendenceRepository makeUpAttendenceRepository,
            IUnitOfWork unitOfWork, ILessonAttendanceRepository lessonAttendanceRepository, IApplicationRepository applicationRepository,
            ICourseRepository courseRepository, IClassRepository classRepository, ILessionRepository lessionRepository, IDocumentRepository documentRepository)
        {
            _makeupClassRepository = makeupClassRepository;
            _makeUpAttendenceRepository = makeUpAttendenceRepository;
            _lessonAttendanceRepository = lessonAttendanceRepository;
            _applicationRepository = applicationRepository;
            _courseRepository = courseRepository;
            _classRepository = classRepository;
            _lessionRepository = lessionRepository;
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
        }

        public int Create(MakeupClassBindingModel model, IList<HttpPostedFile> files)
        {
            var result = _makeupClassRepository.Add(new MakeUpClass()
            {
                Date = model.Date,
                Name = model.Name,
                TimeFromHrs = model.TimeFromHrs,
                TimeFromMin = model.TimeFromMin,
                TimeToHrs = model.TimeToHrs,
                TimeToMin = model.TimeToMin,
                Venue = model.Venue,
                Documents = new List<Document>()
            });

            _unitOfWork.Commit();

            if (result.Id == 0)
            {
                return result.Id;
            }

            for (int i = 0; i < files.Count; i++)
            {
                var directory = "~\\FileUpload\\Course\\MakeupClass\\";
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var pathDirectory = serPath + result.Id + "_" + result.Name;

                if (!Directory.Exists(pathDirectory))
                {
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                }

                var pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + files[i].FileName);
                var pathToSaveDB = directory + "\\" + result.Id + "_" + result.Name + "\\" + Path.GetFileName(files[i].FileName);
                files[i].SaveAs(pathFile);

                Document doc = new Document()
                {
                    Url = pathToSaveDB,
                    ContentType = files[i].ContentType,
                    FileName = files[i].FileName,
                    ModifiedDate = DateTime.Now
                };

                result.Documents.Add(doc);
            }

            _makeupClassRepository.Update(result);

            _unitOfWork.Commit();

            return result.Id;
        }

        public async Task<int> UpdateAsync(MakeupClassBindingModel model, IList<HttpPostedFile> files, int[] deleteFileIds)
        {
            MakeUpClass record = await _makeupClassRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Documents" });

            record.Date = model.Date;
            record.Name = model.Name;
            record.TimeFromHrs = model.TimeFromHrs;
            record.TimeFromMin = model.TimeFromMin;
            record.TimeToHrs = model.TimeToHrs;
            record.TimeToMin = model.TimeToMin;
            record.Venue = model.Venue;

            record.Documents = record.Documents.Where(x => !deleteFileIds.Contains(x.Id)).ToList();

            for (int i = 0; i < files.Count; i++)
            {
                var directory = "~\\FileUpload\\Course\\MakeupClass\\";
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                var pathDirectory = serPath + record.Id + "_" + record.Name;

                if (!Directory.Exists(pathDirectory))
                {
                    Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                }

                var pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + files[i].FileName);
                var pathToSaveDB = directory + "\\" + record.Id + "_" + record.Name + "\\" + Path.GetFileName(files[i].FileName);
                files[i].SaveAs(pathFile);

                Document doc = new Document()
                {
                    Url = pathToSaveDB,
                    ContentType = files[i].ContentType,
                    FileName = files[i].FileName,
                    ModifiedDate = DateTime.Now
                };

                record.Documents.Add(doc);
            }

            _makeupClassRepository.Update(record);
            _documentRepository.DeleteMulti(x => deleteFileIds.Contains(x.Id));
            _unitOfWork.Commit();

            return record.Id;
        }

        public async Task<List<MakeUpClass>> GetMakeUpClassesAsync(MakeupClassSearchModel model)
        {
            List<MakeUpClass> lst = await _makeupClassRepository.GetMulti(x => x.Name.Contains(model.Name)
                                                                            && x.Venue.Contains(model.Venue)
                                                                            && (!model.DateFrom.HasValue || model.DateFrom.HasValue && x.Date >= model.DateFrom)
                                                                            && (!model.DateTo.HasValue || model.DateTo.HasValue && x.Date <= model.DateTo.Value)
                                                                            , new string[] { "Documents" });
            return lst;
        }

        public async Task<MakeUpClass> GetMakeUpClassesByIdAsync(int id)
        {
            MakeUpClass model = await _makeupClassRepository.GetSingleByCondition(x => x.Id == id, new string[] { "Documents"
                , "MakeUpAttendences"
                , "MakeUpAttendences.Application"
                , "MakeUpAttendences.Application.User.Particular"
                , "MakeUpAttendences.Lesson"
                , "MakeUpAttendences.Lesson.Class" });
            return model;
        }

        public async Task<bool> AssignStudentToMakeupClass(AssignToMakeupClassBindingModel models, int userId)
        {
            MakeUpClass makeUpClass = await _makeupClassRepository.GetSingleByCondition(x => x.Id == models.MakeupClassId, new string[] { "MakeUpAttendences", "MakeUpAttendences.Application", "MakeUpAttendences.Application.EnrollmentStatusStorages" });
            var dbResordIds = makeUpClass.MakeUpAttendences.Select(x => x.Id);

            int[] updateIds = models.Data.Where(x => x.Id != 0).Select(x => x.Id).ToArray();
            int[] deleteIds = dbResordIds.Where(x => !updateIds.Contains(x)).ToArray();

            foreach (var item in models.Data)
            {
                var fromLesson = await _lessonAttendanceRepository.GetSingleByCondition(x => x.Id == item.FromLessonId, new string[] { "Lesson", "Lesson.Class" });
                if (item.Id != 0)
                {
                    var updateItem = makeUpClass.MakeUpAttendences.SingleOrDefault();
                    if (updateItem == null)
                    {
                        return false;
                    }
                    updateItem.ApplicationId = item.AppId;
                    updateItem.IsDisplayToStudentPortal = item.IsDiplayOnStudentPortal;
                    updateItem.LessonId = fromLesson.LessonId;
                    updateItem.MakeUpClassId = models.MakeupClassId;
                    updateItem.Application.RemarksAttendance = $"Student re-sit lesson from {fromLesson.Lesson.Class.ClassCode} {fromLesson.Lesson.Date:dd/MM/yyyy} ({fromLesson.Lesson.TimeFromHrs}:{fromLesson.Lesson.TimeFromMin} - {fromLesson.Lesson.TimeToHrs}:{fromLesson.Lesson.TimeToMin}) to {makeUpClass.Name} {makeUpClass.Date:dd/MM/yyyy} ({makeUpClass.TimeFromHrs}:{makeUpClass.TimeFromMin} - {makeUpClass.TimeToHrs}:{makeUpClass.TimeToMin})";
                    updateItem.Application.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                    {
                        ApplicationId = item.AppId,
                        Status = (int)Common.Enums.EnrollmentStatus.Resit,
                        LastModifiedDate = DateTime.Now,
                        LastModifiedBy = userId
                    });
                }
                else
                {
                    Application application = await _applicationRepository.GetSingleByCondition(x=>x.Id == item.AppId, new string[] { "EnrollmentStatusStorages" });
                    MakeUpAttendence muAttendance = new MakeUpAttendence();
                    //muAttendance.ApplicationId = item.AppId;
                    muAttendance.IsDisplayToStudentPortal = item.IsDiplayOnStudentPortal;
                    muAttendance.IsTakeAttendance = true;
                    muAttendance.LessonId = fromLesson.LessonId;
                    muAttendance.MakeUpClassId = models.MakeupClassId;
                    application.RemarksAttendance = $"Student re-sit lesson from {fromLesson.Lesson.Class.ClassCode} {fromLesson.Lesson.Date:dd/MM/yyyy} ({fromLesson.Lesson.TimeFromHrs}:{fromLesson.Lesson.TimeFromMin} - {fromLesson.Lesson.TimeToHrs}:{fromLesson.Lesson.TimeToMin}) to {makeUpClass.Name} {makeUpClass.Date:dd/MM/yyyy} ({makeUpClass.TimeFromHrs}:{makeUpClass.TimeFromMin} - {makeUpClass.TimeToHrs}:{makeUpClass.TimeToMin})";
                    application.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                    {
                        ApplicationId = item.AppId,
                        Status = (int)Common.Enums.EnrollmentStatus.Resit,
                        LastModifiedDate = DateTime.Now,
                        LastModifiedBy = userId
                    });
                    muAttendance.Application = application;
                    makeUpClass.MakeUpAttendences.Add(muAttendance);
                    
                }
            }

            _makeupClassRepository.Update(makeUpClass);
            _makeUpAttendenceRepository.DeleteMulti(x => deleteIds.Contains(x.Id));

            _unitOfWork.Commit();

            return true;
        }

        public async Task<bool> AssignStudentsToMakeupLesson(AssignToMakeupLessonBindingModel models, int userId)
        {
            foreach (var record in models.Data)
            {
                var fromLes = await _lessionRepository.GetSingleByCondition(x => x.Id == record.FromLessonId, new string[] { "Class" });
                var toLes = await _lessionRepository.GetSingleByCondition(x => x.Id == record.LessonId, new string[] { "Class" });
                foreach (var id in models.AppIds)
                {
                    if (!(await _lessonAttendanceRepository.CheckContains(x => x.ApplicationId == id && x.LessonId == record.LessonId)))
                    {
                        var app = await _applicationRepository.GetSingleByCondition(x => x.Id == id);
                        app.RemarksAttendance = $"Student re-sit lesson from {fromLes.Class.ClassCode} {fromLes.Date:dd/MM/yyyy} ({fromLes.TimeFromHrs}:{fromLes.TimeFromMin} - {fromLes.TimeToHrs}:{fromLes.TimeToMin}) to {toLes.Class.ClassCode} {toLes.Date:dd/MM/yyyy} ({toLes.TimeFromHrs}:{toLes.TimeFromMin} - {toLes.TimeToHrs}:{toLes.TimeToMin})";

                        app.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                        {
                            ApplicationId = app.Id,
                            Status = (int)Common.Enums.EnrollmentStatus.Resit,
                            LastModifiedDate = DateTime.Now,
                            LastModifiedBy = userId
                        });
                        _applicationRepository.Update(app);

                        _lessonAttendanceRepository.Add(new LessonAttendance()
                        {
                            LessonId = record.LessonId,
                            FromLessonId = record.FromLessonId,
                            ApplicationId = id,
                            IsMakeUp = true,
                            IsTakeAttendance = true,
                            UserId = app.UserId
                        });
                    }

                }
            }

            _unitOfWork.Commit();
            return true;
        }

        public async Task<bool> AssignStudentValidation(AssignToMakeupLessonBindingModel models)
        {

            foreach (var id in models.AppIds)
            {
                foreach (var item in models.Data)
                {
                    if (await _lessonAttendanceRepository.CheckContains(x => x.ApplicationId == id && x.LessonId == item.LessonId))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<AttendanceContainerViewModel> SearchAllAttendance(AttendanceSearchBindingModel models, string apiPath)
        {
            AttendanceContainerViewModel rtnModel = new AttendanceContainerViewModel();

            var result = _applicationRepository.SearchAllAttendance(models);

            Class classModel = await _classRepository.GetSingleByCondition(x => x.Id == models.ClassId, new string[] { "Lessons", "Exams" });

            rtnModel.IsEnableEligibleForExam = classModel.Exams != null && classModel.Exams.Count > 0;

            // Collect lessons

            rtnModel.Lessons = new List<AttendanceLessonviewModel>();
            rtnModel.Lessons.AddRange(result.SelectMany(x => x.LessonAttendances).Select(y => y.Lesson).Select(x => new AttendanceLessonviewModel()
            {
                Id = x.Id,
                Date = x.Date.ToString("dd/MM/yyyy"),
                FromHrs = x.TimeFromHrs,
                FromMin = x.TimeFromMin,
                ToHrs = x.TimeToHrs,
                ToMin = x.TimeToMin,
                IsMakeupClass = false
            }));
            rtnModel.Lessons.AddRange(result.SelectMany(x => x.MakeUpAttendences).Select(y => y.MakeUpClass).Select(x => new AttendanceLessonviewModel()
            {
                Id = x.Id,
                Date = x.Date.ToString("dd/MM/yyyy"),
                FromHrs = x.TimeFromHrs,
                FromMin = x.TimeFromMin,
                ToHrs = x.TimeToHrs,
                ToMin = x.TimeToMin,
                IsMakeupClass = true
            }));

            rtnModel.Lessons = rtnModel.Lessons.OrderBy(x => x.Date).ThenBy(x => x.FromHrs).ThenBy(x => x.FromMin).ToList();

            rtnModel.AttendanceRequired = result.OrderBy(x => x.AdminAssignedClassModel.AttendanceRequirement).Select(x => x.AdminAssignedClassModel.AttendanceRequirement).Distinct().FirstOrDefault();

            rtnModel.Data = result.Select(x => new AttendanceViewModel()
            {
                ApplicationId = x.Id,
                StudentNo = x.ApplicationNumber,
                SurnameCN = x.User.Particular.SurnameCN,
                GivenNameCN = x.User.Particular.GivenNameCN,
                SurnameEN = x.User.Particular.SurnameEN,
                GivenNameEN = x.User.Particular.GivenNameEN,
                CICNo = x.User.CICNumber,
                AttendanceHrs = x.AdminAssignedClassModel.ClassCommonId.HasValue ? (x.AdminAssignedClassModel.ClassCommonId
                                                                                    == (int)AttendanceRequirementType.Hrs ? (x.AttendanceMarks.HasValue ? x.AttendanceMarks : 0) : null) : null,
                AttendanceLesson = x.AdminAssignedClassModel.ClassCommonId.HasValue ? (x.AdminAssignedClassModel.ClassCommonId
                                                                                    == (int)AttendanceRequirementType.Lesson ? (x.AttendanceMarks.HasValue ? x.AttendanceMarks : 0) : null) : null,
                AttendancePercent = x.AdminAssignedClassModel.ClassCommonId.HasValue ? (x.AdminAssignedClassModel.ClassCommonId
                                                                                    == (int)AttendanceRequirementType.Percent ? (x.AttendanceMarks.HasValue ? x.AttendanceMarks : 0) : null) : null,
                EligibleForExam = x.EligibleForExam,
                EligibleForMakeupClass = x.EligibleForMakeUpClass,
                Remarks = x.RemarksAttendance,
                AttendanceData = GetAttendanceData(x, rtnModel.Lessons),
                //x.LessonAttendances.Select(
                //la => new LessonAttendanceViewModel()
                //{
                //    Id = la.Id,
                //    LessonId = la.Lesson.Id,
                //    IsTakeAttendance = la.IsTakeAttendance,
                //    IsMakeUp = la.IsMakeUp,
                //    IsMakeupClass = false
                //}).Concat(x.MakeUpAttendences.Select(mua => new LessonAttendanceViewModel()
                //{
                //    Id = mua.Id,
                //    LessonId = mua.MakeUpClass.Id,
                //    IsTakeAttendance = mua.IsTakeAttendance,
                //    IsMakeUp = true,
                //    IsMakeupClass = true
                //})).ToList(),
                Docs = x.ApplicationAttendanceDocuments.Select(y => new AttendanceDocViewModel()
                {
                    Id = y.Id,
                    ContentType = y.ContentType,
                    Name = y.FileName,
                    Url = $"{ConfigHelper.GetByKey("DMZAPI")}api/Proxy?url=ApplicationManagement/DownloadMakeupClassDoc/{y.Id}"
                })
            });

            return rtnModel;
        }

        private List<LessonAttendanceViewModel> GetAttendanceData(Application app, List<AttendanceLessonviewModel> lessons)
        {
            var attendanceData = new List<LessonAttendanceViewModel>();

            foreach (var les in lessons)
            {
                if (les.IsMakeupClass)
                {
                    var attendance = app.MakeUpAttendences.SingleOrDefault(x => x.MakeUpClassId == les.Id);
                    if (attendance != null)
                    {
                        attendanceData.Add(new LessonAttendanceViewModel()
                        {
                            Id = attendance.Id,
                            LessonId = attendance.MakeUpClassId,
                            IsTakeAttendance = attendance.IsTakeAttendance,
                            IsMakeUp = true
                        });
                    }
                    else
                    {
                        attendanceData.Add(new LessonAttendanceViewModel()
                        {
                            Id = 0,
                            IsMakeUp = null,
                            IsTakeAttendance = null,
                            LessonId = null
                        });
                    }

                }
                else
                {
                    var attendance = app.LessonAttendances.SingleOrDefault(x => x.LessonId == les.Id);
                    if (attendance != null)
                    {
                        attendanceData.Add(new LessonAttendanceViewModel()
                        {
                            Id = attendance.Id,
                            LessonId = attendance.LessonId,
                            IsTakeAttendance = attendance.IsTakeAttendance,
                            IsMakeUp = attendance.IsMakeUp
                        });
                    }
                    else
                    {
                        attendanceData.Add(new LessonAttendanceViewModel()
                        {
                            Id = 0,
                            IsMakeUp = null,
                            IsTakeAttendance = null,
                            LessonId = null
                        });
                    }
                }
            }

            return attendanceData;
        }

        public async Task<bool> SaveAllAttendance(IEnumerable<AttendanceViewModel> models, HttpFileCollection files, int[] deleteFileIds)
        {
            foreach (var model in models)
            {
                IList<HttpPostedFile> modelFiles = files.GetMultiple(model.StudentNo);

                Application app = await _applicationRepository.GetSingleByCondition(x => x.Id == model.ApplicationId, new string[] { "LessonAttendances", "ApplicationAttendanceDocuments" });

                foreach (var la in app.LessonAttendances)
                {
                    la.IsTakeAttendance = (bool)model.AttendanceData.SingleOrDefault(x => x.Id == la.Id).IsTakeAttendance;
                }

                if (model.AttendancePercent.HasValue)
                {
                    app.AttendanceMarks = model.AttendancePercent;
                }
                if (model.AttendanceHrs.HasValue)
                {
                    app.AttendanceMarks = model.AttendanceHrs;
                }
                if (model.AttendanceLesson.HasValue)
                {
                    app.AttendanceMarks = model.AttendanceLesson;
                }

                app.EligibleForExam = model.EligibleForExam;
                app.EligibleForMakeUpClass = model.EligibleForMakeupClass;
                app.RemarksAttendance = model.Remarks;

                if (app.ApplicationAttendanceDocuments.Count > 0)
                {
                    app.ApplicationAttendanceDocuments = app.ApplicationAttendanceDocuments.Where(x => !deleteFileIds.Contains(x.Id)).ToList();
                }

                for (int i = 0; i < modelFiles.Count; i++)
                {
                    //var directory = "~\\FileUpload\\Applications\\Attendance\\";
                    var directory = ConfigHelper.GetByKey("ApplicationDocumentDirectory");
                    var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
                    var pathDirectory = serPath + ApplicationTypeDocument.AssessmentDocument.ToString() + "\\" + app.Id + "_" + app.ApplicationNumber;

                    if (!Directory.Exists(pathDirectory))
                    {
                        Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                    }

                    var pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + files[i].FileName);
                    var pathToSaveDB = directory + ApplicationTypeDocument.AssessmentDocument.ToString() + "\\" + app.Id + "_" + app.ApplicationNumber + "\\" + Path.GetFileName(files[i].FileName);
                    files[i].SaveAs(pathFile);

                    Document doc = new Document()
                    {
                        Url = pathToSaveDB,
                        ContentType = files[i].ContentType,
                        FileName = files[i].FileName,
                        ModifiedDate = DateTime.Now
                    };
                    app.ApplicationAttendanceDocuments.Add(doc);
                    _applicationRepository.Update(app);
                }
            }

            _documentRepository.DeleteMulti(x => deleteFileIds.Contains(x.Id));
            _unitOfWork.Commit();
            return true;
        }
    }
}
