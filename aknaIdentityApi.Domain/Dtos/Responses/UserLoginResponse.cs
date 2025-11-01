namespace aknaIdentityApi.Domain.Dtos.Responses
{
    /// <summary>
    /// Kullanıcı giriş yanıt modeli
    /// </summary>
    public class UserLoginResponse
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// JWT Access Token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Access Token geçerlilik süresi
        /// </summary>
        public DateTime AccessTokenExpires { get; set; }

        /// <summary>
        /// Refresh Token geçerlilik süresi
        /// </summary>
        public DateTime RefreshTokenExpires { get; set; }

        /// <summary>
        /// Kullanıcı email adresi
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı soyadı
        /// </summary>
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı tipi
        /// </summary>
        public string UserType { get; set; } = string.Empty;

        /// <summary>
        /// Şirket ID
        /// </summary>
        public long? CompanyId { get; set; }

        /// <summary>
        /// Email doğrulandı mı?
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Telefon doğrulandı mı?
        /// </summary>
        public bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Giriş başarılı mı?
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Başarı mesajı
        /// </summary>
        public string Message { get; set; } = "Giriş başarıyla tamamlandı.";
    }
}




