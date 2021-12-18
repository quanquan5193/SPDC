using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class CourseCalendarViewModel : ClassViewModel
    {
        public ICollection<LessonViewModel> Lessons { get; set; }
    }
}
