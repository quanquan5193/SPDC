using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("AdminPermissions")]
    public partial class AdminPermission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public byte Status { get; set; }

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

        public virtual ApplicationUser User { get; set; }
    }
}

