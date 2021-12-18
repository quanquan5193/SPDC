using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service.Services
{
    public interface INotificationService
    {
        Notification CreateNotification(Notification model);

        NotificationUser CreateNotificationUser(NotificationUser model);

        NotificationUser UpdateNotificationUser(NotificationUser model);

        Task<NotificationUser> ChangeReadStatusAsync(int id);

        Task<NotificationUser> ChangeFavouriteStatusAsync(int id);

        Task<NotificationUser> ChangeRemoveStatusAsync(int id);

        NotificationUser GetNotificationUserById(int id);

        Task<IEnumerable<NotificationUser>> GetNotificationUserAsync(int userId, int page, int size);

        Task<int> GetUnreadNotifications(int userId);
    }
    public class NotificationService : INotificationService
    {
        private INotificationRepository _notificationRepository;
        private INotificationUserRepository _notificationUserRepository;
        private IUnitOfWork _unitOfWork;

        public NotificationService(INotificationRepository notificationRepository, INotificationUserRepository notificationUserRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _notificationUserRepository = notificationUserRepository;
            _unitOfWork = unitOfWork;
        }

        public Notification CreateNotification(Notification model)
        {
            Notification notification = _notificationRepository.Add(model);
            _unitOfWork.Commit();
            return notification;
        }

        public NotificationUser CreateNotificationUser(NotificationUser model)
        {
            var notificationUser = _notificationUserRepository.Add(model);
            _unitOfWork.Commit();
            return notificationUser;
        }

        public NotificationUser UpdateNotificationUser(NotificationUser notification)
        {
            _notificationUserRepository.Update(notification);
            _unitOfWork.Commit();
            return notification;
        }

        public async Task<IEnumerable<NotificationUser>> GetNotificationUserAsync(int userId, int page, int size)
        {
            IEnumerable<NotificationUser> list = await _notificationUserRepository.GetMultiPaging(x => !x.IsRemove && x.UserId == userId, "CreatedDate", true, page, size, new string[] { "Notification" });
            return list;
        }

        public NotificationUser GetNotificationUserById(int id)
        {
            return _notificationUserRepository.GetSingleById(id);
        }

        public async Task<NotificationUser> ChangeReadStatusAsync(int id)
        {
            NotificationUser notification = await _notificationUserRepository.GetSingleByCondition(x => x.NotificationId == id);
            notification.IsRead = !notification.IsRead;
            _notificationUserRepository.Update(notification);
            _unitOfWork.Commit();
            return notification;
        }

        public async Task<NotificationUser> ChangeFavouriteStatusAsync(int id)
        {
            NotificationUser notification = await _notificationUserRepository.GetSingleByCondition(x => x.NotificationId == id);
            notification.IsFavourite = !notification.IsFavourite;
            _notificationUserRepository.Update(notification);
            _unitOfWork.Commit();
            return notification;
        }

        public async Task<NotificationUser> ChangeRemoveStatusAsync(int id)
        {
            NotificationUser notification = await _notificationUserRepository.GetSingleByCondition(x => x.NotificationId == id);
            notification.IsRemove = !notification.IsRemove;
            _notificationUserRepository.Update(notification);
            _unitOfWork.Commit();
            return notification;
        }

        public async Task<int> GetUnreadNotifications(int userId)
        {
            int number = await _notificationUserRepository.Count(x => !x.IsRead && x.UserId == userId);
            return number;
        }
    }
}
