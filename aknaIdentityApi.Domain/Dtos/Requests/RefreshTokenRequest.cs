using System.ComponentModel.DataAnnotations;

namespace aknaIdentityApi.Domain.Dtos.Requests
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
