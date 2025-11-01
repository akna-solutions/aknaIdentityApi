using aknaIdentityApi.Domain.Dtos.Requests;

namespace aknaIdentityApi.Domain.Interfaces.Services
{
    /// <summary>
    /// Doğrulama servis arayüzü
    /// </summary>
    public interface IVerificationService
    {
        #region Email Verification

        /// <summary>
        /// Email doğrulama kodunu kontrol eder
        /// </summary>
        /// <param name="request">Email kod doğrulama isteği</param>
        /// <returns>Doğrulama sonucu</returns>
        Task<bool> VerifyEmailCodeAsync(VerifyEmailCodeRequest request);

        /// <summary>
        /// Kullanıcının email doğrulama durumunu günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task UpdateEmailConfirmationStatusAsync(long userId);

        #endregion

        #region Sms Verification

        /// <summary>
        /// Sms doğrulama kodunu kontrol eder
        /// </summary>
        /// <param name="request">Sms kod doğrulama isteği</param>
        /// <returns>Doğrulama sonucu</returns>
        Task<bool> VerifySmsCodeAsync(VerifySmsCodeRequest request);

        /// <summary>
        /// Kullanıcının Sms doğrulama durumunu günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task UpdateSmsConfirmationStatusAsync(long userId);

        #endregion
    }
}