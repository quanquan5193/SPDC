using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Exams")]
    public class Exam
    {
        public Exam()
        {
            ExamApplications = new HashSet<ExamApplication>();
            ResitExamApplications = new HashSet<ResitExamApplication>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClassId { get; set; }

        public string ExamVenue { get; set; }

        public int Type { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(256)]
        public string FromTime { get; set; }

        [Required]
        [StringLength(256)]
        public string ToTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dateline { get; set; }

        [StringLength(256)]
        public string Marks { get; set; }

        public  int? ClassCommonId { get; set; }

        public string ExamVenueText { get; set; }

        public bool IsReExam { get; set; }

        public int? ModuleId { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public virtual Class Class { get; set; }

        public virtual ICollection<ExamApplication> ExamApplications { get; set; }

        public virtual ICollection<ExamApplication> FromExams { get; set; }

        public virtual ICollection<ResitExamApplication> ResitExamApplications { get; set; }

    }
}
