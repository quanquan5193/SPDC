using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels.Notification
{
    public class NotificationViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public int Type { get; set; }

        public int DataId { get; set; }

        public bool IsRead { get; set; }

        public bool IsFavourite { get; set; }

        public bool IsRemove { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
