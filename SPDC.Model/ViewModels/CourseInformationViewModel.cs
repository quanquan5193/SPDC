using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CourseInformationViewModel
    {
        public int CourseID { get; set; }

        public string CourseCategories { get; set; }

        public string CourseCode { get; set; }

        public decimal CourseFee { get; set; }

        public float Duration { get; set; }

        public string CourseName { get; set; }

        public string ModeOfStudy { get; set; }
    }

    public class CourseModeViewModel
    {
        public int CourseModeID { get; set; }

        public string CourseModeName { get; set; }

    }

    public class CourseLocationViewModel
    {
        public int CourseLocationID { get; set; }

        public string CourseLocationName { get; set; }

    }

    public class CourseDetailInfoViewModel
    {
        public CourseDetailInfoViewModel()
        {
            CourseLogoImages = new List<string>();
        }
        public int CourseID { get; set; }

        public string CourseName { get; set; }

        public string CourseCode { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public string ModeOfStudy { get; set; }

        public float Duration { get; set; }

        public float Credits { get; set; }

        public string StudyLocation { get; set; }

        public CourseFeeViewModel CourseFee { get; set; }

        public string ProgrammeLeader { get; set; }

        public string Lecture { get; set; }

        public string MediumOfInstruction { get; set; }

        public string Curriculum { get; set; }

        public string Recognition { get; set; }

        public string AdmissionRequirements { get; set; }

        public string ConditionsofCertificateAward { get; set; }

        public List<EnquiryViewModel> Enquiry { get; set; }
        public string CourseBrochureUrl { get; set; }
        public string ApplicationFormUrl { get; set; }
        public List<string> CourseLogoImages { get; set; }

        public string ObjectiveEN { get; set; }
        public string ObjectiveTC { get; set; }
        public string ObjectiveSC { get; set; }

        public string WaitingTimeEN { get; set; }
        public string WaitingTimeTC { get; set; }
        public string WaitingTimeSC { get; set; }

        public bool ByModule { get; set; }
        public float DurationHrs { get; set; }
        public int DurationLesson { get; set; }
        public float DurationTotal { get; set; }
    }

    public class EnquiryViewModel
    {
        public string ContactPerson { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }
    }

    public class LectureViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

    }

    public class CourseFeeViewModel
    {
        public decimal CourseFee { get; set; }

        public decimal DiscountFee { get; set; }

        public List<CourseFeeModuleViewModel> CourseFeeModule { get; set; }
    }

    public class CourseFeeModuleViewModel
    {
        public int ModuleNo { get; set; }

        public decimal CourseFee { get; set; }
    }

    public class CoursePortalAdminViewModel
    {
        public int CourseID { get; set; }

        public string CourseCategory { get; set; }

        public string CourseCode { get; set; }

        public string CourseNameEN { get; set; }

        public string CourseNameCN { get; set; }

        public float Duration { get; set; }

        public decimal CourseFee { get; set; }

        public int TargetClass { get; set; }

        public bool WithExam { get; set; }

        public bool WithModule { get; set; }

        public bool DisplayCourseInformation { get; set; }

        public int CourseApprovedStatus { get; set; }
    }

    public class CourseApplicationViewModel
    {
        public int CourseID { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public string ModeOfStudy { get; set; }

        public List<ModuleApplicationViewModel> Modules { get; set; }

        public List<ClassApplicationViewModel> Classes { get; set; }

        public ApplicationViewModel ApplicationInfo { get; set; }

    }

    public class ModuleApplicationViewModel
    {
        public int ModuleId { get; set; }

        public int ModuleNo { get; set; }

    }

    public class ClassApplicationViewModel
    {
        public int ClassId { get; set; }

        public string ClassAvailable { get; set; }
    }

    public class CourseApplicationStep6ViewModel
    {
        public int CourseID { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public string ModeOfStudy { get; set; }

        public bool IHaveApplyFor { get; set; }
        
        public string IHaveApplyForText { get; set; }
        
        public bool IsRequiredRecipt { get; set; }

        public List<ModuleApplicationViewModel> Modules { get; set; }

        public ClassApplicationViewModel Classes { get; set; }

    }
}
