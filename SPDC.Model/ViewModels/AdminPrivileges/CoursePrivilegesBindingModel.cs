using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class CoursePrivilegesBindingModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

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
    }
}
