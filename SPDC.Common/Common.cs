using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using SPDC.Common.Enums;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;

namespace SPDC.Common
{

    public static class StaticConfig
    {
        public enum LanguageCode
        {
            EN = 1,
            CN = 2,
            HK = 3
        }
        public const int PixelPerModule = 10;
        public const string IIS_IUSRS = "IIS_IUSRS";
        public const int MaximumFileLength = 20971520;
        public const int MaximumLogoImageLength = 5242880;
        public const string cFormatShortDateTime = "yyyyMMddHHmmssfff";
        public const int WaitingTimeToCleanTempFolder = 600000;
        public const string KeyEncryptMobile = "9ftcEcJBCLBdFypShhMvGbopPJ0wShevPqdI6fGhVoM=";
        public const string VectorEncryptMobile = "aST/AL0Xg54a+3HcwaSgvw==";
        public const string PublichKey = "6fa979f20126cb08aa645a8f495f6d85";
        //public static List<string> imageExtension = new List<string>() { ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".gif" };
        public static List<string> imageMimeTypeExtensions = new List<string>() { "image/jpeg", "image/png", "image/tiff", "image/gif", "image/bmp", "image/webp" };
        public static List<string> docMimeTypeExtensions = new List<string>() { "doc/pdf", "doc/doc", "doc/docx", "doc/xls", "doc/xlsx", "doc/ppt", "doc/pptx", "doc/csv", "doc/txt" };
        public static int NumberOfInvoicePerYear = 99999;
        public static string FailGenerateInvoiceNumber = "Over number of invoice per year";

        //public static string BatchPaymentTemplate = "~/images/BatchAppliction/SPDC_Batch_Application_Template_v1.xlsx";
        //public static string BatchPaymentFiles = "~/FIleUpload/BatchPayment/";
    }

    public class Common
    {
        public const string Success = "Success";
        public const string Error = "Error";

        public static string GetCurrentDomain(HttpContext context)
        {
            var port = context.Request.Url.Port;
            var domain = context.Request.Url.Host;
            var schema = context.Request.Url.Scheme;

            if (context.Request.Url.IsDefaultPort)
            {
                return $"{schema}://{domain}";
            }
            else
            {
                return $"{schema}://{domain}:{port}";
            }
        }

