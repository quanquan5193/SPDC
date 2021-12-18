using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("MakeUpClasses")]
    public class MakeUpClass
    {
        public MakeUpClass()
        {
            Documents = new HashSet<Document>();
            MakeUpAttendences = new HashSet<MakeUpAttendence>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TimeFromMin { get; set; }

        public int TimeFromHrs { get; set; }

        public int TimeToMin { get; set; }

        public int TimeToHrs { get; set; }

        public string Venue { get; set; }

        public string Name { get; set; }

        public ICollection<Document> Documents { get; set; }

        public virtual ICollection<MakeUpAttendence> MakeUpAttendences { get; set; }

    }
}
