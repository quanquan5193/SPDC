using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("MediumOfInstructions")]
    public class MediumOfInstruction
    {
        public MediumOfInstruction()
        {
            Courses = new HashSet<Course>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Id { get; set; }

        public string NameEN { get; set; }

        public string NameCN { get; set; }

        public string NameHK { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
