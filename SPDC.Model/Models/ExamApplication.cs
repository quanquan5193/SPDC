using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ExamApplications")]
    public class ExamApplication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int? FromExamId { get; set; }
        public int ExamId { get; set; }
        public int? AssessmentMark { get; set; }
        public int? AssessmentResult { get; set; }
        public bool IsMakeUp { get; set; }
        public Application Application { get; set; }
        public virtual Exam FromExam { get; set; }
        public virtual Exam Exam { get; set; }
    }
}
