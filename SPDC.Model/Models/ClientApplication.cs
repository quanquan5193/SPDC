using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Clients")]
    public class ClientApplication
    {
        [Key]
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string ClientName { get; set; }

        public string ClientUrl { get; set; }
    }
}
