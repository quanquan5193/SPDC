using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Lessons")]
    public class Lesson
    {
        public Lesson()
        {
            LessonAttendances = new HashSet<LessonAttendance>();
            MakeUpAttendences = new HashSet<MakeUpAttendence>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClassId { get; set; }

        public int No { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int TimeFromMin { get; set; }

        public int TimeFromHrs { get; set; }

        public int TimeToMin { get; set; }

        public int TimeToHrs { get; set; }

        //[Required]
        //[StringLength(256)]
        //public string FromTime { get; set; }

        //[Required]
        //[StringLength(256)]
        //public string ToTime { get; set; }

        public int LocationId { get; set; }

        public string Venue { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }

        public virtual Class Class { get; set; }

        public virtual CourseLocation CourseLocation { get; set; }

        public virtual ICollection<LessonAttendance> LessonAttendances { get; set; }

        public virtual ICollection<MakeUpAttendence> MakeUpAttendences { get; set; }

    }

}
