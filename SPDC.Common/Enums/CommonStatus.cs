using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Common.Enums
{
    public enum CommonStatus
    {
        Deactive,
        Active,
        Deleted
    }

    public enum EmailType
    {
        ActiveEmail,
        ForgotEmail,
        ForgotLoginEmail
    }

    public enum DistinguishDocType
    {
        CourseBrochure = 0,
        ApplicationForm = 1,
        CourseImage = 2,
        CourseSetUp = 3,
        ClassSetUp = 4,
        CancelledPostponed = 5,
        WaivedPayment = 6
    }

    public enum ExamType
    {
        Exam = 1,
        FirstReExam = 2,
        SecondReExam = 3
    }

    public enum ExamResult
    {
        Pass = 1,
        Failed = 2,
        Absent = 3
    }

    public enum Honorific
    {
        [StringValue("先生")]
        Mr = 1,
        [StringValue("太太")]
        Mrs = 2,
        [StringValue("女士")]
        Ms = 3,
        [StringValue("博士")]
        Dr = 4,
        [StringValue("工程師")]
        Ir = 5
    }

    public enum CmsContentType
    {
        [StringValue("Banner & Background Image")]
        BannerAndBackground = 1,
        [StringValue("Announcement")]
        Announcement = 2,
        [StringValue("News and Events")]
        NewsAndEvents = 3,
        [StringValue("Promotional Items")]
        PromotionalItems = 4,
        [StringValue("Other Inner Pages")]
        OtherInnerPages = 5,
        [StringValue("Inclement Weather Arrangement")]
        InclementWeatherArrangement = 6,
        [StringValue("Welcome Message")]
        WelcomeMessage = 7
    }

    public enum ApplicationStatus
    {
        Created = 1,
        SupplementaryInformation = 2,
        Submitted = 3,
        ReturnFirstApproved = 4,
        FirstApproved = 5,
        SecondApproved = 6,
        Rejected = 7,
        Assigned = 8,
        Replaced = 9,
        Withdrawal = 10
    }

    //public enum InvoiceType
    //{
    //    CourseFee = 1,
    //    ReExamFee = 2,
    //    Others = 3
    //}

    public enum InvoiceItemType
    {
        CourseFee = 1,
        ReExamFee = 2,
        Discount = 3,
        Others = 4
    }

    public enum InvoiceStatus
    {
        Created = 1,
        Offered = 2,
        PaidPartially = 3,
        Waived = 4,
        Settled = 5,
        Revised = 6,
        Overpaid = 7,
        Cancelled = 8,
        RefundPendingForApproval = 9,
        PendingForRefund = 10,
        Refunded = 11,
        Overdue = 12,
        SettledByBatch = 13,
    }

    public enum EnrollmentStatus
    {
        Enrolled = 1,
        Completed = 2,
        Graduated = 3,
        Resit = 4,
        Withdrawal = 5,
        Failed = 6,
        Appeal = 7,
        ConditionallyApproved = 8
    }

    public enum ContentApplyingFor
    {
        Both = 0,
        Website = 1,
        Mobile = 2
    }

    public enum FileType
    {
        File = 1,
        Image = 2
    }

    public enum UserNotificationType
    {
        NonRegisteredUsers = 1,
        UserHasNoCourse = 2,
        UserHasCourse = 3
    }

    public enum CourseApprovedStatus
    {
        Created = 0,
        Submitted = 1,
        FirstApproved = 2,
        SecondApproved = 3,
        ThirdApproved = 4,
        Cancel = 5,
        FirstReject = 6,
        SecondReject = 7,
        ThirdReject = 8
    }

    public enum ClassApprovedStatus
    {
        Created = 0,
        Submitted = 1,
        FirstApproved = 2,
        SecondApproved = 3,
        ThirdApproved = 4,
        Cancel = 5,
        FirstReject = 6,
        SecondReject = 7,
        ThirdReject = 8
    }

    public enum SubClassStatus
    {
        Created = 0,
        Rejected = 1,
        Submitted = 2,
        Actived = 3,
        Openned = 4,
        Closed = 5,
        Cancelled = 6,
        Postponed = 7
    }

    public enum SubClassApprovedStatus
    {
        Created = 0,
        Sumitted = 1,
        FirstApproved = 2,
        SecondApproved = 3,
        ThirdApproved = 4,
        Cancel = 5,
        FirstReject = 6,
        SecondReject = 7,
        ThirdReject = 8
    }

    public enum AdditionalClassApprovedStatus
    {
        Created = 0,
        Sumitted = 1,
        FirstApproved = 2,
        SecondApproved = 3,
        ThirdApproved = 4,
        Cancel = 5,
        FirstReject = 6,
        SecondReject = 7,
        ThirdReject = 8
    }

    public enum WaivedPaymentStatus
    {
        Cancelled = 0,
        FirstReject = 1,
        SecondReject = 2,
        ThirdReject = 3,
        Submitted = 4,
        FirstApproved = 5,
        SecondApproved = 6,
        ThirdApproved = 7
    }

    public enum PaymentTransactionType
    {
        Waive = 1,
        DebitNote = 2,
        Cheque = 3,
        OtherPaymentMethod = 4,
        CreditCard = 5,
        ConvinienceStore = 6,
        Refund = 7
    }

    public enum AcceptedBank
    {
        None = 1
    }

    public enum NotificationType
    {
        CMS = 0,
        Course = 1,
        Application = 2
    }

    public enum RefundStatus
    {
        Created = 1,
        Submitted = 2,
        FirstApproved = 3,
        SecondApproved = 4,
        ThirdApproved = 5,
        Cancelled = 6,
        FirstReject = 7,
        SecondReject = 8,
        ThirdReject = 9
    }

    public enum LevelApprovalType
    {
        FirstLevel = 1,
        SecondLevel = 2,
        ThirdLevel = 3
    }

    public enum CMSPublishType
    {
        Create = 0,
        Approve = 1,
        Unapprove = 2,
        Publish = 3,
        Unpublish = 4
    }

    public enum CommunicationLanguageType
    {
        Chinese = 1,
        English = 2
    }

    public enum TypeFileTransaction
    {
        CreateBySelf = 0,
        CreateByBatchPayment = 1
    }

    public enum ApplicationTypeDocument
    {
        AssessmentDocument = 1,
        AttendanceDocument = 2
    }

    public enum AttendanceRequirementType
    {
        Hrs = 1,
        Lesson = 2,
        Percent = 3
    }

    public enum TypeAssessmentEmail
    {
        CollectAttendance = 1,
        CollectCourseCompletion = 2
    }
    public static class StaticEnum
    {
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static TEnum GetValueFromAttribute<TEnum, TAttribute>
           (string text, Func<TAttribute, string> valueFunc) where TAttribute : Attribute
        {
            var type = typeof(TEnum);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(TAttribute)) as TAttribute;
                if (attribute != null)
                {
                    if (valueFunc.Invoke(attribute) == text)
                        return (TEnum)field.GetValue(null);
                }
                else
                {
                    if (field.Name == text)
                        return (TEnum)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "text");
            // or return default(T);
        }
    }

    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion

    }
}
