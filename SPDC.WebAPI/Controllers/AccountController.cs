using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using SPDC.Common;
using SPDC.Data;
using SPDC.Model.Models;
using SPDC.Service.Services;
using SPDC.WebAPI.Helpers;
using SPDC.Model.ViewModels;
using SPDC.WebAPI.Providers;
using SPDC.WebAPI.Results;
using System.Net;
using SPDC.WebAPI.Models;
using AutoMapper;
using static SPDC.Common.StaticConfig;
using System.Linq;
using Newtonsoft.Json;
using SPDC.Common.Enums;
using System.Net.Http.Headers;
using SPDC.Model.BindingModels;
using System.DirectoryServices;
using SPDC.Model.ViewModels.AdminPrivileges;
using SPDC.Data.Repositories;
using SPDC.Model.ViewModels.StudentAccount;
using System.Web.Script.Serialization;
using SPDC.Model.BindingModels.EmailConfirmation;
using SPDC.Model.BindingModels.Permission;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiControllerBase
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private IParticularService _particularService;
        private IWorkExperienceService _workExperienceService;
        private IEmployerRecommendationService _employerRecommendationService;
        private IUserService _userService;
        private IDocumentService _documentService;
        private ISystemPrivilegeService _systemPrivilegeService;
        private ILanguageService _languageService;
        private IClientService _clientService;
        private ICommonDataService _commonDataService;

        public AccountController(IUserService userService, IParticularService particularService, IWorkExperienceService workExperienceService,
            IEmployerRecommendationService employerRecommendationService, IDocumentService documentService, ISystemPrivilegeService systemPrivilegeService,
            ILanguageService languageService, IClientService clientService, ICommonDataService commonDataService)
        {
            _userService = userService;
            _particularService = particularService;
            _workExperienceService = workExperienceService;
            _employerRecommendationService = employerRecommendationService;
            _documentService = documentService;
            _systemPrivilegeService = systemPrivilegeService;
            _languageService = languageService;
            _clientService = clientService;
            _commonDataService = commonDataService;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Admin/Login")]
        public async Task<HttpResponseMessage> Login()
        {
            using (HttpClient http = new HttpClient())
            {
                if (this.Request.Method == HttpMethod.Get)
                {
                    this.Request.Content = null;
                }

#if DEBUG
                HttpRequestMessage httpRequest = new HttpRequestMessage(this.Request.Method, new System.Uri("https://localhost:44307/api/Account/login?m=1"));
#else
                                HttpRequestMessage httpRequest = new HttpRequestMessage(this.Request.Method, new System.Uri((await _clientService.GetClientUrlByNameAsync("ApiPortal")) + "api/Account/login?m=1"));
#endif

                //HttpRequestMessage httpRequest = new HttpRequestMessage(this.Request.Method, new System.Uri((await _clientService.GetClientUrlByNameAsync("ApiPortal")) + "api/Account/login?m=1"));
                httpRequest.Content = this.Request.Content;

                return await http.SendAsync(httpRequest);
            }
        }


        // POST api/Account/Register
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<HttpResponseMessage> Register(RegisterBindingModel model)
        {
            Log.Info($"Start Register: {model.Email}");
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString()), Data = null });
            }

            ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                Log.Error("Email existed");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("email_exist_error_msg", "CretateAccountSuccess", GetLanguageCode().ToString()), Data = null });
            }

            if (String.IsNullOrEmpty(model.CICNumber))
            {
                //Todo: Request CICNumber to SRMS
                //return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "This CICnumber is required", Data = null });
            }

            DateTime? passportDateToAsign = null;
            var hasPassportExiredDate = false;
            if (string.IsNullOrEmpty(model.HKID) && !String.IsNullOrEmpty(model.PassportExpiredDate))
            {
                var expiredDateConvert = DateTime.Now;
                hasPassportExiredDate = DateTime.TryParse(model.PassportExpiredDate, out expiredDateConvert);
                passportDateToAsign = expiredDateConvert;
            }

            if (string.IsNullOrEmpty(model.HKID) && (String.IsNullOrEmpty(model.PassportNo) || !hasPassportExiredDate))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("hkid_passport_missing_error_msg", "CretateAccountSuccess", GetLanguageCode().ToString()), Data = null });
            }

            model.HKID = string.IsNullOrWhiteSpace(model.HKID) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.HKID));
            model.PassportNo = string.IsNullOrWhiteSpace(model.PassportNo) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.PassportNo));
            model.MobileNumber = string.IsNullOrWhiteSpace(model.MobileNumber) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.MobileNumber));

            var hkId = !String.IsNullOrEmpty(model.HKID) ? EncryptUtilities.EncryptAes256(model.HKID) : null;
            var passPort = !String.IsNullOrEmpty(model.PassportNo) ? EncryptUtilities.EncryptAes256(model.PassportNo) : null;
            var mobileNumber = !String.IsNullOrEmpty(model.MobileNumber) ? EncryptUtilities.EncryptAes256(model.MobileNumber) : null;
            var stringHkId = hkId != null ? EncryptUtilities.GetEncryptedString(hkId) : null;
            var stringPassportNo = passPort != null ? EncryptUtilities.GetEncryptedString(passPort) : null;
            var stringMobileNumber = mobileNumber != null ? EncryptUtilities.GetEncryptedString(mobileNumber) : null;

            //bool isCICNumberExist = await _userService.CICNumberExist(model.CICNumber);
            bool isExistHKidOrPassportNo = await _particularService.IsExistHKidOrPassportNo(0, stringHkId, stringPassportNo);
            bool isExistMobileNumber = await _particularService.IsExistMobileNumber(stringMobileNumber);

            if (/*isCICNumberExist ||*/ isExistHKidOrPassportNo || isExistMobileNumber)
            {
                Log.Error("CICNumber, HKid, MobileNumber or PassportNo is existed");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("hkid_passport_exist_error_msg", "CretateAccountSuccess", GetLanguageCode().ToString()), Data = null });
            }
            if (String.IsNullOrEmpty(model.CICNumber))
            {
                model.CICNumber = DateTime.Now.Ticks.ToString();
            }



            var particular = new Particular()
            {
                HKIDNo = hkId,
                PassportNo = passPort,
                PassportExpiryDate = passportDateToAsign,
                SurnameCN = model.SurnameCN,
                GivenNameCN = model.GivenNameCN,
                SurnameEN = model.SurnameEN,
                GivenNameEN = model.GivenNameEN,
                MobileNumber = mobileNumber,
                DateOfBirth = model.DateOfBirth,
                HKIDNoEncrypted = stringHkId,
                PassportNoEncrypted = stringPassportNo,
                MobileNumberEncrypted = stringMobileNumber
            };

            user = new ApplicationUser()
            {
                UserName = model.Email,
                DisplayName = model.SurnameEN + " " + model.GivenNameEN,
                Email = model.Email,
                CICNumber = model.CICNumber,
                CommunicationLanguage = model.CommunicationLanguage,
                Status = 0,
                CreateDate = DateTime.UtcNow,
                IsNotReceiveInfomation = model.IsNotReceiveInfomation,
                Particular = particular
            };

            try
            {
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                UserManager.AddToRoles(user.Id, new string[] { "User" });
                if (!result.Succeeded)
                {
                    Log.Error("Create user failed");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("create_failure_msg", "CretateAccountSuccess", GetLanguageCode().ToString()), Data = null });
                }
                Log.Info("Create user success");
                // images/email/top.jpg // ConfigurationManager.AppSettings["UrlAdmin"]

                var content = await GenerateEmailContent(user, EmailType.ActiveEmail);

                string emailSubject = FileHelper.GetEmailSubject("item1", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");

                var sent = MailHelper.SendMail(user.Email, emailSubject, content/*, GenerateResourceImage()*/);

                if (sent)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);

                    Log.Info("Email was sent");
                    return Request.CreateResponse(HttpStatusCode.OK, new ActionResultModel() { Message = FileHelper.GetServerMessage("create_success_msg", "CretateAccountSuccess", GetLanguageCode().ToString()), Data = null, Success = true });
                }
                else
                {
                    Log.Error("Send email was failed");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("send_mail_error_msg", "CretateAccountSuccess", GetLanguageCode().ToString()), Data = null });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Register: {ex.Message}");
                throw;
            }
        }

        private async Task<string> GenerateEmailContent(ApplicationUser user, EmailType type, string otherContactEmail = null)
        {
            string code = "";
            var url = await _clientService.GetClientUrlByNameAsync("ApplicantPortal");
            string callbackUrl = "";
            string content = "";

            switch (type)
            {
                case EmailType.ActiveEmail:
                    code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    code = System.Web.HttpUtility.UrlEncode(code);
                    callbackUrl = url + (user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "en/" : "hk/") + @"home?email=" + user.Email + "&code=" + code;
                    content = Common.Common.GenerateActiveEmailContent(user, callbackUrl);
                    #region Generate Email template with iamge
                    //content = user.CommunicationLanguage != 1 ? $"<!DOCTYPE html><html><body><div style = 'width: 960px;'><img src = 'cid:TopImage' width = '100%' /><div style = 'width: 90%; margin-left: 10%;' >{user.Particular?.SurnameEN} {user.Particular?.GivenNameEN}<br/>請點擊以下連結啟動貴公司<<註冊專門行業承造商制度>>電子平台帳戶: <a href=\"{callbackUrl}\">Link</a><br/><br/>若無法點擊以上連結，請將網址複制並貼上至新的瀏覽器視窗中。<br/>如有對帳戶的問題有任何查詢，請致電2100 9400與本處職員聯絡。<br/>感謝使用<<註冊專門行業承造商制度>>電子平台。<br/><br/><br/>建造業議會秘書處<br/>註冊事務</div><div style = 'float: left; width: 40%;' ><img src = 'cid:BottomImage' /></div ><div style = 'float: left; width: 60%; padding-top: 10%;' ><b>重要提示:</b><br/><br/>1. 此乃系統自動產生的電郵，請勿回覆。<br/><br/>2. 此電郵提示所載的是保密資料，並可被視為享有法律特權的資料。倘若閣下並非指定的收件人，則不可複製、轉發、公開或使用此信息的任何部分。若此信息被誤送到閣下的郵箱，請刪去信息及存於閣下電腦系統內的所有相關副本，並立即通知寄件者。<br/><br/>3. 經互聯網傳送的電郵信息，不保證準時、完全安全、不含錯誤或電腦病毒。寄件者不會承擔所引致任何錯誤或遺漏的責任。</div></div></body></html>" :
                    //    $"<!DOCTYPE html><html><body><div style = 'width: 960px;'><img src = 'cid:TopImage' width = '100%' /><div style = 'width: 90%; margin-left: 10%;' >Dear Mr.{user.Particular?.SurnameEN} {user.Particular?.GivenNameEN}<br/>Please ckick the following link to activate your SPDC portal account: <a href=\"{callbackUrl}\">Link</a><br/><br/>If clicking the link above does not work, copy and paste the URL in a new browser window instead.<br/>For any enquiry on your account, please contact us at Tel. 2100 9400.<br/><br/><br/>Thank you for using SPDC Portal.<br/>CIC Secretariat.</div><div style = 'float: left; width: 40%;' ><img src = 'cid:BottomImage' /></div ><div style = 'float: left; width: 60%; padding-top: 10%;' ><b> Important Notes:</b><br/><br/>1.This is a system generated email. Please do not reply to this email.<br/><br/>2.This email is confidential.It may also be legally privileged. If you are not the addressee you may not copy, foward, disclose or use any part of it. If you jave received this message in error, please delete it and all copies from your system and notify the sender immediately.<br/><br/>3.Internet communications cannot be guaranteed to be timely, secure, erro or virus - free.The sender does not accept liability for any errors or imissions.</div></div></body></html>";
                    #endregion
                    break;
                case EmailType.ForgotEmail:
                    code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    code = System.Web.HttpUtility.UrlEncode(code);
                    callbackUrl = url + (user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "en/" : "hk/") + @"resetpassword?email=" + user.Email + "&code=" + code;
                    content = Common.Common.GenerateForgetPasswordContent(user, callbackUrl);
                    #region Generate Email template with iamge
                    //content = user.CommunicationLanguage != 1 ? $"<!DOCTYPE html><html><body><div style = 'width: 960px;'><img src = 'cid:TopImage' width = '100%' /><div style = 'width: 90%; margin-left: 10%;' >{user.Particular?.SurnameEN} {user.Particular?.GivenNameEN}<br/>我們已收到你要求重設貴公司<<註冊專門行業承造商制度>>電子平台密碼的要求。<br/>請點擊以下連結輸入新密碼： <a href=\"{callbackUrl}\">Link</a><br/>感謝使用<<註冊專門行業承造商制度>>電子平台。<br/>建造業議會秘書處<br/>註冊事務</div><div style = 'float: left; width: 40%;' ><img src = 'cid:BottomImage' /></div ><div style = 'float: left; width: 60%; padding-top: 10%;' ><b>重要提示:</b><br/><br/>1. 此乃系統自動產生的電郵，請勿回覆。<br/><br/>2. 此電郵提示所載的是保密資料，並可被視為享有法律特權的資料。倘若閣下並非指定的收件人，則不可複製、轉發、公開或使用此信息的任何部分。若此信息被誤送到閣下的郵箱，請刪去信息及存於閣下電腦系統內的所有相關副本，並立即通知寄件者。<br/><br/>3. 經互聯網傳送的電郵信息，不保證準時、完全安全、不含錯誤或電腦病毒。寄件者不會承擔所引致任何錯誤或遺漏的責任。</div></div></body></html>" :
                    //    $"<!DOCTYPE html><html><body><div style = 'width: 960px;'><img src = 'cid:TopImage' width = '100%' /><div style = 'width: 90%; margin-left: 10%;' >Dear Mr.{user.Particular?.SurnameEN} {user.Particular?.GivenNameEN} <br/>We have received your password reset request for RSTCS portal.<br/>Please click the following link to enter a new password: <a href=\"{callbackUrl}\">Link</a><br/><br/><br/>Thank you for using RSTCS Portal.<br/>CIC Secretariat.<br/>Registration Services</div><div style = 'float: left; width: 40%;' ><img src = 'cid:BottomImage' /></div ><div style = 'float: left; width: 60%; padding-top: 10%;' ><b> Important Notes:</b><br/><br/>1.This is a system generated email. Please do not reply to this email.<br/><br/>2.This email is confidential.It may also be legally privileged. If you are not the addressee you may not copy, foward, disclose or use any part of it. If you jave received this message in error, please delete it and all copies from your system and notify the sender immediately.<br/><br/>3.Internet communications cannot be guaranteed to be timely, secure, erro or virus - free.The sender does not accept liability for any errors or imissions.</div></div></body></html>";
                    #endregion
                    break;
                case EmailType.ForgotLoginEmail:
                    code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    code = System.Web.HttpUtility.UrlEncode(code);
                    callbackUrl = url + @"resetpassword?email=" + user.Email + "&code=" + code;
                    content = Common.Common.GenerateForgetLoginEmail(user, callbackUrl);
                    #region Generate Email template with iamge
                    //content = user.CommunicationLanguage != 1 ? $"<!DOCTYPE html><html><body><div style = 'width: 960px;'><img src = 'cid:TopImage' width = '100%' /><div style = 'width: 90%; margin-left: 10%;' >{user.Particular?.SurnameEN} {user.Particular?.GivenNameEN}<br/>我們已收到你要求重設貴公司<<註冊專門行業承造商制度>>電子平台密碼的要求。 你的登入電郵是{user.Email}。<br/>請點擊以下連結輸入新密碼： <a href=\"{callbackUrl}\">Link</a><br/>感謝使用<<註冊專門行業承造商制度>>電子平台。<br/>建造業議會秘書處<br/>註冊事務</div><div style = 'float: left; width: 40%;' ><img src = 'cid:BottomImage' /></div ><div style = 'float: left; width: 60%; padding-top: 10%;' ><b>重要提示:</b><br/><br/>1. 此乃系統自動產生的電郵，請勿回覆。<br/><br/>2. 此電郵提示所載的是保密資料，並可被視為享有法律特權的資料。倘若閣下並非指定的收件人，則不可複製、轉發、公開或使用此信息的任何部分。若此信息被誤送到閣下的郵箱，請刪去信息及存於閣下電腦系統內的所有相關副本，並立即通知寄件者。<br/><br/>3. 經互聯網傳送的電郵信息，不保證準時、完全安全、不含錯誤或電腦病毒。寄件者不會承擔所引致任何錯誤或遺漏的責任。</div></div></body></html>" :
                    //    $"<!DOCTYPE html><html><body><div style = 'width: 960px;'><img src = 'cid:TopImage' width = '100%' /><div style = 'width: 90%; margin-left: 10%;' >Dear Mr.{user.Particular?.SurnameEN} {user.Particular?.GivenNameEN} <br/>We have received your password reset request for RSTCS portal. You login email address is {user.Email}.<br/>Please click the following link to enter a new password: <a href=\"{callbackUrl}\">Link</a><br/><br/><br/>Thank you for using RSTCS Portal.<br/>CIC Secretariat.<br/>Registration Services</div><div style = 'float: left; width: 40%;' ><img src = 'cid:BottomImage' /></div ><div style = 'float: left; width: 60%; padding-top: 10%;' ><b> Important Notes:</b><br/><br/>1.This is a system generated email. Please do not reply to this email.<br/><br/>2.This email is confidential.It may also be legally privileged. If you are not the addressee you may not copy, foward, disclose or use any part of it. If you jave received this message in error, please delete it and all copies from your system and notify the sender immediately.<br/><br/>3.Internet communications cannot be guaranteed to be timely, secure, erro or virus - free.The sender does not accept liability for any errors or imissions.</div></div></body></html>";
                    #endregion
                    break;
                default:
                    content = "Content is undefined";
                    break;
            }

            return content;
        }
        private string MapPath(string seedFile)
        {
            if (HttpContext.Current != null)
                return HostingEnvironment.MapPath(seedFile);

            var absolutePath = new System.Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath; //was AbsolutePath but didn't work with spaces according to comments
            var directoryName = Path.GetDirectoryName(absolutePath);
            var path = Path.Combine(directoryName, ".." + seedFile.TrimStart('~').Replace('/', '\\'));

            return path;
        }

        private List<LinkedResource> GenerateResourceImage()
        {
            var listLinkResources = new List<LinkedResource>();
            string mediaType = MediaTypeNames.Image.Jpeg;

            string topImagePath = MapPath(@"~/images/email/Top.jpg");
            LinkedResource img = new LinkedResource(topImagePath, mediaType);
            img.ContentId = "TopImage";
            img.ContentType.MediaType = mediaType;
            img.TransferEncoding = TransferEncoding.Base64;
            img.ContentType.Name = img.ContentId;
            img.ContentLink = new System.Uri("cid:" + img.ContentId);
            listLinkResources.Add(img);

            string bottomImagePath = MapPath(@"~/images/email/Bottom.jpg");
            LinkedResource img2 = new LinkedResource(bottomImagePath, mediaType);
            img2.ContentId = "BottomImage";
            img2.ContentType.MediaType = mediaType;
            img2.TransferEncoding = TransferEncoding.Base64;
            img2.ContentType.Name = img2.ContentId;
            img2.ContentLink = new System.Uri("cid:" + img2.ContentId);
            listLinkResources.Add(img2);

            return listLinkResources;
        }

        //[HttpPost]
        //[Route("Login")]
        //public async Task<HttpResponseMessage> Login(string acc, string pwd)
        //{
        //    // Please Get LDAP Path Here through the system parameter here
        //    string ldapPath = "ldap://dc1.hkcic.org:389"; //this._factory.SystemParameter.LdapPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        //    Log.Info($"Start Login: {acc}");
        //    try
        //    {
        //        DirectoryEntry ldap = new DirectoryEntry(ldapPath, "CIC" + @"\" + acc, pwd);

        //        DirectorySearcher searcher = new DirectorySearcher(ldap);
        //        searcher.Filter = "(SAMAccountName=" + acc + ")";
        //        SearchResult result = searcher.FindOne();

        //        Log.Info($"Completed Login: {acc}");

        //        return Request.CreateResponse(HttpStatusCode.OK, new ActionResultModel() { Message = "Login Success", Data = null, Success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Failed Login {acc}: {ex.Message}");
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = "Login Fail", Data = null });
        //    }
        //}

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<HttpResponseMessage> ConfirmEmail(EmailConfirmationBindingModel model)
        {
            string email = model.Email;
            string code = model.Code;
            Log.Info($"Start Confirm Email: {email}");
            if (email == null || code == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("active_email_fail", "ServerMessages", GetLanguageCode().ToString()), Data = null });
            }
            IdentityResult result;
            try
            {
                code = System.Web.HttpUtility.UrlDecode(code);
                ApplicationUser user = await UserManager.FindByEmailAsync(email);
                result = await UserManager.ConfirmEmailAsync(user.Id, code);
                if (result.Succeeded)
                {
                    Log.Info($"Completed Confirm Email: {email}");
                    return Request.CreateResponse(HttpStatusCode.OK, new ActionResultModel() { Message = FileHelper.GetServerMessage("active_email_success", "ServerMessages", GetLanguageCode().ToString()), Data = null, Success = true });
                }
            }
            catch (InvalidOperationException ioe)
            {
                Log.Error($"Failed Confirm Email {email}: {ioe}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("active_email_fail", "ServerMessages", GetLanguageCode().ToString()), Data = null });
            }

            Log.Error($"Failed Confirm Email {email}");
            return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("active_email_fail", "ServerMessages", GetLanguageCode().ToString()), Data = null });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("resend-activation-email")]
        public async Task<HttpResponseMessage> ResendActivationEmail(ResendActivationEmailBindingModel model)
        {
            string email = model.Email;

            Log.Info($"Start Resend Email: {email}");
            if (String.IsNullOrEmpty(email))
            {
                Log.Error($"Failed Resend Email: {email}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("email_is_required", "ServerMessages", GetLanguageCode().ToString()), Data = null });
            }

            if (IsAdminSystem(email))
            {
                Log.Error($"Failed Resend Email: {email}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("cannot_send_activation_email", "ServerMessages", GetLanguageCode().ToString()), Data = null });
            }

            ApplicationUser user;
            try
            {
                user = await _userService.GetUserByEmail(email);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Resend Email {email}: {ex.Message}");
                throw;
            }

            if (user != null)
            {
                var isActivated = await UserManager.IsEmailConfirmedAsync(user.Id);

                if (isActivated)
                {
                    Log.Error($"Failed Resend Email: {email}");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("account_already_activated", "ServerMessages"), Data = null });
                }

                var content = "";
                content = await GenerateEmailContent(user, EmailType.ActiveEmail);

                string emailSubject = FileHelper.GetEmailSubject("item1", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");

                var sent = MailHelper.SendMail(user.Email, emailSubject, content/*, GenerateResourceImage()*/);

                if (sent)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);

                    Log.Info($"Completed Resend Email: {email}");
                    return Request.CreateResponse(HttpStatusCode.OK, new ActionResultModel() { Message = FileHelper.GetServerMessage("confirmation_email_was_sent"), Data = null, Success = true });
                }
                else
                {
                    Log.Error($"Failed Resend Email: {email}");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("send_email_was_failed"), Data = null });
                }
            }
            else
            {
                Log.Error($"Failed Resend Email: {email}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("user_does_not_exist"), Data = null });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgot-password")]
        public async Task<HttpResponseMessage> ForgotPassword(ForgotPasswordViewModel model)
        {
            Log.Info($"Start Forgot Password: {model.Email}");
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("object_not_valid"), Data = null });
            }
            ApplicationUser user;
            //user = await UserManager.FindByEmailAsync(model.Email);
            user = await _userService.GetUserByEmail(model.Email);

            if (user == null)
            {
                Log.Error($"Failed Forgot Password: {model.Email}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("ForgotPassword", "email_address_message_label"), Data = null });
            }

            var content = await GenerateEmailContent(user, EmailType.ForgotEmail);

            string emailSubject = FileHelper.GetEmailSubject("item2", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");

            var sent = MailHelper.SendMail(user.Email, emailSubject, content/*, GenerateResourceImage()*/);

            if (sent)
            {
                CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                emailCommonData.ValueInt++;
                _commonDataService.Update(emailCommonData);

                Log.Info($"Completed Forgot Password: {model.Email}");
            }
            else
            {
                Log.Error($"Failed Forgot Password: {model.Email}");
            }

            return Request.CreateResponse(HttpStatusCode.OK, new ActionResultModel() { Message = FileHelper.GetServerMessage("forgot_message_success_title_label", "ForgotPassword"), Data = null, Success = true });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgot-login-email")]
        public async Task<HttpResponseMessage> ForgotLoginEmail(ForgotLoginEmailViewModel model)
        {
            Log.Info($"Start Forgot Email - Mobile: {model.MobileNumber}");
            ApplicationUser user;

            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Forgot Email - Mobile: {model.MobileNumber}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString()), Data = null });
            }

            model.HKID = string.IsNullOrWhiteSpace(model.HKID) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.HKID));
            model.PassportNo = string.IsNullOrWhiteSpace(model.PassportNo) ? "" : EncryptUtilities.OpenSSLDecrypt(HttpUtility.UrlDecode(model.PassportNo));

            if (String.IsNullOrEmpty(model.HKID) && String.IsNullOrEmpty(model.PassportNo))
            {
                Log.Error($"Failed Forgot Email - Mobile: {model.MobileNumber}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("object_not_valid"), Data = null });
            }

            user = await _particularService.GetUserByHKIDOrPassportNo(model);

            if (user == null/* || String.IsNullOrEmpty(user.OtherEmail) || !user.OtherEmail.Equals(model.OtherContactEmail)*/)
            {
                Log.Error($"Failed Forgot Email - Mobile: {model.MobileNumber}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("no_user_or_email_matched"), Data = null });
            }
            else
            {
                var content = await GenerateEmailContent(user, EmailType.ForgotLoginEmail, model.OtherContactEmail);

                string emailSubject = FileHelper.GetEmailSubject("item3", "EmailSubject", user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");

                var sent = MailHelper.SendMail(user.OtherEmail, emailSubject, content/*, GenerateResourceImage()*/);

                if (sent)
                {
                    CommonData emailCommonData = await _commonDataService.GetByKey("EmailSerialNumber");
                    emailCommonData.ValueInt++;
                    _commonDataService.Update(emailCommonData);

                    Log.Info($"Completed Forgot Email - Mobile: {model.MobileNumber}");
                }
                else
                {
                    Log.Error($"Failed Forgot Email - Mobile: {model.MobileNumber}");
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, new ActionResultModel() { Message = FileHelper.GetServerMessage("success", "Common"), Data = null, Success = true });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("reset-password")]
        public async Task<HttpResponseMessage> ResetPassword(ResetPasswordViewModel model)
        {
            Log.Info($"Start Reset Password: {model.Email}");
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("object_not_valid"), Data = null });
            }
            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                Log.Error($"Failed Reset Password: {model.Email}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("no_user_matched"), Data = null });

            }

            model.Token = System.Web.HttpUtility.UrlDecode(model.Token);

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                Log.Info($"Completed Reset Password: {model.Email}");
            }
            else
            {
                Log.Error($"Failed Reset Password: {model.Email}");
            }

            return result.Succeeded ? Request.CreateResponse(HttpStatusCode.OK,
                new ActionResultModel() { Message = FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), Data = null, Success = true }) : Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("can_not_update_new_password"), Data = result.Errors });
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordBindingModel model)
        {
            Log.Info("Start Change Password");
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("object_not_valid"), Data = null });
            }
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            Log.Info($"Start Change Password: {user.Email}");

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("no_user_matched"), Data = null });
            }

            user = await UserManager.FindAsync(user.UserName, model.CurrentPassword);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ActionResultModel() { Message = FileHelper.GetServerMessage("current_password_not_match"), Data = 1 });
            }

            var result = await UserManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                Log.Info($"Completed Change Password: {user.Email}");
            }
            else
            {
                Log.Error($"Failed Change Password: {user.Email}");
            }

            return result.Succeeded ? Request.CreateResponse(HttpStatusCode.OK,
                new ActionResultModel() { Message = FileHelper.GetServerMessage("update_success"), Data = null, Success = true }) : Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("can_not_update_new_password"), Data = null });
        }

        [HttpPost]
        [Route("assign-courses")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> AssignCourses(SystemPrivilegeBindingModel model)
        {
            Log.Info("Start Assign Course");
            ApplicationUser user = await UserManager.FindByIdAsync(model.UserId);
            Log.Info($"Start Assign Course - User: {user.Email}");
            user.SystemPrivileges = new List<SystemPrivilege>();

            foreach (var id in model.CourseIds)
            {
                SystemPrivilege systemPrivilege = new SystemPrivilege();
                systemPrivilege.CourseId = id;
                systemPrivilege.UserId = model.UserId;
                user.SystemPrivileges.Add(systemPrivilege);
            }

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                Log.Error($"Failed Assign Course - User: {user.Email}");
                return GetErrorResult(result);
            }

            Log.Info($"Completed Assign Course - User: {user.Email}");
            return Ok();
        }

        [HttpGet]
        [Route("get-account")]
        public async Task<IHttpActionResult> GetAccountInfo()
        {
            Log.Info($"Start Get Account - Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Account - Id: {id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            var user = await _userService.GetUserByID(id, new string[] { "Particular" });

            //var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                Log.Info($"Completed Get Account - Id: {id}");
            }
            else
            {
                Log.Error($"Failed Get Account - Id: {id}");
            }

            var c = Mapper.Map<AccountInfoViewModel>(user);

            return user != null ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, c)) : Content(HttpStatusCode.NotFound, new ActionResultModel("Email not found"));
        }

        [HttpPost]
        [Route("update-account")]
        public async Task<IHttpActionResult> UpdateAccountInfo(AccountInfomationBindingModel model)
        {
            Log.Info($"Start Update Account: {model.Email}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Account: {model.Email}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Account - Id: {id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            model.Id = id;
            model.CourseInterestedString = new JavaScriptSerializer().Serialize(model.InterestedCourse);
            var result = await _userService.UpdateAccount(model);
            if (result.IsSuccess)
            {
                Log.Info($"Completed Update Account: {model.Email}");
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("update_success", "ServerMessages", GetLanguageCode().ToString()), true));
            }
            else
            {
                Log.Error($"Failed Update Account: {model.Email}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure", "ServerMessages", GetLanguageCode().ToString())));
            }


            //var user = await UserManager.FindByEmailAsync(model.Email);

            //if (user.Email.Equals(model.Email) && user.CICNumber.Equals(model.CICNo))
            //{
            //    var updateSuccess = await UserManager.UpdateAsync(user);
            //    if (updateSuccess.Succeeded)
            //    {
            //        Log.Info($"Completed Update Account: {model.Email}");
            //    }
            //    else
            //    {
            //        Log.Error($"Failed Update Account: {model.Email}");
            //    }
            //    return updateSuccess.Succeeded == true ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, Mapper.Map<AccountInfoViewModel>(user))) :
            //        Content(HttpStatusCode.BadRequest, new ActionResultModel("Update was failed"));
            //}
            //else
            //{
            //    Log.Error($"Failed Update Account: {model.Email}");
            //    return Content(HttpStatusCode.BadRequest, new ActionResultModel("Email or CICNo is not match"));
            //}
        }

        [HttpGet]
        [Route("user-qualifications")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserQualification(int userId = 0)
        {
            Log.Info($"Start Get User Qualification - Id: {HttpContext.Current.User.Identity.GetUserId()}");

            int id = 0;
            if (userId != 0)
            {
                id = userId;
            }
            else
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
                if (!success)
                {
                    Log.Error($"Failed Get User Qualification: {id}");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
                }
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Get User Qualification: {id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var qualification = await _particularService.GetQualificationByUserId(user.Id, (int)GetLanguageCode());
            if (qualification != null)
            {
                Log.Info($"Completed Get User Qualification: {id}");
            }
            else
            {
                Log.Error($"Failed Get User Qualification: {id}");
            }
            return qualification != null ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, qualification)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }

        [HttpGet]
        [Route("user-qualifications-temp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserQualificationTemp(int userId = 0)
        {
            Log.Info($"Start Get User Qualification temp - Id: {HttpContext.Current.User.Identity.GetUserId()}");

            int id = 0;
            if (userId != 0)
            {
                id = userId;
            }
            else
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
                if (!success)
                {
                    Log.Error($"Failed Get User Qualification temp: {id}");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
                }
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Get User Qualification temp: {id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var qualification = await _particularService.GetQualificationByUserIdTemp(user.Id, (int)GetLanguageCode());
            if (qualification != null)
            {
                if (qualification.ListSubQualifications.Count > 0)
                {
                    Log.Info($"Completed Get User Qualification temp: {id}");
                }
                else
                {
                    qualification = await _particularService.GetQualificationByUserId(user.Id, (int)GetLanguageCode());
                }
                Log.Info($"Completed Get User Qualification temp: {id}");
            }
            else
            {
                qualification = await _particularService.GetQualificationByUserId(user.Id, (int)GetLanguageCode());
                if (qualification != null)
                {
                    Log.Info($"Completed Get User Qualification temp: {id}");
                }
                else
                {
                    Log.Error($"Failed Get User Qualification temp: {id}");
                }
            }
            return qualification != null ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, qualification)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }

        [HttpPost]
        [Route("update-qualifications")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateQualification(QualificationViewModel model)
        {
            Log.Info($"Start Update User Qualification - Id: {model.Id}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update User Qualification - Id: {model.Id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }
            var success = await _particularService.UpdateQualifications(model, (int)GetLanguageCode());
            if (success)
            {
                Log.Error($"Completed Update User Qualification - Id: {model.Id}");
            }
            else
            {
                Log.Error($"Failed Update User Qualification - Id: {model.Id}");
            }
            return success ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpPost]
        [Route("update-qualifications-temp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> UpdateQualificationTemp(QualificationViewModel model)
        {

            // Case user don't click async button in E-Application Form step 1
            if (model.Id == 0)
            {
                int id = 0;
                if (!int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id))
                {
                    Log.Error($"Failed Get Personal Particulartemp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
                }
                model.Id = id;
            }

            Log.Info($"Start Update User Qualification temp - Id: {model.Id}");
            var success = await _particularService.UpdateQualificationsTemp(model, (int)GetLanguageCode());
            if (success)
            {
                Log.Error($"Completed Update User Qualification temp - Id: {model.Id}");
            }
            else
            {
                Log.Error($"Failed Update User Qualification temp - Id: {model.Id}");
            }
            return success ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpGet]
        [Route("work-exprience")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserWorkExperience(int userId = 0)
        {
            Log.Info($"Start Get User Work Experience: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            if (userId != 0)
            {
                id = userId;
            }
            else
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
                if (!success)
                {
                    Log.Error($"Failed Get User Work Experience: {HttpContext.Current.User.Identity.GetUserId()}");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
                }
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Get User Work Experience: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var workExperience = await _workExperienceService.GetUserWorkExperience(user.Id, (int)GetLanguageCode());
            if (workExperience != null)
            {
                Log.Info($"Completed Get User Work Experience: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Get User Work Experience: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return workExperience != null ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, workExperience)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }


        [HttpGet]
        [Route("work-exprience-temp")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserWorkExperienceTemp(int userId = 0)
        {
            Log.Info($"Start Get User Work Experience Temp: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            if (userId != 0)
            {
                id = userId;
            }
            else
            {
                var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
                if (!success)
                {
                    Log.Error($"Failed Get User Work Experience Temp: {HttpContext.Current.User.Identity.GetUserId()}");
                    return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
                }
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Get User Work Experience Temp: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var workExperience = await _workExperienceService.GetUserWorkExperienceTemp(user.Id, (int)GetLanguageCode());
            if (workExperience.Count > 0)
            {
                Log.Info($"Completed Get User Work Experience Temp: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                workExperience = await _workExperienceService.GetUserWorkExperience(user.Id, (int)GetLanguageCode());
                if (workExperience != null)
                {
                    Log.Info($"Completed Get User Work Experience Temp: {HttpContext.Current.User.Identity.GetUserId()}");
                }
                else
                {
                    Log.Error($"Failed Get User Work Experience Temp: {HttpContext.Current.User.Identity.GetUserId()}");
                }
            }
            return workExperience != null ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, workExperience)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }

        [HttpPost]
        [Route("update-experience")]
        public async Task<IHttpActionResult> UpdateUserWorkExperience(List<WorkExperienceBindingModel> model)
        {
            Log.Info($"Start Update User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }
            var isUpdated = await _workExperienceService.UpdateUserWorkExperience(model, (int)GetLanguageCode(), id);
            if (isUpdated)
            {
                Log.Info($"Completed Update User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Update User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdated ? Content(HttpStatusCode.OK, new ActionResultModel("Success", true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel("Failed to update"));
        }


        [HttpPost]
        [Route("update-experience-temp")]
        public async Task<IHttpActionResult> UpdateUserWorkExperienceTemp(List<WorkExperienceBindingModel> model)
        {

            Log.Info($"Start Update User Work Experience Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update User Work Experience Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            var isUpdated = await _workExperienceService.UpdateUserWorkExperienceTemp(model, (int)GetLanguageCode(), id);
            if (isUpdated)
            {
                Log.Info($"Completed Update User Work Experience Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Update User Work Experience Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdated ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpPost]
        [Route("export-experience")]
        public async Task<HttpResponseMessage> ExportWorkExperience(List<WorkExperienceBindingModel> model)
        {
            Log.Info($"Start Export User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Export User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Export User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            int type = model.FirstOrDefault() != null ? model.FirstOrDefault().ClassifyWorkingExperience : 0;

            var isUpdated = await _workExperienceService.UpdateUserWorkExperience(model, (int)GetLanguageCode(), id, type);
            if (!isUpdated)
            {
                Log.Error($"Failed Export User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
            }

            var result = await _workExperienceService.ExportWorkExperience(id, (int)GetLanguageCode(), type);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Export User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("export_was_failed")));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Export User Work Experience - User Id: {HttpContext.Current.User.Identity.GetUserId()}");

            return httpResponseMessage;
        }

        [HttpGet]
        [Route("employer-recommendation")]
        public async Task<IHttpActionResult> GetEmployerRecommendation()
        {
            Log.Info($"Start Get Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Info($"Failed Get Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Info($"Failed Get Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var emRecommendation = await _employerRecommendationService.GetEmployerRecommendation(user.Id, (int)GetLanguageCode());

            if (emRecommendation != null)
            {
                Log.Info($"Completed Get Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Info($"Failed Get Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }

            return emRecommendation != null ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, emRecommendation)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }


        [HttpGet]
        [Route("employer-recommendation-temp")]
        public async Task<IHttpActionResult> GetEmployerRecommendationTemp()
        {
            Log.Info($"Start Get Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Info($"Failed Get Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Info($"Failed Get Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            var emRecommendation = await _employerRecommendationService.GetEmployerRecommendationTemp(user.Id, (int)GetLanguageCode());

            if (emRecommendation.Count > 0)
            {
                Log.Info($"Completed Get Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                emRecommendation = await _employerRecommendationService.GetEmployerRecommendation(user.Id, (int)GetLanguageCode());

                if (emRecommendation.Count > 0)
                {
                    Log.Info($"Completed Get Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                }
                else
                {
                    Log.Info($"Failed Get Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                }
            }

            return emRecommendation != null ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, emRecommendation)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
        }


        [HttpPost]
        [Route("update-recommendation")]
        public async Task<IHttpActionResult> UpdateEmployerRecommendation(List<RecommendationEmployerBindingModel> model)
        {
            Log.Info($"Start Update Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Info($"Failed Update Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            if (!ModelState.IsValid)
            {
                Log.Info($"Failed Update Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }
            var isUpdated = await _employerRecommendationService.UpdateEmployerRecommendation(model, (int)GetLanguageCode(), id);
            if (isUpdated)
            {
                Log.Info($"Completed Update Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Info($"Failed Update Employer Recommendation - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdated ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }



        [HttpPost]
        [Route("update-recommendation-temp")]
        public async Task<IHttpActionResult> UpdateEmployerRecommendationTemp(List<RecommendationEmployerBindingModel> model)
        {
            Log.Info($"Start Update Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Info($"Failed Update Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            if (!ModelState.IsValid)
            {
                Log.Info($"Failed Update Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }
            var isUpdated = await _employerRecommendationService.UpdateEmployerRecommendationTemp(model, (int)GetLanguageCode(), id);
            if (isUpdated)
            {
                Log.Info($"Completed Update Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Info($"Failed Update Employer Recommendation Temp - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdated ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpGet]
        [Route("user-document")]
        public async Task<IHttpActionResult> GetUserDocument()
        {
            Log.Info($"Start Get User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Get User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var listFileName = await _documentService.GetListUserDocument(user.Id);

            Log.Info($"Completed Get User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, listFileName));
        }

        [HttpPost]
        [Route("update-user-document")]
        public async Task<IHttpActionResult> UpdateUserDocument()
        {
            Log.Info($"Start Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            string delParams = HttpContext.Current.Request.Params["ListFileToDelete"];

            string[] strList = new string[] { };

            if (!string.IsNullOrEmpty(delParams))
            {
                strList = delParams.Split(',');
            }

            List<int> lstFileDelete = new List<int>();
            foreach (var num in strList)
            {
                lstFileDelete.Add(Convert.ToInt32(num));
            }
            //var lstFileDelete = JsonConvert.DeserializeObject<List<int>>(aaa);
            var lstFileInsert = HttpContext.Current.Request.Files;
            for (int i = 0; i < lstFileInsert.Count; i++)
            {
                var file = lstFileInsert[i];
                if (file.ContentLength > StaticConfig.MaximumFileLength)
                {
                    Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    Content(HttpStatusCode.BadRequest, new ActionResultModel($"File {Path.GetFileName(file.FileName)} is too large"));
                }
            }

            var isUpdate = await _documentService.UpdateUserDocument(lstFileDelete, lstFileInsert, id, user.CICNumber);
            if (isUpdate)
            {
                Log.Info($"Completed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdate ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpPost]
        [Route("update-user-document-temp")]
        public async Task<IHttpActionResult> UpdateUserDocumentTemp()
        {
            Log.Info($"Start Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }
            string delParams = HttpContext.Current.Request.Params["ListFileToDelete"];

            string[] strList = new string[] { };

            if (!string.IsNullOrEmpty(delParams))
            {
                strList = delParams.Split(',');
            }

            List<int> lstFileDelete = new List<int>();
            foreach (var num in strList)
            {
                lstFileDelete.Add(Convert.ToInt32(num));
            }
            //var lstFileDelete = JsonConvert.DeserializeObject<List<int>>(aaa);
            var lstFileInsert = HttpContext.Current.Request.Files;
            for (int i = 0; i < lstFileInsert.Count; i++)
            {
                var file = lstFileInsert[i];
                if (file.ContentLength > StaticConfig.MaximumFileLength)
                {
                    Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                    Content(HttpStatusCode.BadRequest, new ActionResultModel($"File {Path.GetFileName(file.FileName)} is too large"));
                }
            }

            var isUpdate = await _documentService.UpdateUserDocumentTemp(lstFileDelete, lstFileInsert, id, user.CICNumber);
            if (isUpdate)
            {
                Log.Info($"Completed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            else
            {
                Log.Error($"Failed Update User Documment - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            }
            return isUpdate ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("update_failure")));
        }

        [HttpGet]
        [Route("download-user-doc")]
        public async Task<HttpResponseMessage> DownloadUserDocument(int docId)
        {
            Log.Info($"Start Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var result = await _documentService.DownloadUserDocument(docId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("export_was_failed")));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");

            return httpResponseMessage;
        }

        [HttpGet]
        [Route("download-user-doc-temp")]
        public async Task<HttpResponseMessage> DownloadUserDocumentTemp(int docId)
        {
            Log.Info($"Start Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var result = await _documentService.DownloadUserDocument(docId);

            if (!result.IsSuccess)
            {
                Log.Error($"Failed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("export_was_failed")));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);
            Log.Info($"Completed Download User Documment - UserId: {HttpContext.Current.User.Identity.GetUserId()} - Doc Id: {docId}");

            return httpResponseMessage;
        }

        [HttpGet]
        [Route("Admins")]
        public async Task<IHttpActionResult> GetAllAdmin()
        {
            Log.Info("Start Get Admin Accounts");
            var admins = UserManager.Users.Where(x => x.Roles.Any(r => r.RoleId == 2) && !x.Email.Equals("admin@gmail.com")).ToList();

            Log.Info("Completed Get Admin Accounts");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, Mapper.Map<IEnumerable<AdminViewModel>>(admins)));
        }

        [HttpPost]
        [Route("Admins/Create")]
        public async Task<IHttpActionResult> CreateAdminAccount(CreateAdminBindingModel model)
        {
            Log.Info($"Start Create Admin Account - LdapAccount: {model.LdapAccount}");
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.LdapAccount) || !RegexUtilities.IsValidEmail(model.Email))
            {
                Log.Error($"Failed Create Admin Account - LdapAccount: {model.LdapAccount}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("LdapAccount and Email are required.", false, null));
            }

            var identity = (ClaimsIdentity)User.Identity;
            var ldapAcc = identity.Claims.FirstOrDefault(x => x.Type == "ldapUsername").Value;
            var ldapPwd = identity.Claims.FirstOrDefault(x => x.Type == "ldapPassword").Value;

            //var ldapAcc = "quan.ntq";
            //var ldapPwd = "12345aA@";

            var admin = await UserManager.FindByNameAsync(model.LdapAccount);

            if (admin == null)
            {
                try
                {
                    DirectoryEntry ldap = new DirectoryEntry("LDAP://" + ConfigHelper.GetByKey("LDAPPortal"), ConfigHelper.GetByKey("LDAPAccountPrefix") + @"\" + ldapAcc, ldapPwd);
                    DirectorySearcher searcher = new DirectorySearcher(ldap);
                    searcher.Filter = "(SAMAccountName=" + model.LdapAccount + ")";
                    //searcher.Filter = "(&(objectClass=user)(objectcategory=person)(mail=" + model.Email + "*))";
                    SearchResult result = searcher.FindOne();

                    if (result == null)
                    {
                        return Content(HttpStatusCode.BadRequest, new ActionResultModel("LDAP account is not exist.", false, null));
                    }

                    var checkUserEmail = await _userService.GetUserByAdminEmail(model.Email);
                    if (checkUserEmail != null)
                    {
                        Log.Error($"Failed Create Admin Account - LdapAccount: {model.LdapAccount}");
                        return Content(HttpStatusCode.BadRequest, new ActionResultModel("Email existed", false, null));
                    }



                    admin = new ApplicationUser()
                    {
                        AdminEmail = model.Email,
                        DisplayName = result.GetDirectoryEntry().Properties["displayname"].Value.ToString(),
                        CreateDate = DateTime.UtcNow,
                        Status = 1,
                        UserName = model.LdapAccount,
                        CICNumber = DateTime.UtcNow.Ticks.ToString(),
                        AdminPermission = new AdminPermission()
                        {
                            Status = 1
                        }
                    };

                    var createResult = await UserManager.CreateAsync(admin);
                    if (!createResult.Succeeded)
                    {
                        Log.Error($"Failed Create Admin Account - LdapAccount: {model.LdapAccount}");
                        return Content(HttpStatusCode.InternalServerError, new ActionResultModel("Something went wrong. Please contact system admin.", false, createResult.Errors));
                    }
                    UserManager.AddToRoles(admin.Id, new string[] { "Admin" });

                }
                catch (Exception ex)
                {
                    Log.Error($"Failed Create Admin Account - LdapAccount: {model.LdapAccount} Message: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("warning_LDAP_account_exists", "AdminManagement", GetLanguageCode().ToString()), false, null));
            }

            var user = await _userService.GetUserByID(admin.Id, new string[] { "SystemPrivileges" });
            user.AdminEmail = model.Email;

            int[] delIds = user.SystemPrivileges.Where(x => !model.CourseIds.Contains(x.CourseId)).Select(x => x.CourseId).ToArray();
            int[] addIds = model.CourseIds.Where(x => !user.SystemPrivileges.Select(y => y.CourseId).Contains(x)).ToArray();

            bool addResult = _systemPrivilegeService.AddMulti(admin.Id, addIds);
            if (!addResult)
            {
                Log.Error($"Failed Create Admin Account - LdapAccount: {model.LdapAccount}");
                return Content(HttpStatusCode.InternalServerError, new ActionResultModel("Something went wrong. Please contact system admin.", false, "Error when add system privilege"));
            }

            bool deleteResult = _systemPrivilegeService.DeleteMulti(admin.Id, delIds);

            if (!deleteResult)
            {
                Log.Error($"Failed Create Admin Account - LdapAccount: {model.LdapAccount}");
                return Content(HttpStatusCode.InternalServerError, new ActionResultModel("Something went wrong. Please contact system admin.", false, "Error when delete system privilege"));
            }

            Log.Info($"Completed Create Admin Account - LdapAccount: {model.LdapAccount}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, ""));
        }

        [HttpPost]
        [Route("Admins/Criteria")]
        public async Task<IHttpActionResult> GetCriteria(AdminCriteriaBindingModel model)// string coursecode = "", string coursename = "")
        {
            string ldapaccount = model.LDAPAccount;
            int courseId = model.CourseId;
            Log.Info($"Start Get Criteria {ldapaccount}");

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Admin Accounts - UserId: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                Log.Error($"Failed Get Admin Accounts - UserId: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("user_does_not_exist")));
            }

            var permissionsLst = await _systemPrivilegeService.GetUserPermissons(ldapaccount, courseId, id);

            List<CriteriaViewModel> rtnList = new List<CriteriaViewModel>();
            var identity = (ClaimsIdentity)User.Identity;
            var loginAcc = identity.Claims.FirstOrDefault(x => x.Type == "ldapUsername").Value;
            foreach (var permissions in permissionsLst)
            {
                if (permissions.UserName.Equals("ebsluser") || permissions.UserName.Equals("admin"))
                {
                    continue;
                }

                CriteriaViewModel rtnItem = permissions.ToCriteriaViewModel(GetLanguageCode(), courseId);

                rtnList.Add(rtnItem);
            }
            Log.Info($"Completed Get Admin Accounts - UserId: {HttpContext.Current.User.Identity.GetUserId()}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, rtnList));
        }


        [HttpGet]
        [Route("Admins/CriteriaById/{id}")]
        public async Task<IHttpActionResult> GetCriteriaById(int id = 0)
        {
            Log.Info($"Start Get Criteria {id}");
            if (id == 0)
            {
                Log.Error($"Failed Get Criteria {id}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel("LdapAccount is required.", false, null));
            }

            var permissions = await _systemPrivilegeService.GetUserPermissonsById(id);
            CriteriaViewModel rtnItem = permissions.ToCriteriaViewModel(GetLanguageCode(), 0); //"", "");
            Log.Info($"Completed Get Criteria {id}");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, rtnItem));
        }


        // Get Course
        [HttpPost]
        [Route("Admins/CoursePrivileges")]
        public async Task<IHttpActionResult> GetCoursePrivilegesPagging(PrivilegesBindingModel search)
        {
            Log.Info($"Start Get Course Privileges");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Course Privileges");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            var result = await _systemPrivilegeService.GetCoursePrivileges(search, (int)GetLanguageCode(), id);

            Log.Info($"Completed Get Course Privileges");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        // Get Account
        [HttpPost]
        [Route("Admins/AccountSystemPrivileges")]
        public async Task<IHttpActionResult> GetAccountSystemPrivilegesPagging(PrivilegesBindingModel search)
        {
            Log.Info($"Start Get Account Privileges");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Account Privileges");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            var result = await _systemPrivilegeService.GetAccountSystemPrivileges(search, id);

            Log.Info($"Completed Get Account Privileges");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        // Getcontent
        [HttpPost]
        [Route("Admins/ContentPrivileges")]
        public async Task<IHttpActionResult> GetContentPrivilegesPagging(PrivilegesBindingModel search)
        {
            Log.Info($"Start Get Content Privileges");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Content Privileges");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            var result = await _systemPrivilegeService.GetContentPrivileges(search, id);

            Log.Info($"Completed Get Content Privileges");
            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
        }

        // Update course
        [HttpPost]
        [Route("Admins/UpdateCoursePrivileges")]
        public async Task<IHttpActionResult> UpdateCoursePrivileges(CoursePrivilegesBindingModel model)
        {
            Log.Info($"Start Update Course Privileges - User Id: {model.UserId}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Course Privileges - User Id: {model.UserId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update Course Privileges - User Id: {model.UserId}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            try
            {
                var result = await _systemPrivilegeService.UpdateCoursePrivileges(model, id);
                if (result.IsSuccess)
                {
                    Log.Info($"Completed Update Course Privileges - User Id: {model.UserId}");
                }
                else
                {
                    Log.Error($"Failed Update Course Privileges - User Id: {model.UserId}");
                }
                return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true))
                    : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Update Course Privileges - User Id: {model.UserId} - Message: {ex.Message}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message));
            }
        }

        // Update Account
        [HttpPost]
        [Route("Admins/UpdateAccountSystemPrivileges")]
        public async Task<IHttpActionResult> UpdateAccountSystemPrivileges(AccountSystemPrivilegesBindingModel model)
        {
            Log.Info($"Start Update Account Privileges - User Id: {model.UserId}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Account Privileges - User Id: {model.UserId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update Account Privileges - User Id: {model.UserId}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            try
            {
                var result = await _systemPrivilegeService.UpdateAccountSystemPrivileges(model, id);
                if (result.IsSuccess)
                {
                    Log.Info($"Completed Update Account Privileges - User Id: {model.UserId}");
                }
                else
                {
                    Log.Error($"Failed Update Account Privileges - User Id: {model.UserId}");
                }
                return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true))
                    : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Update Account Privileges - User Id: {model.UserId} - Message: {ex.Message}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message));
            }
        }

        // Update Content
        [HttpPost]
        [Route("Admins/UpdateContentPrivileges")]
        public async Task<IHttpActionResult> UpdateContentPrivileges(ContentPrivilegesBindingModel model)
        {
            Log.Info($"Start Update Content Privileges - User Id: {model.UserId}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Update Content Privileges - User Id: {model.UserId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Update Content Privileges - User Id: {model.UserId}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            try
            {
                var result = await _systemPrivilegeService.UpdateContentPrivileges(model, id);
                if (result.IsSuccess)
                {
                    Log.Info($"Completed Update Content Privileges - User Id: {model.UserId}");
                }
                else
                {
                    Log.Error($"Failed Update Content Privileges - User Id: {model.UserId}");
                }

                return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true))
                    : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Update Content Privileges - User Id: {model.UserId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message));
            }
        }

        // Suspend Active
        [HttpPost]
        [Route("Admins/SuspendActiveUser")]
        public async Task<IHttpActionResult> SuspendActiveUser(SuspendActiveBindingModel model)
        {
            Log.Info($"Start Suspend/Acive Admin - User Id: {model.UserId}");
            if (!ModelState.IsValid)
            {
                Log.Error($"Failed Suspend/Acive Admin - User Id: {model.UserId}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Suspend/Acive Admin - User Id: {model.UserId}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            try
            {
                var result = await _systemPrivilegeService.SuspendActiveUser(model, id);
                if (result.IsSuccess)
                {
                    Log.Info($"Completed Suspend/Acive Admin - User Id: {model.UserId}");
                }
                else
                {
                    Log.Error($"Failed Suspend/Acive Admin - User Id: {model.UserId}");
                }
                return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result))
                    : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Suspend/Acive Admin - User Id: {model.UserId} - Message: {ex.Message}");
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message));
            }
        }

        // Edit Admin
        [HttpPost]
        [Route("Admins/EditAdmin")]
        public async Task<IHttpActionResult> EditAdminAccount(AdminAccountBindingModel model)
        {
            Log.Info($"Start Edit Admin - User Id: {model.Id}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Edit Admin - User Id: {model.Id}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            try
            {
                var result = await _systemPrivilegeService.EditAdminAccount(model, id);

                Log.Info($"Completed Edit Admin - User Id: {model.Id}");
                return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, result.IsSuccess));
            }
            catch (Exception ex)
            {
                Log.Error($"Failed Edit Admin - User Id: {model.Id} - Message: {ex.Message}");
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Get Permission
        [HttpGet]
        [Route("Admins/GetPermission")]
        public async Task<IHttpActionResult> GetPermissionAccount()
        {
            Log.Info($"Start Get Permission - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                Log.Error($"Failed Get Permission - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            try
            {
                var result = await _systemPrivilegeService.GetPermissionAccount(id);
                Log.Info($"Completed Get Permission - User Id: {HttpContext.Current.User.Identity.GetUserId()}");
                return Content(HttpStatusCode.OK, new ActionResultModel(result.Message, true, result.Data));
            }
            catch (Exception ex)
            {
                Log.Info($"Start Get Permission - User Id: {HttpContext.Current.User.Identity.GetUserId()} - Message: {ex.Message}");
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("Admins/ManageStudent")]
        public async Task<IHttpActionResult> GetStudentAccountPaging(StudentSearching search)
        {
            try
            {
                var result = await _userService.GetPagingStudentAccount(search);
                return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success", "Common", GetLanguageCode().ToString()), true, result));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(ex.Message));
            }
        }


        [HttpPost]
        [Route("Admins/UpdateStudentAccount")]
        public async Task<IHttpActionResult> EditStudentAccount(StudentAccountBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, new ActionResultModel(BindingModelHelper.GetModelStateErrorMessage(ModelState, "EntityValidationMessage", GetLanguageCode().ToString())));
            }

            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return Content(HttpStatusCode.NotFound, new ActionResultModel() { Message = FileHelper.GetServerMessage("id_is_not_a_number"), Data = null });
            }

            var result = await _userService.UpdateStudentAccount(model, id);
            return result.IsSuccess ? Content(HttpStatusCode.OK, new ActionResultModel(result.Message, true)) : Content(HttpStatusCode.BadRequest, new ActionResultModel(result.Message));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("DownloadQRcode/{id}")]
        public async Task<HttpResponseMessage> DownloadZipFile(int id)
        {

            var newLink = (await _clientService.GetClientUrlByNameAsync("ApplicantPortal")) + "course_search_detail?courseId=" + id;
            //if (link.IndexOf(ConfigHelper.GetByKey("ApplicantPortal")) < 0)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest);
            //}
            var result = await _systemPrivilegeService.DownloadQRCode(newLink);

            if (!result.IsSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("failed_to_download")));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);

            return httpResponseMessage;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("DownloadImageQRcode")]
        public async Task<HttpResponseMessage> DownloadImageQRcode(string link)
        {
            var newLink = HttpUtility.HtmlDecode(link);

            var result = await _systemPrivilegeService.DownloadImageQRcode(newLink);

            if (!result.IsSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ActionResultModel(FileHelper.GetServerMessage("failed_to_download")));
            }

            var httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(result.Data.Stream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = result.Data.FileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(result.Data.FileType);

            return httpResponseMessage;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("EcryptPublishKey")]
        public IHttpActionResult GetEncyptPublishKey()
        {
            return Content(HttpStatusCode.OK, new ActionResultModel("Susscess", true, StaticConfig.PublichKey));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private bool IsAdminSystem(string email)
        {
            List<string> listEmailAdmin = new List<string>() { "superadmin@gmail.com", "admin@gmail.com", "ebsluser@gmail.com" };
            return listEmailAdmin.Contains(email);
        }
        #endregion
    }
}
