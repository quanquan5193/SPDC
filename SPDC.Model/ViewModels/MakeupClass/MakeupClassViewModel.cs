using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.MakeupClass
{
    public class MakeupClassViewModel
    {
        public int Id { get; set; } = 0;

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int TimeFromMin { get; set; }

        public int TimeFromHrs { get; set; }

        public int TimeToMin { get; set; }

        public int TimeToHrs { get; set; }

        public string Venue { get; set; }

        public List<MakeupClassDocViewModel> Documents { get; set; }
        public List<MakeUpAttendenceViewModel> Attendances { get; set; }

    }

    public class MakeupClassDocViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
    }

    public class MakeUpAttendenceViewModel
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int ApplicationId { get; set; }

        public int MakeUpClassId { get; set; }

        public bool IsDisplayToStudentPortal { get; set; }

        public int UserId { get; set; }

        public string ApplicationNumber { get; set; }

        public string StudentSurnameCN { get; set; }

        public string StudentSurnameEN { get; set; }

        public string StudentGivenCN { get; set; }

        public string StudentGivenEN { get; set; }

        public string ClassCode { get; set; }

        public string Date { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }
    }
}
