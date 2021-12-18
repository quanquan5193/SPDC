using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service.Services
{

    public interface IQualificationService
    {
        Task<bool> DeleteQualifications(int qualificationId);

        Task<bool> CreateQualification(QualificationBindingModels model, int languageID, int userID);

        Task<bool> UpdateQualification(QualificationBindingModels model, int languageID, int userID);

        Task<IEnumerable<Qualification>> GetQualification(int userID);

        Task<Qualification> GetQualificationsById(int qualificationId);
    }

    public class QualificationService : IQualificationService
    {

        private IUnitOfWork _unitOfWork;
        private IQualificationTransRepository _qualificationTransRepository;
        private IQualificationRepository _qualificationRepository;

        public QualificationService(IUnitOfWork unitOfWork, IQualificationTransRepository qualificationTransRepository, IQualificationRepository qualificationRepository)
        {
            _unitOfWork = unitOfWork;
            _qualificationTransRepository = qualificationTransRepository;
            _qualificationRepository = qualificationRepository;
        }

        public async Task<bool> CreateQualification(QualificationBindingModels model, int languageID, int userID)
        {
            try
            {
                Qualification qualification = new Qualification()
                {
                    DateObtained = model.DateObtained,
                    UserId = userID
                };

                var result = _qualificationRepository.Add(qualification);

                QualificationsTran qualificationsTran = new QualificationsTran()
                {
                    IssuingAuthority = model.IssuingAuthority,
                    LevelAttained = model.LevelAttained,
                    LanguageId = languageID,
                    QualificationId = result.Id
                };

                _qualificationTransRepository.Add(qualificationsTran);

                _unitOfWork.Commit();
                return true;

            }
            catch (Exception)
            {

                return false;
                throw;
            }
        }

        public async Task<bool> DeleteQualifications(int qualificationId)
        {
            try
            {
                var qualification = await _qualificationRepository.GetSingleByCondition(n => n.Id == qualificationId, new string[] { "QualificationsTrans" });

                if (qualification == null)
                    return false;
                _qualificationTransRepository.DeleteMulti(n => n.QualificationId == qualification.Id);
                _qualificationRepository.Delete(qualification);

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateQualification(QualificationBindingModels model, int languageID, int userID)
        {
            try
            {
                if (model.Id == null)
                {
                    return false;
                }

                var qualification = _qualificationRepository.GetSingleById(model.Id.Value);

                qualification.DateObtained = model.DateObtained;
                qualification.UserId = userID;

                _qualificationRepository.Update(qualification);

                var qualificationTrans = await _qualificationTransRepository.GetSingleByCondition(x => x.QualificationId == model.Id.Value && x.LanguageId == languageID);

                if (qualificationTrans != null)
                {
                    qualificationTrans.IssuingAuthority = model.IssuingAuthority;
                    qualificationTrans.LevelAttained = model.LevelAttained;

                    _qualificationTransRepository.Update(qualificationTrans);
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

        public async Task<IEnumerable<Qualification>> GetQualification(int userID)
        {
            var qualification = await _qualificationRepository.GetMulti(x => x.UserId == userID, new string[] { "QualificationsTrans" });

            return qualification;
        }

        public async Task<Qualification> GetQualificationsById(int qualificationId)
        {
            var experience = await _qualificationRepository.GetSingleByCondition(n => n.Id == qualificationId, new string[] { "QualificationsTrans" });

            return experience;
        }
    }
}
