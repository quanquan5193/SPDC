using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service.Services
{

    public interface IExperienceService
    {
        Task<IEnumerable<WorkExperience>> Getexperience(int userID);

        Task<bool> DeleteWorkExperience(int experienceId);

        Task<bool> CreateExperience(WorkExperienceBindingModel model, int languageID, int userID);

        Task<bool> UpdateExperience(WorkExperienceBindingModel model, int languageID, int userID);

        Task<WorkExperience> GetexperienceById(int experienceId);
    }

    public class ExperienceService : IExperienceService
    {
        private IUnitOfWork _unitOfWork;
        private IWorkExperienceRepository _workExperienceRepository;
        private IWorkExperienceTransRepository _workExperienceTransRepository;

        public ExperienceService(IUnitOfWork unitOfWork, IWorkExperienceRepository workExperienceRepository, IWorkExperienceTransRepository workExperienceTransRepository)
        {
            _unitOfWork = unitOfWork;
            _workExperienceRepository = workExperienceRepository;
            _workExperienceTransRepository = workExperienceTransRepository;
        }

        public async Task<IEnumerable<WorkExperience>> Getexperience(int userID)
        {
            var result = await _workExperienceRepository.GetMulti(x => x.UserId == userID, new string[] { "WorkExperienceTrans" });

            return result;
        }

        public async Task<bool> DeleteWorkExperience(int experienceId)
        {
            try
            {
                var experience = await _workExperienceRepository.GetSingleByCondition(n => n.Id == experienceId, new string[] { "WorkExperienceTrans" });

                if (experience == null)
                    return false;
                _workExperienceTransRepository.DeleteMulti(n => n.WorkExperienceId == experience.Id);
                _workExperienceRepository.Delete(experience);

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> CreateExperience(WorkExperienceBindingModel model, int languageID, int userID)
        {
            try
            {
                WorkExperience workExperience = new WorkExperience()
                {
                    FromYear = model.FromYear,
                    ToYear = model.ToYear,
                    BIMRelated = model.BIMRelated,
                    ClassifyWorkingExperience = model.ClassifyWorkingExperience,
                    UserId = userID
                };

                var result = _workExperienceRepository.Add(workExperience);

                WorkExperienceTran workExperienceTran = new WorkExperienceTran()
                {
                    Location = model.Location,
                    Position = model.Position,
                    WorkExperienceId = result.Id,
                    LanguageId = languageID,
                    JobNature = model.JobNature
                };

                _workExperienceTransRepository.Add(workExperienceTran);

                _unitOfWork.Commit();
                return true;

            }
            catch (Exception)
            {

                return false;
                throw;
            }
        }

        public async Task<bool> UpdateExperience(WorkExperienceBindingModel model, int languageID, int userID)
        {
            try
            {
                if (model.Id == 0)
                {
                    return false;
                }

                var experience = _workExperienceRepository.GetSingleById(model.Id);

                experience.FromYear = model.FromYear;
                experience.ToYear = model.ToYear;
                experience.BIMRelated = model.BIMRelated;
                experience.UserId = userID;
                experience.ClassifyWorkingExperience = model.ClassifyWorkingExperience;

                _workExperienceRepository.Update(experience);

                var experienceTrans = await _workExperienceTransRepository.GetSingleByCondition(x => x.WorkExperienceId == model.Id && x.LanguageId == languageID);

                if (experienceTrans != null)
                {
                    experienceTrans.Location = model.Location;
                    experienceTrans.Position = model.Position;
                    experienceTrans.LanguageId = languageID;
                    experienceTrans.JobNature = model.JobNature;

                    _workExperienceTransRepository.Update(experienceTrans);
                }

                _unitOfWork.Commit();

                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<WorkExperience> GetexperienceById(int experienceId)
        {
            var experience = await _workExperienceRepository.GetSingleByCondition(n => n.Id == experienceId, new string[] { "WorkExperienceTrans" });

            return experience;
        }
    }
}
