using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ModuleCombinations")]
    public class ModuleCombination
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string ModuleNos { get; set; }

        public int CombinationNo { get; set; }

        public decimal CourseFee { get; set; }

        public virtual Course Course { get; set; }
    }
}
