namespace aknaIdentityApi.Domain.Interfaces.Services
{
    public interface ISmsService
    {
        /// <summary>
        /// WhatsApp doğrulama kodu gönderir
        /// </summary>
        /// <param name="phoneNumber">WhatsApp numarası</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task SendSmsVerificationCodeAsync(string phoneNumber, long userId);
    }
}
