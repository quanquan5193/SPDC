using Newtonsoft.Json;
using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels;
using SPDC.Model.BindingModels.Assessment;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.Application;
using SPDC.Model.ViewModels.Assessment;
using SPDC.Model.ViewModels.MakeupClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using static SPDC.Common.StaticConfig;

namespace SPDC.Service.Services
{
    public interface ICourseService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Return null if Resource Code is existed</returns>
        Task<Course> CreateAsync(CreateCourseBindingModel model);

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Return null if Resource Code is existed</returns>
        Task<Course> UpdateAsync(CreateCourseBindingModel model);

        Course GetCourseById(int id);

        Task<List<Course>> GetCourses();

        CourseCategory AddCategory(CourseCategory category);

        Task<IEnumerable<CourseCategory>> GetCategoriesAsync(int langId);

        Task<ResultModel<Course>> UpdateFeeAsync(CourseFeeBindingModel model);

        ResultModel<Course> UpdateAllowance(CourseAllowanceBindingModel model);

        Task<ResultModel<Course>> UpdateProgrammeLeadershipAsync(CourseProgrammeLeadershipBindingModel model);

        Task<IEnumerable<Course>> GetCourseInformation(int langId, IEnumerable<int> coursecategories, int coursemode, int courselocations, string search, int index, string sortBy, bool isDescending, int size);

        Task<IEnumerable<CourseLocation>> GetCourseLocation(int id);

        Task<IEnumerable<CourseType>> GetCourseMode();

        Task<int> Count();

        Task<int> CountTotalSearch(int langId, IEnumerable<int> coursecategories, int coursemode, int courselocations, string search);

        Task<ResultModel<Course>> UpdateCurriculumAsync(CurriculumBindingModel model);

        Task<ResultModel<Course>> UpdateRecognitionAsync(RecognitionBindingModel model);

        Task<ResultModel<Course>> UpdateAdmissionRequirementsAsync(AdmissionRequirementsBindingModel model);

        Task<ResultModel<Course>> UpdateCertificateConditionsAsync(CertificateConditionsBindingModel model);

        Task<ResultModel<Course>> UpdateEnquirysAsync(EnquirysBindingModel model);

        Task<ResultModel<CourseReExaminationBindingModel>> GetReExamination(int courseId);

        Task<ResultModel<bool>> UpdateReExamination(CourseReExaminationBindingModel model);

        Task<ResultModel<List<KeywordBindingModel>>> GetCourseKeyword(int courseId);

        Task<ResultModel<bool>> UpdateCourseKeyword(List<KeywordBindingModel> lstKeyword, int cId);

        Task<Course> GetCourseInformationById(int id);

        Task<string> GetLocationByID(int langid, int locationId);

        Task<string> GetUserNameByID(int langId, int userId);

        IEnumerable<CoursePortalAdminViewModel> GetCourseInformationByAdminPortal(int langId, IEnumerable<int> coursecategories, string coursecode, string coursenameEN, string coursenameCN, bool? displaycourse, int index, string sortBy, bool isDescending, int size, out int count);

        Task<string> GetlanguageByID(int langId);

        //Class CreateClassAdmin(CreateClassAdminBindingModel model);
        Task<ResultModel<CourseDocumentViewModel>> GetCourseDocumentation(int courseId);

        Task<ResultModel<bool>> UpdateCourseDocumentation(int courseId, List<int> lstCourseDelete, HttpFileCollection lstFileUpdate, string courseCode);

        Task<ResultModel<FileReturnViewModel>> DownloadCourseDocument(int documentId);

        Task<Course> GetCourseApplicationForm(int id);
        Task<bool> ShowReExam();

        Task<List<Course>> CloneDataToElastic();

        Task<IList<Class>> GetCourseCalendar(int id);

        Task<IEnumerable<Model.ViewModels.Application.CourseCodeViewModel>> GetCourseCodeByFilter(CourseCodeFilter filter);
        Task<IEnumerable<Model.ViewModels.Application.CourseCodeViewModel>> GetCourseCodeByText(string key);
        Task<IEnumerable<string>> GetCourseNameByString(CourseAssessmentFilter filter);
        Task<IEnumerable<CourseAssessmentViewModel>> GetCourseCodeAssessment(CourseAssessmentFilter filter);

        Task<IEnumerable<ClassCodeViewModel>> GetClassCodeByCourseCode(string coursecode);
        Task<bool> CheckCourseHasExamination(int courseId);

