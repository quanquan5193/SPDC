using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("UserDocumentTemp")]
    public class UserDocumentTemp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int DocumentId { get; set; }
        
        public virtual Document Document { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}
