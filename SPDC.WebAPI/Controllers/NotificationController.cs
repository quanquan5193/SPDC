using AutoMapper;
using Microsoft.AspNet.Identity;
using SPDC.Model.Models;
using SPDC.Model.ViewModels.Notification;
using SPDC.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPDC.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Notifications")]
    [EnableCors("*", "*", "*")]
    public class NotificationController : ApiControllerBase
    {
        private INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("Page/{page}/Size/{size}")]
        public async Task<IHttpActionResult> GetNotification(int page, int size)
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return BadRequest("User is not exist");
            }
            IEnumerable<NotificationUser> result = await _notificationService.GetNotificationUserAsync(id, page, size);

            var returnList = Mapper.Map<IEnumerable<NotificationViewModel>>(result);

            return Ok(returnList);
        }

        [HttpGet]
        [Route("Unread")]
        public async Task<IHttpActionResult> GetUnreadNotification()
        {
            int id = 0;
            var success = int.TryParse(HttpContext.Current.User.Identity.GetUserId(), out id);
            if (!success)
            {
                return BadRequest("User is not exist");
            }
            int result = await _notificationService.GetUnreadNotifications(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("Read/{id}")]
        public async Task<IHttpActionResult> ReadNotificationAsync(int id)
        {
            var notification = await _notificationService.ChangeReadStatusAsync(id);
            return Ok(notification);
        }

        [HttpPost]
        [Route("Remove/{id}")]
        public async Task<IHttpActionResult> RemoveNotificationAsync(int id)
        {
            var notification = await _notificationService.ChangeRemoveStatusAsync(id);
            return Ok(notification);
        }

        [HttpPost]
        [Route("Favourite/{id}")]
        public async Task<IHttpActionResult> FavouriteNotificationAsync(int id)
        {
            var notification = await _notificationService.ChangeFavouriteStatusAsync(id);
            return Ok(notification);
        }
    }
}
