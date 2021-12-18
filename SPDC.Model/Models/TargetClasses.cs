using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SPDC.Model.Models
{
    [Table("TargetClasses")]
    public class TargetClasses
    {
        public TargetClasses()
        {
            //Classes = new HashSet<Class>();
        }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //public int ClassCommonId { get; set; }

        public int TargetNumberClass { get; set; }

        [StringLength(256)]
        public string AcademicYear { get; set; }

        //public virtual ClassCommon ClassCommon { get; set; }
        
        public virtual Course Course { get; set; }
    }
}
