using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Common
{
    public class SystemParameterInfo
    {
        public static string SessionTimeout = "SessionTimeout";
        public static string LDAPUrl = "LDAPUrl";
        public static string LDAPAccountPrefix = "LDAPAccountPrefix";
        public static string SMTPHost = "SMTPHost";
        public static string SMTPPort = "SMTPPort";
        public static string SMTPUsername = "SMTPUsername";
        public static string SMTPPassword = "SMTPPassword";
        public static string SMTPFromName = "SMTPFromName";
        public static string EmailRegistrationActivationExpired = "EmailRegistrationActivationExpired";
        public static string AutoSendClassCommencementReminder = "AutoSendClassCommencementReminder";
        public static string NewlyUploadCourseMaterialNotify = "NewlyUploadCourseMaterialNotify";
        public static string AutoSendMailPaymentReminder = "AutoSendMailPaymentReminder";

        public string Key { get; set; }

        public int ValueInt { get; set; }

        public string ValueString { get; set; }

        public double ValueDouble { get; set; }
    }
}
