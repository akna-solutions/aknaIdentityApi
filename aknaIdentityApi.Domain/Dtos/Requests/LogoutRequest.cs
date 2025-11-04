using System.ComponentModel.DataAnnotations;

namespace aknaIdentityApi.Domain.Dtos.Requests
{
    public class LogoutRequest
    {
        [Required(ErrorMessage = "User ID is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Device ID is required")]
        public string DeviceId { get; set; } = string.Empty;
    }
}
