using aknaIdentityApi.Domain.Entities;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(long userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(long userId);
        Task<int> GetUnreadCountAsync(long userId);
        Task<bool> MarkAsReadAsync(long notificationId);
        Task<bool> MarkAllAsReadAsync(long userId);
    }
}
