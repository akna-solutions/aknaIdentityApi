namespace aknaIdentityApi.Domain.Configuration
{
    /// <summary>
    /// JWT ayarları configuration sınıfı
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// JWT secret key - güçlü bir anahtar olmalı
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Token'ı kimin verdiği (genellikle uygulama adı)
        /// </summary>
        public string Issuer { get; set; } = "AKNA-Identity-API";

        /// <summary>
        /// Token'ın kime verildiği (genellikle client uygulama)
        /// </summary>
        public string Audience { get; set; } = "AKNA-Identity-Client";

        /// <summary>
        /// Token geçerlilik süresi (saat)
        /// </summary>
        public int ExpirationHours { get; set; } = 24;

        /// <summary>
        /// Refresh token geçerlilik süresi (gün)
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; } = 30;

        /// <summary>
        /// Token yenileme özelliği aktif mi?
        /// </summary>
        public bool AllowRefreshToken { get; set; } = true;

        /// <summary>
        /// Multiple device login'e izin verilsin mi?
        /// </summary>
        public bool AllowMultipleDeviceLogin { get; set; } = true;

        /// <summary>
        /// Clock skew - zaman farkı toleransı (dakika)
        /// </summary>
        public int ClockSkewMinutes { get; set; } = 5;
    }
}