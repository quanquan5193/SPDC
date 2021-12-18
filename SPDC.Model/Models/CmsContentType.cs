using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CmsContentTypes")]
    public class CmsContentType
    {
        public CmsContentType()
        {
            CmsContents = new HashSet<CmsContent>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string NameTC { get; set; }
        [MaxLength(256)]
        public string NameSC { get; set; }

        [MaxLength(256)]
        public string CmsType { get; set; }

        public virtual ICollection<CmsContent> CmsContents { get; set; }
    }
}