        public static MemoryStream GeneratePdf(string url)
        {
            PdfDocument doc = new PdfDocument();
            PdfPageSettings setting = new PdfPageSettings();
            setting.Size = new SizeF(1000, 1000);
            setting.Margins = new Spire.Pdf.Graphics.PdfMargins(30);

            PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
            //htmlLayoutFormat.IsWaiting = true;

            Thread thread = new Thread(() =>
            {
                doc.LoadFromHTML(url, false, false, false, setting, htmlLayoutFormat);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            //doc.SaveToStream(stream, Spire.Pdf.FileFormat.PDF);
            var tempFilePdf = url.Replace(".html", ".pdf");
            tempFilePdf = GenFileNameDuplicate(tempFilePdf);

            doc.SaveToFile(tempFilePdf);
            var stream = new MemoryStream(File.ReadAllBytes(tempFilePdf));
            File.Delete(tempFilePdf);
            doc.Close();
            return stream;
        }

        public static string GeneratePdfWithoutDeleteFile(string url)
        {
            PdfDocument doc = new PdfDocument();
            PdfPageSettings setting = new PdfPageSettings();
            setting.Size = new SizeF(1000, 1000);
            setting.Margins = new Spire.Pdf.Graphics.PdfMargins(30);

            PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
            //htmlLayoutFormat.IsWaiting = true;

            Thread thread = new Thread(() =>
            {
                doc.LoadFromHTML(url, false, false, false, setting, htmlLayoutFormat);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            //doc.SaveToStream(stream, Spire.Pdf.FileFormat.PDF);
            var tempFilePdf = url.Replace(".html", ".pdf");
            tempFilePdf = GenFileNameDuplicate(tempFilePdf);

            doc.SaveToFile(tempFilePdf);
            return tempFilePdf;
        }


        public static string GenerateActiveEmailContent(ApplicationUser user, string callbackUrl)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            string itemName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item1EN.cshtml" : "Item1TC.cshtml";
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);
            var displayName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? user.Particular.SurnameEN + " " + user.Particular.GivenNameEN : user.Particular.SurnameCN + " " + user.Particular.GivenNameCN;

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.ActivationLink", callbackUrl);
            #region old code
            //var displayName = user.CommunicationLanguage == 1 && (!string.IsNullOrEmpty(user.Particular?.SurnameCN) || !string.IsNullOrEmpty(user.Particular?.GivenNameCN)) ? user.Particular?.SurnameCN + " " + user.Particular?.GivenNameCN
            //    : user.Particular?.SurnameEN + " " + user.Particular?.GivenNameEN;
            ////string honorificString = user.CommunicationLanguage != 1 ? ((Honorific)user.Particular.Honorific).ToString() : (StaticEnum.GetStringValue((Honorific)user.Particular.Honorific)).ToString();
            //string honorificString = null;
            //var content = user.CommunicationLanguage != 1 ? $"<!DOCTYPE html><html><body><div style='width: 30.5%; margin-left: 10%;'><div>Dear {honorificString} {displayName}<br />Please click the following link to activate your SPDC portal account:<br/> <a href=\"{callbackUrl}\">Link</a><br />If clicking the link above does not work, copy and paste theURL in a new browser window instead.<br />For any enquiry on your account, please contact us at Tel. 2100 9000.<br />Thank you for using SPDC Portal.<br />School of Professional Development in Construction (SPDC)<br />Hong Kong Institute of Construction (HKIC)<br>******************************************************************************************</div><div> Important Notes:<br />1.This is a system generated email. Please do not reply to this email.<br />2.This email is confidential. It may also be legally privileged. If you are not the addressee you may not copy, foward, disclose or use any part of it. If you jave received this message in error, please delete it and all copies from your system and notify the sender immediately.<br />3. Internet communications cannot be guaranteed to be timely, secure, error or virus-free. The sender does not accept liability for any errors or omissions.</div></div></body></html>"
            //    : $"<!DOCTYPE html><html><body><div style='width: 30.5%; margin-left: 10%;'><div>{displayName} {honorificString}<br />請點擊以下連結啟動閣下的<<建造專業進修院校>>電子平台帳戶：<br/> <a href=\"{callbackUrl}\">Link</a><br / 若無法點擊以上連結，請將網址複制並貼上至新的瀏覽器視窗中。><br />如有對帳戶的問題有任何查詢，請致電2100 9000與本院校職員聯絡。><br />感謝使用<<建造專業進修院校>>電子平台。><br />建造專業進修院校><br />香港建造學院><br />******************************************************************************************</div><div> Important Notes:<br />重要提示:><br />1. 此乃系統自動產生的電郵，請勿回覆。.><br />2.此電郵提示所載的是保密資料，並可被視為享有法律特權的資料。倘若閣下並非指定的收件人，則不可複製、轉發、公開或使用此信息的任何部分。若此信息被誤送到閣下的郵箱，請刪去信息及存於閣下電腦系統內的所有相關副本，並立即通知寄件者。><br />3. 經互聯網傳送的電郵信息，不保證準時、完全安全、不含錯誤或電腦病毒。寄件者不會承擔所引致任何錯誤或遺漏的責任。><br /></div></div></body></html>";
            #endregion

            return content;
        }

        public static string GenerateForgetPasswordContent(ApplicationUser user, string callbackUrl)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            string itemName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item2EN.cshtml" : "Item2TC.cshtml";
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);
            var displayName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? user.Particular.SurnameEN + " " + user.Particular.GivenNameEN : user.Particular.SurnameCN + " " + user.Particular.GivenNameCN;

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.ActivationLink", callbackUrl);
            #region old code
            //var displayName = user.CommunicationLanguage == 1 && (!string.IsNullOrEmpty(user.Particular?.SurnameCN) || !string.IsNullOrEmpty(user.Particular?.GivenNameCN)) ? user.Particular?.SurnameCN + " " + user.Particular?.GivenNameCN
            //    : user.Particular?.SurnameEN + " " + user.Particular?.GivenNameEN;
            ////string honorificString = user.CommunicationLanguage != 1 ? ((Honorific)user.Particular.Honorific).ToString() : (StaticEnum.GetStringValue((Honorific)user.Particular.Honorific)).ToString();
            //string honorificString = null;
            //var content = user.CommunicationLanguage != 1 ? $"<!DOCTYPE html><html><body><div style='width: 30.5%; margin-left: 10%;'><div>Dear {honorificString} {displayName},<br />We have received your password reset request for SPDC portal.<br />Please click the following link to enter a new password:<br /><a href=\"{callbackUrl}\">Link</a><br />Thank you for using SPDC Portal. <br />School of Professional Development in Construction (SPDC)<br />Hong Kong Institute of Construction (HKIC)<br />*******************************************************************************************</div><div>Important Notes:<br />1. This is a system generated email. Please do not reply to this email.<br />2. This email is confidential. It may also be legally privileged. If you are not the addressee you may not copy, forward, disclose or use any part of it. If you have received this message in error, please delete it and all copies from your system and notify the sender immediately.<br />3. Internet communications cannot be guaranteed to be timely, secure, error or virus-free. The sender does not accept liability for any errors or omissions.</div></div></body></html>"
            //    : $"<!DOCTYPE html><html><body><div style='width: 30.5%; margin-left: 10%;'><div>{honorificString} {displayName}<br />我們已收到閣下要求重設<<建造專業進修院校>>電子平台密碼的要求。<br />請點擊以下連結輸入新密碼：<br /><a href=\"{callbackUrl}\">Link</a><br />感謝使用<<建造專業進修院校>>電子平台。<br />建造專業進修院校<br />香港建造學院<br />*******************************************************************************************</div><div>重要提示:<br />1. 此乃系統自動產生的電郵，請勿回覆。.<br />2. 此電郵提示所載的是保密資料，並可被視為享有法律特權的資料。倘若閣下並非指定的收件人，則不可複製、轉發、公開或使用此信息的任何部分。若此信息被誤送到閣下的郵箱，請刪去信息及存於閣下電腦系統內的所有相關副本，並立即通知寄件者。<br />3. 經互聯網傳送的電郵信息，不保證準時、完全安全、不含錯誤或電腦病毒。寄件者不會承擔所引致任何錯誤或遺漏的責任。</div></div></body></html>";
            #endregion
            return content;
        }

