using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.BindingModels
{
    public class CreateCourseBindingModel
    {
        public int Id { get; set; } = 0;

        public int CourseCategory { get; set; }

        public string CourseCode { get; set; }

        public string CourseNameEN { get; set; }

        public string CourseNameCN { get; set; }

        public string CourseNameHK { get; set; }

        public int TargetClassSize { get; set; }

        public int Mode { get; set; }

        public float DurationHrs { get; set; }

        public float DurationTotal { get; set; }

        public int Lessons { get; set; }

        public float Credits { get; set; }

        public int Venue { get; set; }

        public int LevelOfApproval { get; set; }

        public bool Display { get; set; }

        public string ObjectiveEN { get; set; }
        public string ObjectiveTC { get; set; }
        public string ObjectiveSC { get; set; }

        public string WaitingTimeEN { get; set; }
        public string WaitingTimeTC { get; set; }
        public string WaitingTimeSC { get; set; }
    }

    public class CourseFeeBindingModel
    {
        public int Id { get; set; } = 0;

        public decimal CourseFee { get; set; }

        public decimal Discounted { get; set; }

        public string FeeRemarks1 { get; set; }

        public string FeeRemarks2 { get; set; }

        public bool ByModule { get; set; } = false;

        public List<ModuleBindingModel> Modules { get; set; }
        public List<CombinationBindingModel> Combinations { get; set; }
    }

    public class CombinationBindingModel
    {
        public List<DropdownModuleModel> Modules { get; set; }

        public int CombinationNo { get; set; }

        public decimal CombinationFee { get; set; }
    }

    public class DropdownModuleModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }

    public class ModuleBindingModel
    {
        public int ModuleNo { get; set; }

        public string ModuleNameEN { get; set; }

        public string ModuleNameCN { get; set; }

        public string ModuleNameHK { get; set; }

        public decimal Fee { get; set; }
    }

    public class CourseAllowanceBindingModel
    {
        public int Id { get; set; } = 0;

        public decimal Allowance { get; set; }

        public bool IsAllowanceProvided { get; set; }
    }

    public class CourseProgrammeLeadershipBindingModel
    {
        public int Id { get; set; } = 0;

        public int? ProgrammeLeaderId { get; set; }

        public int? Lecturer { get; set; }

        public int? MediumOfInstruction { get; set; }
    }

    public class CurriculumBindingModel
    {
        public int Id { get; set; } = 0;

        public string CurriculumEN { get; set; }

        public string CurriculumCN { get; set; }

        public string CurriculumHK { get; set; }
    }

    public class RecognitionBindingModel
    {
        public int Id { get; set; } = 0;

        public string RecognitionEN { get; set; }

        public string RecognitionCN { get; set; }

        public string RecognitionHK { get; set; }
    }

    public class AdmissionRequirementsBindingModel
    {
        public int Id { get; set; } = 0;

        public string AdmissionRequirementsEN { get; set; }

        public string AdmissionRequirementsCN { get; set; }

        public string AdmissionRequirementsHK { get; set; }
    }

    public class CertificateConditionsBindingModel
    {
        public int Id { get; set; } = 0;

        public string CertificateConditionsEN { get; set; }

        public string CertificateConditionsCN { get; set; }

        public string CertificateConditionsHK { get; set; }
    }

    public class EnquirysBindingModel
    {
        public int Id { get; set; } = 0;
        public IEnumerable<EnquiryBindingModel> Enquiries { get; set; }
    }

    public class EnquiryBindingModel
    {
        public int EnquiryNo { get; set; }

        public string ContactPersonEN { get; set; }

        public string ContactPersonCN { get; set; }

        public string ContactPersonHK { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

    }

    public class CourseSearchBindingModel
    {
        public int[] coursecategories { get; set; } = new int[] { };

        public int coursemode { get; set; } = 0;

        public int courselocations { get; set; } = 0;

        public string search { get; set; } = "";

        public int index { get; set; } = 1;

        public string sortBy { get; set; } = "Id";

        public bool isDescending { get; set; } = false;

        public int size { get; set; } = 20;
    }

    public class CourseSearchAdminPortalBindingModel
    {
        public int[] coursecategories { get; set; }

        public string coursecode { get; set; } = "";

        public string coursenameEN { get; set; } = "";

        public string coursenameCN { get; set; } = "";

        public bool? displaycourse { get; set; }

        public int index { get; set; } = 1;

        public string sortBy { get; set; } = "Id";

        public bool isDescending { get; set; } = true;

        public int size { get; set; } = 20;
    }
}
