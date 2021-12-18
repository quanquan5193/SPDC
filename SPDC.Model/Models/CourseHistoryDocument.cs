using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class CourseHistoryDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseHistoryId { get; set; }

        public int DocumentId { get; set; }

        public virtual CourseAppovedStatusHistory CourseAppovedStatusHistory { get; set; }

        public virtual Document Document { get; set; }
    }
}
