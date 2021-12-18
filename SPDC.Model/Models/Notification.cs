using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Notifications")]
    public class Notification
    {
        public Notification()
        {
            NotificationUsers = new HashSet<NotificationUser>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public int Type { get; set; }

        public int DataId { get; set; }

        public virtual ICollection<NotificationUser> NotificationUsers { get; set; }

    }
}
