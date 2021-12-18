using SPDC.Common;
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
    public interface IEmployerRecommendationService
    {
        Task<List<RecommendationEmployerBindingModel>> GetEmployerRecommendation(int id, int langCode);

        Task<List<RecommendationEmployerBindingModel>> GetEmployerRecommendationTemp(int id, int langCode);

        Task<bool> UpdateEmployerRecommendation(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId);
        Task<bool> UpdateEmployerRecommendationTempToTable(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId);
        Task<bool> DeleteEmployerRecommendationTemp(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId);
        Task<bool> UpdateEmployerRecommendationTemp(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId);

        Task<bool> CreateRecommendation(RecommendationEmployerNewBindingModel lstModel, int langCode, int userId);

        Task<IEnumerable<EmployerRecommendation>> GetRecommendation(int userID);

        Task<bool> DeleteRecommendation(int recommendationId);

        Task<bool> UpdateRecommendation(RecommendationEmployerNewBindingModel lstModel, int langCode, int userId);

        Task<EmployerRecommendation> GetRecommendationById(int recommendationId);
    }
    public class EmployerRecommendationService : IEmployerRecommendationService
    {
        IUnitOfWork _unitOfWork;
        IEmployerRecommendationsRepository _employerRecommendations;
        IEmployerRecommendationsTransRepository _employerRecommendationsTrans;
        IEmployerRecommendationsTempRepository _employerRecommendationsTemp;
        IEmployerRecommendationsTransTempRepository _employerRecommendationsTransTemp;
        public EmployerRecommendationService(IUnitOfWork unitOfWork, IEmployerRecommendationsRepository employerRecommendations, IEmployerRecommendationsTransRepository employerRecommendationsTrans
            , IEmployerRecommendationsTempRepository employerRecommendationsTemp, IEmployerRecommendationsTransTempRepository employerRecommendationsTransTemp)
        {
            _unitOfWork = unitOfWork;
            _employerRecommendations = employerRecommendations;
            _employerRecommendationsTrans = employerRecommendationsTrans;
            _employerRecommendationsTemp = employerRecommendationsTemp;
            _employerRecommendationsTransTemp = employerRecommendationsTransTemp;
        }

        public async Task<List<RecommendationEmployerBindingModel>> GetEmployerRecommendation(int id, int langCode)
        {
            List<RecommendationEmployerBindingModel> listRecommendationBinding = new List<RecommendationEmployerBindingModel>();
            var listRecommendation = await _employerRecommendations.GetMulti(x => x.UserId == id, new string[] { "EmployerRecommendationTrans" });

            for (int i = 0; i < listRecommendation.Count; i++)
            {
                var model = new RecommendationEmployerBindingModel();
                listRecommendation[i].ConvertToRecommendationEmployerBindingModel(model);
                listRecommendationBinding.Add(model);
            }

            return listRecommendationBinding;
        }


        public async Task<List<RecommendationEmployerBindingModel>> GetEmployerRecommendationTemp(int id, int langCode)
        {
            List<RecommendationEmployerBindingModel> listRecommendationBinding = new List<RecommendationEmployerBindingModel>();
            var listRecommendation = await _employerRecommendationsTemp.GetMulti(x => x.UserId == id, new string[] { "EmployerRecommendationTempTrans" });

            for (int i = 0; i < listRecommendation.Count; i++)
            {
                var model = new RecommendationEmployerBindingModel();
                listRecommendation[i].ConvertToRecommendationEmployerBindingModelTemp(model);
                listRecommendationBinding.Add(model);
            }

            return listRecommendationBinding;
        }

        public async Task<bool> UpdateEmployerRecommendation(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId)
        {
            if (lstModel == null) return true;
            try
            {
                var lstRecommend = await _employerRecommendations.GetMulti(n => n.UserId == userId, new string[] { "EmployerRecommendationTrans" });

                var lstInsert = lstModel.Where(n => n.Id == 0);
                var lstUpdate = lstRecommend.Where(n => lstModel.Any(x => x.Id == n.Id));
                var lstDelete = lstRecommend.Where(n => !lstUpdate.Select(x => x.Id).Contains(n.Id));

                foreach (var item in lstDelete)
                {
                    _employerRecommendationsTrans.DeleteMulti(n => n.EmployerRecommendationId == item.Id);
                    _employerRecommendations.Delete(item);
                }

                foreach (var item in lstUpdate)
                {
                    _employerRecommendations.Update(EntityHelpers.ToRecommendationEmployer(lstModel.Single(x => x.Id == item.Id), item, userId, langCode));
                }

                foreach (var item in lstInsert)
                {
                    _employerRecommendations.Add(EntityHelpers.ToRecommendationEmployer(item, null, userId, langCode));
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateEmployerRecommendationTempToTable(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId)
        {
            if (lstModel == null) return true;
            try
            {
                var lstRecommend = await _employerRecommendations.GetMulti(n => n.UserId == userId, new string[] { "EmployerRecommendationTrans" });

                var lstInsert = lstModel;
                var lstDelete = lstRecommend;

                foreach (var item in lstDelete)
                {
                    _employerRecommendationsTrans.DeleteMulti(n => n.EmployerRecommendationId == item.Id);
                    _employerRecommendations.Delete(item);
                }
                _unitOfWork.Commit();

                foreach (var item in lstInsert)
                {
                    _employerRecommendations.Add(EntityHelpers.ToRecommendationEmployer(item, null, userId, langCode));
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> DeleteEmployerRecommendationTemp(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId)
        {
            if (lstModel == null) return true;
            try
            {
                var lstRecommend = await _employerRecommendationsTemp.GetMulti(n => n.UserId == userId, new string[] { "EmployerRecommendationTempTrans" });

                var lstInsert = lstModel;
                var lstDelete = lstRecommend;

                foreach (var item in lstDelete)
                {
                    _employerRecommendationsTransTemp.DeleteMulti(n => n.EmployerRecommendationTemp.Id == item.Id);
                    _employerRecommendationsTemp.Delete(item);
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateEmployerRecommendationTemp(List<RecommendationEmployerBindingModel> lstModel, int langCode, int userId)
        {
            if (lstModel == null) return true;
            try
            {
                var lstRecommend = await _employerRecommendationsTemp.GetMulti(n => n.UserId == userId, new string[] { "EmployerRecommendationTempTrans" });
                var lstInsert = lstModel;
                var lstDelete = lstRecommend;
                foreach (var item in lstDelete)
                {
                    _employerRecommendationsTransTemp.DeleteMulti(n => n.EmployerRecommendationTemp.Id == item.Id);
                    _employerRecommendationsTemp.Delete(item);
                }
                _unitOfWork.Commit();

                foreach (var item in lstInsert)
                {
                    _employerRecommendationsTemp.Add(EntityHelpers.ToRecommendationEmployerTemp(item, null, userId, langCode));
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> CreateRecommendation(RecommendationEmployerNewBindingModel lstModel, int langCode, int userId)
        {

            try
            {
                EmployerRecommendation employerRecommendation = new EmployerRecommendation();
                employerRecommendation.Tel = lstModel.ContactPersonTel;
                employerRecommendation.UserId = userId;

                var resulr = _employerRecommendations.Add(employerRecommendation);

                EmployerRecommendationTran employerRecommendationTran = new EmployerRecommendationTran();
                employerRecommendationTran.CompanyName = lstModel.CompanyName;
                employerRecommendationTran.ContactPerson = lstModel.ContactPersonName;
                employerRecommendationTran.LanguageId = langCode;
                employerRecommendationTran.EmployerRecommendationId = resulr.Id;
                employerRecommendationTran.Position = lstModel.ContactPersonPosition;
                employerRecommendationTran.Email = lstModel.ContactPersonEmail;

                _employerRecommendationsTrans.Add(employerRecommendationTran);

                _unitOfWork.Commit();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<IEnumerable<EmployerRecommendation>> GetRecommendation(int userID)
        {
            var recommendation = await _employerRecommendations.GetMulti(x => x.UserId == userID, new string[] { "EmployerRecommendationTrans" });

            return recommendation;
        }

        public async Task<bool> DeleteRecommendation(int recommendationId)
        {
            try
            {
                var recommendation = await _employerRecommendations.GetSingleByCondition(n => n.Id == recommendationId, new string[] { "EmployerRecommendationTrans" });

                if (recommendation == null)
                    return false;
                _employerRecommendationsTrans.DeleteMulti(n => n.EmployerRecommendationId == recommendation.Id);
                _employerRecommendations.Delete(recommendation);

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateRecommendation(RecommendationEmployerNewBindingModel lstModel, int langCode, int userId)
        {
            try
            {
                if (lstModel.Id == null)
                {
                    return false;
                }

                var recommendation = _employerRecommendations.GetSingleById(lstModel.Id.Value);

                recommendation.Tel = lstModel.ContactPersonTel;
                recommendation.UserId = userId;

                _employerRecommendations.Update(recommendation);

                var recommendationTrans = await _employerRecommendationsTrans.GetSingleByCondition(x => x.EmployerRecommendationId == lstModel.Id.Value && x.LanguageId == langCode);

                if (recommendationTrans != null)
                {
                    recommendationTrans.CompanyName = lstModel.CompanyName;
                    recommendationTrans.ContactPerson = lstModel.ContactPersonName;
                    recommendationTrans.LanguageId = langCode;
                    recommendationTrans.Position = lstModel.ContactPersonPosition;
                    recommendationTrans.Email = lstModel.ContactPersonEmail;

                    _employerRecommendationsTrans.Update(recommendationTrans);
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

        public async Task<EmployerRecommendation> GetRecommendationById(int recommendationId)
        {
            var experience = await _employerRecommendations.GetSingleByCondition(n => n.Id == recommendationId, new string[] { "EmployerRecommendationTrans" });

            return experience;
        }
    }
}
