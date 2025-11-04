using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;
using aknaIdentityApi.Domain.Interfaces.UnitOfWorks;

namespace aknaIdentityApi.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IUnitOfWork unitOfWork;

        public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            this.notificationRepository = notificationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Notification> CreateNotificationAsync(long userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                ReadAt = null,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedUser = "system",
                IsDeleted = false
            };

            await notificationRepository.AddAsync(notification);
            await unitOfWork.CommitAsync();

            return notification;
        }

        public async Task<Notification?> GetNotificationByIdAsync(long id)
        {
            return await notificationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(long userId)
        {
            return await notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(long userId)
        {
            return await notificationRepository.GetUnreadByUserIdAsync(userId);
        }

        public async Task<int> GetUnreadCountAsync(long userId)
        {
            return await notificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task<bool> MarkAsReadAsync(long notificationId)
        {
            return await notificationRepository.MarkAsReadAsync(notificationId);
        }

        public async Task<bool> MarkAllAsReadAsync(long userId)
        {
            return await notificationRepository.MarkAllAsReadAsync(userId);
        }

        public async Task<bool> DeleteNotificationAsync(long id)
        {
            var notification = await notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                return false;
            }

            notification.IsDeleted = true;
            notification.UpdatedDate = DateTime.UtcNow;
            await notificationRepository.UpdateAsync(notification);
            await unitOfWork.CommitAsync();

            return true;
        }
    }
}
