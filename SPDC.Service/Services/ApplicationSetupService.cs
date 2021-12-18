using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using SPDC.Common;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;

namespace SPDC.Service.Services
{
    public interface IApplicationSetupService
    {
        Task<ResultModel<ApplicationSetups>> GetApplicationById(int applicationSetupId);
        Task<ResultModel<ApplicationSetups>> GetApplicationByCourseId(int courseId);
        ApplicationSetups CreateApplicationSetup(ApplicationSetups model);
        Task<ApplicationSetups> UpdateApplicationSetup(ApplicationSetups model);
        Task<bool> CheckCourseApplyAvailable(int courseId);
        Task<IEnumerable<InvoiceViewModel>> GetInvoicesByApplicationId(int applicationId);
    }
    public class ApplicationSetupService : IApplicationSetupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationSetupRepository _applicationSetupRepository;
        private readonly IRelevantWorkRepository _relevantWorkRepository;
        private readonly IRelevantMembershipRepository _relevantMembershipRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public ApplicationSetupService(IUnitOfWork unitOfWork, IApplicationSetupRepository applicationSetupRepository, IRelevantWorkRepository relevantWorkRepository,
            IRelevantMembershipRepository relevantMembershipRepository, ICourseRepository courseRepository, IInvoiceRepository invoiceRepository)
        {
            _unitOfWork = unitOfWork;
            _applicationSetupRepository = applicationSetupRepository;
            _relevantMembershipRepository = relevantMembershipRepository;
            _relevantWorkRepository = relevantWorkRepository;
            _courseRepository = courseRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResultModel<ApplicationSetups>> GetApplicationById(int applicationSetupId)
        {
            //init
            var result = new ResultModel<ApplicationSetups>();

            var objApplicationSetups = await _applicationSetupRepository.GetSingleByCondition(x => x.Id == applicationSetupId,
                new[] { "RelevantWork", "RelevantMembership" });
            if (objApplicationSetups == null)
            {
                result.Message = "There is no class not match.";
                result.Data = null;
                return result;
            }
            //assign data
            result.Message = Common.Common.Success;
            result.IsSuccess = true;
            result.Data = objApplicationSetups;
            return result;
        }

        public async Task<ResultModel<ApplicationSetups>> GetApplicationByCourseId(int courseId)
        {
            //init
            var result = new ResultModel<ApplicationSetups>();

            var objApplicationSetups = await _applicationSetupRepository.GetSingleByCondition(x => x.CourseId == courseId,
            new[] { "RelevantWork", "RelevantMembership" });

            //var objApplicationSetups = await _courseRepository.GetSingleByCondition(x => x.Id == courseId,
                //new[] {"ApplicationSetup", "RelevantWork", "RelevantMembership" });
            if (objApplicationSetups == null)
            {
                result.Message = "There is no class not match."; // TODO: @quan.dang: FE throw this message as error message, please check it out
                result.Data = null;
                return result;
            }
            //assign data
            result.Message = Common.Common.Success;
            result.IsSuccess = true;
            result.Data = objApplicationSetups;
            return result;
        }

        public ApplicationSetups CreateApplicationSetup(ApplicationSetups model)
        {
            //find course
            var objCourse = _courseRepository.GetSingleById(model.CourseId);
            objCourse.ApplicationSetup = model;
            objCourse.IsSetApplicationSetup = true;
            _courseRepository.Update(objCourse);
            _unitOfWork.Commit();
            return objCourse.ApplicationSetup;
        }

        public async Task<ApplicationSetups> UpdateApplicationSetup(ApplicationSetups model)
        {
            var entiApplicationSetup = await _applicationSetupRepository.GetSingleByCondition(x => x.Id == model.Id, new[] { "RelevantMembership", "RelevantWork" });
            //Convert
            ConvertCustomModelToModel(entiApplicationSetup, model);
            ConvertMembershipCustomModelToModel(entiApplicationSetup.RelevantMembership, model.RelevantMembership);
            ConvertWorkCustomModelToModel(entiApplicationSetup.RelevantWork, model.RelevantWork);
            _applicationSetupRepository.Update(entiApplicationSetup);
            _unitOfWork.Commit();
            return model;
        }


        private static void ConvertCustomModelToModel(ApplicationSetups modelClass, ApplicationSetups objClassMap)
        {
            modelClass.Id = objClassMap.Id;
            modelClass.CourseId = objClassMap.CourseId;
            modelClass.ShowEducationLevel = objClassMap.ShowEducationLevel;
            //modelClass.RelevantMembershipId = objClassMap.RelevantMembershipId;
            //modelClass.RelevantWorkId = objClassMap.RelevantWorkId;
            modelClass.ShowRecommendation = objClassMap.ShowRecommendation;
            modelClass.ShowDocument = objClassMap.ShowDocument;
            modelClass.ShowCitf = objClassMap.ShowCitf;
            modelClass.ShowReceipt = objClassMap.ShowReceipt;
            modelClass.FundingSchema = objClassMap.FundingSchema;
        }

        private static void ConvertMembershipCustomModelToModel(RelevantMemberships modelMemberships, RelevantMemberships objMemberships)
        {
            modelMemberships.Id = objMemberships.Id;
            modelMemberships.ShowMembershipTable = objMemberships.ShowMembershipTable;
            modelMemberships.ShowTwoYears = objMemberships.ShowTwoYears;
            modelMemberships.ShowKnowledge = objMemberships.ShowKnowledge;
            modelMemberships.ShowBimBasic = objMemberships.ShowBimBasic;
        }

        private static void ConvertWorkCustomModelToModel(RelevantWorks modelRelevantWorks, RelevantWorks objRelevantWorks)
        {
            modelRelevantWorks.Id = objRelevantWorks.Id;
            modelRelevantWorks.ShowTwoYears = objRelevantWorks.ShowTwoYears;
            modelRelevantWorks.ShowThreeYears = objRelevantWorks.ShowThreeYears;
            modelRelevantWorks.ShowFourYears = objRelevantWorks.ShowFourYears;
            modelRelevantWorks.ShowFiveYears = objRelevantWorks.ShowFiveYears;
            modelRelevantWorks.ShowThreeYearsLeak = objRelevantWorks.ShowThreeYearsLeak;
            modelRelevantWorks.ShowWorkingExperience = objRelevantWorks.ShowWorkingExperience;
            modelRelevantWorks.ShowLetterTemplate = objRelevantWorks.ShowLetterTemplate;
            modelRelevantWorks.Specify = objRelevantWorks.Specify;
            
        }

        public async Task<bool> CheckCourseApplyAvailable(int courseId)
        {
            var available = await _applicationSetupRepository.CheckContains(x => x.CourseId == courseId);
            return available;
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoicesByApplicationId(int applicationId)
        {
            var result = (await _invoiceRepository.GetMulti(x => x.ApplicationId == applicationId, new string[] { "Application.Course.CourseTrans",
            "Application.User.Particular", "InvoiceItems"}))
            .Select(c => c.ToInvoiceViewModel());

            return result;
        }
    }
}
