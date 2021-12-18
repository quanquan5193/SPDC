using SPDC.Common.Enums;
using SPDC.Model.BindingModels.Invoice;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.AdminPrivileges;
using SPDC.Model.ViewModels.Application;
using SPDC.Model.ViewModels.ApplicationManagement;
using SPDC.Model.ViewModels.Approval;
using SPDC.Model.ViewModels.Assessment;
using SPDC.Model.ViewModels.BatchPayment;
using SPDC.Model.ViewModels.Invoice;
using SPDC.Model.ViewModels.MakeupClass;
using SPDC.Model.ViewModels.StudentAndClassManagement;
using SPDC.Model.ViewModels.Transaction;
using SPDC.Model.ViewModels.Transaction.RefundTransaction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPDC.Common.StaticConfig;


namespace SPDC.Common
{
    public static class EntityHelpers
    {
        public static void ConvertParticularModel(this Particular model, ParticularBindingModel parti, int langCode, string hkidEncrypt, string passportEncrypt, string mobileEncrypt)
        {
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
            //var isCovnerted = DateTime.TryParse(parti.PassportExpiredDate, out expiredDateConvert);
            //if (isCovnerted)
            //{
            //    passportDateToAsign = expiredDateConvert;
            //}

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

            if (model.ParticularTrans.Count < 3)
            {
                if (model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 1) == null)
                {
                    model.ParticularTrans.Add(new ParticularTran()
                    {
                        LanguageId = 1,
                        PresentEmployer = parti.PresentEmployer,
                        Position = parti.Position
                    });
                }

                if (model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 2) == null)
                {
                    model.ParticularTrans.Add(new ParticularTran()
                    {
                        LanguageId = 2,
                        PresentEmployer = parti.PresentEmployer,
                        Position = parti.Position
                    });
                }

