using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CourseDocuments")]
    public class CourseDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int DocumentId { get; set; }

        public int? LessonId { get; set; }

        public int? DistinguishDocType { get; set; }

        public virtual Course Course { get; set; }

        public virtual Document Document { get; set; }

        public virtual ICollection<DownloadDocumentTracking> DownloadDocumentTrackings { get; set; }
    }
}