        public static string GenerateForgetLoginEmail(ApplicationUser user, string callbackUrl)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            string itemName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item3EN.cshtml" : "Item3TC.cshtml";
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);
            var displayName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? user.Particular.SurnameEN + " " + user.Particular.GivenNameEN : user.Particular.SurnameCN + " " + user.Particular.GivenNameCN;

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.ActivationLink", callbackUrl);
            content = content.Replace("@Model.LoginEmail", user.Email);

            #region old code
            //var displayName = user.CommunicationLanguage == 1 && (!string.IsNullOrEmpty(user.Particular?.SurnameCN) || !string.IsNullOrEmpty(user.Particular?.GivenNameCN)) ? user.Particular?.SurnameCN + " " + user.Particular?.GivenNameCN
            //    : user.Particular?.SurnameEN + " " + user.Particular?.GivenNameEN;

            ////string honorificString = user.CommunicationLanguage != 1 ? ((Honorific)user.Particular.Honorific).ToString() : (StaticEnum.GetStringValue((Honorific)user.Particular.Honorific)).ToString();
            //string honorificString = null;
            //var content = user.CommunicationLanguage != 1 ? $"<!DOCTYPE html><html><body><div style='width: 30.5%; margin-left: 10%;'><div>Dear {displayName} {honorificString},<br />We have received your login email address retrieval and reset password request for SPDC portal.<br />Your Login Email Address is: {user.Email}<br />Please click the following link to enter an email address for reactivate your account:<br /><a href=\"{callbackUrl}\">Link</a><br />Thank you for using SPDC Portal.<br />School of Professional Development in Construction (SPDC)<br />Hong Kong Institute of Construction (HKIC)<br />*******************************************************************************************</div><div>Important Notes:<br />1. This is a system generated email. Please do not reply to this email.<br />2. This email is confidential. It may also be legally privileged. If you are not the addressee you may not copy, forward, disclose or use any part of it. If you have received this message in error, please delete it and all copies from your system and notify the sender immediately.<br />3. Internet communications cannot be guaranteed to be timely, secure, error or virus-free. The sender does not accept liability for any errors or omissions.</div></div></body></html>"
            //    : $"<!DOCTYPE html><html><body><div style='width: 30.5%; margin-left: 10%;'><div>{displayName} {honorificString}<br />我們已收到閣下要求重啟<<建造專業進修院校>>電子平台登入電郵地址及密碼的要求。<br />你的登入電郵地址是： {user.Email}<br />請點擊以下連結輸入新電郵地址：<br /><a href=\"{callbackUrl}\">Link</a><br />感謝使用<<建造專業進修院校>>電子平台。<br />建造專業進修院校<br />香港建造學院<br />*******************************************************************************************</div><div>重要提示:<br />1. 此乃系統自動產生的電郵，請勿回覆。.<br />2. 此電郵提示所載的是保密資料，並可被視為享有法律特權的資料。倘若閣下並非指定的收件人，則不可複製、轉發、公開或使用此信息的任何部分。若此信息被誤送到閣下的郵箱，請刪去信息及存於閣下電腦系統內的所有相關副本，並立即通知寄件者。<br />3. 經互聯網傳送的電郵信息，不保證準時、完全安全、不含錯誤或電腦病毒。寄件者不會承擔所引致任何錯誤或遺漏的責任。</div></div></body></html>";
            #endregion

