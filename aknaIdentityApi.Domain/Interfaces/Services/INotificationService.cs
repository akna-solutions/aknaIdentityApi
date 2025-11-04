using aknaIdentityApi.Domain.Entities;

namespace aknaIdentityApi.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(long userId, string title, string message);
        Task<Notification?> GetNotificationByIdAsync(long id);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(long userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(long userId);
        Task<int> GetUnreadCountAsync(long userId);
        Task<bool> MarkAsReadAsync(long notificationId);
        Task<bool> MarkAllAsReadAsync(long userId);
        Task<bool> DeleteNotificationAsync(long id);
    }
}
