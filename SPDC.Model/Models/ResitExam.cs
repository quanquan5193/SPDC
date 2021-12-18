using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ResitExames")]
    public class ResitExam
    {
        public ResitExam()
        {
            ResitExamApplications = new HashSet<ResitExamApplication>();
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

        public int TypeOfReExam { get; set; }

        public DateTime ResitExamApplicationDeadline { get; set; }

        public virtual ICollection<ResitExamApplication> ResitExamApplications { get; set; }

    }
}
