using aknaIdentityApi.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace aknaIdentityApi.Domain.Entities
{
    [Table("UserTokens")]
    public class UserToken : BaseEntity
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Cihaz ID
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Access Token (JWT)
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Access Token geçerlilik tarihi
        /// </summary>
        public DateTime AccessTokenExpires { get; set; }

        /// <summary>
        /// Refresh Token geçerlilik tarihi
        /// </summary>
        public DateTime RefreshTokenExpires { get; set; }

        /// <summary>
        /// Token aktif mi?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Son kullanım tarihi
        /// </summary>
        public DateTime LastUsedAt { get; set; }

        /// <summary>
        /// IP adresi
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User Agent bilgisi
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Token tipi (Login, Refresh, etc.)
        /// </summary>
        public string TokenType { get; set; } = "Login";

        /// <summary>
        /// Token iptal edildi mi?
        /// </summary>
        public bool IsRevoked { get; set; } = false;

        /// <summary>
        /// Token iptal edilme tarihi
        /// </summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>
        /// Navigasyon property - User
        /// </summary>
        public virtual User? User { get; set; }
    }
}