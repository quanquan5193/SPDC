using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class PrivilegesViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string LDAPAccount { get; set; }

        public int CourseId { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public byte Status { get; set; }

        public bool IsCreateContent { get; set; }

        public bool IsViewContent { get; set; }

        public bool IsEditContent { get; set; }

        public bool IsDeleteContent { get; set; }

        public bool IsApproveContent { get; set; }

        public bool IsUnapproveContent { get; set; }

        public bool IsPublishContent { get; set; }

        public bool IsUnpublishContent { get; set; }

        public bool IsCreateCourse { get; set; }

        public bool IsViewCourse { get; set; }

        public bool IsEditCourse { get; set; }

        public bool IsCreateAdmin { get; set; }

        public bool IsSuspendAdmin { get; set; }

        public bool IsActiveAdmin { get; set; }

        public bool IsEditAdmin { get; set; }

        public bool IsAssignAdmin { get; set; }
    }
}