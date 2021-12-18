using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("DownloadDocumentTrackings")]
    public class DownloadDocumentTracking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseDocumentId { get; set; }

        public int ApplicationId { get; set; }

        public DateTime DownloadDate { get; set; }

        public virtual Application Application { get; set; }

        public virtual CourseDocument CourseDocument { get; set; }
    }
}
