using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPDC.Model.Models.CIC
{
    public class CreateCourseRequestModel
    {
        [Required]
        public string Is_Allowance { get; set; }

        [Required]
        public string Teaching_Lang { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public decimal Duration { get; set; }

        [Required]
        public decimal Attendance_Percent { get; set; }

        [Required]
        public string Is_Epay { get; set; }

        [Required]
        public string Course_Code { get; set; }

        [Required]
        public string Course_Name { get; set; }

        [Required]
        public string Course_Name_TC { get; set; }

        [Required]
        public string Course_Name_SC { get; set; }

        [Required]
        public string Category_Code { get; set; }

        [Required]
        public string Module_Type { get; set; }

        [Required]
        public List<TrainingCampusModel> Training_Campus_List { get; set; }

        [Required]
        public List<SubjectOfficerModel> Subject_Officer_List { get; set; }

        [Required]
        public List<AdmissionRequirementModel> Admission_List { get; set; }

        [Required]
        public string[] Module_Course_Codes { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public int Max_Retake_Course_Count { get; set; }

        public int Fees_Code { get; set; }

        public int Resit_Exam_Fees_Code { get; set; }

        public string Fee_Remarks { get; set; }

        public string Resit_Exam_Remarks { get; set; }

        public string Condition_Of_Certificate_Award_Eng { get; set; }

        public string Condition_Of_Certificate_Award_Chi { get; set; }

        public string External_Recognition_Eng { get; set; }

        public string External_Recognition_Chi { get; set; }

        public string Enquiry { get; set; }

        public string Enquiry_TC { get; set; }

        public string Enquiry_SC { get; set; }
    }
}