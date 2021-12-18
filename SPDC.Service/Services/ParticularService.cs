using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Common;
using SPDC.Model.BindingModels;
using static SPDC.Common.StaticConfig;

namespace SPDC.Service.Services
{
    public interface IParticularService
    {
        bool Add(Particular model);
        Task<ApplicationUser> GetUserByHKIDOrPassportNo(ForgotLoginEmailViewModel model);
        Task<bool> IsExistHKidOrPassportNo(int userId, string hkId = null, string passportNo = null);
        Task<ParticularBindingModel> GetParticularByUserId(int userId, int langCode);
        Task<ParticularBindingModel> GetParticularByUserTempId(int userId, int langCode);
        Task<bool> UpdateParticularById(ParticularBindingModel model, int langCode, string hkidEncrypt, string passportEncrypt, string mobileEncrypt);
        Task<bool> UpdateParticularByIdTempToTable(int particularId);
        Task<bool> UpdateParticularByIdTemp(ParticularBindingModel model, int langCode, string hkidEncrypt, string passportEncrypt, string mobileEncrypt);
        Task<QualificationViewModel> GetQualificationByUserId(int id, int langCode);
        Task<QualificationViewModel> GetQualificationByUserIdTemp(int id, int langCode);
        Task<bool> UpdateQualifications(QualificationViewModel model, int langCode);
        Task<bool> UpdateQualificationsTempToTable(QualificationViewModel model, int langCode);
        Task<bool> UpdateQualificationsTemp(QualificationViewModel model, int langCode);
        Task<bool> IsExistMobileNumber(string mobileNumber, int userId = 0);

        Task<bool> deleteUpdateParticularTemp(ParticularBindingModel model);
        Task<bool> DeleterQualificationsTemp(QualificationViewModel model);
        Task<ParticularBindingModel> GetParticularByHKIDOrPassportId(ApplicationBindingModel model, int langCode);
    }
    public class ParticularService : IParticularService
    {
        IParticularRepository _particularRepository;
        IParticularTranRepository _particularTranRepository;
        IParticularTempRepository _particularTempRepository;
        IParticularTempTranRepository _particularTempTranRepository;
        IUserRepository _userRepository;
        IQualificationRepository _qualificationRepository;
        IQualificationTransRepository _qualificationTransRepository;
        IQualificationTempRepository _qualificationTempRepository;
        IQualificationTransTempRepository _qualificationTransTempRepository;
        IUnitOfWork _unitOfWork;

        public ParticularService(IUnitOfWork unitOfWork, IUserRepository userRepository, IParticularRepository particularRepository, IParticularTempRepository particularTempRepository, IQualificationRepository qualificationRepository, IQualificationTempRepository qualificationTempRepository, IQualificationTransRepository qualificationTransRepository,
            IQualificationTransTempRepository qualificationTransTempRepository, IParticularTempTranRepository particularTempTranRepository, IParticularTranRepository particularTranRepository)
        {
            _particularRepository = particularRepository;
            _particularTempRepository = particularTempRepository;
            _particularTempTranRepository = particularTempTranRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _qualificationTransRepository = qualificationTransRepository;
            _qualificationRepository = qualificationRepository;
            _qualificationTransTempRepository = qualificationTransTempRepository;
            _qualificationTempRepository = qualificationTempRepository;
            _particularTranRepository = particularTranRepository;
        }

