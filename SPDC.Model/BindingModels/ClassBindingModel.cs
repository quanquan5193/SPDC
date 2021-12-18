using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;

namespace SPDC.Model.BindingModels
{
    public class ClassBindingModel
    {
        public int Id { get; set; } = 0;

        public string ClassCode { get; set; }

        public int CourseId { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public bool IsExam { get; set; }

        public bool IsReExam { get; set; }

        public int ExamPassingMask { get; set; }

        public decimal ReExamFees { get; set; }

        public string AcademicYear { get; set; }

        public int Capacity { get; set; }

        public bool InvisibleOnWebsite { get; set; }

        public int SubClassStatus { get; set; }

        public int SubClassApprovedStatus { get; set; }

        public ICollection<LessonBindingModel> Lessons { get; set; }

        public ICollection<ExamBindingModel> Exams { get; set; }

        public int? AttendanceRequirement { get; set; }

        public int? ClassCommonId { get; set; }

        public int? EnrollmentNumber { get; set; }

        public int TargetClassId { get; set; }

        public byte? CountReExam { get; set; }
    }

    public class CreateClassAdminBindingModel
    {
        public int Id { get; set; } = 0;

        public string ClassCode { get; set; }

        public int CourseId { get; set; }

        public DateTime CommencementDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public bool IsExam { get; set; }

        public bool IsReExam { get; set; }

        public int ExamPassingMask { get; set; }

        public decimal ReExamFees { get; set; }

        public string AcademicYear { get; set; }

        public bool InvisibleOnWebsite { get; set; }

        public List<LessonBindingModel> Lessons { get; set; }
    }

    public class TargetClassBindingModel
    {
        public int Id { get; set; } = 0;

        public int CourseId { get; set; }

        public int ClassCommonId { get; set; }

        public int TargetNumberClass { get; set; }

        public ClassCommonBindingModel ClassCommonBindingModel { get; set; }

        public ICollection<ClassBindingModel> ClassBindingModels { get; set; }
    }

    public class ClassCommonBindingModel
    {
        public int Id { get; set; } = 0;

        public string TypeName { get; set; }

        public int? TypeCommon { get; set; }

    }

    public class SelectModuleItem
    {
        public int Id { get; set; }

        public  string ModuleName { get; set; }
    }



}
