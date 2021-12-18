using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CourseCategories")]
    public class CourseCategory
    {
        public CourseCategory()
        {
            CourseCategories1 = new HashSet<CourseCategory>();
            CourseCategorieTrans = new HashSet<CourseCategorieTran>();
            Courses = new HashSet<Course>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Status { get; set; }

        public int? ParentId { get; set; }

        public virtual ICollection<CourseCategory> CourseCategories1 { get; set; }

        public virtual CourseCategory CourseCategory1 { get; set; }

        public virtual ICollection<CourseCategorieTran> CourseCategorieTrans { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }

}
