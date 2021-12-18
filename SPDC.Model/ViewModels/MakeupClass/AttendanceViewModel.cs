using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.MakeupClass
{
    public class AttendanceContainerViewModel
    {
        public int? AttendanceRequired { get; set; }
        public bool IsEnableEligibleForExam { get; set; }
        public IEnumerable<AttendanceViewModel> Data { get; set; }
        public List<AttendanceLessonviewModel> Lessons { get; set; }
    }

    public class AttendanceLessonviewModel{
        public int Id { get; set; }
        public string Date { get; set; }
        public int FromHrs { get; set; }
        public int ToHrs { get; set; }
        public int FromMin { get; set; }
        public int ToMin { get; set; }
        public bool IsMakeupClass { get; set; }
    }


    public class AttendanceViewModel
    {
        public int ApplicationId { get; set; }
        public string StudentNo { get; set; }
        public string SurnameCN { get; set; }
        public string GivenNameCN { get; set; }
        public string SurnameEN { get; set; }
        public string GivenNameEN { get; set; }
        public string CICNo { get; set; }
        public int? AttendancePercent { get; set; }
        public int? AttendanceLesson { get; set; }
        public int? AttendanceHrs { get; set; }
        public bool EligibleForExam { get; set; }
        public bool EligibleForMakeupClass { get; set; }
        public string Remarks { get; set; }

        public List<LessonAttendanceViewModel> AttendanceData { get; set; }
        public IEnumerable<AttendanceDocViewModel> Docs { get; set; }
    }

    public class LessonAttendanceViewModel
    {
        public int Id { get; set; }
        public int? LessonId { get; set; }
        public bool? IsTakeAttendance { get; set; }
        public bool? IsMakeUp { get; set; }
    }

    public class AttendanceDocViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }

    }
}
