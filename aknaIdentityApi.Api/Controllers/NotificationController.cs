using aknaIdentityApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aknaIdentityApi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            var notification = await notificationService.CreateNotificationAsync(request.UserId, request.Title, request.Message);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.Id }, new { data = notification, success = true });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(long id)
        {
            var notification = await notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound(new { message = "Notification not found", success = false });
            }

            return Ok(new { data = notification, success = true });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(long userId)
        {
            var notifications = await notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(new { data = notifications, success = true });
        }

        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadNotifications(long userId)
        {
            var notifications = await notificationService.GetUnreadNotificationsByUserIdAsync(userId);
            return Ok(new { data = notifications, success = true });
        }

        [HttpGet("user/{userId}/unread-count")]
        public async Task<IActionResult> GetUnreadCount(long userId)
        {
            var count = await notificationService.GetUnreadCountAsync(userId);
            return Ok(new { count = count, success = true });
        }

        [HttpPost("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var result = await notificationService.MarkAsReadAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Notification not found", success = false });
            }

            return Ok(new { message = "Notification marked as read", success = true });
        }

        [HttpPost("user/{userId}/mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead(long userId)
        {
            var result = await notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read", success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(long id)
        {
            var result = await notificationService.DeleteNotificationAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Notification not found", success = false });
            }

            return Ok(new { message = "Notification deleted successfully", success = true });
        }
    }

    public class CreateNotificationRequest
    {
        public long UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
