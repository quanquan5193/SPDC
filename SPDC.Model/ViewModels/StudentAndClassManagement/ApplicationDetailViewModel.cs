using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.StudentAndClassManagement
{
    public class ApplicationDetailViewModel
    {
        public int Id { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public string Email { get; set; }

        public string SurNameEnglish { get; set; }

        public string SurNameChinese { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Sex { get; set; }

        public string TelNo { get; set; }

        public string FaxNo { get; set; }

        public string PresentEmployer { get; set; }

        public string ClassPreferenceCode { get; set; }

        public string ModeOfStudy { get; set; }

        public string OtherEmailContact { get; set; }

        public string GivenNameEnglish { get; set; }
        
        public string GivenNameChinese { get; set; }

        public int Age { get; set; }

        public string HKIDno { get; set; }

        public string MobileNumber { get; set; }

        public string Position { get; set; }
    }
}
