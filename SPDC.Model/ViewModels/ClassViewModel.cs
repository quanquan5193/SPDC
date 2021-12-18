using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPDC.Model.Models;

namespace SPDC.Model.ViewModels
{
    //Show in list class
    public class ClassViewModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string ClassCode { get; set; }

        public string AcademicYear { get; set; }

        public int? AttendanceRequirement { get; set; }

        public int? ClassCommonId { get; set; }

        public int? EnrollmentNumber { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public bool? InvisibleOnWebsite { get; set; } = false;

        public int? Capacity { get; set; }

        public int SubClassStatus { get; set; }

        public int SubClassApprovedStatus { get; set; }
    }

    /// <summary>
    /// Show in detaill when click toggle 
    /// </summary>
    public class ClassViewDetailModel : ClassViewModel
    {
        public ClassViewDetailModel()
        {
            Lessons = new List<LessonViewModel>();
            Exams = new List<ExamViewModel>();
            FirstReExam = new List<ExamViewModel>();
            SecondReExams = new List<ExamViewModel>();
        }
        public bool IsExam { get; set; }

        public bool IsReExam { get; set; }

        public int ExamPassingMask { get; set; }

        public decimal ReExamFees { get; set; }

        public ICollection<LessonViewModel> Lessons { get; set; }

        public ICollection<ExamViewModel> Exams { get; set; }

        public ICollection<ExamViewModel> FirstReExam { get; set; }

        public ICollection<ExamViewModel> SecondReExams { get; set; }

        public int TargetClassId { get; set; }

        public byte? CountReExam { get; set; }
    }

    public class ClassDetailViewModel
    {
        public ClassDetailViewModel()
        {
            Lessons = new List<LessonViewModel>();
        }

        public string ClassCode { get; set; }

        public int CourseId { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public bool IsExam { get; set; }

        public bool IsReExam { get; set; }

        public int? ExamPassingMask { get; set; }

        public decimal? ReExamFees { get; set; }

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

        public byte? CountReExam { get; set; }

        public int SubClassStatus { get; set; }

        public int SubClassApprovedStatus { get; set; }

        //[JsonIgnore]
        public virtual Course Course { get; set; }

        public virtual ClassCommon ClassCommon { get; set; }

        //public virtual ClassCommon NewAttendanceRequirementType { get; set; }

        public virtual ICollection<ExamViewModel> Exams { get; set; }

        public virtual ICollection<ExamViewModel> FirstExams { get; set; }

        public virtual ICollection<ExamViewModel> SecondExams { get; set; }

        public virtual ICollection<LessonViewModel> Lessons { get; set; }
        //[JsonIgnore]
        //public virtual TargetClasses TargetClasses { get; set; }

        public virtual ICollection<SubClassApprovedStatusHistory> SubClassApprovedStatusHistories { get; set; }

        public virtual ICollection<SubClassDraft> SubClassDrafts { get; set; }
    }

    public class TargetClassViewModel
    {
        public TargetClassViewModel()
        {
            ClassCommonViewModel = new ClassCommonViewModel();
            ClassViewDetailModels = new List<ClassViewDetailModel>();
        }
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int ClassCommonId { get; set; }

        public int TargetNumberClass { get; set; }

        public bool IsOpenApplicationSetup { get; set; }

        public bool CanApplyForReExam { get; set; }

        public int ClassApprovedStatus { get; set; }

        public string AcademicYear { get; set; }

        public ClassCommonViewModel ClassCommonViewModel { get; set; }

        public ICollection<ClassViewDetailModel> ClassViewDetailModels { get; set; }

    }

    public class ClassCommonViewModel
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public int? TypeCommon { get; set; }

    }

    public class SelectItemClass
    {
        public int Id { get; set; }
        public string ClassCode { get; set; }
    }
}
