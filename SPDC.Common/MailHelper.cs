using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPDC.Common
{

    public class MailHelper
    {
        public static bool SendMail(string toEmail, string subject, string content, List<LinkedResource> linkResource = null, string path = null, bool isDeleteFile = false)
        {
            try
            {
                var host = SystemParameterProvider.Instance.GetValueString(SystemParameterInfo.SMTPHost);
                var port = SystemParameterProvider.Instance.GetValueInt(SystemParameterInfo.SMTPPort);
                var fromEmail = SystemParameterProvider.Instance.GetValueString(SystemParameterInfo.SMTPUsername);
                var password = SystemParameterProvider.Instance.GetValueString(SystemParameterInfo.SMTPPassword);
                var fromName = SystemParameterProvider.Instance.GetValueString(SystemParameterInfo.SMTPFromName);
                var smtpClient = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromEmail, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                };

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(content, Encoding.UTF8, MediaTypeNames.Text.Html);

                if (linkResource != null)
                {
                    foreach (var img in linkResource)
                    {
                        htmlView.LinkedResources.Add(img);
                    }
                }

                var mail = new MailMessage
                {
                    Subject = subject,
                    From = new MailAddress(fromEmail, fromName)
                };

                mail.To.Add(new MailAddress(toEmail));
                mail.AlternateViews.Add(htmlView);
                mail.IsBodyHtml = true;

                if(path != null)
                {
                    var attachment = GenerateAttachment(path);
                    mail.Attachments.Add(attachment);
                    if (isDeleteFile && File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

                smtpClient.Send(mail);

                return true;
            }
            catch (SmtpException smex)
            {

                return false;
            }
        }

        private static Attachment GenerateAttachment(string pathFile)
        {
            Attachment attachment = new Attachment(pathFile);
            ContentDisposition disposition = attachment.ContentDisposition;
            disposition.CreationDate = File.GetCreationTime(pathFile);
            disposition.ModificationDate = File.GetLastWriteTime(pathFile);
            disposition.ReadDate = File.GetLastAccessTime(pathFile);
            disposition.FileName = Path.GetFileName(pathFile);
            disposition.Size = new FileInfo(pathFile).Length;
            disposition.DispositionType = DispositionTypeNames.Attachment;

            //Attachment attachment = new Attachment(stream, "Collect Attendance Certification", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            //ContentDisposition disposition = attachment.ContentDisposition;
            //disposition.CreationDate = DateTime.Now;
            //disposition.ModificationDate = DateTime.Now;
            //disposition.ReadDate = DateTime.Now;
            //disposition.FileName = "Collect Attendance Certification";
            //disposition.Size = stream.Length;
            //disposition.DispositionType = DispositionTypeNames.Attachment;

            return attachment;
        }

        public static LinkedResource GenerateEmailImage(MemoryStream ms, string contentType, string contentId)
        {
            var resourse =  new LinkedResource(ms, contentType);
            resourse.ContentId = contentId;
            resourse.TransferEncoding = TransferEncoding.Base64;

            return resourse;
        }
    }

}
