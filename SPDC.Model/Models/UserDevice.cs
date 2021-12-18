using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("UserDevices")]
    public class UserDevice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(256)]
        [Index("INDEX_DEVICETOKEN", IsUnique = true)]
        public string DeviceToken { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}
