
using System.ComponentModel.DataAnnotations;


namespace aknaIdentityApi.Domain.Dtos.Responses
{
    /// <summary>
    /// Şifre değiştirme isteği
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        [Required(ErrorMessage = "Kullanıcı ID gereklidir")]
        public long UserId { get; set; }

        /// <summary>
        /// Mevcut şifre
        /// </summary>
        [Required(ErrorMessage = "Mevcut şifre gereklidir")]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// Yeni şifre
        /// </summary>
        [Required(ErrorMessage = "Yeni şifre gereklidir")]
        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Yeni şifre tekrarı
        /// </summary>
        [Required(ErrorMessage = "Şifre tekrarı gereklidir")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
