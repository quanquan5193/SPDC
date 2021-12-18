using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using SPDC.Model.ViewModels.StudentAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPDC.Service.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAll(int index, int pageSize);

        Task<int> Count();

        Task<bool> CICNumberExist(string cicNumber);

        Task<IEnumerable<ApplicationUser>> GetAllUser(int role = 0);

        Task<ApplicationUser> GetUserByID(int userID, string[] includes = null);

        Task<PaginationSet<StudentAccountViewModel>> GetPagingStudentAccount(StudentSearching search);

        Task<ResultModel<bool>> UpdateStudentAccount(StudentAccountBindingModel model, int adminId);

        Task<ResultModel<bool>> UpdateAccount(AccountInfomationBindingModel model);

        Task<IEnumerable<SendMailModel>> GetUsersForNofitication(UserNotificationType userType);

        Task<ApplicationUser> GetUserByEmail(string email);

        Task<ApplicationUser> GetUserByAdminEmail(string email);
    }
    public class UserService : IUserService
    {
        IUserRepository _repository;
        IUnitOfWork _unitOfWork;
        IAdminPermissionRepository _adminPermissionRepository;
        IUserSubscriptionRepository _userSubscriptionRepository;

        public UserService(IUnitOfWork unitOfWork, IUserRepository repository, IAdminPermissionRepository adminPermissionRepository, IUserSubscriptionRepository userSubscriptionRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _adminPermissionRepository = adminPermissionRepository;
            _userSubscriptionRepository = userSubscriptionRepository;
        }

        public async Task<bool> CICNumberExist(string cicNumber)
        {
            return await _repository.CheckContains(x => x.CICNumber == cicNumber);
        }

        public async Task<int> Count()
        {
            var c = await _repository.Count(x => true);
            return c;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAll(int index, int pageSize)
        {
            var lst = await _repository.GetMultiPaging(x => true, "Id", false, index, pageSize, new[] { "Roles", "Logins", "Claims" });
            return lst;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUser(int role = 0)
        {
            var lst = await _repository.GetMulti(x => true, new[] { "Roles", "Logins", "Claims" });

            var resultUser = (await _repository.GetAll());

            return resultUser;
        }

        public async Task<PaginationSet<StudentAccountViewModel>> GetPagingStudentAccount(StudentSearching search)
        {
            PaginationSet<StudentAccountViewModel> result = new PaginationSet<StudentAccountViewModel>();

            search.HKID = string.IsNullOrWhiteSpace(search.HKID) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(search.HKID));
            search.PassportNo = string.IsNullOrWhiteSpace(search.PassportNo) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(search.PassportNo));

            var hkId = !String.IsNullOrEmpty(search.HKID) ? EncryptUtilities.EncryptAes256(search.HKID) : null;
            var passPort = !String.IsNullOrEmpty(search.PassportNo) ? EncryptUtilities.EncryptAes256(search.PassportNo) : null;

            var hkidEncrypt = hkId != null ? EncryptUtilities.GetEncryptedString(hkId) : null;
            var passportEncrypt = passPort != null ? EncryptUtilities.GetEncryptedString(passPort) : null;

            var listUser = (await _repository.GetMultiPaging(x =>
            (string.IsNullOrEmpty(search.StudentNameEN) ? true : (x.Particular.SurnameEN + x.Particular.GivenNameEN).Replace(" ", string.Empty).ToLower().Contains(search.StudentNameEN.Replace(" ", string.Empty).ToLower())) &&
            (string.IsNullOrEmpty(search.StudentNameCN) ? true : (x.Particular.SurnameCN + x.Particular.GivenNameCN).Replace(" ", string.Empty).ToLower().Contains(search.StudentNameCN.Replace(" ", string.Empty).ToLower())) &&
            (string.IsNullOrEmpty(hkidEncrypt) ? true : (x.Particular.HKIDNoEncrypted.Equals(hkidEncrypt))) &&
            (string.IsNullOrEmpty(passportEncrypt) ? true : (x.Particular.PassportNoEncrypted.Equals(passportEncrypt))) &&
            (string.IsNullOrEmpty(search.CICNumber) ? true : (x.CICNumber.Contains(search.CICNumber))) &&
            (string.IsNullOrEmpty(search.LoginEmail) ? true : (x.Email.Contains(search.LoginEmail))) &&
            (x.Roles.Any(v => v.UserId == x.Id && v.RoleId == 3))
            ,
            "Id",
            true,
            search.Index,
            search.Size,
            new string[] { "Particular" }
            )).Select(c => new StudentAccountViewModel
            {
                Id = c.Id,
                StudentNameEN = c.Particular.SurnameEN + " " + c.Particular.GivenNameEN,
                StudentNameCN = c.Particular.SurnameCN + " " + c.Particular.GivenNameCN,
                HKID = c.Particular.HKIDNo != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(c.Particular.HKIDNo)) : null,
                Passport = c.Particular.PassportNo != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(c.Particular.PassportNo)) : null,
                PassportExpiredDate = c.Particular.PassportExpiryDate,
                CICNumber = c.CICNumber,
                LoginEmail = c.Email,
                SurNameCN = c.Particular.SurnameCN,
                GivenNameCN = c.Particular.GivenNameCN,
                SurNameEN = c.Particular.SurnameEN,
                GivenNameEN = c.Particular.GivenNameEN,
                MobileNumber = c.Particular.MobileNumber != null ? EncryptUtilities.OpenSSLEncrypt(EncryptUtilities.DecryptAes256(c.Particular.MobileNumber)) : null,
                DateOfBirth = c.Particular.DateOfBirth,
                CommunicationLanguage = c.CommunicationLanguage
            });

            var total = await _repository.Count(x =>
            (string.IsNullOrEmpty(search.StudentNameEN) ? true : (x.Particular.SurnameEN + x.Particular.GivenNameEN).Replace(" ", string.Empty).ToLower().Contains(search.StudentNameEN.Replace(" ", string.Empty).ToLower())) &&
            (string.IsNullOrEmpty(search.StudentNameCN) ? true : (x.Particular.SurnameCN + x.Particular.GivenNameCN).Replace(" ", string.Empty).ToLower().Contains(search.StudentNameCN.Replace(" ", string.Empty).ToLower())) &&
            (string.IsNullOrEmpty(hkidEncrypt) ? true : (x.Particular.HKIDNoEncrypted.Equals(hkidEncrypt))) &&
            (string.IsNullOrEmpty(passportEncrypt) ? true : (x.Particular.PassportNoEncrypted.Equals(passportEncrypt))) &&
            (string.IsNullOrEmpty(search.CICNumber) ? true : (x.CICNumber.Contains(search.CICNumber))) &&
            (string.IsNullOrEmpty(search.LoginEmail) ? true : (x.Email.Contains(search.LoginEmail))) &&
            (x.Roles.Any(v => v.UserId == x.Id && v.RoleId == 3)));

            result.Page = search.Index;
            result.TotalCount = total;
            result.Items = listUser;
            return result;
        }

        public async Task<ResultModel<bool>> UpdateStudentAccount(StudentAccountBindingModel model, int adminId)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            var userAdmin = await _adminPermissionRepository.GetSingleByCondition(x => x.Id == adminId);

            if (userAdmin == null || userAdmin.IsActiveAdmin == false)
            {
                result.Message = "Account has no permission";
                return result;
            }

            var user = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Particular", "Particular.ParticularTrans" });
            if (!model.Email.Trim().Equals(user.Email.Trim()))
            {
                var checkEmailExist = await _repository.CheckContains(x =>
                (x.AdminEmail.Trim().Equals(model.Email.Trim()) || x.Email.Trim().Equals(model.Email.Trim()))
                && x.Id != user.Id
                );
                if (checkEmailExist)
                {
                    result.Message = "Email already existed";
                    return result;
                }

            }

            if (!string.IsNullOrWhiteSpace(model.PassportNo))
            {
                model.PassportNo = EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.PassportNo));
            }
            var passportEncrypt = string.IsNullOrEmpty(model.PassportNo) ? null : EncryptUtilities.EncryptAes256(model.PassportNo.Trim());
            var passportEncryptString = passportEncrypt == null ? null : EncryptUtilities.GetEncryptedString(passportEncrypt);

            if (passportEncryptString != null && !passportEncryptString.Equals(user.Particular.PassportNoEncrypted))
            {
                var checkPassportExist = await _repository.CheckContains(x => x.Particular.PassportNoEncrypted.Equals(passportEncryptString) && x.Id != model.Id);
                if (checkPassportExist)
                {
                    result.Message = "Passport already existed";
                    return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Hkid))
            {
                model.Hkid = EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.Hkid));
            }
            var hkidEncrypt = string.IsNullOrEmpty(model.Hkid) ? null : EncryptUtilities.EncryptAes256(model.Hkid.Trim());
            var hkidEncryptString = hkidEncrypt == null ? null : EncryptUtilities.GetEncryptedString(hkidEncrypt);
            if (hkidEncryptString != null && !hkidEncryptString.Equals(user.Particular.HKIDNoEncrypted))
            {
                var checHkidExist = await _repository.CheckContains(x => x.Particular.HKIDNoEncrypted.Equals(hkidEncryptString) && x.Id != model.Id);
                if (checHkidExist)
                {
                    result.Message = "HKID already existed";
                    return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.MobileNumber))
            {
                model.MobileNumber = EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.MobileNumber));
            }
            var mobileNumberEncrypt = string.IsNullOrEmpty(model.MobileNumber) ? null : EncryptUtilities.EncryptAes256(model.MobileNumber.Trim());
            var mobileNumberEncryptString = mobileNumberEncrypt == null ? null : EncryptUtilities.GetEncryptedString(mobileNumberEncrypt);
            if (mobileNumberEncryptString != null && !mobileNumberEncryptString.Equals(user.Particular.MobileNumberEncrypted))
            {
                var checMobileNumberExist = await _repository.CheckContains(x => x.Particular.MobileNumberEncrypted.Equals(mobileNumberEncryptString) && x.Id != model.Id);
                if (checMobileNumberExist)
                {
                    result.Message = "Mobile number already existed";
                    return result;
                }
            }

            user.Email = model.Email.Trim();
            user.Particular.PassportNo = passportEncrypt;
            user.Particular.PassportNoEncrypted = passportEncryptString;
            user.Particular.PassportExpiryDate = model.PassportExpiredDate;
            user.Particular.HKIDNo = hkidEncrypt;
            user.Particular.HKIDNoEncrypted = hkidEncryptString;
            user.Particular.MobileNumber = EncryptUtilities.EncryptAes256(model.MobileNumber);
            user.Particular.DateOfBirth = model.DateOfBirth;
            user.Particular.SurnameCN = model.SurNameCN;
            user.Particular.GivenNameCN = model.GivenNameCN;
            user.Particular.SurnameEN = model.SurNameEN;
            user.Particular.GivenNameEN = model.GivenNameEN;
            user.CommunicationLanguage = model.CommunicationLanguage;

            _repository.Update(user);
            _unitOfWork.Commit();

            result.Message = "Success";
            result.IsSuccess = true;
            return result;
        }


        public async Task<ApplicationUser> GetUserByID(int userID, string[] includes = null)
        {
            var resultUser = (await _repository.GetSingleByCondition(x => x.Id == userID, includes));

            return resultUser;
        }

        public async Task<ResultModel<bool>> UpdateAccount(AccountInfomationBindingModel model)
        {
            ResultModel<bool> result = new ResultModel<bool>();
            ApplicationUser user = await _repository.GetSingleByCondition(x => x.Id == model.Id, new string[] { "Particular" });
            if (user == null) return result;

            user.DisplayName = model.DisplayName;
            user.OtherEmail = model.OtherEmailContact;
            user.CommunicationLanguage = model.CommunicationLanguage;
            user.IsNotReceiveInfomation = model.IsNotReceiveInfomation;
            user.Particular.InterestedTypeOfCourse = model.CourseInterestedString;
            _repository.Update(user);
            _unitOfWork.Commit();

            result.IsSuccess = true;
            result.Message = "Success";
            return result;
        }

        public async Task<IEnumerable<SendMailModel>> GetUsersForNofitication(UserNotificationType userType)
        {
            switch (userType)
            {
                case UserNotificationType.NonRegisteredUsers:
                    return (await _userSubscriptionRepository.GetAll()).Select(s => new SendMailModel()
                    {
                        UserId = s.Id,
                        Email = s.Email,
                        CommunicationLanguage = s.CommunicationLanguage,
                        InterestedTypeOfCourse = RegexUtilities.ConvertToIntArray(s.InterestedTypeOfCourse),
                        FirstNameCN = s.FirstNameCN,
                        LastNameCN = s.LastNameCN,
                        FirstNameEN = s.FirstNameEN,
                        LastNameEN = s.LastNameEN
                    });
                case UserNotificationType.UserHasNoCourse:
                    var userHasNoCourse = await _repository.GetMulti(x => x.Applications.Count == 0 && !x.Roles.Any(r => r.RoleId == 2) && x.CommunicationLanguage.HasValue, new string[] { "UserDevices", "Particular" });
                    return userHasNoCourse.Select(s => new SendMailModel()
                    {
                        UserId = s.Id,
                        Email = s.Email,
                        CommunicationLanguage = (int)s.CommunicationLanguage,
                        FirstNameCN = s.Particular.GivenNameCN,
                        LastNameCN = s.Particular.SurnameCN,
                        FirstNameEN = s.Particular.GivenNameEN,
                        LastNameEN = s.Particular.SurnameEN,
                        DeviceToken = s.UserDevices.Count > 0 ? s.UserDevices.Select(x => x.DeviceToken).ToArray() : new string[0]
                    });
                case UserNotificationType.UserHasCourse:
                    var userHasCourse = await _repository.GetMulti(x => x.Applications.Count > 0 && !x.Roles.Any(r => r.RoleId == 2) && x.CommunicationLanguage.HasValue, new string[] { "UserDevices", "Particular" });
                    return userHasCourse.Select(s => new SendMailModel()
                    {
                        UserId = s.Id,
                        Email = s.Email,
                        CommunicationLanguage = (int)s.CommunicationLanguage,
                        FirstNameCN = s.Particular.GivenNameCN,
                        LastNameCN = s.Particular.SurnameCN,
                        FirstNameEN = s.Particular.GivenNameEN,
                        LastNameEN = s.Particular.SurnameEN,
                        DeviceToken = s.UserDevices.Count > 0 ? s.UserDevices.Select(x => x.DeviceToken).ToArray() : new string[0]
                    });
                default:
                    return null;
            }
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _repository.GetSingleByCondition(x => x.Email.Equals(email), new string[] { "Particular" });
        }

        public async Task<ApplicationUser> GetUserByAdminEmail(string email)
        {
            return await _repository.GetSingleByCondition(x => x.AdminEmail.Equals(email));
        }
    }
}
