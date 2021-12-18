using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CriteriaViewModel
    {
        public int UserId { get; set; }
        public string LDAPAccount { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
        public AdminPermisstionViewModel AdminPermisstion { get; set; }
        public List<SystemPrivilegeViewModel> SystemPrivileges { get; set; }

    }

    public class AdminPermisstionViewModel
    {
        public int Id { get; set; }

        public bool IsCreateContent { get; set; }

        public bool IsViewContent { get; set; }

        public bool IsEditContent { get; set; }

        public bool IsDeleteContent { get; set; }

        public bool IsApproveContent { get; set; }

        public bool IsUnapproveContent { get; set; }

        public bool IsPublishContent { get; set; }

        public bool IsUnpublishContent { get; set; }

        public bool IsCreateAdmin { get; set; }

        public bool IsSuspendAdmin { get; set; }

        public bool IsActiveAdmin { get; set; }

        public bool IsEditAdmin { get; set; }

        public bool IsAssignAdmin { get; set; }

        public bool IsBatchApplication { get; set; }

        public bool IsBatchPayment { get; set; }

        public bool IsAttendance { get; set; }

        public bool IsAssessment { get; set; }
    }

    public class SystemPrivilegeViewModel
    {
        public bool IsCreateCourse { get; set; }

        public bool IsViewCourse { get; set; }

        public bool IsEditCourse { get; set; }

        public bool IsUserCalendar { get; set; }

        public bool IsSubmitAndCancelCourse { get; set; }

        public bool IsFirstApproveAndRejectCourse { get; set; }

        public bool IsSecondpproveAndRejectCourse { get; set; }

        public bool IsThirdApproveAndRejectCourse { get; set; }

        public bool IsSubmitAndCancelClass { get; set; }

        public bool IsFirstApproveAndRejectClass { get; set; }

        public bool IsSecondpproveAndRejectClass { get; set; }

        public bool IsThirdApproveAndRejectClass { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public int Id { get; set; }
    }
}

