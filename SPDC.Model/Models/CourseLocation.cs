using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CourseLocation")]
    public class CourseLocation
    {
        public CourseLocation()
        {
            CourseLocationTrans = new HashSet<CourseLocationTran>();
            Exams = new HashSet<Exam>();
            Lessons = new HashSet<Lesson>();
            Courses = new HashSet<Course>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Status { get; set; }

        public string VenueCode { get; set; }

        public virtual ICollection<CourseLocationTran> CourseLocationTrans { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }

}
