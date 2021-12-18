using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("MakeUpAttendences")]
    public class MakeUpAttendence
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int ApplicationId { get; set; }

        public int MakeUpClassId { get; set; }

        public bool IsDisplayToStudentPortal { get; set; }

        public bool IsTakeAttendance { get; set; }

        public virtual Application Application { get; set; }

        public virtual Lesson Lesson { get; set; }

        public virtual MakeUpClass MakeUpClass { get; set; }

    }
}