                if (model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 3) == null)
                {
                    model.ParticularTrans.Add(new ParticularTran()
                    {
                        LanguageId = 3,
                        PresentEmployer = parti.PresentEmployer,
                        Position = parti.Position
                    });
                }
            }
            else
            {
                model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 1).PresentEmployer = parti.PresentEmployer;
                model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 1).Position = parti.Position;
                model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 2).PresentEmployer = parti.PresentEmployer;
                model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 2).Position = parti.Position;
                model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 3).PresentEmployer = parti.PresentEmployer;
                model.ParticularTrans.FirstOrDefault(x => x.LanguageId == 3).Position = parti.Position;
            }

            model.HKIDNoEncrypted = hkidEncrypt;
            model.PassportNoEncrypted = passportEncrypt;
            model.MobileNumberEncrypted = mobileEncrypt;
        }


        public static void ConvertParticularModelTempEdit(this ParticularTemp model, ParticularBindingModel parti, int langCode, string hkidEncrypt, string passportEncrypt, string mobileEncrypt)
        {
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

            model.HKIDNoEncrypted = hkidEncrypt;
            model.PassportNoEncrypted = passportEncrypt;
            model.MobileNumberEncrypted = mobileEncrypt;
        }



        public static void ConvertToQualificationViewModel(this Particular model, QualificationViewModel qual)
        {
            qual.Id = model.Id;
            qual.RelatedQualifications1Check = model.RelatedQualifications1Check;
            qual.RelatedQualifications2Check = model.RelatedQualifications2Check;
            qual.RelatedQualifications3Check = model.RelatedQualifications3Check;
            qual.RelatedQualifications2Year = qual.RelatedQualifications2Check == true ? model.RelatedQualifications2Year : null;
            qual.RelatedQualifications1Text = qual.RelatedQualifications1Check == true ? model.ParticularTrans.FirstOrDefault()?.RelatedQualifications1Text : null;
            qual.RelatedQualifications2Text = qual.RelatedQualifications2Check == true ? model.ParticularTrans.FirstOrDefault()?.RelatedQualifications2Text : null;
        }

        public static void ConvertToQualificationViewModelTemp(this ParticularTemp model, QualificationViewModel qual)
        {
            qual.Id = model.Id;
            qual.RelatedQualifications1Check = model.RelatedQualifications1Check;
            qual.RelatedQualifications2Check = model.RelatedQualifications2Check;
            qual.RelatedQualifications3Check = model.RelatedQualifications3Check;
            qual.RelatedQualifications2Year = qual.RelatedQualifications2Check == true ? model.RelatedQualifications2Year : null;
            qual.RelatedQualifications1Text = qual.RelatedQualifications1Check == true ? model.ParticularTempTrans.FirstOrDefault()?.RelatedQualifications1Text : null;
            qual.RelatedQualifications2Text = qual.RelatedQualifications2Check == true ? model.ParticularTempTrans.FirstOrDefault()?.RelatedQualifications2Text : null;
        }

        public static SubQualificationViewModel ConvertToSubQualificationViewModel(this Qualification model, SubQualificationViewModel subQual)
        {
            var issuingAuthority = model.QualificationsTrans.FirstOrDefault()?.IssuingAuthority;
            var levelAttained = model.QualificationsTrans.FirstOrDefault()?.LevelAttained;
            if (string.IsNullOrEmpty(issuingAuthority) && string.IsNullOrEmpty(levelAttained))
            {
                return null;
            }

            subQual = new SubQualificationViewModel();
            subQual.Id = model.Id;
            subQual.DateObtained = model.DateObtained;
            subQual.IssuingAuthority = issuingAuthority;
            subQual.LevelAttained = levelAttained;

            return subQual;
        }

        public static SubQualificationViewModel ConvertToSubQualificationViewModelTemp(this QualificationTemp model, SubQualificationViewModel subQual)
        {
            var issuingAuthority = model.QualificationTempTrans.FirstOrDefault()?.IssuingAuthority;
            var levelAttained = model.QualificationTempTrans.FirstOrDefault()?.LevelAttained;
            if (string.IsNullOrEmpty(issuingAuthority) && string.IsNullOrEmpty(levelAttained))
            {
                return null;
            }

            subQual = new SubQualificationViewModel();
            subQual.Id = model.Id;
            subQual.DateObtained = model.DateObtained;
            subQual.IssuingAuthority = issuingAuthority;
            subQual.LevelAttained = levelAttained;

            return subQual;
        }

        public static void ConvertToWorkExperienceBindingModel(this WorkExperience model, WorkExperienceBindingModel workEx)
        {
            workEx.Id = model.Id;
            workEx.UserId = model.UserId;
            workEx.FromYear = model.FromYear;
            workEx.ToYear = model.ToYear;
            workEx.BIMRelated = model.BIMRelated;
            workEx.ClassifyWorkingExperience = model.ClassifyWorkingExperience;
            workEx.Location = model.WorkExperienceTrans.FirstOrDefault().Location;
            workEx.Position = model.WorkExperienceTrans.FirstOrDefault().Position;
            workEx.JobNature = model.WorkExperienceTrans.FirstOrDefault().JobNature;
        }


        public static void ConvertToWorkExperienceBindingModelTemp(this WorkExperienceTemp model, WorkExperienceBindingModel workEx)
        {
            workEx.Id = model.Id;
            workEx.UserId = model.UserId;
            workEx.FromYear = model.FromYear;
            workEx.ToYear = model.ToYear;
            workEx.BIMRelated = model.BIMRelated;
            workEx.ClassifyWorkingExperience = model.ClassifyWorkingExperience;
            workEx.Location = model.WorkExperienceTempTrans.FirstOrDefault().Location;
            workEx.Position = model.WorkExperienceTempTrans.FirstOrDefault().Position;
            workEx.JobNature = model.WorkExperienceTempTrans.FirstOrDefault().JobNature;
        }

        public static void ConvertToRecommendationEmployerBindingModel(this EmployerRecommendation model, RecommendationEmployerBindingModel recommend)
        {
            recommend.Id = model.Id;
            recommend.Tel = model.Tel;
            recommend.UserId = model.UserId;
            recommend.CompanyName = model.EmployerRecommendationTrans.FirstOrDefault()?.CompanyName;
            recommend.ContactPersonName = model.EmployerRecommendationTrans.FirstOrDefault()?.ContactPerson;
            recommend.ContactPersonPosition = model.EmployerRecommendationTrans.FirstOrDefault()?.Position;
            recommend.ContactPersonEmail = model.EmployerRecommendationTrans.FirstOrDefault()?.Email;
        }

        public static void ConvertToRecommendationEmployerBindingModelTemp(this EmployerRecommendationTemp model, RecommendationEmployerBindingModel recommend)
        {
            recommend.Id = model.Id;
            recommend.Tel = model.Tel;
            recommend.UserId = model.UserId;
            recommend.CompanyName = model.EmployerRecommendationTempTrans.FirstOrDefault()?.CompanyName;
            recommend.ContactPersonName = model.EmployerRecommendationTempTrans.FirstOrDefault()?.ContactPerson;
            recommend.ContactPersonPosition = model.EmployerRecommendationTempTrans.FirstOrDefault()?.Position;
            recommend.ContactPersonEmail = model.EmployerRecommendationTempTrans.FirstOrDefault()?.Email;
        }

        public static Document ToDocument(string path, string fileType, string fileName, int userId)
        {
            Document doc = new Document();
            doc.Url = path;
            doc.ContentType = fileType;
            doc.FileName = fileName;
            doc.ModifiedDate = DateTime.Now;
            doc.UserDocuments.Add(new UserDocument()
            {
                UserId = userId,
            });
            return doc;
        }

        public static Document ToDocumentForTempUser(string path, string fileType, string fileName, int userId)
        {
            Document doc = new Document();
            doc.Url = path;
            doc.ContentType = fileType;
            doc.FileName = fileName;
            doc.ModifiedDate = DateTime.Now;
            doc.UserDocumentsTemps.Add(new UserDocumentTemp()
            {
                UserId = userId,
            });
            return doc;
        }

        public static Document ToDocumentTemp(string path, string fileType, string fileName, int userId)
        {
            Document doc = new Document();
            doc.Url = path;
            doc.ContentType = fileType;
            doc.FileName = fileName;
            doc.ModifiedDate = DateTime.Now;
            doc.UserDocumentsTemps.Add(new UserDocumentTemp()
            {
                UserId = userId,
            });
            return doc;
        }

        public static EmployerRecommendation ToRecommendationEmployer(RecommendationEmployerBindingModel reEmployer, EmployerRecommendation model, int userId, int langCode)
        {
            if (model == null)
            {
                model = new EmployerRecommendation();
                model.UserId = userId;
            }
            model.Tel = reEmployer.Tel;
            if (model.EmployerRecommendationTrans.FirstOrDefault(x => x.LanguageId == langCode) == null)
            {
                model.EmployerRecommendationTrans.Add(new EmployerRecommendationTran()
                {
                    LanguageId = langCode,
                    CompanyName = reEmployer.CompanyName,
                    ContactPerson = reEmployer.ContactPersonName,
                    Position = reEmployer.ContactPersonPosition,
                    Email = reEmployer.ContactPersonEmail

                });
            }
            else
            {
                var trans = model.EmployerRecommendationTrans.FirstOrDefault(x => x.LanguageId == langCode);
                trans.CompanyName = reEmployer.CompanyName;
                trans.ContactPerson = reEmployer.ContactPersonName;
                trans.Position = reEmployer.ContactPersonPosition;
                trans.Email = reEmployer.ContactPersonEmail;
            }
            return model;
        }

        public static EmployerRecommendationTemp ToRecommendationEmployerTemp(RecommendationEmployerBindingModel reEmployer, EmployerRecommendationTemp model, int userId, int langCode)
        {
            if (model == null)
            {
                model = new EmployerRecommendationTemp();
                model.UserId = userId;
            }
            model.Tel = reEmployer.Tel;
            if (model.EmployerRecommendationTempTrans.FirstOrDefault(x => x.LanguageId == langCode) == null)
            {
                model.EmployerRecommendationTempTrans.Add(new EmployerRecommendationTempTran()
                {
                    LanguageId = langCode,
                    CompanyName = reEmployer.CompanyName,
                    ContactPerson = reEmployer.ContactPersonName,
                    Position = reEmployer.ContactPersonPosition,
                    Email = reEmployer.ContactPersonEmail

                });
            }
            else
            {
                var trans = model.EmployerRecommendationTempTrans.FirstOrDefault(x => x.LanguageId == langCode);
                trans.CompanyName = reEmployer.CompanyName;
                trans.ContactPerson = reEmployer.ContactPersonName;
                trans.Position = reEmployer.ContactPersonPosition;
                trans.Email = reEmployer.ContactPersonEmail;
            }
            return model;
        }

        public static ParticularBindingModel ToParticularBindingModel(Particular par, int langCode)
        {
            ParticularBindingModel model = new ParticularBindingModel();
            model.Id = par.Id;
            model.SurnameEN = par.SurnameEN;
            model.GivenNameEN = par.GivenNameEN;
            model.SurnameCN = par.SurnameCN;
            model.GivenNameCN = par.GivenNameCN;
            model.DateOfBirth = par.DateOfBirth;
            model.Age = (int)(DateTime.Now - par.DateOfBirth).TotalDays / 365;
            model.Gender = par.Gender ?? default;
            model.HKIDNo = par.HKIDNo != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(par.HKIDNo)) : null;
            model.PassportNo = par.PassportNo != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(par.PassportNo)) : null;
            //model.PassportExpiredDate = par.PassportExpiryDate?.ToString("dd-MM-yyyy");
            model.PassportExpiredDate = par.PassportExpiryDate;
            model.MobileNumber = par.MobileNumber != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(par.MobileNumber)) : null;
            model.MobileNumberPrefix = par.MobileNumberPrefix;
            model.TelNo = par.TelNo;
            model.FaxNo = par.FaxNo;
            model.RegionEN = par.RegionEN;
            model.RegionCN = par.RegionCN;
            model.DistrictEN = par.DistrictEN;
            model.DistrictCN = par.DistrictCN;
            model.StreetNumberEN = par.StreetNumberEN;
            model.StreetNumberCN = par.StreetNumberCN;
            model.StreetRoadEN = par.StreetRoadEN;
            model.StreetRoadCN = par.StreetRoadCN;
            model.EstateQuartersVillageEN = par.EstateQuartersVillageEN;
            model.EstateQuartersVillageCN = par.EstateQuartersVillageCN;
            model.BuildingEN = par.BuildingEN;
            model.BuildingCN = par.BuildingCN;
            model.FloorEN = par.FloorEN;
            model.FloorCN = par.FloorCN;
            model.RmFtUnitEN = par.RmFtUnitEN;
            model.RmFtUnitCN = par.RmFtUnitCN;
            model.EducationLevelEN = par.EducationLevelEN;
            model.EducationLevelCN = par.EducationLevelCN;
            model.PresentEmployer = par.ParticularTrans.FirstOrDefault(x => x.LanguageId == langCode) != null ? par.ParticularTrans.FirstOrDefault(x => x.LanguageId == langCode).PresentEmployer : null;
            model.Position = par.ParticularTrans.FirstOrDefault(x => x.LanguageId == langCode) != null ? par.ParticularTrans.FirstOrDefault(x => x.LanguageId == langCode).Position : null;
            model.Honorific = par.Honorific;

            model.SameAddress = par.SameAddress;
            model.ResidentialRegionEN = par.ResidentialRegionEN;
            model.ResidentialDistrictEN = par.ResidentialDistrictEN;
            model.ResidentialStreetNumberEN = par.ResidentialStreetNumberEN;
            model.ResidentialStreetRoadEN = par.ResidentialStreetRoadEN;
            model.ResidentialEstateQuartersVillageEN = par.ResidentialEstateQuartersVillageEN;
            model.ResidentialBuildingEN = par.ResidentialBuildingEN;
            model.ResidentialFloorEN = par.ResidentialFloorEN;
            model.ResidentialRmFtUnitEN = par.ResidentialRmFtUnitEN;
            model.ResidentialRegionCN = par.ResidentialRegionCN;
            model.ResidentialDistrictCN = par.ResidentialDistrictCN;
            model.ResidentialStreetNumberCN = par.ResidentialStreetNumberCN;
            model.ResidentialStreetRoadCN = par.ResidentialStreetRoadCN;
            model.ResidentialEstateQuartersVillageCN = par.ResidentialEstateQuartersVillageCN;
            model.ResidentialBuildingCN = par.ResidentialBuildingCN;
            model.ResidentialFloorCN = par.ResidentialFloorCN;
            model.ResidentialRmFtUnitCN = par.ResidentialRmFtUnitCN;
            model.IsPrimamy = par.IsPrimamy;
            model.IsSecondary = par.IsSecondary;
            model.IsTechInst = par.IsTechInst;
            model.IsUniversityCollege = par.IsUniversityCollege;

            return model;
        }

        public static ParticularBindingModel ToParticularBindingModelTemp(ParticularTemp par, int langCode)
        {
            ParticularBindingModel model = new ParticularBindingModel();
            model.Id = par.Id;
            model.SurnameEN = par.SurnameEN;
            model.GivenNameEN = par.GivenNameEN;
            model.SurnameCN = par.SurnameCN;
            model.GivenNameCN = par.GivenNameCN;
            model.DateOfBirth = par.DateOfBirth;
            model.Age = (int)(DateTime.Now - par.DateOfBirth).TotalDays / 365;
            model.Gender = par.Gender ?? default;
            model.HKIDNo = par.HKIDNo != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(par.HKIDNo)) : null;
            model.PassportNo = par.PassportNo != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(par.PassportNo)) : null;
            //model.PassportExpiredDate = par.PassportExpiryDate?.ToString("dd-MM-yyyy");
            model.PassportExpiredDate = par.PassportExpiryDate;
            model.MobileNumber = par.MobileNumber != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(par.MobileNumber)) : null;
            model.MobileNumberPrefix = par.MobileNumberPrefix;
            model.TelNo = par.TelNo;
            model.FaxNo = par.FaxNo;
            model.RegionEN = par.RegionEN;
            model.RegionCN = par.RegionCN;
            model.DistrictEN = par.DistrictEN;
            model.DistrictCN = par.DistrictCN;
            model.StreetNumberEN = par.StreetNumberEN;
            model.StreetNumberCN = par.StreetNumberCN;
            model.StreetRoadEN = par.StreetRoadEN;
            model.StreetRoadCN = par.StreetRoadCN;
            model.EstateQuartersVillageEN = par.EstateQuartersVillageEN;
            model.EstateQuartersVillageCN = par.EstateQuartersVillageCN;
            model.BuildingEN = par.BuildingEN;
            model.BuildingCN = par.BuildingCN;
            model.FloorEN = par.FloorEN;
            model.FloorCN = par.FloorCN;
            model.RmFtUnitEN = par.RmFtUnitEN;
            model.RmFtUnitCN = par.RmFtUnitCN;
            model.EducationLevelEN = par.EducationLevelEN;
            model.EducationLevelCN = par.EducationLevelCN;
            model.PresentEmployer = par.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == langCode) != null ? par.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == langCode).PresentEmployer : null;
            model.Position = par.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == langCode) != null ? par.ParticularTempTrans.FirstOrDefault(x => x.LanguageId == langCode).Position : null;
            model.Honorific = par.Honorific;

            model.SameAddress = par.SameAddress;
            model.ResidentialRegionEN = par.ResidentialRegionEN;
            model.ResidentialDistrictEN = par.ResidentialDistrictEN;
            model.ResidentialStreetNumberEN = par.ResidentialStreetNumberEN;
            model.ResidentialStreetRoadEN = par.ResidentialStreetRoadEN;
            model.ResidentialEstateQuartersVillageEN = par.ResidentialEstateQuartersVillageEN;
            model.ResidentialBuildingEN = par.ResidentialBuildingEN;
            model.ResidentialFloorEN = par.ResidentialFloorEN;
            model.ResidentialRmFtUnitEN = par.ResidentialRmFtUnitEN;
            model.ResidentialRegionCN = par.ResidentialRegionCN;
            model.ResidentialDistrictCN = par.ResidentialDistrictCN;
            model.ResidentialStreetNumberCN = par.ResidentialStreetNumberCN;
            model.ResidentialStreetRoadCN = par.ResidentialStreetRoadCN;
            model.ResidentialEstateQuartersVillageCN = par.ResidentialEstateQuartersVillageCN;
            model.ResidentialBuildingCN = par.ResidentialBuildingCN;
            model.ResidentialFloorCN = par.ResidentialFloorCN;
            model.ResidentialRmFtUnitCN = par.ResidentialRmFtUnitCN;
            model.IsPrimamy = par.IsPrimamy;
            model.IsSecondary = par.IsSecondary;
            model.IsTechInst = par.IsTechInst;
            model.IsUniversityCollege = par.IsUniversityCollege;

            return model;
        }


        public static WorkExperience ToWorkExperience(WorkExperienceBindingModel workModel, WorkExperience model, int userId, int langCode)
        {
            if (model == null)
            {
                model = new WorkExperience();
                model.UserId = userId;
            }
            model.FromYear = workModel.FromYear;
            model.ToYear = workModel.ToYear;
            model.BIMRelated = workModel.BIMRelated;
            model.ClassifyWorkingExperience = workModel.ClassifyWorkingExperience;
            if (model.WorkExperienceTrans.FirstOrDefault(x => x.LanguageId == langCode) == null)
            {
                model.WorkExperienceTrans.Add(new WorkExperienceTran()
                {
                    LanguageId = langCode,
                    Location = workModel.Location,
                    Position = workModel.Position,
                    JobNature = workModel.JobNature
                });
            }
            else
            {
                var trans = model.WorkExperienceTrans.FirstOrDefault(x => x.LanguageId == langCode);
                trans.Location = workModel.Location;
                trans.Position = workModel.Position;
                trans.JobNature = workModel.JobNature;
            }

            return model;
        }

        public static WorkExperienceTemp ToWorkExperienceTemp(WorkExperienceBindingModel workModel, WorkExperienceTemp model, int userId, int langCode)
        {

            if (model == null)
            {
                model = new WorkExperienceTemp();
                if (userId == 0)
                {
                    model.UserId = workModel.UserId;
                }
                else
                {
                    model.UserId = userId;
                }
            }
            model.FromYear = workModel.FromYear;
            model.ToYear = workModel.ToYear;
            model.BIMRelated = workModel.BIMRelated;
            model.ClassifyWorkingExperience = workModel.ClassifyWorkingExperience;
            if (model.WorkExperienceTempTrans.FirstOrDefault(x => x.LanguageId == langCode) == null)
            {
                model.WorkExperienceTempTrans.Add(new WorkExperienceTempTran()
                {
                    LanguageId = langCode,
                    Location = workModel.Location,
                    Position = workModel.Position,
                    JobNature = workModel.JobNature,

                });
            }
            else
            {
                var trans = model.WorkExperienceTempTrans.FirstOrDefault(x => x.LanguageId == langCode);
                trans.Location = workModel.Location;
                trans.Position = workModel.Position;
                trans.JobNature = workModel.JobNature;
            }

            return model;
        }



        public static Qualification ToQualification(SubQualificationViewModel model, Qualification qual, int userId, int langCode)
        {
            if (qual == null)
            {
                qual = new Qualification();
                qual.UserId = userId;
            }
            qual.DateObtained = model.DateObtained;
            if (qual.QualificationsTrans.FirstOrDefault(x => x.LanguageId == langCode) == null)
            {
                qual.QualificationsTrans.Add(new QualificationsTran()
                {
                    IssuingAuthority = model.IssuingAuthority,
                    LevelAttained = model.LevelAttained,
                    LanguageId = langCode
                });
            }
            else
            {
                var trans = qual.QualificationsTrans.FirstOrDefault(x => x.LanguageId == langCode);
                trans.IssuingAuthority = model.IssuingAuthority;
                trans.LevelAttained = model.LevelAttained;
            }

            return qual;
        }


        public static QualificationTemp ToQualificationTemp(SubQualificationViewModel model, QualificationTemp qual, int userId, int langCode, int ApplicationId)
        {
            if (qual == null)
            {
                qual = new QualificationTemp();
                qual.UserId = userId;
            }
            qual.DateObtained = model.DateObtained;
            if (qual.QualificationTempTrans.FirstOrDefault(x => x.LanguageId == langCode) == null)
            {
                qual.QualificationTempTrans.Add(new QualificationTempTran()
                {
                    IssuingAuthority = model.IssuingAuthority,
                    LevelAttained = model.LevelAttained,
                    LanguageId = langCode
                });
            }
            else
            {
                var trans = qual.QualificationTempTrans.FirstOrDefault(x => x.LanguageId == langCode);
                trans.IssuingAuthority = model.IssuingAuthority;
                trans.LevelAttained = model.LevelAttained;

            }

            return qual;
        }

        public static CourseReExaminationBindingModel ToCourseReExaminationBindingModel(this Course model)
        {
            var bindingModel = new CourseReExaminationBindingModel();
            bindingModel.Id = model.Id;
            bindingModel.CanApplyForReExam = model.CanApplyForReExam;
            bindingModel.ReExamFee = model.ReExamFee;
            bindingModel.ReExamRemarks = model.ReExamRemarks;
            return bindingModel;
        }

        public static Keyword ToKeyword(KeywordBindingModel model, Keyword keyword, int courseId = 0)
        {
            if (courseId != 0)
            {
                keyword.CourseId = courseId;
            }
            keyword.WordEN = model.WordEN;
            keyword.WordCN = model.WordCN;
            keyword.WordHK = model.WordHK;
            return keyword;
        }

        public static KeywordBindingModel ToKeywordBindingModel(this Keyword model)
        {
            var bindingModel = new KeywordBindingModel();
            bindingModel.Id = model.Id;
            bindingModel.CourseId = model.CourseId;
            bindingModel.WordEN = model.WordEN;
            bindingModel.WordCN = model.WordCN;
            bindingModel.WordHK = model.WordHK;
            return bindingModel;
        }

        public static Document ToDoccumentForCourse(string path, string fileType, string fileName, int courseId, int distinguishDocType, int? lessonId = null)
        {
            Document doc = new Document();
            doc.Url = path;
            doc.ContentType = fileType;
            doc.FileName = fileName;
            doc.CourseDocuments.Add(new CourseDocument()
            {
                LessonId = lessonId,
                CourseId = courseId,
                DistinguishDocType = distinguishDocType
            });
            return doc;
        }

        public static SubCourseDocument ToSubCourseDocument(Document doc)
        {
            SubCourseDocument model = new SubCourseDocument();
            model.Id = doc.Id;
            model.FileName = doc.FileName;
            return model;
        }

        public static CmsContent ToCmsContent(CMSBindingModel bindModel, int userId, CmsContent model = null)
        {
            if (model == null)
            {
                model = new CmsContent();
                model.CreateDate = DateTime.Now;
                model.CreateBy = userId;
                model.ApproveStatus = false;
            }

            model.Id = bindModel.Id;
            model.ContentTypeId = bindModel.ContentTypeId;
            model.Title = bindModel.Title;
            model.SEOUrlLink = bindModel.SEOUrlLink;
            model.ImageSEO = bindModel.ImageSEO;
            model.AnnoucementDate = bindModel.AnnoucementDate;
            model.Description = bindModel.Description;
            model.FullDescription = bindModel.FullDescription;
            model.ShortDescription = bindModel.ShortDescription;
            if (model.PublishStatus == false && bindModel.PublishStatus)
            {
                model.LastPublishDate = DateTime.Now;
            }
            model.PublishStatus = bindModel.PublishStatus;
            model.ShowOnLandingPage = bindModel.ShowOnLandingPage;

            if (model.ApproveStatus == false)
            {
                model.ApproveStatus = bindModel.ApproveStatus;
            }

            model.LastModifiedDate = DateTime.Now;
            model.LastModifiedBy = userId;
            model.CmsStatus = bindModel.CmsStatus;
            model.ApplyingFor = bindModel.ApplyingFor;
            model.OrderNumber = bindModel.OrderNumber;
            model.ReleaseDate = bindModel.ReleaseDate?.ToStartOfTheDay();
            model.EndDate = bindModel.EndDate?.ToEndOfTheDay();

            return model;
        }

        public static CMSBindingModel ToCMSBindingModel(this CmsContent model, string userNameCreated, string userNameModified, string urlImage, string matchedTitle = "")
        {
            CMSBindingModel viewCMS = new CMSBindingModel();
            viewCMS.Id = model.Id;
            viewCMS.ContentTypeId = model.ContentTypeId;
            viewCMS.Title = model.Title;
            viewCMS.SEOUrlLink = model.SEOUrlLink;
            viewCMS.ImageSEO = model.ImageSEO;
            viewCMS.AnnoucementDate = model.AnnoucementDate;
            viewCMS.Description = model.Description;
            viewCMS.FullDescription = model.FullDescription;
            viewCMS.ShortDescription = model.ShortDescription;
            viewCMS.PublishStatus = model.PublishStatus;
            viewCMS.ShowOnLandingPage = model.ShowOnLandingPage;
            viewCMS.CreateDate = model.CreateDate;
            viewCMS.CreateByName = userNameCreated;
            viewCMS.LastModifiedDate = model.LastModifiedDate;
            viewCMS.ModifiedByName = userNameModified;
            viewCMS.ApproveStatus = model.ApproveStatus;
            viewCMS.LastPublishDate = model.LastPublishDate;
            viewCMS.CmsImageId = model.CmsImages.Count > 0 ? model.CmsImages.FirstOrDefault()?.Id ?? 0 : 0;
            viewCMS.ImagePath = urlImage + viewCMS.CmsImageId;
            viewCMS.CmsStatus = model.CmsStatus;
            viewCMS.ApplyingFor = model.ApplyingFor;
            viewCMS.CorrespondingTitle = matchedTitle == "" ? "" : model.Title + " + " + matchedTitle;
            viewCMS.MatchedItemId = model.MatchedItemId;
            viewCMS.OrderNumber = model.OrderNumber;
            viewCMS.ReleaseDate = model.ReleaseDate;
            viewCMS.EndDate = model.EndDate;
            return viewCMS;
        }

        public static SubListCmsContentTypeViewModel ToCmsContentTypeViewModel(this Model.Models.CmsContentType model, int langCode)
        {
            SubListCmsContentTypeViewModel modelReturn = new SubListCmsContentTypeViewModel();
            modelReturn.Id = model.Id;
            modelReturn.Name = langCode == 1 ? model.Name : (langCode == 2 ? model.NameTC : model.NameSC);
            modelReturn.CmsType = model.CmsType;
            return modelReturn;
        }

        public static LandingPageAnnouncementViewModel ToLandingPageAnnouncementViewModel(this CmsContent cms)
        {
            var model = new LandingPageAnnouncementViewModel();
            model.Id = cms.Id;
            model.Title = cms.Title;
            model.Day = cms.AnnoucementDate.HasValue ? cms.AnnoucementDate.Value.Day : 0;
            model.Month = cms.AnnoucementDate.HasValue ? ((DateTime)cms.AnnoucementDate).ToString("MMM", CultureInfo.InvariantCulture) : "";
            model.SEOUrlLink = cms.SEOUrlLink;
            return model;
        }

        public static LandingPageViewModel ToLandingPageViewModel(this CmsContent cms)
        {
            return new LandingPageViewModel()
            {
                Id = cms.Id,
                ShortDescription = cms.ShortDescription,
                Title = cms.Title,
                SEOUrlLink = cms.SEOUrlLink
            };
        }

        public static CoursePrivilegesViewModel ToCoursePrivilegesViewModel(this SystemPrivilege model, int langCode)
        {
            CoursePrivilegesViewModel viewMod = new CoursePrivilegesViewModel();
            viewMod.UserId = model.UserId;
            viewMod.LDAPAccount = model.User?.UserName;
            viewMod.Email = model.User?.AdminEmail;
            viewMod.CourseId = model.CourseId;
            viewMod.CourseCode = model.Course.CourseCode;
            viewMod.CourseName = model.Course.CourseTrans.FirstOrDefault(n => n.LanguageId == langCode)?.CourseName;
            viewMod.IsCreateCourse = model.IsCreateCourse;
            viewMod.IsEditCourse = model.IsEditCourse;
            viewMod.IsViewCourse = model.IsViewCourse;

            viewMod.IsUserCalendar = model.IsUserCalendar;
            viewMod.IsSubmitAndCancelCourse = model.IsSubmitAndCancelCourse;
            viewMod.IsFirstApproveAndRejectCourse = model.IsFirstApproveAndRejectCourse;
            viewMod.IsSecondpproveAndRejectCourse = model.IsSecondpproveAndRejectCourse;
            viewMod.IsThirdApproveAndRejectCourse = model.IsThirdApproveAndRejectCourse;
            viewMod.IsSubmitAndCancelClass = model.IsSubmitAndCancelClass;
            viewMod.IsFirstApproveAndRejectClass = model.IsFirstApproveAndRejectClass;
            viewMod.IsSecondpproveAndRejectClass = model.IsSecondpproveAndRejectClass;
            viewMod.IsThirdApproveAndRejectClass = model.IsThirdApproveAndRejectClass;


            return viewMod;
        }

        public static AccountSystemPrivilegesViewModel ToAdminAccountBindingModel(this AdminPermission model, string userName, string email)
        {
            AccountSystemPrivilegesViewModel viewMod = new AccountSystemPrivilegesViewModel();
            viewMod.Id = model.Id;
            viewMod.LDAPAccount = userName;
            viewMod.Email = email;
            viewMod.IsCreateAdmin = model.IsCreateAdmin;
            viewMod.IsSuspendAdmin = model.IsSuspendAdmin;
            viewMod.IsActiveAdmin = model.IsActiveAdmin;
            viewMod.IsEditAdmin = model.IsEditAdmin;
            viewMod.IsAssignAdmin = model.IsAssignAdmin;

            return viewMod;
        }


        public static ContentPrivilegesViewModel ToContentPrivilegesViewModel(this AdminPermission model, string userName, string email)
        {
            ContentPrivilegesViewModel viewMod = new ContentPrivilegesViewModel();
            viewMod.Id = model.Id;
            viewMod.LDAPAccount = userName;
            viewMod.Email = email;
            viewMod.IsCreateContent = model.IsCreateContent;
            viewMod.IsViewContent = model.IsViewContent;
            viewMod.IsEditContent = model.IsEditContent;
            viewMod.IsDeleteContent = model.IsDeleteContent;

            viewMod.IsApproveContent = model.IsApproveContent;
            viewMod.IsUnapproveContent = model.IsUnapproveContent;
            viewMod.IsPublishContent = model.IsPublishContent;
            viewMod.IsUnpublishContent = model.IsUnpublishContent;

            viewMod.IsBatchApplication = model.IsBatchApplication;
            viewMod.IsBatchPayment = model.IsBatchPayment;
            viewMod.IsAttendance = model.IsAttendance;
            viewMod.IsAssessment = model.IsAssessment;

            return viewMod;
        }

        public static AccountPermissionViewModel ToAccountPermissionViewModel(this AdminPermission model)
        {
            AccountPermissionViewModel viewMod = new AccountPermissionViewModel();
            viewMod.Id = model.Id;
            viewMod.Status = model.Status;
            viewMod.IsCreateContent = model.IsCreateContent;
            viewMod.IsViewContent = model.IsViewContent;
            viewMod.IsEditContent = model.IsEditContent;
            viewMod.IsDeleteContent = model.IsDeleteContent;
            viewMod.IsApproveContent = model.IsApproveContent;
            viewMod.IsUnapproveContent = model.IsUnapproveContent;
            viewMod.IsPublishContent = model.IsPublishContent;
            viewMod.IsUnpublishContent = model.IsUnpublishContent;
            viewMod.IsCreateAdmin = model.IsCreateAdmin;
            viewMod.IsSuspendAdmin = model.IsSuspendAdmin;
            viewMod.IsActiveAdmin = model.IsActiveAdmin;
            viewMod.IsEditAdmin = model.IsEditAdmin;
            viewMod.IsAssignAdmin = model.IsAssignAdmin;

            viewMod.IsBatchApplication = model.IsBatchApplication;
            viewMod.IsBatchPayment = model.IsBatchPayment;
            viewMod.IsAttendance = model.IsAttendance;
            viewMod.IsAssessment = model.IsAssessment;
            return viewMod;
        }

        public static CoursePermissionViewModel ToCoursePermissionViewModel(this SystemPrivilege model)
        {
            CoursePermissionViewModel viewMod = new CoursePermissionViewModel();
            viewMod.UserId = model.UserId;
            viewMod.CourseId = model.CourseId;
            viewMod.IsCreateCourse = model.IsCreateCourse;
            viewMod.IsViewCourse = model.IsViewCourse;
            viewMod.IsEditCourse = model.IsEditCourse;

            viewMod.IsUserCalendar = model.IsUserCalendar;
            viewMod.IsSubmitAndCancelCourse = model.IsSubmitAndCancelCourse;
            viewMod.IsFirstApproveAndRejectCourse = model.IsFirstApproveAndRejectCourse;
            viewMod.IsSecondpproveAndRejectCourse = model.IsSecondpproveAndRejectCourse;
            viewMod.IsThirdApproveAndRejectCourse = model.IsThirdApproveAndRejectCourse;
            viewMod.IsSubmitAndCancelClass = model.IsSubmitAndCancelClass;
            viewMod.IsFirstApproveAndRejectClass = model.IsFirstApproveAndRejectClass;
            viewMod.IsSecondpproveAndRejectClass = model.IsSecondpproveAndRejectClass;
            viewMod.IsThirdApproveAndRejectClass = model.IsThirdApproveAndRejectClass;

            return viewMod;
        }

        public static CriteriaViewModel ToCriteriaViewModel(this ApplicationUser permissions, LanguageCode lang, int courseId)//string coursecode, string coursename)
        {
            CriteriaViewModel rtnItem = new CriteriaViewModel();

            rtnItem.LDAPAccount = permissions.UserName;
            rtnItem.UserId = permissions.Id;
            rtnItem.Email = permissions.AdminEmail;
            rtnItem.Status = permissions.Status;
            rtnItem.AdminPermisstion = new AdminPermisstionViewModel();
            if (permissions.AdminPermission != null)
            {
                rtnItem.AdminPermisstion.Id = permissions.AdminPermission.Id;
                rtnItem.AdminPermisstion.IsCreateContent = permissions.AdminPermission.IsCreateContent;
                rtnItem.AdminPermisstion.IsViewContent = permissions.AdminPermission.IsViewContent;
                rtnItem.AdminPermisstion.IsEditContent = permissions.AdminPermission.IsEditContent;
                rtnItem.AdminPermisstion.IsDeleteContent = permissions.AdminPermission.IsDeleteContent;
                rtnItem.AdminPermisstion.IsApproveContent = permissions.AdminPermission.IsApproveContent;
                rtnItem.AdminPermisstion.IsUnapproveContent = permissions.AdminPermission.IsUnapproveContent;
                rtnItem.AdminPermisstion.IsPublishContent = permissions.AdminPermission.IsPublishContent;
                rtnItem.AdminPermisstion.IsUnpublishContent = permissions.AdminPermission.IsUnpublishContent;
                rtnItem.AdminPermisstion.IsCreateAdmin = permissions.AdminPermission.IsCreateAdmin;
                rtnItem.AdminPermisstion.IsSuspendAdmin = permissions.AdminPermission.IsSuspendAdmin;
                rtnItem.AdminPermisstion.IsActiveAdmin = permissions.AdminPermission.IsActiveAdmin;
                rtnItem.AdminPermisstion.IsEditAdmin = permissions.AdminPermission.IsEditAdmin;
                rtnItem.AdminPermisstion.IsAssignAdmin = permissions.AdminPermission.IsAssignAdmin;

                rtnItem.AdminPermisstion.IsBatchPayment = permissions.AdminPermission.IsBatchPayment;
                rtnItem.AdminPermisstion.IsBatchApplication = permissions.AdminPermission.IsBatchApplication;
                rtnItem.AdminPermisstion.IsAttendance = permissions.AdminPermission.IsAttendance;
                rtnItem.AdminPermisstion.IsAssessment = permissions.AdminPermission.IsAssessment;
            }
            rtnItem.SystemPrivileges = new List<SystemPrivilegeViewModel>();

            foreach (var item in permissions.SystemPrivileges)
            {
                SystemPrivilegeViewModel tempPermission = new SystemPrivilegeViewModel();
                tempPermission.IsCreateCourse = item.IsCreateCourse;
                tempPermission.IsViewCourse = item.IsViewCourse;
                tempPermission.IsEditCourse = item.IsEditCourse;

                tempPermission.IsUserCalendar = item.IsUserCalendar;
                tempPermission.IsSubmitAndCancelCourse = item.IsSubmitAndCancelCourse;
                tempPermission.IsFirstApproveAndRejectCourse = item.IsFirstApproveAndRejectCourse;
                tempPermission.IsSecondpproveAndRejectCourse = item.IsSecondpproveAndRejectCourse;
                tempPermission.IsThirdApproveAndRejectCourse = item.IsThirdApproveAndRejectCourse;
                tempPermission.IsSubmitAndCancelClass = item.IsSubmitAndCancelClass;
                tempPermission.IsFirstApproveAndRejectClass = item.IsFirstApproveAndRejectClass;
                tempPermission.IsSecondpproveAndRejectClass = item.IsSecondpproveAndRejectClass;
                tempPermission.IsThirdApproveAndRejectClass = item.IsThirdApproveAndRejectClass;

                tempPermission.CourseCode = item.Course.CourseCode;
                tempPermission.Id = item.CourseId;

                if (lang == LanguageCode.EN)
                {
                    tempPermission.CourseName = item.Course.CourseTrans.SingleOrDefault(x => x.LanguageId == (int)LanguageCode.EN).CourseName;
                }
                if (lang == LanguageCode.HK)
                {
                    tempPermission.CourseName = item.Course.CourseTrans.SingleOrDefault(x => x.LanguageId == (int)LanguageCode.HK).CourseName;
                }
                if (lang == LanguageCode.CN)
                {
                    tempPermission.CourseName = item.Course.CourseTrans.SingleOrDefault(x => x.LanguageId == (int)LanguageCode.CN).CourseName;
                }
                rtnItem.SystemPrivileges.Add(tempPermission);
            }

            var filterCourse = rtnItem.SystemPrivileges.Where(x => courseId == 0 ? true : x.Id == courseId).ToList();//x.CourseCode.Contains(coursecode) && x.CourseName.Contains(coursename)).ToList();

            rtnItem.SystemPrivileges = filterCourse;

            return rtnItem;
        }

        public static CourseApprovedHistoryViewModel ToCourseApprovedHistoryViewModel(this CourseAppovedStatusHistory model, string displayName)
        {
            var result = new CourseApprovedHistoryViewModel();
            result.Id = model.Id;
            result.ApprovalUpdatedBy = displayName;
            result.AppovalStatusFrom = model.AppovalStatusFrom;
            result.ApprovalStatusTo = model.ApprovalStatusTo;
            result.ApprovalUpdatedTime = model.ApprovalUpdatedTime;
            result.ApprovalRemarks = model.ApprovalRemarks;
            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=";
            foreach (var item in model.CourseHistoryDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListDocuments.Add(new CourseApprovedDocument()
                {
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static ClassApprovedHistoryViewModel ToClassApprovedHistoryViewModel(this ClassAppovedStatusHistory model, string displayName)
        {
            var result = new ClassApprovedHistoryViewModel();
            result.Id = model.Id;
            result.ApprovalUpdatedBy = displayName;
            result.AppovalStatusFrom = model.AppovalStatusFrom;
            result.ApprovalStatusTo = model.ApprovalStatusTo;
            result.ApprovalUpdatedTime = model.ApprovalUpdatedTime;
            result.ApprovalRemarks = model.ApprovalRemarks;
            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=";
            foreach (var item in model.ClassHistoryDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListDocuments.Add(new ClassApprovedDocument()
                {
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static SubClassApprovedHistoryViewModel ToSubClassApprovedHistoryViewModel(this SubClassApprovedStatusHistory model, string displayName)
        {
            var result = new SubClassApprovedHistoryViewModel();
            result.Id = model.Id;

            result.NewClassCode = model.SubClassDraft.NewClassCode;
            result.NewAttendanceRequirement = model.SubClassDraft.NewAttendanceRequirement;
            result.NewAttendanceRequirementTypeId = model.SubClassDraft.NewAttendanceRequirementTypeId;
            result.NewClassCommencementDate = model.SubClassDraft.NewClassCommencementDate;
            result.NewClassCompletionDate = model.SubClassDraft.NewClassCompletionDate;
            result.NewClassCapacity = model.SubClassDraft.NewClassCapacity;
            result.NewClassStatus = model.SubClassDraft.NewClassStatus;
            result.ApprovedStatus = model.Class.SubClassApprovedStatus;
            result.Remarks = model.Remarks;
            result.ApprovedBy = displayName;
            result.ApprovalUpdatedTime = model.ApprovalUpdatedTime;
            result.ApprovalStatusFrom = model.ApprovalStatusFrom;
            result.ApprovalStatusTo = model.ApprovalStatusTo;

            foreach (var item in model.SubClassHistoryDocuments)
            {
                var doc = new SubClassApprovedDocument();
                doc.FileName = item.Document.FileName;
                doc.DownloadUrl = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=Courses/download-document?docId=" + item.DocumentId;
                result.ListDocuments.Add(doc);
            }


            return result;
        }

        public static StudentClassManageViewModel ToStudentClassManageViewModel(this Application model)
        {
            if (model == null) return null;
            var result = new StudentClassManageViewModel();
            result.Id = model.Id;
            result.UserId = model.UserId;
            result.CourseId = model.CourseId;
            result.CourseCode = model.Course.CourseCode;
            result.StudentPreferredClass = model.StudentPreferredClassModel?.ClassCode;
            result.AdminAssignedClass = model.AdminAssignedClass ?? 0;
            result.AssignedPerClass = model.AdminAssignedClassModel != null ? new AttendanceOnCapacity() { AssignedStudents = model.AdminAssignedClassModel.EnrollmentNumber ?? 0, Capacity = model.AdminAssignedClassModel.Capacity } : null;
            result.SubmissionDate = model.ApplicationLastSubmissionDate ?? DateTime.Now;
            result.ApplicationNumber = model.ApplicationNumber;
            result.CICNumber = model.User.CICNumber;
            result.StudentNameChinese = model.User.Particular?.SurnameCN + " " + model.User.Particular?.GivenNameCN;
            result.StudentNameEnglish = model.User.Particular?.SurnameEN + " " + model.User.Particular?.GivenNameEN;
            result.ApplicationStatus = model.Status;
            result.InvoiceStatus = model.Invoices.Count() > 0 ? model.Invoices.LastOrDefault(/*x => x.InvoiceItems.Any(c => c.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee)*/).Status : 0;

            //    Created = 1,
            //Offered = 2,
            //PaidPartially = 3,
            //Waived = 4,
            //Settled = 5,
            //Revised = 6,
            //Overpaid = 7,
            //Cancelled = 8,
            //RefundPendingForApproval = 9,
            //PendingForRefund = 10,
            //Refunded = 11,
            //Overdue = 12,
            //SettledByBatch = 13,

            switch (result.InvoiceStatus)
            {
                case 5:
                case 7:
                case 4:
                case 13:
                    result.EnrollmentStatus = (int)Enums.EnrollmentStatus.Enrolled;
                    break;
                default:
                    result.EnrollmentStatus = model.EnrollmentStatusStorages.Count > 0 ? model.EnrollmentStatusStorages.OrderByDescending(i => i.Id).FirstOrDefault().Status : 0;
                    break;
            }
            //Update later
            result.PaymentReminder = false;                     //Update later
            result.EnrollmentEmailNotification = false;         //Update later

            var listClassAvaiable = model.Course.Classes.Where(x =>
            x.SubClassStatus == (int)Enums.SubClassStatus.Actived || x.SubClassStatus == (int)Enums.SubClassStatus.Openned ||
            x.SubClassStatus == (int)Enums.SubClassStatus.Cancelled || x.SubClassStatus == (int)Enums.SubClassStatus.Postponed);

            foreach (var item in listClassAvaiable)
            {
                result.ListClasAvaiable.Add(new ClassAvaiable()
                {
                    Id = item.Id,
                    ClassCode = item.ClassCode
                });
            }

            return result;
        }

        public static ApplicationDetailViewModel ToApplicationDetailViewModel(this Application model, int lang)
        {
            if (model == null) return null;
            var result = new ApplicationDetailViewModel();
            result.Id = model.Id;
            result.CourseCode = model.Course.CourseCode;
            result.CourseName = model.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == lang).CourseName;
            result.Email = model.User.Email;
            result.SurNameEnglish = model.User.Particular.SurnameEN;
            result.GivenNameEnglish = model.User.Particular.GivenNameEN;
            result.SurNameChinese = model.User.Particular.SurnameCN;
            result.GivenNameChinese = model.User.Particular.GivenNameCN;
            result.DateOfBirth = model.User.Particular.DateOfBirth;
            result.Sex = model.User.Particular.Gender ?? false;
            result.TelNo = model.User.Particular.TelNo;
            result.FaxNo = model.User.Particular.FaxNo;
            result.PresentEmployer = model.User.Particular.ParticularTrans.FirstOrDefault(x => x.LanguageId == lang)?.PresentEmployer;
            result.ClassPreferenceCode = model.StudentPreferredClassModel?.ClassCode;
            result.ModeOfStudy = lang == (int)LanguageCode.EN ? model.StudentPreferredClassModel?.Course.CourseType.NameEN :
                (lang == (int)LanguageCode.CN ? model.StudentPreferredClassModel?.Course.CourseType.NameCN : model.StudentPreferredClassModel?.Course.CourseType.NameHK);
            result.OtherEmailContact = model.User.OtherEmail;
            result.Age = (int)(DateTime.Now - model.User.Particular.DateOfBirth).TotalDays / 365;
            result.HKIDno = model.User.Particular.HKIDNo != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(model.User.Particular.HKIDNo)) : null;
            result.MobileNumber = model.User.Particular.MobileNumber != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(model.User.Particular.MobileNumber)) : null;
            result.Position = model.User.Particular.ParticularTrans.FirstOrDefault(x => x.LanguageId == lang)?.Position;

            return result;
        }

        public static ApplicationApprovedHistoryViewModel ToApplicationApprovedHistoryViewModel(this ApplicationApprovedStatusHistory model, string displayName)
        {
            var result = new ApplicationApprovedHistoryViewModel();
            result.Id = model.Id;
            result.ApprovalUpdatedBy = displayName;
            result.AppovalStatusFrom = model.AppovalStatusFrom;
            result.ApprovalStatusTo = model.ApprovalStatusTo;
            result.ApprovalUpdatedTime = model.ApprovalUpdatedTime;
            result.ApprovalRemarks = model.ApprovalRemarks;
            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=ApplicationManagement/download-document/?docId=";
            foreach (var item in model.ApplicationHistoryDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListDocuments.Add(new ApplicationApprovedDocument()
                {
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static SummaryTableViewModel ToSummaryTableViewModel(this Class model)
        {
            var result = new SummaryTableViewModel();
            result.ClassCode = model.ClassCode;
            result.Capacity = model.Capacity;
            result.StudentAssigned = model.EnrollmentNumber ?? 0;

            result.SummaryCourseFee.Created = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.Created));
            result.SummaryCourseFee.Offered = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.Offered));
            result.SummaryCourseFee.Waived = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.PaidPartially));
            result.SummaryCourseFee.PaidPartially = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.Waived));
            result.SummaryCourseFee.Settled = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.Settled));
            result.SummaryCourseFee.SettledByBatch = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.SettledByBatch));
            result.SummaryCourseFee.Revised = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.Revised));
            result.SummaryCourseFee.Overdue = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee) && c.Status == (int)Enums.InvoiceStatus.Overpaid));

            result.SummaryReExamFee.Created = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.Created));
            result.SummaryReExamFee.Offered = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.Offered));
            result.SummaryReExamFee.Waived = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.PaidPartially));
            result.SummaryReExamFee.PaidPartially = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.Waived));
            result.SummaryReExamFee.Settled = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.Settled));
            result.SummaryReExamFee.SettledByBatch = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.SettledByBatch));
            result.SummaryReExamFee.Revised = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.Revised));
            result.SummaryReExamFee.Overdue = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee) && c.Status == (int)Enums.InvoiceStatus.Overpaid));

            result.SummaryOtherFee.Created = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.Created));
            result.SummaryOtherFee.Offered = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.Offered));
            result.SummaryOtherFee.Waived = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.PaidPartially));
            result.SummaryOtherFee.PaidPartially = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.Waived));
            result.SummaryOtherFee.Settled = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.Settled));
            result.SummaryOtherFee.SettledByBatch = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.SettledByBatch));
            result.SummaryOtherFee.Revised = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.Revised));
            result.SummaryOtherFee.Overdue = model.AdminAssignedApplicationModels.Count(x => x.Invoices.Any(c => c.InvoiceItems.Any(v => v.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Others) && c.Status == (int)Enums.InvoiceStatus.Overpaid));

            return result;
        }

        public static Invoice ToInvoiceCreateBindingModel(this InvoiceCreateBindingModel model, int userId, Application application, Invoice result, string lastInvoiceNumber = null, string courseCode = null)
        {
            result.Id = model.InvoiceId;
            result.ApplicationId = model.ApplicationId;
            result.Status = (int)Enums.InvoiceStatus.Created;
            result.RequiresHardCopyReceipt = model.RequiredHardCopy;
            result.PaymentDueDate = model.PaymentDueDate;
            result.LastModifiedBy = userId;
            result.LastModifiedDate = DateTime.Now;

            if (result.InvoiceNumber == null)
            {
                result.InvoiceNumber = GenerateInvoiceNumber(lastInvoiceNumber, courseCode);
                if (result.InvoiceNumber.Equals(Common.Error) || result.InvoiceNumber.Equals(StaticConfig.FailGenerateInvoiceNumber))
                {
                    return null;
                }
            }

            result.CreateDate = result.CreateDate.HasValue ? result.CreateDate : DateTime.Now;
            result.CreateBy = result.CreateBy.HasValue ? result.CreateBy : userId;

            //Reset item and Fee
            result.InvoiceItems = new List<InvoiceItem>();
            result.Fee = 0;

            foreach (var item in model.ListInvoiceItems)
            {
                var invoiceItem = new InvoiceItem();
                if (item.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee)
                {
                    result.Fee += item.UnitPrice;
                    invoiceItem.Price = item.UnitPrice;
                }
                else if (item.InvoiceItemTypeId == (int)Enums.InvoiceItemType.Discount)
                {
                    if (!item.IsDiscount)
                    {
                        invoiceItem.Price = 0;
                    }
                    else
                    {
                        result.Fee -= application.Course.DiscountFee ?? 0;
                        invoiceItem.Price = application.Course.DiscountFee ?? 0;
                    }

                    invoiceItem.IsDiscount = item.IsDiscount;
                }
                else if (item.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee)
                {
                    result.Fee += application.Course.ReExamFee ?? 0;
                    invoiceItem.Price = application.Course.ReExamFee ?? 0;

                }
                else
                {
                    result.Fee += item.UnitPrice;
                    invoiceItem.Price = item.UnitPrice;
                }

                invoiceItem.InvoiceItemTypeId = item.InvoiceItemTypeId;
                invoiceItem.IsDiscount = item.IsDiscount;
                invoiceItem.EnglishName = item.NameEnglish;
                invoiceItem.ChineseName = item.NameChinese;
                result.InvoiceItems.Add(invoiceItem);
            }

            if (model.ListInvoiceItems.Any(x => x.InvoiceItemTypeId == (int)Enums.InvoiceItemType.ReExamFee))
            {
                result.TypeReExam = HandleReExam(application);
            }
            else
            {
                result.TypeReExam = 0;
            }


            return result;
        }

        private static string GenerateInvoiceNumber(string lastInvoiceNumber, string courseCode)
        {
            if (string.IsNullOrEmpty(lastInvoiceNumber))
            {
                return courseCode + "-" + DateTime.Now.Year.ToString() + "-" + "00001I";
            }
            int lastNumber = 0;
            var increment = lastInvoiceNumber.Substring(lastInvoiceNumber.Length - 6, 5);
            var success = int.TryParse(increment, out lastNumber);

            if (!success)
            {
                return Common.Error;
            }

            var invoiceNumber = lastNumber + 1;
            if (invoiceNumber > StaticConfig.NumberOfInvoicePerYear)
            {
                return StaticConfig.FailGenerateInvoiceNumber;
            }

            var result = DateTime.Now.Year.ToString() + invoiceNumber.ToString("D5") + "I";
            return result;
        }

        private static int HandleReExam(Application application)
        {
            var adminAssignClassModel = application.AdminAssignedClassModel;
            if (adminAssignClassModel != null && adminAssignClassModel.IsReExam == true && adminAssignClassModel.Exams.Count() > 0)
            {
                var examAvailable = adminAssignClassModel.Exams.Where(x => x.Date > DateTime.Now && x.Type != (int)Enums.ExamType.Exam).OrderBy(c => c.Date);
                if (examAvailable != null)
                {
                    return examAvailable.First().Type;
                }
            }
            return 0;
        }

        public static InvoiceItem ToInvoiceItemBindingModel(this InvoiceItemBindingModel model)
        {
            var result = new InvoiceItem();
            result.Price = model.UnitPrice;
            result.InvoiceItemTypeId = model.InvoiceItemTypeId;
            result.EnglishName = model.NameEnglish;
            result.ChineseName = model.NameChinese;
            result.IsDiscount = model.IsDiscount;

            return result;
        }

        public static MyCourseApplicationViewModel ToMyCourseApplicationViewModel(this Application model, int langCode)
        {
            var result = new MyCourseApplicationViewModel();
            string _ApplicationStatusName = "";
            string _InvoiceStatusForCourseFeeName = "";
            result.Id = model.Id;
            result.CourseId = model.CourseId;
            result.ApplicationNumber = model.ApplicationNumber;
            result.CourseCode = model.Course.CourseCode;
            result.CourseName = model.Course.CourseTrans.FirstOrDefault(x => x.LanguageId == langCode).CourseName;
            result.Duration = model.Course.DurationHrs;
            result.DurationLesson = model.Course.DurationLesson;
            result.DurationTotal = model.Course.DurationTotal;
            result.StudyMode = langCode == (int)LanguageCode.EN ? model.Course.CourseType.NameEN : (langCode == (int)LanguageCode.CN ? model.Course.CourseType.NameCN : model.Course.CourseType.NameHK);
            result.Credits = model.Course.Credits;
            result.LastSubmissionDate = model.ApplicationLastSubmissionDate;
            result.ApplicationStatus = model.Status;
            if (model.Status > 0)
            {
                _ApplicationStatusName = Enum.GetName(typeof(Enums.ApplicationStatus), model.Status);
            }
            result.ApplicationStatusName = _ApplicationStatusName;
            if (model.Invoices.Count > 0)
            {
                result.InvoiceStatusForCourseFee = model.Invoices.LastOrDefault(x => x.InvoiceItems.Any(c => c.InvoiceItemTypeId == (int)Enums.InvoiceItemType.CourseFee)).Status;
                if (result.InvoiceStatusForCourseFee > 0)
                {
                    _InvoiceStatusForCourseFeeName = Enum.GetName(typeof(Enums.InvoiceStatus), result.InvoiceStatusForCourseFee);
                }
            }
            result.InvoiceStatusForCourseFeeName = _InvoiceStatusForCourseFeeName;
            return result;
        }

        public static InvoiceBindingModel ToInvoiceBindingModel(this Invoice model)
        {
            var result = new InvoiceBindingModel();

            result.InvoiceNumber = model.InvoiceNumber;
            result.InvoiceCreatedDate = model.CreateDate;
            result.ReciptNumber = null;
            result.ReciptCreatedDate = null;

            result.LinkInvoicePdf = ConfigHelper.GetByKey("DMZAPI") + $"api/Proxy?url=Application/DownloadInvoice?invoiceId={model.Id}";
            result.LinkReciptPdf = "";

            return result;
        }

        public static InvoiceViewModel ToInvoiceViewModel(this Invoice model)
        {
            var result = new InvoiceViewModel();
            result.InvoiceId = model.Id;
            result.CourseCode = model.Application.Course.CourseCode;
            result.CourseNameEN = model.Application.Course.CourseTrans.FirstOrDefault(c => c.LanguageId == (int)LanguageCode.EN).CourseName;
            result.CourseNameEN = model.Application.Course.CourseTrans.FirstOrDefault(c => c.LanguageId == (int)LanguageCode.CN).CourseName;
            result.StudentNameEN = model.Application.User.Particular.SurnameEN + " " + model.Application.User.Particular.GivenNameEN;
            result.StudentNameCN = model.Application.User.Particular.SurnameCN + " " + model.Application.User.Particular.GivenNameCN;
            result.InvoiceNumber = model.InvoiceNumber;
            result.Fee = model.Fee;
            result.PaymentDueDate = model.PaymentDueDate;
            result.RequiredhardCopy = model.RequiresHardCopyReceipt;
            result.Status = model.Status;
            result.WaiverApprovedStatus = model.WaiverApprovedStatus;
            result.CommunicationLanguage = model.Application.User.CommunicationLanguage;

            foreach (var item in model.InvoiceItems)
            {
                var invoiceItem = new InvoiceItemViewModel();
                invoiceItem.Id = item.Id;
                invoiceItem.NameEN = item.EnglishName;
                invoiceItem.NameCN = item.ChineseName;
                invoiceItem.Price = item.Price;
                invoiceItem.IsDiscount = item.IsDiscount;
                invoiceItem.InvoiceItemTypeId = item.InvoiceItemTypeId;

                result.InvoiceItems.Add(invoiceItem);
            }

            return result;
        }

        public static WaivedPaymentApprovedHistoryViewModel ToWaivedPaymentApprovedHistoryViewModel(this WaiverApprovedStatusHistory model, string displayName)
        {
            var result = new WaivedPaymentApprovedHistoryViewModel();

            result.Id = model.Id;
            result.ApprovalUpdatedBy = displayName;
            result.AppovalStatusFrom = model.ApprovalStatusFrom;
            result.ApprovalStatusTo = model.ApprovalStatusTo;
            result.ApprovalUpdatedTime = model.ApprovalUpdatedTime;
            result.ApprovalRemarks = model.ApprovalRemarks;
            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=ApplicationManagement/download-document?docId=";
            foreach (var item in model.WaivedHistoryDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListDocuments.Add(new WaivedApprovedDocument()
                {
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static TransactionViewModel ToTransactionViewModel(this PaymentTransaction model)
        {
            var result = new TransactionViewModel();
            result.Id = model.Id;
            result.InvoiceId = model.InvoiceId;
            result.TypeOfPayment = model.TransactionType;
            result.Amount = model.Amount;
            result.BankCodeAndBankName = model.AcceptedBankId;
            result.RefNo = model.RefNo;
            result.PaymentDate = model.PaymentDate;
            result.Remarks = model.Remarks;
            result.ReasonForRefund = model.ReasonForRefund;

            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=ApplicationManagement/DownloadTransaction?docId=";
            foreach (var item in model.PaymentTransactionDocuments)
            {
                int typeDocument = item.TypeOfDocument == (int)SPDC.Common.Enums.TypeFileTransaction.CreateBySelf ? 0 : 1;
                var documentUrl = url + item.DocumentId + "&typeFileTransaction=" + typeDocument;
                var fileName = item.Document.FileName;
                result.ListDocuments.Add(new TransactionDocument()
                {
                    Id = item.DocumentId,
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static RefundTransactionViewModel ToRefundTransactionViewModel(this RefundTransaction model)
        {
            var result = new RefundTransactionViewModel();
            result.Id = model.Id;
            result.InvoiceId = model.InvoiceId;
            result.TypeOfPayment = model.TransactionTypeId;
            result.Amount = model.Amount;
            result.BankCodeAndBankName = model.AcceptedBankId;
            result.RefNo = model.RefNo;
            result.PaymentDate = model.PaymentDate;
            result.Remarks = model.Remarks;
            result.ReasonForRefund = model.ReasonForRefund;

            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=ApplicationManagement/DownloadTransaction?docId=";
            foreach (var item in model.RefundTransactionDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListDocuments.Add(new Model.ViewModels.Transaction.RefundTransaction.RefundTransactionDocument()
                {
                    Id = item.DocumentId,
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static RefundTransactionApprovedHistoryViewModel ToRefundTransactionApprovedHistoryViewModel(this RefundTransactionApprovedStatusHistory model, string userName)
        {
            var result = new RefundTransactionApprovedHistoryViewModel();

            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=ApplicationManagement/DownloadTransaction?docId=";


            result.Id = model.RefundTransaction.Id;
            result.InvoiceId = model.RefundTransaction.InvoiceId;
            result.TypeOfPayment = model.RefundTransaction.TransactionTypeId;
            result.Amount = model.RefundTransaction.Amount;
            result.BankCodeAndBankName = model.RefundTransaction.AcceptedBankId;
            result.RefNo = model.RefundTransaction.RefNo;
            result.PaymentDate = model.RefundTransaction.PaymentDate;
            result.Remarks = model.RefundTransaction.Remarks;
            result.ReasonForRefund = model.RefundTransaction.ReasonForRefund;
            result.RefundApprovedStatus = model.RefundTransaction.RefundApprovedStatus;

            foreach (var item in model.RefundTransaction.RefundTransactionDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListRefundDocuments.Add(new RefundApprovedDocument()
                {
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            result.ApprovedId = model.Id;
            result.ApprovalUpdatedBy = userName;
            result.AppovalStatusFrom = model.ApprovalStatusFrom;
            result.ApprovalStatusTo = model.ApprovalStatusTo;
            result.ApprovalUpdatedTime = model.ApprovalUpdatedTime;
            result.ApprovalRemarks = model.ApprovalRemarks;

            foreach (var item in model.RefundTransactionHistoryDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListApprovedDocuments.Add(new RefundApprovedDocument()
                {
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static RefundTransactionApprovedHistoryViewModel ToRefundTransactionApprovedHistoryViewModelFromRefundTransaction(this RefundTransaction model)
        {
            if (model == null) { return null; }
            var result = new RefundTransactionApprovedHistoryViewModel();

            var url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=ApplicationManagement/DownloadTransaction?docId=";


            result.Id = model.Id;
            result.InvoiceId = model.InvoiceId;
            result.TypeOfPayment = model.TransactionTypeId;
            result.Amount = model.Amount;
            result.BankCodeAndBankName = model.AcceptedBankId;
            result.RefNo = model.RefNo;
            result.PaymentDate = model.PaymentDate;
            result.Remarks = model.Remarks;
            result.ReasonForRefund = model.ReasonForRefund;
            result.RefundApprovedStatus = model.RefundApprovedStatus;

            foreach (var item in model.RefundTransactionDocuments)
            {
                var documentUrl = url + item.DocumentId;
                var fileName = item.Document.FileName;
                result.ListRefundDocuments.Add(new RefundApprovedDocument()
                {
                    FileName = fileName,
                    DownloadUrl = documentUrl
                });
            }

            return result;
        }

        public static BatchPaymentViewModel ToBatchPaymentViewModel(this BatchPayment model)
        {
            var result = new BatchPaymentViewModel();

            result.Id = model.Id;
            result.BatchPaymentDate = model.PaymentDate;
            result.BatchPaymentAmount = model.BatchPaymentAmount;
            result.ReferenceNumber = model.RefNo;
            result.Remarks = model.Remarks;

            return result;
        }

        public static UserForSettledViewModel ToUserForSettledViewModel(this ApplicationUser model, string name)
        {
            var result = new UserForSettledViewModel();
            if ((model.Particular.SurnameEN + model.Particular.GivenNameEN).ToLower().Contains(name))
            {
                result.ApplicantName = model.Particular.SurnameEN + " " + model.Particular.GivenNameEN;
                result.IsChineseName = false;
            }
            else
            {
                result.ApplicantName = model.Particular.SurnameCN + " " + model.Particular.GivenNameCN;
                result.IsChineseName = true;
            }
            result.Id = model.Id;
            result.CICNumber = model.CICNumber;

            return result;
        }

        public static TargetClassViewModel ToTargetClassViewModel(this Course model)
        {
            var result = new TargetClassViewModel();
            result.Id = model.Id;
            result.CourseId = model.Id;
            result.TargetNumberClass = model.TargetClasses.TargetNumberClass;
            result.ClassApprovedStatus = model.ClassApprovedStatus;
            result.AcademicYear = model.TargetClasses.AcademicYear;
            result.CanApplyForReExam = model.CanApplyForReExam;
            //result.ClassCommonId = model.TargetClasses.ClassCommonId;
            //result.ClassCommonViewModel.Id = model.TargetClasses.ClassCommonId;
            //result.ClassCommonViewModel.TypeCommon = model.TargetClasses.ClassCommon.TypeCommon;
            //result.ClassCommonViewModel.TypeName = model.TargetClasses.ClassCommon.TypeName;

            foreach (var item in model.Classes)
            {
                var aClass = new ClassViewDetailModel();
                aClass.Id = item.Id;
                aClass.Capacity = item.Capacity;
                aClass.ClassCommonId = item.ClassCommonId;
                aClass.CommencementDate = item.CommencementDate;
                aClass.CompletionDate = item.CompletionDate;
                aClass.CourseId = item.CourseId;
                aClass.InvisibleOnWebsite = item.InvisibleOnWebsite;
                aClass.AttendanceRequirement = item.AttendanceRequirement;
                aClass.EnrollmentNumber = item.EnrollmentNumber;
                aClass.SubClassStatus = item.SubClassStatus;
                aClass.SubClassApprovedStatus = item.SubClassApprovedStatus;
                aClass.ClassCode = item.ClassCode;
                aClass.IsExam = item.IsExam;
                aClass.IsReExam = item.IsReExam;
                aClass.ExamPassingMask = item.ExamPassingMask ?? 0;
                aClass.ReExamFees = item.ReExamFees ?? 0;
                aClass.CountReExam = item.CountReExam;
                aClass.SubClassApprovedStatus = item.SubClassApprovedStatus;

                foreach (var lesson in item.Lessons)
                {
                    var lessonViewModel = new LessonViewModel();
                    lessonViewModel.Id = lesson.Id;
                    lessonViewModel.ClassId = lesson.ClassId;
                    lessonViewModel.No = lesson.No;
                    lessonViewModel.Date = lesson.Date;
                    //lessonViewModel.FromTime = lesson.FromTime;
                    lessonViewModel.Venue = lesson.Venue;
                    //lessonViewModel.ToTime = lesson.ToTime;
                    lessonViewModel.LocationId = lesson.LocationId;

                    lessonViewModel.TimeFromMin = lesson.TimeFromMin;
                    lessonViewModel.TimeFromHrs = lesson.TimeFromHrs;
                    lessonViewModel.TimeToMin = lesson.TimeToMin;
                    lessonViewModel.TimeToHrs = lesson.TimeToHrs;

                    aClass.Lessons.Add(lessonViewModel);
                }

                foreach (var exam in item.Exams)
                {
                    var examViewModel = new ExamViewModel();
                    examViewModel.Id = exam.Id;
                    examViewModel.ClassId = exam.ClassId;
                    examViewModel.ExamVenue = exam.ExamVenue;
                    examViewModel.Type = exam.Type;
                    examViewModel.ClassCommonId = exam.ClassCommonId;
                    examViewModel.Date = exam.Date;
                    examViewModel.FromTime = exam.FromTime;
                    examViewModel.ToTime = exam.ToTime;
                    examViewModel.Dateline = exam.Dateline;
                    examViewModel.Marks = exam.Marks;
                    examViewModel.IsReExam = exam.IsReExam;
                    examViewModel.ExamVenueText = exam.ExamVenueText;
                    examViewModel.ModuleId = exam.ModuleId;

                    if (exam.Type == (int)Enums.ExamType.Exam)
                    {
                        if (exam.Type == (int)Enums.ExamType.SecondReExam)
                            aClass.SecondReExams.Add(examViewModel);
                        else
                            aClass.FirstReExam.Add(examViewModel);
                    }
                    else
                    {
                        aClass.Exams.Add(examViewModel);
                    }
                }


                result.ClassViewDetailModels.Add(aClass);
            }


            return result;
        }

        public static BatchPaymentItemViewModel UsersToBatchPaymentItemViewModel(this ApplicationUser model, bool isChineseName)
        {
            var result = new BatchPaymentItemViewModel();

            result.UserId = model.Id;
            result.ApplicantName = isChineseName ? model.Particular.SurnameCN + " " + model.Particular.GivenNameCN : model.Particular.SurnameEN + " " + model.Particular.GivenNameEN;
            result.CicNumber = model.CICNumber;
            result.ApplicationIdSelected = model.Applications.FirstOrDefault().Id;
            result.ListCourseAvailable = model.Applications.Select(p => p.ToCourseAvailable()).ToList();

            return result;
        }

        public static BatchPaymentItemViewModel ApplicationToBatchPaymentItemViewModel(this IGrouping<ApplicationUser, Application> model, bool isChineseName, int? selectedApplicationId = null)
        {
            var result = new BatchPaymentItemViewModel();

            result.UserId = model.Key.Id;
            result.ApplicantName = isChineseName ? model.Key.Particular.SurnameCN + " " + model.Key.Particular.GivenNameCN : model.Key.Particular.SurnameEN + " " + model.Key.Particular.GivenNameEN;
            result.CicNumber = model.Key.CICNumber;
            result.ApplicationIdSelected = selectedApplicationId.HasValue ? model.FirstOrDefault(x => x.Id == selectedApplicationId).Id : model.FirstOrDefault().Id;
            result.ListCourseAvailable = model.Select(x => x.ToCourseAvailable()).ToList();

            return result;
        }

        public static CourseAvailable ToCourseAvailable(this Application model)
        {
            var course = model.Course;
            if (course == null) { return null; }

            var lastInvoice = model.Invoices.LastOrDefault();
            var result = new CourseAvailable();
            result.CourseId = course.Id;
            result.CourseCode = course.CourseCode;
            result.ApplicationId = model.Id;
            result.AcademicYear = course.TargetClasses?.AcademicYear;
            result.InvoiceStatus = lastInvoice?.Status;

            if (lastInvoice?.Status == (int)SPDC.Common.Enums.InvoiceStatus.PaidPartially)
            {
                decimal tempFee = 0;

                foreach (var item in lastInvoice.PaymentTransactions)
                {
                    tempFee += item.Amount;
                }

                result.CourseFee = lastInvoice.Fee - tempFee;
            }
            else
            {
                result.CourseFee = lastInvoice?.Fee;
            }

            return result;
        }

        public static MakeupClassViewModel ToMakeUpClassViewModel(this MakeUpClass model, string apiPath)
        {
            MakeupClassViewModel vm = new MakeupClassViewModel();
            vm.Date = model.Date;
            vm.Id = model.Id;
            vm.Name = model.Name;
            vm.TimeFromHrs = model.TimeFromHrs;
            vm.TimeFromMin = model.TimeFromMin;
            vm.TimeToHrs = model.TimeToHrs;
            vm.TimeToMin = model.TimeToMin;
            vm.Venue = model.Venue;
            vm.Documents = new List<MakeupClassDocViewModel>();
            vm.Attendances = new List<MakeUpAttendenceViewModel>();

            foreach (Document doc in model.Documents)
            {
                vm.Documents.Add(new MakeupClassDocViewModel()
                {
                    Id = doc.Id,
                    Name = doc.FileName,
                    ContentType = doc.ContentType,
                    Url = $"{ConfigHelper.GetByKey("DMZAPI")}api/Proxy?url=ApplicationManagement/DownloadMakeupClassDoc/{doc.Id}"
                });
            }

            foreach (var att in model.MakeUpAttendences)
            {
                vm.Attendances.Add(new MakeUpAttendenceViewModel
                {
                    Id = att.Id,
                    ApplicationId = att.ApplicationId,
                    IsDisplayToStudentPortal = att.IsDisplayToStudentPortal,
                    LessonId = att.LessonId,
                    MakeUpClassId = att.MakeUpClassId,
                    ApplicationNumber = att.Application.ApplicationNumber,
                    ClassCode = att.Lesson.Class.ClassCode,
                    Date = att.Lesson.Date.ToString("dd/MM/yyyy"),
                    TimeFrom = att.Lesson.TimeFromHrs + " " + att.Lesson.TimeFromMin,
                    TimeTo = att.Lesson.TimeToHrs + " " + att.Lesson.TimeToMin,
                    UserId = att.Application.UserId,
                    StudentGivenCN = att.Application.User.Particular.GivenNameCN ?? "",
                    StudentGivenEN = att.Application.User.Particular.GivenNameEN ?? "",
                    StudentSurnameCN = att.Application.User.Particular.SurnameCN ?? "",
                    StudentSurnameEN = att.Application.User.Particular.SurnameEN ?? ""
                });
            }

            return vm;
        }
        public static List<MakeupClassViewModel> ToMakeUpClassViewModels(this List<MakeUpClass> models, string apiPath)
        {
            List<MakeupClassViewModel> vms = new List<MakeupClassViewModel>();

            foreach (var model in models)
            {
                MakeupClassViewModel vm = new MakeupClassViewModel();
                vm.Date = model.Date;
                vm.Id = model.Id;
                vm.Name = model.Name;
                vm.TimeFromHrs = model.TimeFromHrs;
                vm.TimeFromMin = model.TimeFromMin;
                vm.TimeToHrs = model.TimeToHrs;
                vm.TimeToMin = model.TimeToMin;
                vm.Venue = model.Venue;
                vm.Documents = new List<MakeupClassDocViewModel>();
                foreach (Document doc in model.Documents)
                {
                    vm.Documents.Add(new MakeupClassDocViewModel()
                    {
                        Id = doc.Id,
                        Name = doc.FileName,
                        ContentType = doc.ContentType,
                        Url = $"{ConfigHelper.GetByKey("DMZAPI")}api/Proxy?url=ApplicationManagement/DownloadMakeupClassDoc/{doc.Id}"
                    });
                }
                vms.Add(vm);
            }
            return vms;
        }

        public static AssessmentTableViewModel ToAssessmentRecordViewModel(this List<Application> model)
        {
            var result = new AssessmentTableViewModel();
            var firstApplication = model.FirstOrDefault();
            result.CurrentClassStatus = firstApplication.AdminAssignedClassModel.SubClassStatus;
            result.AttendanceRequirement = firstApplication.AdminAssignedClassModel.AttendanceRequirement ?? 0;
            result.CommonId = firstApplication.AdminAssignedClassModel.ClassCommonId ?? 0;
            result.ExamPassingRequirementSuffix = firstApplication.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)ExamType.Exam)?.ClassCommonId;
            result.MaxReExamCount = firstApplication.AdminAssignedClassModel.CountReExam;

            foreach (var application in model)
            {
                var applicationModel = new AssessmentRecordViewModel();
                applicationModel.ApplicationId = application.Id;

                applicationModel.StudentNo = application.ApplicationNumber;
                applicationModel.ChineseName = application.User.Particular.SurnameCN + " " + application.User.Particular.GivenNameCN;
                applicationModel.EnglishName = application.User.Particular.SurnameEN + " " + application.User.Particular.GivenNameEN;
                applicationModel.CICno = application.User.CICNumber;

                var totalLessonOfClass = application.AdminAssignedClassModel.Lessons.Count() == 0 ? 1 : application.AdminAssignedClassModel.Lessons.Count();
                var totalLessonAttendance = application.LessonAttendances.Where(x => x.IsTakeAttendance);
                var totalLessonMakeUp = application.MakeUpAttendences.Where(x => x.IsTakeAttendance);

                applicationModel.AttendancePercentage = (int)(((double)totalLessonAttendance.Count() + (double)totalLessonMakeUp.Count()) / totalLessonOfClass * 100);
                applicationModel.AttendanceLesson = totalLessonAttendance.Count() + totalLessonMakeUp.Count();

                #region Calculate Hour Attendance
                var toDay = DateTime.Now;
                double totalAttendanceMinute = 0;

                foreach (var item in totalLessonAttendance)
                {
                    var fromTime = new DateTime(toDay.Year, toDay.Month, toDay.Day, item.Lesson.TimeFromHrs, item.Lesson.TimeFromMin, 0);
                    var toTime = new DateTime(toDay.Year, toDay.Month, toDay.Day, item.Lesson.TimeToHrs, item.Lesson.TimeToMin, 0);

                    TimeSpan difference = toTime - fromTime;
                    totalAttendanceMinute += difference.TotalMinutes;
                }

                foreach (var item in totalLessonMakeUp)
                {
                    var fromTime = new DateTime(toDay.Year, toDay.Month, toDay.Day, item.MakeUpClass.TimeFromHrs, item.MakeUpClass.TimeFromMin, 0);
                    var toTime = new DateTime(toDay.Year, toDay.Month, toDay.Day, item.MakeUpClass.TimeToHrs, item.MakeUpClass.TimeToMin, 0);

                    TimeSpan difference = toTime - fromTime;
                    totalAttendanceMinute += difference.TotalMinutes;
                }

                var hourAttendance = Math.Round(totalAttendanceMinute / 60, 1);
                #endregion
                applicationModel.AttendanceHours = hourAttendance;
                applicationModel.EligibleForAttendance = application.EligibleForAttendanceCertification;
                applicationModel.AttendanceCertificateIssueDate = application.AttendanceCertificateIssueDate;
                applicationModel.ExamAssessmentMarks = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)SPDC.Common.Enums.ExamType.Exam)?.ExamApplications.FirstOrDefault(x => x.ApplicationId == application.Id)?.AssessmentMark;
                applicationModel.ExamAssessmentResult = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)SPDC.Common.Enums.ExamType.Exam)?.ExamApplications.FirstOrDefault(x => x.ApplicationId == application.Id)?.AssessmentResult;
                applicationModel.FirstExamAssessmentMarks = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)SPDC.Common.Enums.ExamType.FirstReExam)?.ExamApplications.FirstOrDefault(x => x.ApplicationId == application.Id)?.AssessmentMark;
                applicationModel.FirstExamAssessmentResult = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)SPDC.Common.Enums.ExamType.FirstReExam)?.ExamApplications.FirstOrDefault(x => x.ApplicationId == application.Id)?.AssessmentResult;
                applicationModel.SecondExamAssessmentMarks = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)SPDC.Common.Enums.ExamType.SecondReExam)?.ExamApplications.FirstOrDefault(x => x.ApplicationId == application.Id)?.AssessmentMark;
                applicationModel.SecondExamAssessmentResult = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)SPDC.Common.Enums.ExamType.SecondReExam)?.ExamApplications.FirstOrDefault(x => x.ApplicationId == application.Id)?.AssessmentResult;
                applicationModel.EnrollmentStatus = application.EnrollmentStatusStorages.LastOrDefault()?.Status;
                applicationModel.EligibleForCourseCompletion = application.EligibleForCourseCompletionCertification;
                applicationModel.CourseCompletionCertificateIssueDate = application.CourseCompletionCertificateIssueDate;
                applicationModel.Credit = application.Course.Credits;
                applicationModel.CreditAcquired = application.CreditAcquired;
                applicationModel.Remarks = application.RemarksExam;
                applicationModel.EligibleForResitExam = application.EligibleForResitExam;
                applicationModel.FirstReExamDate = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)Enums.ExamType.FirstReExam)?.Date;
                applicationModel.SecondReExamDate = application.AdminAssignedClassModel.Exams.FirstOrDefault(x => x.Type == (int)Enums.ExamType.SecondReExam)?.Date;

                foreach (var item in application.ApplicationAssessmentDocuments)
                {
                    applicationModel.ListSupportingDocument.Add(new FileViewModel()
                    {
                        DocId = item.Document.Id,
                        FileName = item.Document.FileName,
                        Url = ConfigHelper.GetByKey("DMZAPI") + "api/Proxy?url=DownloadMakeupClassDoc/" + item.Document.Id
                    });
                }
                //applicationModel.ListSupportingDocument

                var reExamInvoice = application.Invoices.Where(x => x.InvoiceItems.Any(c => c.InvoiceItemTypeId == (int)SPDC.Common.Enums.InvoiceItemType.ReExamFee));
                applicationModel.FirstReExamInvoiceStatus = reExamInvoice.FirstOrDefault(x => x.TypeReExam == (int)Enums.ExamType.FirstReExam)?.Status;
                applicationModel.SecondReExamInvoiceStatus = reExamInvoice.FirstOrDefault(x => x.TypeReExam == (int)Enums.ExamType.SecondReExam)?.Status;

                applicationModel.EmailStatus = application.EmailStatus != null ? application.EmailStatus.Split(',') : null;

                result.ListAssessmentRecord.Add(applicationModel);
            }

            return result;
        }

        public static StudentSearchViewModel ToStudentSearchViewModel(this Application model)
        {
            return new StudentSearchViewModel()
            {
                ApplicationId = model.Id,
                ApplicationNumber = model.ApplicationNumber,
                StudentGivenCN = model.User.Particular.GivenNameCN,
                StudentGivenEN = model.User.Particular.GivenNameEN,
                StudentSurnameCN = model.User.Particular.SurnameCN,
                StudentSurnameEN = model.User.Particular.SurnameEN,
                UserId = model.UserId
            };
        }

        public static List<ReExamFromViewModel> ToReExamFromViewModel(this Class model)
        {
            var result = new List<ReExamFromViewModel>();
            var examOrdered = model.Exams.Count > 0 ? model.Exams.OrderBy(c => c.Date) : null;
            foreach (var item in examOrdered)
            {
                if (item.Type == (int)Enums.ExamType.Exam)
                {
                    continue;
                }
                var examViewModel = new ReExamFromViewModel();
                examViewModel.ExamId = item.Id;
                examViewModel.DisplayName = model.ClassCode + " " + item.Date.ToShortDateString() + " (" + item.FromTime + " - " + item.ToTime + ")";
                examViewModel.ExamVenue = item.ExamVenue;
                result.Add(examViewModel);
            }

            return result;
        }

        public static Particular ConvertParticularTempToParticular(this Particular model, ParticularTemp tempModel)
        {
            model.SurnameEN = tempModel.SurnameEN;
            model.GivenNameEN = tempModel.GivenNameEN;
            model.SurnameCN = tempModel.SurnameCN;
            model.GivenNameCN = tempModel.GivenNameCN;
            model.DateOfBirth = tempModel.DateOfBirth;
            model.Gender = tempModel.Gender;
            model.HKIDNo = tempModel.HKIDNo;
            model.PassportNo = tempModel.PassportNo;

            model.PassportExpiryDate = tempModel.PassportExpiryDate;
            model.MobileNumber = tempModel.MobileNumber;
            model.MobileNumberPrefix = tempModel.MobileNumberPrefix;
            model.TelNo = tempModel.TelNo;
            model.FaxNo = tempModel.FaxNo;
            model.RegionEN = tempModel.RegionEN;
            model.RegionCN = tempModel.RegionCN;
            model.DistrictEN = tempModel.DistrictEN;
            model.DistrictCN = tempModel.DistrictCN;
            model.StreetNumberEN = tempModel.StreetNumberEN;
            model.StreetNumberCN = tempModel.StreetNumberCN;
            model.StreetRoadEN = tempModel.StreetRoadEN;
            model.StreetRoadCN = tempModel.StreetRoadCN;
            model.EstateQuartersVillageEN = tempModel.EstateQuartersVillageEN;
            model.EstateQuartersVillageCN = tempModel.EstateQuartersVillageCN;
            model.BuildingEN = tempModel.BuildingEN;
            model.BuildingCN = tempModel.BuildingCN;
            model.FloorEN = tempModel.FloorEN;
            model.FloorCN = tempModel.FloorCN;
            model.RmFtUnitEN = tempModel.RmFtUnitEN;
            model.RmFtUnitCN = tempModel.RmFtUnitCN;
            model.EducationLevelEN = tempModel.EducationLevelEN;
            model.EducationLevelCN = tempModel.EducationLevelCN;
            model.Honorific = tempModel.Honorific;


            model.SameAddress = tempModel.SameAddress;
            model.ResidentialRegionEN = tempModel.ResidentialRegionEN;
            model.ResidentialDistrictEN = tempModel.ResidentialDistrictEN;
            model.ResidentialStreetNumberEN = tempModel.ResidentialStreetNumberEN;
            model.ResidentialStreetRoadEN = tempModel.ResidentialStreetRoadEN;
            model.ResidentialEstateQuartersVillageEN = tempModel.ResidentialEstateQuartersVillageEN;
            model.ResidentialBuildingEN = tempModel.ResidentialBuildingEN;
            model.ResidentialFloorEN = tempModel.ResidentialFloorEN;
            model.ResidentialRmFtUnitEN = tempModel.ResidentialRmFtUnitEN;
            model.ResidentialRegionCN = tempModel.ResidentialRegionCN;
            model.ResidentialDistrictCN = tempModel.ResidentialDistrictCN;
            model.ResidentialStreetNumberCN = tempModel.ResidentialStreetNumberCN;
            model.ResidentialStreetRoadCN = tempModel.ResidentialStreetRoadCN;
            model.ResidentialEstateQuartersVillageCN = tempModel.ResidentialEstateQuartersVillageCN;
            model.ResidentialBuildingCN = tempModel.ResidentialBuildingCN;
            model.ResidentialFloorCN = tempModel.ResidentialFloorCN;
            model.ResidentialRmFtUnitCN = tempModel.ResidentialRmFtUnitCN;
            model.IsPrimamy = tempModel.IsPrimamy;
            model.IsSecondary = tempModel.IsSecondary;
            model.IsTechInst = tempModel.IsTechInst;
            model.IsUniversityCollege = tempModel.IsUniversityCollege;

            foreach (var item in tempModel.ParticularTempTrans)
            {
                model.ParticularTrans.Add(new ParticularTran()
                {
                    LanguageId = item.LanguageId,
                    RelatedQualifications1Text = item.RelatedQualifications1Text,
                    RelatedQualifications2Text = item.RelatedQualifications2Text,
                    PresentEmployer = item.PresentEmployer,
                    Position = item.Position
                });
            }

            model.HKIDNoEncrypted = tempModel.HKIDNoEncrypted;
            model.PassportNoEncrypted = tempModel.PassportNoEncrypted;
            model.MobileNumberEncrypted = tempModel.MobileNumberEncrypted;


            return model;
        }
    }
}

