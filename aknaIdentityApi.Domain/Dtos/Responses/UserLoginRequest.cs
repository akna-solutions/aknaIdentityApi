
using System.ComponentModel.DataAnnotations;

namespace aknaIdentityApi.Domain.Dtos.Responses
{
    /// <summary>
    /// Kullanıcı giriş isteği
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// Email adresi
        /// </summary>
        [Required(ErrorMessage = "Email adresi gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre
        /// </summary>
        [Required(ErrorMessage = "Şifre gereklidir")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Cihaz ID
        /// </summary>
        [Required(ErrorMessage = "Cihaz ID gereklidir")]
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Cihaz tipi (iOS, Android, Web)
        /// </summary>
        public string DeviceType { get; set; } = "Web";

        /// <summary>
        /// Cihaz modeli
        /// </summary>
        public string? DeviceModel { get; set; }

        /// <summary>
        /// IP adresi
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User Agent bilgisi
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Beni hatırla
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }
}
