using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("NotificationUser")]
    public class NotificationUser
    {

        [Key, Column(Order = 0)]
        public int NotificationId { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        public bool IsRead { get; set; }

        public bool IsFavourite { get; set; }

        public bool IsRemove { get; set; }

        public DateTime CreatedDate { get; set; }

        public Notification Notification { get; set; }

        public ApplicationUser User { get; set; }

    }
}
