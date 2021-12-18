using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("CommonData")]
    public class CommonData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string ValueString { get; set; }
        public int ValueInt { get; set; }
        public double ValueDouble { get; set; }

    }
}
