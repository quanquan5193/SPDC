using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SPDC.Model.Models
{
    [Table("Classes")]
    public class Class
    {
        public Class()
        {
            StudentPreferredApplicationModels = new HashSet<Application>();
            AdminAssignedApplicationModels = new HashSet<Application>();
            SubClassApprovedStatusHistories = new HashSet<SubClassApprovedStatusHistory>();
            SubClassDrafts = new HashSet<SubClassDraft>();
            Exams = new HashSet<Exam>();
            Lessons = new HashSet<Lesson>();
        }

        [Required]
        [StringLength(50)]
        [Index("INDEX_CLASSCODE", IsUnique = true)]
        public string ClassCode { get; set; }

        public int CourseId { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public bool IsExam { get; set; }

        public bool IsReExam { get; set; }

        public int? ExamPassingMask { get; set; }

        public decimal? ReExamFees { get; set; }

        //[StringLength(256)]
        //public string AcademicYear { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool? InvisibleOnWebsite { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public int Capacity { get; set; }

        public int ClassNumber { get; set; }

        public int? AttendanceRequirement { get; set; }

        public int? ClassCommonId { get; set; }

        public int? EnrollmentNumber { get; set; }

        //public int? TargetClassId { get; set; }

        public byte? CountReExam { get; set; }

        public int SubClassStatus { get; set; }

        public int SubClassApprovedStatus { get; set; }

        //[StringLength(50)]
        //public string NewClassCode { get; set; }

        //public int? NewAttendanceRequirement { get; set; }

        //public int? NewAttendanceRequirementTypeId { get; set; }

        //public DateTime? NewClassCommencementDate { get; set; }

        //public DateTime? NewClassCompletionDate { get; set; }

        //public int? NewClassCapacity { get; set; }

        //public int NewClassStatus { get; set; }

        public virtual ICollection<Application> StudentPreferredApplicationModels { get; set; }

        public virtual ICollection<Application> AdminAssignedApplicationModels { get; set; }
        //[JsonIgnore]
        public virtual Course Course { get; set; }

        public virtual ClassCommon ClassCommon { get; set; }

        //public virtual ClassCommon NewAttendanceRequirementType { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
        //[JsonIgnore]
        //public virtual TargetClasses TargetClasses { get; set; }

        public virtual ICollection<SubClassApprovedStatusHistory> SubClassApprovedStatusHistories { get; set; }

        public virtual ICollection<SubClassDraft> SubClassDrafts { get; set; }

    }
}