        Task<bool> IsCourseCodeExisted(string courseCode, int courseId = 0);
        Task<bool> CanUserApplyCourse(int courseId, int userId);
    }

    public class CourseService : ICourseService
    {
        private IUnitOfWork _unitOfWork;
        private ICourseRepository _repository;
        private ICourseCategoryRepository _categoryRepository;
        private IModuleTranRepository _moduleTranRepository;
        private IModuleRepository _moduleRepository;
        private ICombinationRepository _combinationRepository;
        private IEnquiryRepository _enquiryRepository;
        private ICourserTypeRepository _coursetypeRepository;
        private ICourseLocationRepository _courselocationRepository;
        private IKeywordRepository _keywordRepository;
        private ICourseLocationTransRepository _courselocationTransRepository;
        private IParticularRepository _particularRepository;
        private IDocumentRepository _documentRepository;
        private ICourseDocumentsRepository _courseDocumentsRepository;
        private ICourseTransRepository _courseTransRepository;
        private ILanguageRepository _languageRepository;
        private IClassRepository _classRepository;
        private IApplicationRepository _applicationRepository;

        public CourseService(IUnitOfWork unitOfWork,
            ICourseRepository repository,
            ICourseCategoryRepository categoryRepository,
            ICourserTypeRepository coursetypeRepository,
            ICourseLocationRepository courselocationRepository,
            IKeywordRepository keywordRepository,
            IModuleTranRepository moduleTranRepository, IModuleRepository moduleRepository, ICombinationRepository combinationRepository,
            IEnquiryRepository enquiryRepository, ICourseLocationTransRepository courselocationTransRepository, IParticularRepository particularRepository, ICourseTransRepository courseTransRepository, ILanguageRepository languageRepository,
            IDocumentRepository documentRepository, ICourseDocumentsRepository courseDocumentsRepository,
            IClassRepository classRepository, IApplicationRepository applicationRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _categoryRepository = categoryRepository;

            _coursetypeRepository = coursetypeRepository;
            _courselocationRepository = courselocationRepository;
            _keywordRepository = keywordRepository;
            _moduleTranRepository = moduleTranRepository;
            _moduleRepository = moduleRepository;
            _combinationRepository = combinationRepository;
            _enquiryRepository = enquiryRepository;
            _courselocationTransRepository = courselocationTransRepository;
            _particularRepository = particularRepository;
            _courseTransRepository = courseTransRepository;
            _languageRepository = languageRepository;
            _documentRepository = documentRepository;
            _courseDocumentsRepository = courseDocumentsRepository;
            _classRepository = classRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task<Course> CreateAsync(CreateCourseBindingModel model)
        {
            if (await IsCourseCodeExisted(model.CourseCode))
                return null;

            Course course = new Course()
            {
                CategoryId = model.CourseCategory,
                CourseCode = model.CourseCode,
                TargetClassSize = model.TargetClassSize,
                CourseTypeId = model.Mode,
                DurationHrs = model.DurationHrs,
                DurationTotal = model.DurationTotal,
                DurationLesson = model.Lessons,
                Credits = model.Credits,
                CourseVenueId = model.Venue,
                LevelOfApprovalId = model.LevelOfApproval,
                InvisibleOnWebsite = model.Display,
                Status = (int)CommonStatus.Active,
                CourseTrans = new List<CourseTran>(),

                ObjectiveEN = model.ObjectiveEN,
                ObjectiveSC = model.ObjectiveSC,
                ObjectiveTC = model.ObjectiveTC,
                WaitingTimeEN = model.WaitingTimeEN,
                WaitingTimeSC = model.WaitingTimeSC,
                WaitingTimeTC = model.WaitingTimeTC
            };

            var result = _repository.Add(course);
            _unitOfWork.Commit();

            course.CourseTrans.Add(new CourseTran()
            {
                LanguageId = 1,
                CourseId = course.Id,
                CourseName = model.CourseNameEN,
                CourseTitle = model.CourseNameEN
            });
            course.CourseTrans.Add(new CourseTran()
            {
                LanguageId = 2,
                CourseId = course.Id,
                CourseName = model.CourseNameCN,
                CourseTitle = model.CourseNameCN
            });
            course.CourseTrans.Add(new CourseTran()
            {
                LanguageId = 3,
                CourseId = course.Id,
                CourseName = model.CourseNameHK,
                CourseTitle = model.CourseNameHK
            });

            _repository.Update(course);

            _unitOfWork.Commit();
            return result;
        }

        public Course GetCourseById(int id)
        {
            var coures = _repository.GetSingleById(id);
            return coures;
        }

        public async Task<List<Course>> GetCourses()
        {
            var courses = await _repository.GetMulti(x => true, "Id", false, new string[] { "CourseTrans", "TargetClasses" });
            return (List<Course>)courses;
        }

        public CourseCategory AddCategory(CourseCategory category)
        {
            var result = _categoryRepository.Add(category);
            _unitOfWork.Commit();
            return result;
        }

        public async Task<IEnumerable<CourseCategory>> GetCategoriesAsync(int langId)
        {
            var result = (await _categoryRepository.GetMulti(x => x.Status == (int)CommonStatus.Active, new string[] { "CourseCategorieTrans" })).Select(
                i => new CourseCategory()
                {
                    Id = i.Id,
                    Status = i.Status,
                    CourseCategorieTrans = i.CourseCategorieTrans.Where(cct => cct.LanguageId == langId).ToList()
                }).ToArray();

            return result;
        }

        public async Task<ResultModel<Course>> UpdateFeeAsync(CourseFeeBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Modules", "Modules.ModuleTrans", "ModuleCombinations" });

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }

            course.ByModule = model.ByModule;

            if (!model.ByModule)
            {
                course.FeeRemarks1 = model.FeeRemarks1;
                course.FeeRemarks2 = model.FeeRemarks2;
                course.CourseFee = model.CourseFee;
                course.DiscountFee = model.Discounted;
            }
            else
            {

                // Storage Modules
                if (course.Modules.Count >= model.Modules.Count())
                {
                    List<Module> delItems = new List<Module>();
                    foreach (var module in course.Modules)
                    {
                        var updateItem = model.Modules.SingleOrDefault(x => x.ModuleNo == module.ModuleNo);
                        if (updateItem != null)
                        {
                            module.Fee = updateItem.Fee;
                            foreach (var mt in module.ModuleTrans)
                            {
                                if (mt.LanguageId == 1)
                                    mt.ModuleName = updateItem.ModuleNameEN;
                                if (mt.LanguageId == 2)
                                    mt.ModuleName = updateItem.ModuleNameCN;
                                if (mt.LanguageId == 3)
                                    mt.ModuleName = updateItem.ModuleNameHK;
                            }
                        }
                        else
                        {
                            delItems.Add(module);
                        }
                    }
                    foreach (var delItem in delItems)
                    {
                        _moduleTranRepository.DeleteMulti(x => x.ModuleId == delItem.Id);
                        _moduleRepository.Delete(delItem);
                    }
                }
                else
                {
                    foreach (var updateModel in model.Modules)
                    {
                        var dbItem = course.Modules.SingleOrDefault(x => x.ModuleNo == updateModel.ModuleNo);
                        if (dbItem != null)
                        {
                            dbItem.Fee = updateModel.Fee;
                            foreach (var mt in dbItem.ModuleTrans)
                            {
                                if (mt.LanguageId == 1)
                                    mt.ModuleName = updateModel.ModuleNameEN;
                                if (mt.LanguageId == 2)
                                    mt.ModuleName = updateModel.ModuleNameCN;
                                if (mt.LanguageId == 3)
                                    mt.ModuleName = updateModel.ModuleNameHK;
                            }
                        }
                        else
                        {
                            course.Modules.Add(
                            new Module()
                            {
                                ModuleNo = updateModel.ModuleNo,
                                Fee = updateModel.Fee,
                                ModuleTrans = new List<ModuleTran>()
                                {
                                new ModuleTran()
                                {
                                    LanguageId = 1,
                                    ModuleName = updateModel.ModuleNameEN
                                },
                                new ModuleTran()
                                {
                                    LanguageId = 2,
                                    ModuleName = updateModel.ModuleNameCN
                                },
                                new ModuleTran()
                                {
                                    LanguageId = 3,
                                    ModuleName = updateModel.ModuleNameHK
                                }
                                }
                            });
                        }
                    }
                }

                // Storage combination 
                if (course.ModuleCombinations.Count >= model.Combinations.Count())
                {
                    List<ModuleCombination> delItems = new List<ModuleCombination>();
                    foreach (var combination in course.ModuleCombinations)
                    {
                        var updateItem = model.Combinations.SingleOrDefault(x => x.CombinationNo == combination.CombinationNo);
                        if (updateItem != null)
                        {
                            combination.ModuleNos = JsonConvert.SerializeObject(updateItem.Modules);
                            combination.CombinationNo = updateItem.CombinationNo;
                            combination.CourseFee = updateItem.CombinationFee;
                        }
                        else
                        {
                            delItems.Add(combination);
                        }
                    }
                    foreach (var delItem in delItems)
                    {
                        _combinationRepository.Delete(delItem);
                    }
                }
                else
                {
                    foreach (var updateModel in model.Combinations)
                    {
                        var dbItem = course.ModuleCombinations.SingleOrDefault(x => x.CombinationNo == updateModel.CombinationNo);
                        if (dbItem != null)
                        {
                            dbItem.CourseId = course.Id;
                            dbItem.ModuleNos = JsonConvert.SerializeObject(updateModel.Modules);
                            dbItem.CombinationNo = updateModel.CombinationNo;
                            dbItem.CourseFee = updateModel.CombinationFee;
                        }
                        else
                        {
                            course.ModuleCombinations.Add(new ModuleCombination()
                            {
                                CombinationNo = updateModel.CombinationNo,
                                CourseId = course.Id,
                                CourseFee = updateModel.CombinationFee,
                                ModuleNos = JsonConvert.SerializeObject(updateModel.Modules)
                            });

                        }
                    }
                }
            }

            _repository.Update(course);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<IEnumerable<Course>> GetCourseInformation(int langId, IEnumerable<int> coursecategories, int coursemode, int courselocations, string search, int index, string sortBy, bool isDescending, int size)
        {

            List<string> lstKeyword = new List<string>();

            if (!string.IsNullOrEmpty(search))
            {
                lstKeyword = (await _keywordRepository.GetMulti(x => x.WordCN.Contains(search) || x.WordEN.Contains(search) || x.WordHK.Contains(search))).Select(x => x.CourseId.ToString()).ToList();
            }

            var resultcourselocation = (await _repository.GetMultiPaging(
                x => (coursemode != 0 ? x.CourseTypeId == coursemode : true)
                && (courselocations != 0 ? x.CourseVenueId == courselocations : true)
                && (coursecategories.Count() > 0 ? coursecategories.Contains(x.CategoryId) : true)
                && (!string.IsNullOrEmpty(search) ? (lstKeyword.Contains(x.Id.ToString())) : true)
                && x.InvisibleOnWebsite == true && x.CourseApprovedStatus == (int)ClassApprovedStatus.ThirdApproved,
                (sortBy != "CourseCategories" && sortBy != "ModeOfStudy" && sortBy != "CourseName") ? sortBy : "Id",
                isDescending,
                index,
                size,
                new string[] { "CourseType", "CourseTrans", "CourseCategory.CourseCategorieTrans" })).Select(
                 i => new Course()
                 {
                     Id = i.Id,
                     CourseType = i.CourseType,
                     CourseCode = i.CourseCode,
                     CourseFee = i.CourseFee,
                     CourseTrans = i.CourseTrans.Where(cct => cct.LanguageId == langId).ToList(),
                     DurationTotal = i.DurationTotal,
                     CourseCategory = i.CourseCategory,
                 });

            return resultcourselocation;
        }

        public async Task<IEnumerable<CourseLocation>> GetCourseLocation(int langid)
        {
            var resultcourselocation = (await _courselocationRepository.GetMulti(x => x.Status == (int)CommonStatus.Active, new string[] { "CourseLocationTrans" })).Select(
                 i => new CourseLocation()
                 {
                     Id = i.Id,
                     Status = i.Status,
                     CourseLocationTrans = i.CourseLocationTrans.Where(cct => cct.LanguageId == langid).ToList()
                 });

            return resultcourselocation;
        }

        public async Task<int> Count()
        {
            var c = await _repository.Count(x => true);
            return c;
        }

        public async Task<IEnumerable<CourseType>> GetCourseMode()
        {
            var result = await _coursetypeRepository.GetAll();

            return result;
        }

        public ResultModel<Course> UpdateAllowance(CourseAllowanceBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = _repository.GetSingleById(model.Id);

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }

            course.Allowance = model.Allowance;
            course.IsAllowanceProvided = model.IsAllowanceProvided;

            _repository.Update(course);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<ResultModel<Course>> UpdateProgrammeLeadershipAsync(CourseProgrammeLeadershipBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id);

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }
            if (model.ProgrammeLeaderId > 0)
            {
                course.ProgrammeLeaderId = model.ProgrammeLeaderId;
            }
            if (model.MediumOfInstruction > 0)
            {
                course.MediumOfInstructionId = model.MediumOfInstruction;
            }
            if (model.Lecturer > 0)
            {
                course.LecturerId = model.Lecturer;
            }

            _repository.Update(course);
            _unitOfWork.Commit();

            var course1 = await _repository.GetSingleByCondition(x => x.Id == model.Id);

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<ResultModel<Course>> UpdateCurriculumAsync(CurriculumBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "CourseTrans" });

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }

            foreach (var ct in course.CourseTrans)
            {
                if (ct.LanguageId == 1)
                    ct.Curriculum = model.CurriculumEN;
                if (ct.LanguageId == 2)
                    ct.Curriculum = model.CurriculumCN;
                if (ct.LanguageId == 3)
                    ct.Curriculum = model.CurriculumHK;
            }

            _repository.Update(course);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<ResultModel<Course>> UpdateRecognitionAsync(RecognitionBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "CourseTrans" });

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }

            foreach (var ct in course.CourseTrans)
            {
                if (ct.LanguageId == 1)
                    ct.Recognition = model.RecognitionEN;
                if (ct.LanguageId == 2)
                    ct.Recognition = model.RecognitionCN;
                if (ct.LanguageId == 3)
                    ct.Recognition = model.RecognitionHK;
            }

            _repository.Update(course);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<ResultModel<Course>> UpdateAdmissionRequirementsAsync(AdmissionRequirementsBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "CourseTrans" });

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }

            foreach (var ct in course.CourseTrans)
            {
                if (ct.LanguageId == 1)
                    ct.AdmissionRequirements = model.AdmissionRequirementsEN;
                if (ct.LanguageId == 2)
                    ct.AdmissionRequirements = model.AdmissionRequirementsCN;
                if (ct.LanguageId == 3)
                    ct.AdmissionRequirements = model.AdmissionRequirementsHK;
            }

            _repository.Update(course);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<ResultModel<Course>> UpdateCertificateConditionsAsync(CertificateConditionsBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "CourseTrans" });

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }

            foreach (var ct in course.CourseTrans)
            {
                if (ct.LanguageId == 1)
                    ct.ConditionsOfCertificate = model.CertificateConditionsEN;
                if (ct.LanguageId == 2)
                    ct.ConditionsOfCertificate = model.CertificateConditionsCN;
                if (ct.LanguageId == 3)
                    ct.ConditionsOfCertificate = model.CertificateConditionsHK;
            }

            _repository.Update(course);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<ResultModel<Course>> UpdateEnquirysAsync(EnquirysBindingModel model)
        {
            var result = new ResultModel<Course>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Enquiries" });

            if (course == null)
            {
                result.Message = "Not found";
                return result;
            }


            // Storage combination 
            if (course.Enquiries.Count >= model.Enquiries.Count())
            {
                List<Enquiry> delItems = new List<Enquiry>();
                foreach (var enquiry in course.Enquiries)
                {
                    var updateItem = model.Enquiries.SingleOrDefault(x => x.EnquiryNo == enquiry.EnquiryNo);
                    if (updateItem != null)
                    {
                        enquiry.CourseId = course.Id;
                        enquiry.EnquiryNo = updateItem.EnquiryNo;
                        enquiry.ContactPersonEN = updateItem.ContactPersonEN;
                        enquiry.ContactPersonCN = updateItem.ContactPersonCN;
                        enquiry.ContactPersonHK = updateItem.ContactPersonHK;
                        enquiry.Phone = updateItem.Phone;
                        enquiry.Fax = updateItem.Fax;
                        enquiry.Email = updateItem.Email;
                    }
                    else
                    {
                        delItems.Add(enquiry);
                    }
                }
                foreach (var delItem in delItems)
                {
                    _enquiryRepository.Delete(delItem);
                }
            }
            else
            {
                foreach (var updateModel in model.Enquiries)
                {
                    var dbItem = course.Enquiries.SingleOrDefault(x => x.EnquiryNo == updateModel.EnquiryNo);
                    if (dbItem != null)
                    {
                        dbItem.CourseId = course.Id;
                        dbItem.EnquiryNo = updateModel.EnquiryNo;
                        dbItem.ContactPersonEN = updateModel.ContactPersonEN;
                        dbItem.ContactPersonCN = updateModel.ContactPersonCN;
                        dbItem.ContactPersonHK = updateModel.ContactPersonHK;
                        dbItem.Phone = updateModel.Phone;
                        dbItem.Fax = updateModel.Fax;
                        dbItem.Email = updateModel.Email;
                    }
                    else
                    {
                        course.Enquiries.Add(new Enquiry()
                        {
                            CourseId = course.Id,
                            EnquiryNo = updateModel.EnquiryNo,
                            ContactPersonEN = updateModel.ContactPersonEN,
                            ContactPersonCN = updateModel.ContactPersonCN,
                            ContactPersonHK = updateModel.ContactPersonHK,
                            Phone = updateModel.Phone,
                            Fax = updateModel.Fax,
                            Email = updateModel.Email
                        });

                    }
                }
            }

            _repository.Update(course);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course;

            return result;
        }

        public async Task<ResultModel<CourseReExaminationBindingModel>> GetReExamination(int courseId)
        {
            var result = new ResultModel<CourseReExaminationBindingModel>();

            var course = await _repository.GetSingleByCondition(x => x.Id == courseId);

            if (course == null)
            {
                result.Message = "There is no course match";
                result.Data = null;
                return result;
            }

            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = course.ToCourseReExaminationBindingModel();
            return result;
        }

        public async Task<ResultModel<bool>> UpdateReExamination(CourseReExaminationBindingModel model)
        {
            var result = new ResultModel<bool>();

            var course = await _repository.GetSingleByCondition(x => x.Id == model.Id);

            if (course != null)
            {
                course.CanApplyForReExam = model.CanApplyForReExam;
                course.ReExamFee = model.ReExamFee;
                course.ReExamRemarks = model.ReExamRemarks;
                _repository.Update(course);
                _unitOfWork.Commit();
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = true;
            }
            else
            {
                result.Message = "There is no course match";
                result.Data = false;
            }

            return result;
        }

        public async Task<ResultModel<List<KeywordBindingModel>>> GetCourseKeyword(int courseId)
        {
            var result = new ResultModel<List<KeywordBindingModel>>();
            var course = await _repository.GetSingleByCondition(x => x.Id == courseId, new string[] { "Keywords" });

            if (course == null)
            {
                result.Message = "There is no course match";
                result.Data = null;
                return result;
            }

            var lstKeyWord = new List<KeywordBindingModel>();
            foreach (var item in course.Keywords)
            {
                lstKeyWord.Add(item.ToKeywordBindingModel());
            }
            result.Message = "Success";
            result.IsSuccess = true;
            result.Data = lstKeyWord;
            return result;

        }

        public async Task<ResultModel<bool>> UpdateCourseKeyword(List<KeywordBindingModel> lstKeyword, int cId)
        {
            int courseId = 0;
            var result = new ResultModel<bool>();
            if (cId > 0)
            {
                courseId = cId;
            }
            else
            {
                result.Message = "There is no course found";
                return result;
            }

            try
            {
                var course = await _repository.GetSingleByCondition(x => x.Id == courseId, new string[] { "Keywords" });
                //var courseKeywords = course.Keywords;

                var lstInsert = lstKeyword.Where(x => x.Id == 0);
                var lstUpdate = course.Keywords.Where(x => lstKeyword.Any(n => n.Id == x.Id));
                var lstDelete = course.Keywords.Where(x => !lstUpdate.Select(n => n.Id).Contains(x.Id));

                foreach (var item in lstDelete.ToList())
                {
                    _keywordRepository.Delete(item);
                }

                foreach (var item in lstUpdate)
                {
                    EntityHelpers.ToKeyword(lstKeyword.Single(x => x.Id == item.Id), item);
                }

                foreach (var item in lstInsert)
                {
                    course.Keywords.Add(EntityHelpers.ToKeyword(item, new Keyword(), courseId));
                }
                _repository.Update(course);
                _unitOfWork.Commit();

                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = "Failed: " + ex.Message;
                return result;
                throw;
            }
        }

        public async Task<Course> GetCourseInformationById(int id)
        {
            var resultcourselocation = (await _repository.GetSingleByCondition(x => x.Id == id,
                new string[] { "CourseType", "ProgrammeLeader", "LevelofApproval", "Lecturer", "Enquiries", "Modules", "CourseTrans", "Modules.ModuleTrans", "MediumOfInstruction", "CourseDocuments", "CourseDocuments.Document", "Keywords", "ModuleCombinations" }));

            return resultcourselocation;
        }

        public async Task<string> GetLocationByID(int langid, int locationId)
        {
            var loactionName = (await _courselocationTransRepository.GetSingleByCondition(x => x.LanguageId == langid && x.CourseLocationId == locationId));

            return loactionName != null ? loactionName.Name : "";
        }

        public async Task<int> CountTotalSearch(int langId, IEnumerable<int> coursecategories, int coursemode, int courselocations, string search)
        {

            List<string> lstKeyword = new List<string>();

            if (!string.IsNullOrEmpty(search))
            {
                lstKeyword = (await _keywordRepository.GetMulti(x => x.WordCN.Contains(search) || x.WordEN.Contains(search) || x.WordHK.Contains(search))).Select(x => x.CourseId.ToString()).ToList();
            }

            var resultcourselocation = (await _repository.Count(
                x => (coursemode != 0 ? x.CourseTypeId == coursemode : true)
                && (courselocations != 0 ? x.CourseVenueId == courselocations : true)
                && (coursecategories.Count() > 0 ? coursecategories.Contains(x.CategoryId) : true)
                && (!string.IsNullOrEmpty(search) ? (lstKeyword.Contains(x.Id.ToString())) : true)
                && x.InvisibleOnWebsite == true && x.CourseApprovedStatus == (int)ClassApprovedStatus.ThirdApproved));

            return resultcourselocation;
        }

        public async Task<string> GetUserNameByID(int langId, int userId)
        {
            var userName = (await _particularRepository.GetSingleByCondition(x => x.Id == userId));

            return userName == null ? "" : (langId == 1 ? userName.SurnameEN + " " + userName.GivenNameEN : userName.GivenNameCN + userName.SurnameCN);
        }

        public async Task<string> GetlanguageByID(int langId)
        {
            var language = (await _languageRepository.GetSingleByCondition(x => x.Id == langId));

            return language == null ? "" : language.Name;
        }

        public IEnumerable<CoursePortalAdminViewModel> GetCourseInformationByAdminPortal(int langId, IEnumerable<int> coursecategories, string coursecode, string coursenameEN, string coursenameCN, bool? displaycourse, int index, string sortBy, bool isDescending, int size, out int count)
        {
            IEnumerable<CoursePortalAdminViewModel> records = _repository.GetCoursesPaging(langId, coursecategories, coursecode, coursenameEN, coursenameCN, displaycourse, index, sortBy, isDescending, size, new string[] { "Modules", "CourseCategory", "CourseCategory.CourseCategorieTrans", "CourseTrans" }, out count);

            return records;
        }

        //public Class CreateClassAdmin(CreateClassAdminBindingModel model)
        //{
        //    Class classes = new Class()
        //    {
        //        ClassCode = model.ClassCode,
        //        CourseId = model.CourseId,
        //        CommencementDate = model.CommencementDate,
        //        CompletionDate = model.CompletionDate,
        //        IsExam = model.IsExam,
        //        IsReExam = model.IsReExam,
        //        ExamPassingMask = model.ExamPassingMask,
        //        ReExamFees = model.ReExamFees,
        //        AcademicYear = model.AcademicYear,
        //        InvisibleOnWebsite = model.InvisibleOnWebsite
        //    };

        //    var result = _classRepository.Add(classes);
        //    _unitOfWork.Commit();

        //    return result;
        //}
        public async Task<ResultModel<CourseDocumentViewModel>> GetCourseDocumentation(int courseId)
        {
            var documentCouse = await _documentRepository.GetMulti(x => x.CourseDocuments.Any(n => n.CourseId == courseId), new string[] { "CourseDocuments" });

            var result = new ResultModel<CourseDocumentViewModel>();
            try
            {
                CourseDocumentViewModel model = new CourseDocumentViewModel();
                model.Id = courseId;
                foreach (var item in documentCouse)
                {
                    if (item.CourseDocuments.FirstOrDefault().DistinguishDocType == (int)Common.Enums.DistinguishDocType.CourseBrochure)
                    {
                        model.ListCourseBrochure.Add(EntityHelpers.ToSubCourseDocument(item));
                    }
                    else if (item.CourseDocuments.FirstOrDefault().DistinguishDocType == (int)Common.Enums.DistinguishDocType.ApplicationForm)
                    {
                        model.ListApplicationForm.Add(EntityHelpers.ToSubCourseDocument(item));
                    }
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
                throw;
            }
        }

        public async Task<ResultModel<bool>> UpdateCourseDocumentation(int courseId, List<int> lstCourseDelete, HttpFileCollection lstFileUpdate, string courseCode)
        {
            var result = new ResultModel<bool>();
            var lstCourseBrochureFile = new List<HttpPostedFile>();
            var lstApplicationForm = new List<HttpPostedFile>();

            var lstCourseDocument = await _documentRepository.GetMulti(x => x.CourseDocuments.Any(n => n.CourseId == courseId), new string[] { "CourseDocuments" });

            try
            {
                var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
                var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);


                for (int i = 0; i < lstFileUpdate.Count; i++)
                {
                    if (lstFileUpdate.AllKeys[i].Equals("lstLogoImages"))
                    {
                        var logo = lstFileUpdate[i];
                        if (!CheckFileLength(logo, result, FileType.Image))
                        {
                            return result;
                        }
                    }
                }

                if (lstFileUpdate != null && lstFileUpdate.Count > 0)
                {
                    for (int i = 0; i < lstFileUpdate.Count; i++)
                    {
                        if (lstFileUpdate.AllKeys[i].Equals("lstLogoImages"))
                        {
                            var logo = lstFileUpdate[i];

                            string originalFileExtension = Path.GetExtension(logo.FileName);
                            string originalFileName = courseCode + "\\" + DistinguishDocType.CourseImage.ToString() + "\\" + Path.GetFileName(logo.FileName);
                            var pathDirectory = serPath + courseCode + "\\" + DistinguishDocType.CourseImage.ToString();

                            if (!Directory.Exists(pathDirectory))
                            {
                                Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                            }
                            var pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + logo.FileName);
                            logo.SaveAs(pathFile);

                            var fileNameToSaveDB = Path.GetFileName(pathFile);
                            var pathToSaveDB = courseCode + "\\" + DistinguishDocType.CourseImage.ToString() + "\\" + fileNameToSaveDB;
                            _documentRepository.Add(EntityHelpers.ToDoccumentForCourse(pathToSaveDB, logo.ContentType, fileNameToSaveDB, courseId, (int)DistinguishDocType.CourseImage));
                        }
                        else if (lstFileUpdate.AllKeys[i].Equals("lstCourseBrochureFile"))
                        {
                            var file = lstFileUpdate[i];
                            if (!CheckFileLength(file, result))
                            {
                                return result;
                            }
                            string originalFileExtension = Path.GetExtension(file.FileName);
                            string originalFileName = courseCode + "\\" + DistinguishDocType.CourseBrochure.ToString() + "\\" + Path.GetFileName(file.FileName);
                            var pathDirectory = serPath + courseCode + "\\" + DistinguishDocType.CourseBrochure.ToString();

                            if (Directory.Exists(pathDirectory))
                            {
                                var listCourseDocuments = _courseDocumentsRepository.GetCourseDocumentByQueryable(x => x.CourseId == courseId && x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.CourseBrochure);
                                var listCourseDocumentsId = listCourseDocuments.Select(c => c.DocumentId).ToList();
                                _courseDocumentsRepository.DeleteMulti(x => listCourseDocumentsId.Contains(x.DocumentId));
                                _documentRepository.DeleteMulti(x => listCourseDocumentsId.Contains(x.Id));

                                #region Remove file inside folder before remove folder
                                DirectoryInfo theDirectory = new DirectoryInfo(pathDirectory);
                                foreach (FileInfo subFile in theDirectory.GetFiles())
                                {
                                    subFile.Delete();
                                }
                                #endregion

                                Directory.Delete(pathDirectory);
                            }
                            Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                            var pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + file.FileName);
                            file.SaveAs(pathFile);

                            var fileNameToSaveDB = Path.GetFileName(pathFile);
                            var pathToSaveDB = courseCode + "\\" + DistinguishDocType.CourseBrochure.ToString() + "\\" + fileNameToSaveDB;
                            _documentRepository.Add(EntityHelpers.ToDoccumentForCourse(pathToSaveDB, file.ContentType, fileNameToSaveDB, courseId, (int)DistinguishDocType.CourseBrochure));
                        }
                        else if (lstFileUpdate.AllKeys[i].Equals("lstApplicationForm"))
                        {
                            var file = lstFileUpdate[i];
                            if (!CheckFileLength(file, result))
                            {
                                return result;
                            }
                            string originalFileExtension = Path.GetExtension(file.FileName);
                            string originalFileName = courseCode + "\\" + DistinguishDocType.ApplicationForm.ToString() + "\\" + Path.GetFileName(file.FileName);
                            var pathDirectory = serPath + courseCode + "\\" + DistinguishDocType.ApplicationForm.ToString();

                            if (Directory.Exists(pathDirectory))
                            {
                                var listCourseDocuments = _courseDocumentsRepository.GetCourseDocumentByQueryable(x => x.CourseId == courseId && x.DistinguishDocType == (int)Common.Enums.DistinguishDocType.ApplicationForm);
                                var listCourseDocumentsId = listCourseDocuments.Select(c => c.DocumentId).ToList();
                                _courseDocumentsRepository.DeleteMulti(x => listCourseDocumentsId.Contains(x.DocumentId));
                                _documentRepository.DeleteMulti(x => listCourseDocumentsId.Contains(x.Id));

                                #region Remove file inside folder before remove folder
                                DirectoryInfo theDirectory = new DirectoryInfo(pathDirectory);
                                foreach (FileInfo subFile in theDirectory.GetFiles())
                                {
                                    subFile.Delete();
                                }
                                #endregion

                                Directory.Delete(pathDirectory);
                            }
                            Common.Common.CreateDirectoryAndGrantFullControlPermission(pathDirectory);
                            var pathFile = Common.Common.GenFileNameDuplicate(pathDirectory + "\\" + file.FileName);
                            file.SaveAs(pathFile);

                            var fileNameToSaveDB = Path.GetFileName(pathFile);
                            var pathToSaveDB = courseCode + "\\" + DistinguishDocType.ApplicationForm.ToString() + "\\" + fileNameToSaveDB;
                            _documentRepository.Add(EntityHelpers.ToDoccumentForCourse(pathToSaveDB, file.ContentType, fileNameToSaveDB, courseId, (int)DistinguishDocType.ApplicationForm));
                        }
                    }
                }

                for (int i = 0; i < lstCourseDelete.Count; i++)
                {
                    var doc = lstCourseDocument.FirstOrDefault(x => x.Id == lstCourseDelete[i]);
                    if (doc != null)
                    {
                        var courseDoc = doc.CourseDocuments.FirstOrDefault(x => x.DocumentId == doc.Id);
                        if (courseDoc != null)
                        {
                            _courseDocumentsRepository.Delete(courseDoc);
                        }

                        var tempUrl = serPath + doc.Url;
                        if (doc != null && File.Exists(tempUrl))
                        {
                            File.Delete(tempUrl);
                        }
                        _documentRepository.Delete(doc);
                    }
                }

                _unitOfWork.Commit();
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
                throw;
            }
        }

        //public async Task<ResultModel<string>> DownloadCourseDocument(int documentId)
        //{
        //    var result = new ResultModel<string>();
        //    var doc = _documentRepository.GetSingleById(documentId);
        //    string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

        //    var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
        //    var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
        //    var pathDoc = Path.Combine(serPath, doc.Url);

        //    if (File.Exists(pathDoc))
        //    {
        //        var tempDirectory = ConfigHelper.GetByKey("TempFileToDownload");
        //        string uniqueTempFolder = doc.Url.Substring(0, doc.Url.IndexOf('\\') - 1) + "-" + DateTime.Now.ToString(StaticConfig.cFormatShortDateTime);
        //        var tempFileToDownloadPhysicalPath = Path.Combine(tempDirectory, "Course_Document/", uniqueTempFolder + "/" + doc.FileName + doc.ContentType);
        //        try
        //        {
        //            tempFileToDownloadPhysicalPath = System.Web.HttpContext.Current.Server.MapPath(tempFileToDownloadPhysicalPath);
        //            string folderPath = Path.GetDirectoryName(tempFileToDownloadPhysicalPath);
        //            if (!Directory.Exists(folderPath))
        //            {
        //                Common.Common.CreateDirectoryAndGrantFullControlPermission(folderPath);
        //            }

        //            File.Copy(pathDoc, tempFileToDownloadPhysicalPath, true);

        //            if (File.Exists(tempFileToDownloadPhysicalPath))
        //            {
        //                var index = tempDirectory.IndexOf('/') + 1;

        //                string tempFileToDownloadVirtualPath = Common.Uri.Combine(domainName, HostingEnvironment.ApplicationVirtualPath, tempDirectory.Substring(index, tempDirectory.Length - index) + "Course_Document/", uniqueTempFolder, doc.FileName + doc.ContentType);
        //                tempFileToDownloadVirtualPath = tempFileToDownloadVirtualPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        //                Common.Common.CleanupTempFolderTaskExcute(tempFileToDownloadPhysicalPath);
        //                result.Message = "Success";
        //                result.IsSuccess = true;
        //                result.Data = tempFileToDownloadVirtualPath;
        //                return result;
        //            }
        //        }
        //        catch (IOException ex)
        //        {
        //            result.Message = ex.Message;
        //            return result;
        //            throw;
        //        }
        //    }
        //    result.Message = "File is not found";
        //    return result;
        //}

        public async Task<ResultModel<FileReturnViewModel>> DownloadCourseDocument(int documentId)
        {
            var result = new ResultModel<FileReturnViewModel>();
            var doc = _documentRepository.GetSingleById(documentId);
            if (doc == null)
            {
                result.Message = "File is not found";
                return result;
            }
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

            var directory = ConfigHelper.GetByKey("CourseDocumentDirectory");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(directory);
            var pathDoc = Path.Combine(serPath, doc.Url);

            if (File.Exists(pathDoc))
            {
                var stream = new MemoryStream(File.ReadAllBytes(pathDoc));
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = stream,
                    FileType = doc.ContentType,
                    FileName = doc.FileName
                };
                return result;
            }
            result.Message = "File is not found";
            return result;
        }

        private bool CheckFileLength(HttpPostedFile file, ResultModel<bool> result, Common.Enums.FileType typeFile = FileType.File)
        {
            switch (typeFile)
            {
                case FileType.File:
                    if (file.ContentLength > StaticConfig.MaximumFileLength)
                    {
                        result.Message = "A file is too large";
                        result.IsSuccess = false;
                        return false;
                    }
                    return true;
                case FileType.Image:
                    if (file.ContentLength > StaticConfig.MaximumLogoImageLength)
                    {
                        result.Message = "A file is too large";
                        result.IsSuccess = false;
                        return false;
                    }
                    return true;
                default:
                    result.Message = "A file is too large";
                    result.IsSuccess = false;
                    return false;
            }

        }

        public async Task<Course> UpdateAsync(CreateCourseBindingModel model)
        {
            if (await IsCourseCodeExisted(model.CourseCode, model.Id))
                return null;

            Course course = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "CourseTrans" });

            course.CategoryId = model.CourseCategory;
            course.CourseCode = model.CourseCode;
            course.TargetClassSize = model.TargetClassSize;
            course.CourseTypeId = model.Mode;
            course.DurationHrs = model.DurationHrs;
            course.DurationTotal = model.DurationTotal;
            course.DurationLesson = model.Lessons;
            course.Credits = model.Credits;
            course.CourseVenueId = model.Venue;
            course.LevelOfApprovalId = model.LevelOfApproval;
            course.InvisibleOnWebsite = model.Display;

            course.ObjectiveEN = model.ObjectiveEN;
            course.ObjectiveSC = model.ObjectiveSC;
            course.ObjectiveTC = model.ObjectiveTC;
            course.WaitingTimeEN = model.WaitingTimeEN;
            course.WaitingTimeSC = model.WaitingTimeSC;
            course.WaitingTimeTC = model.WaitingTimeTC;

            foreach (var tran in course.CourseTrans)
            {
                if (tran.LanguageId == 1)
                {
                    tran.CourseName = model.CourseNameEN;
                    tran.CourseTitle = model.CourseNameEN;
                }
                if (tran.LanguageId == 2)
                {
                    tran.CourseName = model.CourseNameCN;
                    tran.CourseTitle = model.CourseNameCN;
                }
                if (tran.LanguageId == 3)
                {
                    tran.CourseName = model.CourseNameHK;
                    tran.CourseTitle = model.CourseNameHK;
                }
            }
            _repository.Update(course);
            _unitOfWork.Commit();

            return course;
        }

        public async Task<Course> GetCourseApplicationForm(int id)
        {
            var resultcourselocation = (await _repository.GetSingleByCondition(x => x.Id == id,
                new string[] { "CourseType", "Modules", "CourseTrans", "Classes" }));

            return resultcourselocation;
        }

        public async Task<bool> ShowReExam()
        {
            var result = await _repository.GetSingleByCondition(x => true);
            return result != null && result.CanApplyForReExam;
        }

        public async Task<List<Course>> CloneDataToElastic()
        {
            var result = await _repository.GetMulti(x => true
            , new string[] { "CourseType", "ProgrammeLeader", "Lecturer", "Enquiries", "CourseVenue", "CourseVenue.CourseLocationTrans", "Modules", "Modules.ModuleTrans", "CourseCategory", "CourseCategory.CourseCategorieTrans", "CourseTrans", "MediumOfInstruction" });

            return result;
        }

        public async Task<IList<Class>> GetCourseCalendar(int id)
        {
            var result = await _classRepository.GetMulti(i => i.CourseId == id, new string[] { "Lessons" });


            return result;
        }

        public async Task<IEnumerable<Model.ViewModels.Application.CourseCodeViewModel>> GetCourseCodeByFilter(CourseCodeFilter filter)
        {
            var result = (await _repository.GetMulti(x =>
            (filter.CourseTypeId != 0 ? x.CourseTypeId == filter.CourseTypeId : true) && (filter.CourseCategoryId != 0 ? x.CategoryId == filter.CourseCategoryId : true)))
            .Select(c => new Model.ViewModels.Application.CourseCodeViewModel()
            {
                Id = c.Id,
                CourseCode = c.CourseCode
            });

            return result;
        }

        public async Task<IEnumerable<Model.ViewModels.Application.CourseCodeViewModel>> GetCourseCodeByText(string key)
        {
            var result = await _repository.GetMulti(x => x.CourseCode.Contains(key));
            return result.Select(x => new Model.ViewModels.Application.CourseCodeViewModel() { Id = x.Id, CourseCode = x.CourseCode });
        }
        public async Task<IEnumerable<string>> GetCourseNameByString(CourseAssessmentFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.CourseNameEN) && string.IsNullOrWhiteSpace(filter.CourseNameCN))
            {
                return null;
            }
            IEnumerable<string> result;
            if (!string.IsNullOrWhiteSpace(filter.CourseNameEN))
            {
                result = (await _courseTransRepository.GetMulti(x => x.CourseName.ToLower().Contains(filter.CourseNameEN) && x.LanguageId == (int)LanguageCode.EN)).Select(x => x.CourseName);
            }
            else
            {
                result = (await _courseTransRepository.GetMulti(x => x.CourseName.ToLower().Contains(filter.CourseNameCN) && (x.LanguageId == (int)LanguageCode.CN || x.LanguageId == (int)LanguageCode.HK)))
                    .Select(x => x.CourseName);
            }

            return result;
        }

        public async Task<IEnumerable<CourseAssessmentViewModel>> GetCourseCodeAssessment(CourseAssessmentFilter filter)
        {
            var result = await _repository.GetCourseAssessment(filter);

            return result;
        }

        public async Task<IEnumerable<ClassCodeViewModel>> GetClassCodeByCourseCode(string coursecode)
        {
            var result = await _repository.GetSingleByCondition(x => x.CourseCode.Equals(coursecode), new string[] { "Classes" });
            return result.Classes.Select(x => new ClassCodeViewModel()
            {
                Id = x.Id,
                ClassCode = x.ClassCode
            });
        }

        public async Task<bool> CheckCourseHasExamination(int courseId)
        {
            var result = await _repository.CheckContains(x => x.Id == courseId && x.Classes.Any(y => y.Lessons.Count > 0));
            return result;
        }

        public async Task<bool> IsCourseCodeExisted(string courseCode, int courseId = 0)
        {
            var result = await _repository.GetSingleByCondition(x => x.CourseCode == courseCode && x.Id != courseId);
            return result != null;
        }

        public async Task<bool> CanUserApplyCourse(int courseId, int userId)
        {
            var result = await _applicationRepository.CheckContains(x =>
                                                            x.CourseId == courseId
                                                            && x.UserId == userId
                                                            && (x.Status == (int)ApplicationStatus.Created 
                                                                || x.Status == (int)ApplicationStatus.Submitted 
                                                                || x.Status == (int) ApplicationStatus.SupplementaryInformation));
            return !result;
        }
    }
}