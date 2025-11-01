namespace aknaIdentityApi.Domain.Dtos.Responses
{
    /// <summary>
    /// Kullanıcı kayıt yanıt modeli
    /// </summary>
    public class UserRegisterResponse
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// JWT Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token geçerlilik süresi
        /// </summary>
        public DateTime TokenExpires { get; set; }

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
        /// Şirket ID (varsa)
        /// </summary>
        public long? CompanyId { get; set; }

        /// <summary>
        /// Kayıt başarılı mı?
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Başarı mesajı
        /// </summary>
        public string Message { get; set; } = "Kayıt başarıyla tamamlandı.";
    }
}