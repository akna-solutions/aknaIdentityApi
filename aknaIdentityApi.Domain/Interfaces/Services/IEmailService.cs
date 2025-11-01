
namespace aknaIdentityApi.Domain.Interfaces.Services
{
    /// <summary>
    /// Email servisi interface'i
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Email doğrulama kodu gönderir
        /// </summary>
        /// <param name="email">Alıcı email adresi</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task SendEmailVerificationCodeAsync(string email, long userId);

        /// <summary>
        /// Genel email gönderme metodu
        /// </summary>
        /// <param name="to">Alıcı email</param>
        /// <param name="subject">Konu</param>
        /// <param name="body">İçerik</param>
        /// <param name="isHtml">HTML formatında mı</param>
        /// <returns></returns>
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);

        /// <summary>
        /// Şifre sıfırlama email'i gönderir
        /// </summary>
        /// <param name="email">Alıcı email adresi</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task SendPasswordResetCodeAsync(string email, long userId);

        /// <summary>
        /// Hoş geldin email'i gönderir
        /// </summary>
        /// <param name="email">Alıcı email adresi</param>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns></returns>
        Task SendWelcomeEmailAsync(string email, string userName);
    }
}