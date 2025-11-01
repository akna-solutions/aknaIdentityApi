namespace aknaIdentityApi.Domain.Configurations
{
    /// <summary>
    /// Email ayarları configuration sınıfı
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// SMTP sunucusu (örn: smtp.gmail.com)
        /// </summary>
        public string SmtpServer { get; set; } = string.Empty;

        /// <summary>
        /// SMTP portu (genellikle 587 veya 465)
        /// </summary>
        public int SmtpPort { get; set; } = 587;

        /// <summary>
        /// Gönderen email adresi
        /// </summary>
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gönderen adı (email'de görünen isim)
        /// </summary>
        public string FromName { get; set; } = "AKNA Transport";

        /// <summary>
        /// Email şifresi veya App Password
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// SSL/TLS kullanılsın mı (önerilen: true)
        /// </summary>
        public bool EnableSsl { get; set; } = true;

        /// <summary>
        /// Varsayılan kimlik bilgileri kullanılsın mı (genellikle false)
        /// </summary>
        public bool UseDefaultCredentials { get; set; } = false;

        /// <summary>
        /// Bağlantı zaman aşımı (milisaniye)
        /// </summary>
        public int Timeout { get; set; } = 30000; // 30 saniye

        /// <summary>
        /// Email gönderimi etkin mi?
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Test modu - gerçek email gönderilmez, sadece log'a yazılır
        /// </summary>
        public bool TestMode { get; set; } = false;

        /// <summary>
        /// Test modunda kullanılacak email adresi
        /// </summary>
        public string? TestEmailAddress { get; set; }
    }
}