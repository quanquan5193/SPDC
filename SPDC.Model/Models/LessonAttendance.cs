using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("LessonAttendances")]
    public partial class LessonAttendance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int ApplicationId { get; set; }

        public int UserId { get; set; }

        public bool IsTakeAttendance { get; set; }

        public bool IsMakeUp { get; set; }

        public int FromLessonId { get; set; }

        public virtual Application Application { get; set; }

        public virtual Lesson Lesson { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
