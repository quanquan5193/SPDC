using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static QRCoder.QRCodeGenerator;
using static SPDC.Common.StaticConfig;

namespace SPDC.Common
{
    public class FileHelper
    {
        public static string GenerateQRCode(string link, string fileDirect)
        {
            using (var generator = new QRCodeGenerator())
            {
                using (var data = generator.CreateQrCode(link, ECCLevel.M))
                {
                    using (var code = new QRCode(data))
                    {
                        using (var bitmap = code.GetGraphic(StaticConfig.PixelPerModule, Color.Black, Color.White, true))
                        {
                            //fileDirect = Common.GenFileNameDuplicate(folderDirect + @"\link.png");
                            bitmap.Save(fileDirect, ImageFormat.Png);
                            return fileDirect;
                        }
                    }
                }
            }
        }

        public static string GetEmailSubject(string field, string formName, string langCode)
        {
            string path;

            if (HttpContext.Current != null)
            {
                path = System.Web.HttpContext.Current.Server.MapPath($"~/Templates/EmailSubjects/{formName + langCode}.json");

            }
            else
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "EmailSubjects", $"{formName}{langCode}.json");
            }


            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText(path));
                return o1.Property(field).Value.ToString();
            }
            catch (Exception ex)
            {
                return "File multi language is not found or message have not setted yet ";
            }
        }

        public static string GetNotificationTitle(string field, string formName, string langCode)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath($"~/Templates/Notifications/{formName + langCode}.json");
            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText(path));
                return o1.Property(field).Value.ToString();
            }
            catch
            {
                return "File multi language is not found or message have not setted yet ";
            }
        }

        public static string GetServerMessage(string field, string fileName, string langCode)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath($"~/MultiLanguage/{fileName + langCode}.json");
            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText(path));
                return o1.Property(field).Value.ToString();
            }
            catch
            {
                return "File multi language is not found or message have not setted yet ";
            }
        }

        public static string GetServerMessage(string field, string fileName = "ServerMessages")
        {
            var code = "EN";

            var a = HttpContext.Current.Request.Headers.GetValues("Accept-Language");

            if (a.Length > 0)
            {
                code = a.First();
            }

            string lang;

            switch (code)
            {
                case "EN":
                case "en":
                    lang = LanguageCode.EN.ToString();
                    break;
                case "CN":
                case "cn":
                    lang = LanguageCode.CN.ToString();
                    break;
                case "HK":
                case "hk":
                    lang = LanguageCode.HK.ToString();
                    break;
                default:
                    lang = LanguageCode.EN.ToString();
                    break;
            }

            return GetServerMessage(field, fileName, lang);
        }

    }
}