        public bool Add(Particular model)
        {
            try
            {
                _particularRepository.Add(model);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsExistHKidOrPassportNo(int userId, string hkId = null, string passportNo = null)
        {
            if (String.IsNullOrEmpty(passportNo))
            {
                var user = await _particularRepository.GetSingleByCondition(n => n.HKIDNoEncrypted.Equals(hkId));
                return (user != null && user.Id != userId) ? true : false;
            }
            else
            {
                var user = await _particularRepository.GetSingleByCondition(n => n.PassportNoEncrypted.Equals(passportNo));
                return (user != null && user.Id != userId) ? true : false;
            }
        }

        public async Task<ApplicationUser> GetUserByHKIDOrPassportNo(ForgotLoginEmailViewModel model)
        {
            var dataEncode = !String.IsNullOrEmpty(model.HKID) ? EncryptUtilities.EncryptAes256(model.HKID) : EncryptUtilities.EncryptAes256(model.PassportNo);
            var stringEncoded = EncryptUtilities.GetEncryptedString(dataEncode);
            var user = !String.IsNullOrEmpty(model.HKID) ? await _particularRepository.GetSingleByCondition(n => n.HKIDNoEncrypted.Equals(stringEncoded), new string[] { "User" }) : await _particularRepository.GetSingleByCondition(n => n.PassportNoEncrypted.Equals(stringEncoded), new string[] { "User" });
            var numberEncoded = EncryptUtilities.GetEncryptedString(EncryptUtilities.EncryptAes256(model.MobileNumber));
            if (user != null && user.MobileNumberEncrypted.Equals(numberEncoded) && DateTime.Compare(new DateTime(model.DateOfBirth.Year, model.DateOfBirth.Month, model.DateOfBirth.Day), new DateTime(user.DateOfBirth.Year, user.DateOfBirth.Month, user.DateOfBirth.Day)) == 0)
            {
                user.User.OtherEmail = model.OtherContactEmail;
                _particularRepository.Update(user);
                _unitOfWork.Commit();
                return user.User;
            }

            return null;
        }

        public async Task<ParticularBindingModel> GetParticularByUserId(int userId, int langCode)
        {
            var par = await _particularRepository.GetSingleByCondition(n => n.Id == userId, new string[] { "ParticularTrans" });
            return EntityHelpers.ToParticularBindingModel(par, langCode);
        }

        public async Task<ParticularBindingModel> GetParticularByUserTempId(int userId, int langCode)
        {
            var par = await _particularTempRepository.GetSingleByCondition(n => n.Id == userId, new string[] { "ParticularTempTrans" });
            if (par == null)
            {
                return null;
            }
            else
            {
                return EntityHelpers.ToParticularBindingModelTemp(par, langCode);
            }

        }

        public async Task<bool> UpdateParticularById(ParticularBindingModel model, int langCode, string hkidEncrypt, string passportEncrypt, string mobileEncrypt)
        {
            var parModel = await _particularRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "ParticularTrans" });
            if (parModel == null)
                return false;
            try
            {
                parModel.ConvertParticularModel(model, langCode, hkidEncrypt, passportEncrypt, mobileEncrypt);
                _particularRepository.Update(parModel);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }


        public async Task<bool> UpdateParticularByIdTempToTable(int particularId)
        {
            var tempParModel = await _particularTempRepository.GetSingleByCondition(x => x.Id == particularId, new string[] { "ParticularTempTrans" });

            var parModel = await _particularRepository.GetSingleByCondition(x => x.Id == particularId, new string[] { "ParticularTrans" });
            if (parModel == null || tempParModel == null)
                return false;
            try
            {
                _particularTranRepository.DeleteMulti(x => x.ParticularId == particularId);
                parModel = parModel.ConvertParticularTempToParticular(tempParModel);
                _particularRepository.Update(parModel);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public async Task<bool> UpdateParticularByIdTemp(ParticularBindingModel parti, int langCode, string hkidEncrypt, string passportEncrypt, string mobileEncrypt)
        {
            var parModel = await _particularTempRepository.GetSingleByCondition(x => x.Id == parti.Id, new string[] { "ParticularTempTrans" });
            //var parModel = await _particularTempRepository.GetSingleByCondition(x => x.Id == parti.Id && x.ApplicationId == parti.ApplicationId, new string[] { "ParticularTempTrans" });
            try
            {
                if (parModel == null)
                {
                    var model = new ParticularTemp();

                    model.Id = parti.Id;
                    model.SurnameEN = parti.SurnameEN;
                    model.GivenNameEN = parti.GivenNameEN;
                    model.SurnameCN = parti.SurnameCN;
                    model.GivenNameCN = parti.GivenNameCN;
                    model.DateOfBirth = parti.DateOfBirth;
                    model.Gender = parti.Gender;
                    model.HKIDNo = !String.IsNullOrEmpty(parti.HKIDNo) ? EncryptUtilities.EncryptAes256(parti.HKIDNo) : null;
                    model.PassportNo = !String.IsNullOrEmpty(parti.PassportNo) ? EncryptUtilities.EncryptAes256(parti.PassportNo) : null;

                    //Convert string to datetime?
                    DateTime? passportDateToAsign = null;
                    var expiredDateConvert = DateTime.Now;
                    passportDateToAsign = parti.PassportExpiredDate;
                    model.PassportExpiryDate = passportDateToAsign;
                    model.MobileNumber = !String.IsNullOrEmpty(parti.MobileNumber) ? EncryptUtilities.EncryptAes256(parti.MobileNumber) : null;
                    model.MobileNumberPrefix = parti.MobileNumberPrefix;
                    model.TelNo = parti.TelNo;
                    model.FaxNo = parti.FaxNo;
                    model.RegionEN = parti.RegionEN;
                    model.RegionCN = parti.RegionCN;
                    model.DistrictEN = parti.DistrictEN;
                    model.DistrictCN = parti.DistrictCN;
                    model.StreetNumberEN = parti.StreetNumberEN;
                    model.StreetNumberCN = parti.StreetNumberCN;
                    model.StreetRoadEN = parti.StreetRoadEN;
                    model.StreetRoadCN = parti.StreetRoadCN;
                    model.EstateQuartersVillageEN = parti.EstateQuartersVillageEN;
                    model.EstateQuartersVillageCN = parti.EstateQuartersVillageCN;
                    model.BuildingEN = parti.BuildingEN;
                    model.BuildingCN = parti.BuildingCN;
                    model.FloorEN = parti.FloorEN;
                    model.FloorCN = parti.FloorCN;
                    model.RmFtUnitEN = parti.RmFtUnitEN;
                    model.RmFtUnitCN = parti.RmFtUnitCN;
                    model.EducationLevelEN = parti.EducationLevelEN;
                    model.EducationLevelCN = parti.EducationLevelCN;
                    model.Honorific = parti.Honorific;

                    model.SameAddress = parti.SameAddress;
                    model.ResidentialRegionEN = parti.ResidentialRegionEN;
                    model.ResidentialDistrictEN = parti.ResidentialDistrictEN;
                    model.ResidentialStreetNumberEN = parti.ResidentialStreetNumberEN;
                    model.ResidentialStreetRoadEN = parti.ResidentialStreetRoadEN;
                    model.ResidentialEstateQuartersVillageEN = parti.ResidentialEstateQuartersVillageEN;
                    model.ResidentialBuildingEN = parti.ResidentialBuildingEN;
                    model.ResidentialFloorEN = parti.ResidentialFloorEN;
                    model.ResidentialRmFtUnitEN = parti.ResidentialRmFtUnitEN;
                    model.ResidentialRegionCN = parti.ResidentialRegionCN;
                    model.ResidentialDistrictCN = parti.ResidentialDistrictCN;
                    model.ResidentialStreetNumberCN = parti.ResidentialStreetNumberCN;
                    model.ResidentialStreetRoadCN = parti.ResidentialStreetRoadCN;
                    model.ResidentialEstateQuartersVillageCN = parti.ResidentialEstateQuartersVillageCN;
                    model.ResidentialBuildingCN = parti.ResidentialBuildingCN;
                    model.ResidentialFloorCN = parti.ResidentialFloorCN;
                    model.ResidentialRmFtUnitCN = parti.ResidentialRmFtUnitCN;
                    model.IsPrimamy = parti.IsPrimamy;
                    model.IsSecondary = parti.IsSecondary;
                    model.IsTechInst = parti.IsTechInst;
                    model.IsUniversityCollege = parti.IsUniversityCollege;
                    model.HKIDNoEncrypted = hkidEncrypt;
                    model.PassportNoEncrypted = passportEncrypt;
                    model.MobileNumberEncrypted = mobileEncrypt;

                    if (model.ParticularTempTrans.Count < 3)
                    {
                        if (model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 1) == null)
                        {
                            model.ParticularTempTrans.Add(new ParticularTempTran()
                            {
                                LanguageId = 1,
                                PresentEmployer = parti.PresentEmployer,
                                Position = parti.Position
                            });
                        }

                        if (model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 2) == null)
                        {
                            model.ParticularTempTrans.Add(new ParticularTempTran()
                            {
                                LanguageId = 2,
                                PresentEmployer = parti.PresentEmployer,
                                Position = parti.Position
                            });
                        }

                        if (model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 3) == null)
                        {
                            model.ParticularTempTrans.Add(new ParticularTempTran()
                            {
                                LanguageId = 3,
                                PresentEmployer = parti.PresentEmployer,
                                Position = parti.Position
                            });
                        }
                    }
                    else
                    {
                        model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 1).PresentEmployer = parti.PresentEmployer;
                        model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 1).Position = parti.Position;
                        model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 2).PresentEmployer = parti.PresentEmployer;
                        model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 2).Position = parti.Position;
                        model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 3).PresentEmployer = parti.PresentEmployer;
                        model.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == 3).Position = parti.Position;
                    }
                    var result = _particularTempRepository.Add(model);
                    _unitOfWork.Commit();
                    return true;
                }
                else
                {
                    parModel.ConvertParticularModelTempEdit(parti, langCode, hkidEncrypt, passportEncrypt, mobileEncrypt);
                    _particularTempRepository.Update(parModel);
                    _unitOfWork.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }


        public async Task<bool> deleteUpdateParticularTemp(ParticularBindingModel model)
        {
            try
            {
                var parModel = await _particularTempRepository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "ParticularTempTrans" });
                var parModelTran = await _particularTempTranRepository.GetMulti(x => x.ParticularTemp.Id == model.Id, new string[] { "ParticularTemp" });
                foreach (var item in parModelTran)
                {
                    _particularTempTranRepository.DeleteMulti(n => n.Id == item.Id);
                    _unitOfWork.Commit();
                }
                _particularTempRepository.Delete(parModel);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }



        public async Task<QualificationViewModel> GetQualificationByUserId(int id, int langCode)
        {
            QualificationViewModel qualView = new QualificationViewModel();
            var particular = await _particularRepository.GetSingleByCondition(n => n.Id == id, new string[] { "ParticularTrans" });
            var qual = await _qualificationRepository.GetMulti(n => n.UserId == id, new string[] { "QualificationsTrans" });
            if (particular == null)
            {
                return null;
            }

            particular.ConvertToQualificationViewModel(qualView);

            if (qual != null)
            {
                for (int i = 0; i < qual.Count; i++)
                {
                    SubQualificationViewModel subQual = null;
                    subQual = qual[i].ConvertToSubQualificationViewModel(subQual);
                    if (subQual != null)
                    {
                        qualView.ListSubQualifications.Add(subQual);
                    }
                }
            }

            return qualView;
        }

        public async Task<QualificationViewModel> GetQualificationByUserIdTemp(int id, int langCode)
        {
            QualificationViewModel qualView = new QualificationViewModel();
            var particular = await _particularTempRepository.GetSingleByCondition(n => n.Id == id, new string[] { "ParticularTempTrans" });
            var qual = await _qualificationTempRepository.GetMulti(n => n.UserId == id, new string[] { "QualificationTempTrans" });
            if (particular == null)
            {
                return null;
            }

            particular.ConvertToQualificationViewModelTemp(qualView);

            if (qual != null)
            {
                for (int i = 0; i < qual.Count; i++)
                {
                    SubQualificationViewModel subQual = null;
                    subQual = qual[i].ConvertToSubQualificationViewModelTemp(subQual);
                    if (subQual != null)
                    {
                        qualView.ListSubQualifications.Add(subQual);
                    }
                }
            }

            return qualView;
        }

        public async Task<bool> UpdateQualifications(QualificationViewModel model, int langCode)
        {
            try
            {
                var particular = await _particularRepository.GetSingleByCondition(n => n.Id == model.Id, new string[] { "ParticularTrans" });
                if (particular == null)
                    return false;

                particular.RelatedQualifications1Check = model.RelatedQualifications1Check;
                particular.RelatedQualifications2Check = model.RelatedQualifications2Check;
                particular.RelatedQualifications2Year = model.RelatedQualifications2Year;
                particular.RelatedQualifications3Check = model.RelatedQualifications3Check;
                if (particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN) != null)
                {
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                if (particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN) != null)
                {
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                if (particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK) != null)
                {
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                else
                {
                    particular.ParticularTrans.Add(new ParticularTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.EN,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                    particular.ParticularTrans.Add(new ParticularTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.CN,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                    particular.ParticularTrans.Add(new ParticularTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.HK,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                }

                var qualification = await _qualificationRepository.GetMulti(n => n.UserId == model.Id, new string[] { "QualificationsTrans" });

                var listQualificationToInsert = model.ListSubQualifications.Where(n => n.Id == 0 || n.Id == null);
                var listQualificationToUpdate = qualification.Where(n => model.ListSubQualifications.Any(m => m.Id == n.Id));
                var listQualificationToDelete = qualification.Where(n => !listQualificationToUpdate.Select(p => p.Id).Contains(n.Id));


                foreach (var item in listQualificationToDelete)
                {
                    _qualificationTransRepository.DeleteMulti(n => n.QualificationId == item.Id);
                    _qualificationRepository.Delete(item);
                }

                foreach (var item in listQualificationToUpdate)
                {
                    _qualificationRepository.Update(EntityHelpers.ToQualification(model.ListSubQualifications.Single(x => x.Id == item.Id), item, model.Id, langCode));
                }

                foreach (var item in listQualificationToInsert)
                {
                    _qualificationRepository.Add(EntityHelpers.ToQualification(item, null, model.Id, langCode));
                }
                _particularRepository.Update(particular);

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateQualificationsTempToTable(QualificationViewModel model, int langCode)
        {
            try
            {
                var particular = await _particularRepository.GetSingleByCondition(n => n.Id == model.Id, new string[] { "ParticularTrans" });
                if (particular == null)
                    return false;

                particular.RelatedQualifications1Check = model.RelatedQualifications1Check;
                particular.RelatedQualifications2Check = model.RelatedQualifications2Check;
                particular.RelatedQualifications2Year = model.RelatedQualifications2Year;
                particular.RelatedQualifications3Check = model.RelatedQualifications3Check;
                if (particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN) != null)
                {
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                if (particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN) != null)
                {
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                if (particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK) != null)
                {
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                else
                {
                    particular.ParticularTrans.Add(new ParticularTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.EN,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                    particular.ParticularTrans.Add(new ParticularTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.CN,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                    particular.ParticularTrans.Add(new ParticularTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.HK,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                }
                var qualification = await _qualificationRepository.GetMulti(n => n.UserId == model.Id, new string[] { "QualificationsTrans" });

                var listQualificationToInsert = model.ListSubQualifications;
                var listQualificationToDelete = qualification;

                foreach (var item in listQualificationToDelete)
                {
                    _qualificationTransRepository.DeleteMulti(n => n.QualificationId == item.Id);
                    _qualificationRepository.Delete(item);
                    _unitOfWork.Commit();
                }

                foreach (var item in listQualificationToInsert)
                {
                    _qualificationRepository.Add(EntityHelpers.ToQualification(item, null, model.Id, langCode));
                }
                _particularRepository.Update(particular);

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateQualificationsTemp(QualificationViewModel model, int langCode)
        {
            try
            {
                var particular = await _particularTempRepository.GetSingleByCondition(n => n.Id == model.Id, new string[] { "ParticularTempTrans" });
                if (particular == null)
                    return false;

                particular.RelatedQualifications1Check = model.RelatedQualifications1Check;
                particular.RelatedQualifications2Check = model.RelatedQualifications2Check;
                particular.RelatedQualifications2Year = model.RelatedQualifications2Year;
                particular.RelatedQualifications3Check = model.RelatedQualifications3Check;
                if (particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN) != null)
                {
                    particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.EN).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                if (particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN) != null)
                {
                    particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.CN).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                if (particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK) != null)
                {
                    particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK).RelatedQualifications1Text = model.RelatedQualifications1Text;
                    particular.ParticularTempTrans.FirstOrDefault(n => n.LanguageId == (int)LanguageCode.HK).RelatedQualifications2Text = model.RelatedQualifications2Text;
                }
                else
                {
                    particular.ParticularTempTrans.Add(new ParticularTempTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.EN,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                    particular.ParticularTempTrans.Add(new ParticularTempTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.CN,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                    particular.ParticularTempTrans.Add(new ParticularTempTran()
                    {
                        ParticularId = particular.Id,
                        LanguageId = (int)LanguageCode.HK,
                        RelatedQualifications1Text = model.RelatedQualifications1Text,
                        RelatedQualifications2Text = model.RelatedQualifications2Text
                    });
                }

                var listQualificationToInsert = model.ListSubQualifications;
                var qualification = await _qualificationTempRepository.GetMulti(n => n.UserId == model.Id, new string[] { "QualificationTempTrans" });
                if (qualification.Count == 0)
                {

                    foreach (var item in listQualificationToInsert)
                    {
                        _qualificationTempRepository.Add(EntityHelpers.ToQualificationTemp(item, null, model.Id, langCode, model.ApplicationId));
                    }
                    _particularTempRepository.Update(particular);

                    _unitOfWork.Commit();
                    return true;
                }
                else
                {
                    var listQualificationToDelete = qualification;
                    foreach (var item in listQualificationToDelete)
                    {
                        _qualificationTransTempRepository.DeleteMulti(n => n.QualificationTemp.Id == item.Id);
                        _qualificationTempRepository.Delete(item);
                    }
                    _unitOfWork.Commit();
                    foreach (var item in listQualificationToInsert)
                    {
                        item.Id = 0;
                        _qualificationTempRepository.Add(EntityHelpers.ToQualificationTemp(item, null, model.Id, langCode, model.ApplicationId));
                    }
                    _particularTempRepository.Update(particular);

                    _unitOfWork.Commit();
                    return true;
                }


            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> DeleterQualificationsTemp(QualificationViewModel model)
        {
            try
            {
                var qualification = await _qualificationTempRepository.GetMulti(n => n.UserId == model.Id, new string[] { "QualificationTempTrans" });
                foreach (var item in qualification)
                {
                    _qualificationTransTempRepository.DeleteMulti(n => n.QualificationTemp.Id == item.Id);
                    _qualificationTempRepository.Delete(item);
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


        public async Task<bool> IsExistMobileNumber(string mobileNumberEncryptedString, int userId = 0)
        {
            var model = await _particularRepository.GetSingleByCondition(x => x.MobileNumberEncrypted.Equals(mobileNumberEncryptedString));

            if (userId != 0)
            {
                var particular = await _particularRepository.GetSingleByCondition(x => x.Id == userId);
                return model != null && !particular.MobileNumberEncrypted.Equals(model.MobileNumberEncrypted) ? true : false;
            }
            return model != null ? true : false;
        }

        public async Task<ParticularBindingModel> GetParticularByHKIDOrPassportId(ApplicationBindingModel model, int langCode)
        {

            var dataEncryptToCompare = !string.IsNullOrEmpty(model.PassportID) ? EncryptUtilities.EncryptAes256(model.PassportID) : EncryptUtilities.EncryptAes256(model.HKIdNo);
            var dataToCompare = dataEncryptToCompare != null ? EncryptUtilities.GetEncryptedString(dataEncryptToCompare) : null;

            var result = await _particularRepository.GetSingleByCondition((x => !string.IsNullOrEmpty(model.PassportID) ? x.PassportNoEncrypted.Equals(dataToCompare) : x.HKIDNoEncrypted.Equals(dataToCompare))
                                               , new string[] { "ParticularTrans" });

            return EntityHelpers.ToParticularBindingModel(result, langCode);

        }
    }
}
