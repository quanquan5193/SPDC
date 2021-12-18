using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class UserDocumentTempViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ApplicationId { get; set; }

        public virtual Document Document{ get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
