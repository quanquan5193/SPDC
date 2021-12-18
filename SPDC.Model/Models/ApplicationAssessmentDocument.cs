using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    public class ApplicationAssessmentDocument
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public int DocumentId { get; set; }

        public virtual Document Document { get; set; }

        public virtual Application Application { get; set; }
    }
}
