using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CourseViewModel
    {
        public string CourseCode { get; set; }

        public int CategoryId { get; set; }

        public int CourseTypeId { get; set; }

        public decimal CourseFee { get; set; }

        public string FeeRemarks1 { get; set; }

        public string FeeRemarks2 { get; set; }

        public decimal? Allowance { get; set; }

        public bool IsAllowanceProvided { get; set; }

        public decimal? DiscountFee { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public int? ProgrammeLeaderId { get; set; }

        public int? LecturerId { get; set; }

        public int? MediumOfInstructionId { get; set; }

        public bool ByModule { get; set; }

        public bool IsShowEducationLevel { get; set; }

        public bool IsShowMemvershipsQualifications { get; set; }

        public bool IsShowWorkExperiences { get; set; }

        public bool IsShowEmployerRecommendation { get; set; }

        public int Status { get; set; }

        public int Id { get; set; }

        public bool? InvisibleOnWebsite { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public int? CourseVenueId { get; set; }

        public float DurationHrs { get; set; }

        public int DurationLesson { get; set; }

        public float DurationTotal { get; set; }

        public int TargetClassSize { get; set; }

        public float Credits { get; set; }

        public int? LevelOfApprovalId { get; set; }

        public bool CanApplyForReExam { get; set; }

        public decimal? ReExamFee { get; set; }

        public string ReExamRemarks { get; set; }

        public string ObjectiveEN { get; set; }
        public string ObjectiveTC { get; set; }
        public string ObjectiveSC { get; set; }

        public string WaitingTimeEN { get; set; }
        public string WaitingTimeTC { get; set; }
        public string WaitingTimeSC { get; set; }

        public int CourseApprovedStatus { get; set; }

        public int ClassApprovedStatus { get; set; }

        public bool IsSetApplicationSetup { get; set; }

        public virtual ICollection<ApplicationViewModel> Applications { get; set; }

        public virtual ICollection<ClassViewModel> Classes { get; set; }

        public virtual CourseCategoryViewModel CourseCategory { get; set; }

        //public virtual Lecturer Lecturer { get; set; }

        //public virtual ProgrammeLeader ProgrammeLeader { get; set; }

        //public virtual MediumOfInstruction MediumOfInstruction { get; set; }

        //public virtual LevelofApproval LevelofApproval { get; set; }

        public virtual ICollection<CourseDocumentViewModel> CourseDocuments { get; set; }

        //public virtual ICollection<CourseTran> CourseTrans { get; set; }

        //public virtual CourseType CourseType { get; set; }

        //public virtual ICollection<Enquiry> Enquiries { get; set; }

        //public virtual ICollection<Keyword> Keywords { get; set; }

        //public virtual ICollection<Module> Modules { get; set; }

        //public virtual ICollection<StudentRemark> StudentRemarkModels { get; set; }

        //public virtual ICollection<SystemPrivilege> SystemPrivileges { get; set; }

        //public virtual ICollection<ModuleCombination> ModuleCombinations { get; set; }

        //public virtual CourseLocation CourseVenue { get; set; }

        //public virtual ApplicationSetups ApplicationSetup { get; set; }

        //public virtual ICollection<CourseAppovedStatusHistory> CourseAppovedStatusHistories { get; set; }

        //public virtual ICollection<ClassAppovedStatusHistory> ClassAppovedStatusHistories { get; set; }
        ////public virtual TargetClasses TargetClass { get; set; }

        //public virtual ICollection<AdditionalClassesApproval> AdditionalClassesApprovals { get; set; }

        //public virtual TargetClasses TargetClasses { get; set; }
    }
}
