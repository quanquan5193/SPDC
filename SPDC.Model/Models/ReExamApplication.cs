using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ResitExamApplications")]
    public class ResitExamApplication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int ApplicationId { get; set; }

        public int ResitExamId { get; set; }

        public int Exam_Id { get; set; }
        
        public int? AssessmentMark { get; set; }

        public int? AssessmentResult { get; set; }

        public virtual Application Application { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual ResitExam ResitExam { get; set; }
    }
}
