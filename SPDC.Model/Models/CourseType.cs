using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CourseTypes")]
    public class CourseType
    {
        public CourseType()
        {
            Courses = new HashSet<Course>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(256)]
        public string NameEN { get; set; }

        [StringLength(256)]
        public string NameHK { get; set; }

        [StringLength(256)]
        public string NameCN { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

    }
}

