using SPDC.Common;
using SPDC.Common.Enums;
using SPDC.Model.ViewModels.AutoEmail;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SPDC.Service.Services
{
    public class AutoEmailService : IAutoEmailService
    {
        private readonly string _connectionString = string.Empty;
        public AutoEmailService()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public async Task ClassCommencementReminder()
        {
            var commencementDay = SystemParameterProvider.Instance.GetValueInt(SystemParameterInfo.AutoSendClassCommencementReminder);
            var lst = new List<ClassCommencementDateReminderViewModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT c.CourseCode, cls.ClassCode, cls.CommencementDate, u.Email, u.DisplayName, u.CommunicationLanguage FROM dbo.Applications AS a INNER JOIN dbo.Users AS u ON a.UserId = u.Id INNER JOIN dbo.Classes AS cls ON	a.AdminAssignedClass = cls.Id INNER JOIN dbo.Courses AS c ON cls.CourseId = c.Id WHERE DATEDIFF(DAY,cls.CommencementDate, GETDATE()) = @Compare";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Compare", commencementDay);
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        lst.Add(new ClassCommencementDateReminderViewModel()
                        {
                            ClassCode = reader.GetString(reader.GetOrdinal("ClassCode")),
                            CommencementDate = reader.GetDateTime(reader.GetOrdinal("CommencementDate")),
                            CommunicationLanguage = reader.GetInt32(reader.GetOrdinal("CommunicationLanguage")),
                            CourseCode = reader.GetString(reader.GetOrdinal("CourseCode")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }

            }

            foreach (var i in lst)
            {
                var emailTemplate = i.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item18EN.cshtml" : "Item18TC.cshtml";
                string emailSubject = FileHelper.GetEmailSubject("item18", "EmailSubject", i.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");



                MailHelper.SendMail(i.Email
                    , string.Format(emailSubject, i.ClassCode)
                    , Common.Common.GenerateItem18Email(i.CourseCode
                                                        , i.DisplayName
                                                        , i.ClassCode
                                                        , i.CommencementDate.ToString("dd/MM/yyyy")
                                                        , emailTemplate));
            }
        }

        public async Task NewlyUploadCourseMaterialNotify()
        {
            var notifyMaterialDay = SystemParameterProvider.Instance.GetValueInt(SystemParameterInfo.NewlyUploadCourseMaterialNotify);
            var lst = new List<NewlyUploadCourseMaterialNotifyViewModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT cls.CommencementDate, cls.ClassCode, u.Email, u.DisplayName , (SELECT TOP(1) CourseName FROM dbo.CourseTrans WHERE LanguageId = u.CommunicationLanguage) AS CourseName FROM dbo.Classes AS cls INNER JOIN dbo.Courses AS c ON cls.CourseId = c.Id INNER JOIN dbo.Applications AS a ON cls.Id = a.AdminAssignedClass INNER JOIN dbo.Users AS u ON a.UserId = u.Id WHERE DATEDIFF(DAY,GETDATE(),cls.CommencementDate) = @Compare";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Compare", notifyMaterialDay);
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        lst.Add(new NewlyUploadCourseMaterialNotifyViewModel()
                        {
                            ClassCode = reader.GetString(reader.GetOrdinal("CommencementDate")),
                            CommunicationLanguage = reader.GetInt32(reader.GetOrdinal("CommunicationLanguage")),
                            CourseName = reader.GetString(reader.GetOrdinal("CourseCode")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }

            }

            foreach (var i in lst)
            {
                var emailTemplate = i.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item21EN.cshtml" : "Item21TC.cshtml";
                string emailSubject = FileHelper.GetEmailSubject("item21", "EmailSubject", i.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");



                MailHelper.SendMail(i.Email
                    , string.Format(emailSubject, i.ClassCode)
                    , Common.Common.GenerateItem21Email(i.CourseName
                                                        , i.DisplayName
                                                        , i.ClassCode                                                        
                                                        , emailTemplate));
            }
        }

        public async Task PaymentReminder()
        {
            var paymentDueDay = SystemParameterProvider.Instance.GetValueInt(SystemParameterInfo.AutoSendMailPaymentReminder);

            var lst = new List<PaymentDueDateReminderViewModel>();

            using (var conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT u.Email, u.DisplayName, i.InvoiceNumber, u.CommunicationLanguage, (SELECT TOP (1) CourseName FROM dbo.CourseTrans WHERE LanguageId = u.CommunicationLanguage) AS CourseName FROM dbo.Invoices AS i INNER JOIN dbo.Applications AS a ON i.ApplicationId = a.Id INNER JOIN dbo.Users AS u ON a.UserId = u.Id INNER JOIN dbo.Courses AS c ON a.CourseId = c.Id WHERE i.Status = 2 AND DATEDIFF(DAY,GETDATE(), i.PaymentDueDate) = @Compare";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Compare", paymentDueDay);
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        lst.Add(new PaymentDueDateReminderViewModel()
                        {                            
                            CommunicationLanguage = reader.GetInt32(reader.GetOrdinal("CommencementDate")),
                            CourseName = reader.GetString(reader.GetOrdinal("CourseName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            InvoiceNumber = reader.GetString(reader.GetOrdinal("InvoiceNumber")),                            
                        }); 
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }

            }

            foreach (var item in lst)
            {
                var emailTemplate = item.CommunicationLanguage == (int)CommunicationLanguageType.English ? "Item14EN.cshtml" : "Item14TC.cshtml";
                string emailSubject = FileHelper.GetEmailSubject("item14", "EmailSubject", item.CommunicationLanguage == (int)CommunicationLanguageType.English ? "EN" : "TC");



                MailHelper.SendMail(item.Email
                    , string.Format(emailSubject, item.InvoiceNumber)
                    , Common.Common.GenerateItem14Email(item.CourseName
                                                        , item.DisplayName
                                                        , item.InvoiceNumber
                                                        , emailTemplate));
            }
        }

        //public Task ReExamReminder()
        //{
        //    throw new NotImplementedException();
        //}

        public Task ReviewExamReminder()
        {
            throw new NotImplementedException();
        }
    }
}
