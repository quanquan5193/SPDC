using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.AdminPrivileges
{
    public class ContentPrivilegesBindingModel
    {
        [Required]
        public int UserId { get; set; }

        public bool IsCreateContent { get; set; }

        public bool IsViewContent { get; set; }

        public bool IsEditContent { get; set; }

        public bool IsDeleteContent { get; set; }

        public bool IsApproveContent { get; set; }

        public bool IsUnapproveContent { get; set; }

        public bool IsPublishContent { get; set; }

        public bool IsUnpublishContent { get; set; }

        public bool IsBatchApplication { get; set; }

        public bool IsBatchPayment { get; set; }

        public bool IsAttendance { get; set; }

        public bool IsAssessment { get; set; }

    }
}
