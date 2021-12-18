using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SPDC.Common;
using SPDC.Service;
using SPDC.Service.Services;

namespace SPDC.WebAPI.Helpers
{
    public class FireBackgroundTaskHelper
    {

        public static void CheckInvoiceOverdue()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(ConfigHelper.GetByKey("BASEAPI") + "api/ApplicationManagement/CheckInvoiceOverdue").GetAwaiter().GetResult();
            }
        }

        public static void AutoUpdateCMSStatus()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(ConfigHelper.GetByKey("BASEAPI") + "api/Content/AutoUpdateCMSStatus").GetAwaiter().GetResult();
            }
        }

        public static void AutoUpdateEnrollmentStatus()
        {
            EnrollmentUpdater enrollmentUpdater = new EnrollmentUpdater();
            enrollmentUpdater.UpdateEnrollment();
        }

        public static async Task AutoEmail()
        {
            var service = new AutoEmailService();
            await service.ClassCommencementReminder();
            await service.PaymentReminder();
            await service.NewlyUploadCourseMaterialNotify();
            //await service.ReExamReminder();

            //await service.ReviewExamReminder();
        }
    }
}