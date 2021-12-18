using SPDC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPDC.WebAPI.Models
{
    public class NotificationEmailBindingModel
    {
        public int cmsId { get; set; }

        public int cmsMatchedId { get; set; }

        public UserNotificationType[] UserType { get; set; }

        public int[] CourseTypes { get; set; }

    }
}