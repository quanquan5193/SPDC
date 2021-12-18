using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CmsImages")]
    public class CmsImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? CmsId { get; set; }

        [Required]
        [StringLength(256)]
        public string Url { get; set; }

        [Required]
        [StringLength(256)]
        public string ContentType { get; set; }

        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        public virtual CmsContent CmsContent { get; set; }
    }
}
