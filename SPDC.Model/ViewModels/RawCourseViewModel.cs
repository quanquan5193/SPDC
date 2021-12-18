using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class RawCourseViewModel
    {
        public int Id { get; set; }

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

        public DateTime? DateCreated { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? DateModified { get; set; }

        public int? UpdatedBy { get; set; }

        public int Status { get; set; }


        public bool? InvisibleOnWebsite { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public int CourseVenueId { get; set; }

        public float DurationHrs { get; set; }

        public float DurationTotal { get; set; }

        public int DurationLesson { get; set; }

        public int TargetClassSize { get; set; }

        public float Credits { get; set; }

        public int? LevelOfApprovalId { get; set; }

        public bool CanApplyForReExam { get; set; }

        public decimal? ReExamFee { get; set; }

        public string ReExamRemarks { get; set; }

        public IEnumerable<RawDocument> LogoImages { get; set; }

        public string ObjectiveEN { get; set; }
        public string ObjectiveTC { get; set; }
        public string ObjectiveSC { get; set; }

        public string WaitingTimeEN { get; set; }
        public string WaitingTimeTC { get; set; }
        public string WaitingTimeSC { get; set; }
        public int CourseApprovedStatus { get; set; }

        public List<RawCourseDocument> CourseDocuments { get; set; }

        public List<RawCourseTran> CourseTrans { get; set; }

        public List<RawEnquiry> Enquiries { get; set; }

        public List<RawKeyword> Keywords { get; set; }

        public List<RawModule> Modules { get; set; }

        public List<RawModuleCombination> ModuleCombinations { get; set; }

        public RawCourseCategory CourseCategory { get; set; }
    }

    public class RawModuleCombination
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string ModuleNos { get; set; }

        public int CombinationNo { get; set; }

        public decimal CourseFee { get; set; }

    }

    public class RawCourseCategory {
        public int Id { get; set; }

        public int Status { get; set; }

        public int? ParentId { get; set; }

        public  List<RawCourseCategorieTran> CourseCategorieTrans { get; set; }
    }

    public class RawCourseCategorieTran
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }
    }

    public class RawModule
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public decimal Fee { get; set; }

        public int ModuleNo { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public  List<RawModuleTran> ModuleTrans { get; set; }
    }

    public class RawModuleTran
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public string Hours { get; set; }
    }


    public class RawKeyword
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string WordEN { get; set; }

        public string WordCN { get; set; }

        public string WordHK { get; set; }
    }

    public class RawEnquiry
    {
        public int Id { get; set; }

        public int? CourseId { get; set; }

        public int EnquiryNo { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string ContactPersonEN { get; set; }

        public string ContactPersonCN { get; set; }

        public string ContactPersonHK { get; set; }
    }

    public class RawCourseDocument
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int DocumentId { get; set; }

        public int? LessonId { get; set; }

        public int DistinguishDocType { get; set; }

        public RawDocument Document { get; set; }
    }

    public class RawDocument
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public DateTime ModifiedDate { get; set; }
    }

    public class RawCourseTran
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public string CourseTitle { get; set; }

        public string Curriculum { get; set; }

        public string ConditionsOfCertificate { get; set; }

        public string Recognition { get; set; }

        public string AdmissionRequirements { get; set; }
    }
}