            return content;
        }

        public static string GenerateMatchNewEventEmailContent(SendMailModel user, string callbackUrl, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);
            string displayName = user.CommunicationLanguage == (int)CommunicationLanguageType.English ? user.FirstNameEN + " " + user.LastNameEN : user.LastNameCN + " " + user.FirstNameEN;
            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.ActivationLink", callbackUrl);

            //string content = $"<!DOCTYPE html><html><body><div style = 'width: 30.5%; margin-left: 10%;' ><div> Hello  {displayName},<br/>Please click <a href =\"{callbackUrl}\">here</a> to view SPDC latest news and events<br /><img src = 'cid:QRCode' width = '100%' /><br/>School of Professional Development in Construction(SPDC) <br/></div></div> </body> </html> ";
            return content;
        }

        public static string GenerateItem5Email(string courseCode, string callbackUrl, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseCode);
            content = content.Replace("@Model.ActivationLink", callbackUrl);
            return content;

        }

        public static string GenerateItem6Email(string courseName, string displayName, string[] supplementaryInformationRequired, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);

            string requires = String.Empty;
            foreach (var item in supplementaryInformationRequired)
            {
                requires = requires + item + "<br />";
            }
            content = content.Replace("@Model.SupplementaryInformationRequired", requires);
            return content;

        }

        public static string GenerateItem7Email(string courseName, string displayName, string[] reasons, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);

            string requires = String.Empty;
            foreach (var item in reasons)
            {
                requires = requires + item + "<br />";
            }
            content = content.Replace("@Model.Reason", requires);
            return content;

        }

        public static string GenerateItem8Email(string courseCode, string callbackUrl, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseCode);
            content = content.Replace("@Model.CallbackUrl", callbackUrl);
            return content;

        }

        public static string GenerateItem9Email(string courseCode, string callbackUrl, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseCode);
            content = content.Replace("@Model.CallbackUrl", callbackUrl);
            return content;

        }
        public static string GenerateItem10Email(string courseCode, string callbackUrl, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseCode);
            content = content.Replace("@Model.CallbackUrl", callbackUrl);
            return content;

        }

        public static string GenerateItem20Email(string itemName, string classCode, string displayName, string courseName, string date)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.ClassCode", classCode);
            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);
            content = content.Replace("@Model.Date", courseName);

            return content;
        }

        public static string GenerateItem19Email(string classCode, string displayName, string courseName, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.ClassCode", classCode);
            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);

            return content;
        }

        public static string GenerateItem22Email(string classCode, string displayName, string courseName, string itemName, HttpServerUtility server)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.ClassCode", classCode);
            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);

            return content;
        }

        public static string GenerateItem23Email(string classCode, string displayName, string courseName, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.ClassCode", classCode);
            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);

            return content;
        }

        public static string GenerateItem24Email(string displayName, string courseName, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);

            return content;
        }

        public static string GenerateItem26Email(string courseName, string path)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + "Item26EN.cshtml";
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseName);
            content = content.Replace("@Model.CallbackUrl", path);

            return content;
        }

        public static string GenerateItem27Email(string courseName, string path)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + "Item27EN.cshtml";
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseName);
            content = content.Replace("@Model.CallbackUrl", path);

            return content;
        }

        public static string GenerateItem28Email(string courseName, string path)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + "Item28EN.cshtml";
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseName);
            content = content.Replace("@Model.CallbackUrl", path);

            return content;
        }

        public static string GenerateItem29Email(string courseName, string path)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + "Item29EN.cshtml";
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseName);
            content = content.Replace("@Model.CallbackUrl", path);

            return content;
        }

        public static string GenerateItem30Email(string courseName, string path, string approver)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + "Item30EN.cshtml";
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseName);
            content = content.Replace("@Model.CallbackUrl", path);
            content = content.Replace("@Model.Approver", approver);

            return content;
        }

        public static string GenerateItem31Email(string courseName, string path, string approver)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + "Item31EN.cshtml";
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.CourseCode", courseName);
            content = content.Replace("@Model.CallbackUrl", path);
            content = content.Replace("@Model.Approver", approver);

            return content;
        }

        public static string GenerateItem35Email(string courseName, string displayName, string callbackUrl, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);
            content = content.Replace("@Model.ActivationLink", callbackUrl);
            return content;

        }

        public static string GenerateItem20Attachment(string destinationPath, dynamic bindingValues)
        {
            var templatePath = ConfigHelper.GetByKey("AttachtmentTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + "Item20.docx";

            if (!Directory.Exists(destinationPath))
            {
                CreateDirectoryAndGrantFullControlPermission(destinationPath);
            }

            destinationPath = destinationPath + "\\" + "Attachment_Ineligible for Exam_不合資格參加考試通知書_出席率不足(Chinese).docx";
            File.Copy(serPath, destinationPath, true);

            using (WordprocessingDocument myDocument1 = WordprocessingDocument.Open(destinationPath, true))
            {
                string docText = null;

                using (StreamReader sr = new StreamReader(myDocument1.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var item in bindingValues)
                {
                    //docText = RegexUtilities.Replace(docText, item.Key, item.Value);
                    docText = docText.Replace(item.Key, item.Value);
                }


                using (StreamWriter sw = new StreamWriter(myDocument1.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                    //sw.Flush();
                }
            }

            return destinationPath;

        }

        public static string GenerateInvoiceAttachment(string destinationPath, dynamic bindingValues, HttpServerUtility server)
        {
            var templatePath = ConfigHelper.GetByKey("InvoiceAttachment");
            var serPath = server.MapPath(templatePath) + "receipt.html";
            if (!File.Exists(serPath))
            {
                return null;
            }

            if (!Directory.Exists(destinationPath))
            {
                CreateDirectoryAndGrantFullControlPermission(destinationPath);
            }

            destinationPath = Common.GenFileNameDuplicate(destinationPath + "InvoiceAttachmentTemp" + DateTime.Now.Ticks) + ".html";
            File.Copy(serPath, destinationPath, true);

            string text = File.ReadAllText(destinationPath);
            foreach (var item in bindingValues)
            {
                //docText = RegexUtilities.Replace(docText, item.Key, item.Value);
                text = text.Replace(item.Key, item.Value);
            }
            File.WriteAllText(destinationPath, text);
            return Common.GeneratePdfWithoutDeleteFile(destinationPath);

        }

        public static string GenerateItem22Attachment(string destinationPath, dynamic bindingValues, HttpServerUtility server)
        {
            var templatePath = ConfigHelper.GetByKey("AttachtmentTemplate");
            var serPath = server.MapPath(templatePath) + "Item22.docx";

            if (!Directory.Exists(destinationPath))
            {
                CreateDirectoryAndGrantFullControlPermission(destinationPath);
            }

            destinationPath = destinationPath + "\\" + "Attachment_Collect Certificate_領證書通知書 with SPDC Logo (Chinese).docx";
            File.Copy(serPath, destinationPath, true);

            using (WordprocessingDocument myDocument1 = WordprocessingDocument.Open(destinationPath, true))
            {
                string docText = null;

                using (StreamReader sr = new StreamReader(myDocument1.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var item in bindingValues)
                {
                    //docText = RegexUtilities.Replace(docText, item.Key, item.Value);
                    docText = docText.Replace(item.Key, item.Value);
                }


                using (StreamWriter sw = new StreamWriter(myDocument1.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                    //sw.Flush();
                }
            }

            return destinationPath;

        }

        public static string GenerateItem23Attachment(string destinationPath, dynamic bindingValues, bool isPass, bool isExam)
        {
            string fileName = string.Empty;
            string reFileName = string.Empty;
            if (isPass && isExam)
            {
                fileName = "Item23_Pass.docx";
                reFileName = "Attachment_Exam Result_Pass_考試成績通知書_合格 (Chinese).docx";
            }
            else if (isPass && !isExam)
            {
                fileName = "Item23_Fail.docx";
                reFileName = "Attachment_Exam Result_Fail or Absent_考試成績通知書_不合格_缺席 (Chinese).docx";
            }
            else if (!isPass && isExam)
            {
                fileName = "Item23_ReExamPass.docx";
                reFileName = "Attachment_Re-Exam Result_Pass_重考成績通知書_合格 (Chinese).docx";
            }
            else if (!isPass && !isExam)
            {
                fileName = "Item23_ReExamFail.docx";
                reFileName = "Attachment_Re-Exam Result_Fail or Absent_重考成績通知書_不合格_缺席 (Chinese).docx";
            }

            var templatePath = ConfigHelper.GetByKey("AttachtmentTemplate");
            var serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + fileName;

            if (!Directory.Exists(destinationPath))
            {
                CreateDirectoryAndGrantFullControlPermission(destinationPath);
            }

            destinationPath = destinationPath + "\\" + reFileName;
            File.Copy(serPath, destinationPath, true);

            using (WordprocessingDocument myDocument1 = WordprocessingDocument.Open(destinationPath, true))
            {
                string docText = null;

                using (StreamReader sr = new StreamReader(myDocument1.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var item in bindingValues)
                {
                    //docText = RegexUtilities.Replace(docText, item.Key, item.Value);
                    docText = docText.Replace(item.Key, item.Value);
                }


                using (StreamWriter sw = new StreamWriter(myDocument1.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                    //sw.Flush();
                }
            }

            return destinationPath;

        }

        public static string GenerateItem24Attachment(string destinationPath, dynamic bindingValues, HttpServerUtility server)
        {
            var templatePath = ConfigHelper.GetByKey("AttachtmentTemplate");
            var serPath = server.MapPath(templatePath) + "Item24.docx";

            if (!Directory.Exists(destinationPath))
            {
                CreateDirectoryAndGrantFullControlPermission(destinationPath);
            }

            destinationPath = destinationPath + "\\" + "Attachment_Collect Certificate_領證書通知書 with SPDC Logo (Chinese).docx";
            File.Copy(serPath, destinationPath, true);

            using (WordprocessingDocument myDocument1 = WordprocessingDocument.Open(destinationPath, true))
            {
                string docText = null;

                using (StreamReader sr = new StreamReader(myDocument1.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var item in bindingValues)
                {
                    //docText = RegexUtilities.Replace(docText, item.Key, item.Value);
                    docText = docText.Replace(item.Key, item.Value);
                }


                using (StreamWriter sw = new StreamWriter(myDocument1.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                    //sw.Flush();
                }
            }

            return destinationPath;

        }

        public static string GenerateItem18Email(string courseName, string displayName, string classCode, string commencementDate, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = string.Empty;

            if (HttpContext.Current != null)
            {
                serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            }
            else
            {
                serPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Emails", itemName);
            }

            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);
            content = content.Replace("@Model.ClassCode", classCode);
            content = content.Replace("@Model.CommencementDate", commencementDate);

            return content;

        }

        public static string GenerateItem14Email(string courseName, string displayName, string invoiceNo, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = string.Empty;

            if (HttpContext.Current != null)
            {
                serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            }
            else
            {
                serPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Emails", itemName);
            }

            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.InvoiceNo", invoiceNo);
            content = content.Replace("@Model.CourseName", courseName);
            content = content.Replace("@Model.DisplayName", displayName);

            return content;

        }

        public static string GenerateItem21Email(string courseName, string displayName, string classCode, string itemName)
        {
            var templatePath = ConfigHelper.GetByKey("EmailTemplate");
            var serPath = string.Empty;

            if (HttpContext.Current != null)
            {
                serPath = System.Web.HttpContext.Current.Server.MapPath(templatePath) + itemName;
            }
            else
            {
                serPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Emails", itemName);
            }

            string content = File.ReadAllText(serPath);

            content = content.Replace("@Model.DisplayName", displayName);
            content = content.Replace("@Model.CourseName", courseName);
            content = content.Replace("@Model.ClassCode", classCode);

            return content;

        }

        public static void CleanupTempFolderTaskExcute(string fileToDelete)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = StaticConfig.WaitingTimeToCleanTempFolder;
            timer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                timer_Elapsed(sender, e, fileToDelete);
            };
            timer.Enabled = true;
            timer.Start();
        }

        public static string GenFileNameDuplicate(string path, int count = 1)
        {
            if (File.Exists(path))
            {

                string originalFileExtension = Path.GetExtension(path);
                if (count >= 2)
                {
                    var index = path.LastIndexOf('(');
                    var newPath = path.Substring(0, index);
                    newPath = newPath + $"({count})" + originalFileExtension;
                    return GenFileNameDuplicate(newPath, count + 1);
                }
                else
                {
                    var index = path.LastIndexOf('.');
                    var newPath = path.Substring(0, index);
                    newPath = newPath + $"({count})" + originalFileExtension;
                    return GenFileNameDuplicate(newPath, count + 1);
                }
            }
            else
            {
                return path;
            }
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e, string fileToDelete)
        {
            string folderPath = Path.GetDirectoryName(fileToDelete);
            DeleteFile(fileToDelete);
            DeleteFolder(folderPath);

            ((System.Timers.Timer)sender).Stop();
            ((System.Timers.Timer)sender).Dispose();
        }

        private static void DeleteFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            catch (IOException)
            {
                return;
            }
        }

        private static void DeleteFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void CreateDirectoryAndGrantFullControlPermission(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    DirectoryInfo dInfo = Directory.CreateDirectory(folderPath);
                    GrantFullControlPermissionForDirectory(StaticConfig.IIS_IUSRS, folderPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void GrantFullControlPermissionForDirectory(string identity, string folderPath)
        {
            //IIS_IUSRS
            try
            {
                bool modified;
                FileSystemRights fileSystemRights = FileSystemRights.FullControl;
                InheritanceFlags inheritanceFlags = InheritanceFlags.None;
                FileSystemAccessRule accessRule = new FileSystemAccessRule(identity, fileSystemRights, inheritanceFlags, PropagationFlags.NoPropagateInherit, AccessControlType.Allow);
                DirectoryInfo dInfo = new DirectoryInfo(folderPath);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                dSecurity.ModifyAccessRule(AccessControlModification.Set, accessRule, out modified);
                InheritanceFlags iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
                FileSystemAccessRule inheritancedAccessRule = new FileSystemAccessRule(identity, fileSystemRights, iFlags, PropagationFlags.InheritOnly, AccessControlType.Allow);
                dSecurity.ModifyAccessRule(AccessControlModification.Add, inheritancedAccessRule, out modified);
                dInfo.SetAccessControl(dSecurity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class Uri : System.Uri
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Uri"/> class.
        /// </summary>
        /// <param name="uriString">
        /// The uri string.
        /// </param>
        public Uri(string uriString)
            : base(uriString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uri"/> class.
        /// </summary>
        /// <param name="uriString">
        /// The uri string.
        /// </param>
        /// <param name="dontEscape">
        /// The dont escape.
        /// </param>
        /// <remarks>
        /// Uri(string, bool)
        ///     Uri constructor. Assumes that input string is canonically escaped
        /// </remarks>
        [Obsolete(
            "The constructor has been deprecated. Please use new Uri(string). The dontEscape parameter is deprecated and is always false. http://go.microsoft.com/fwlink/?linkid=14202"
            )]

        public Uri(string uriString, bool dontEscape) : base(uriString, dontEscape)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uri"/> class.
        /// </summary>
        /// <param name="baseUri">
        /// The base uri.
        /// </param>
        /// <param name="relativeUri">
        /// The relative uri.
        /// </param>
        /// <param name="dontEscape">
        /// The dont escape.
        /// </param>
        /// <remarks>
        /// Uri(Uri, string, bool)
        ///     Uri combinatorial constructor. Do not perform character escaping if
        ///     DontEscape is true
        /// </remarks>
        [Obsolete(
            "The constructor has been deprecated. Please new Uri(Uri, string). The dontEscape parameter is deprecated and is always false. http://go.microsoft.com/fwlink/?linkid=14202"
            )]

        public Uri(Uri baseUri, string relativeUri, bool dontEscape) : base(baseUri, relativeUri, dontEscape)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uri"/> class.
        /// </summary>
        /// <param name="uriString">
        /// The uri string.
        /// </param>
        /// <param name="uriKind">
        /// The uri kind.
        /// </param>
        /// <remarks>
        /// Uri(string, UriKind);
        /// </remarks>
        public Uri(string uriString, UriKind uriKind)
            : base(uriString, uriKind)
        {
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uri"/> class.
        /// </summary>
        /// <param name="baseUri">
        /// The base uri.
        /// </param>
        /// <param name="relativeUri">
        /// The relative uri.
        /// </param>
        /// <remarks>
        /// Uri(Uri, string)
        ///     Construct a new Uri from a base and relative URI. The relative URI may
        ///     also be an absolute URI, in which case the resultant URI is constructed
        ///     entirely from it
        /// </remarks>
        public Uri(Uri baseUri, string relativeUri)

            : base(baseUri, relativeUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uri"/> class.
        /// </summary>
        /// <param name="baseUri">
        /// The base uri.
        /// </param>
        /// <param name="relativeUri">
        /// The relative uri.
        /// </param>
        /// <remarks>
        /// Uri(Uri , Uri )
        ///     Note: a static Create() method should be used by users, not this .ctor
        /// </remarks>
        public Uri(Uri baseUri, Uri relativeUri)
            : base(baseUri, relativeUri)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uri"/> class.
        /// </summary>
        /// <param name="serializationInfo">
        /// The serialization info.
        /// </param>
        /// <param name="streamingContext">
        /// The streaming context.
        /// </param>
        /// <remarks>
        /// ISerializable constructor
        /// </remarks>
        protected Uri(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// The combine.
        /// </summary>
        /// <param name="parts">
        /// The parts.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Combine(params string[] parts)
        {
            if (parts == null || parts.Length == 0) return string.Empty;

            var urlBuilder = new StringBuilder();
            foreach (var part in parts)
            {
                var tempUrl = tryCreateRelativeOrAbsolute(part);
                urlBuilder.Append(tempUrl);
            }
            return VirtualPathUtility.RemoveTrailingSlash(urlBuilder.ToString());
        }

        private static string tryCreateRelativeOrAbsolute(string s)
        {
            System.Uri uri;
            TryCreate(s, UriKind.RelativeOrAbsolute, out uri);
            string tempUrl = VirtualPathUtility.AppendTrailingSlash(uri.ToString());
            return tempUrl;
        }
    }
}
