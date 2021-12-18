using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Keywords")]
    public  class Keyword
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        [StringLength(256)]
        public string WordEN { get; set; }

        [StringLength(256)]
        public string WordCN { get; set; }

        [StringLength(256)]
        public string WordHK { get; set; }

        public virtual Course Course { get; set; }
    }
}
