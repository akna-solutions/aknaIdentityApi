using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;

namespace aknaIdentityApi.Business.Services
{
    /// <summary>
    /// VerificationService
    /// </summary>
    public class VerificationService : IVerificationService
    {
        private readonly IVerificationRepository verificationRepository;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Constructor of VerificationService
        /// </summary>
        /// <param name="verificationRepository"></param>
        /// <param name="userRepository"></param>
        public VerificationService(IVerificationRepository verificationRepository, IUserRepository userRepository)
        {
            this.verificationRepository = verificationRepository;
            this.userRepository = userRepository;
        }

        #region Email Verification

        /// <summary>
        /// Kullanıcı emailini onaylar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> VerifyEmailCodeAsync(VerifyEmailCodeRequest request)
        {
            return await verificationRepository.VerifyCodeAsync(
                request.UserId,
                request.VerificationCode,
                VerificationType.EmailConfirmation);
        }

        /// <summary>
        /// Kullanıcı email onaylama statusunu günceller
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateEmailConfirmationStatusAsync(long userId)
        {
            await userRepository.UpdateEmailConfirmationStatusAsync(userId);
        }

        #endregion

        #region Sms Verification

        /// <summary>
        /// Sms doğrulama kodunu kontrol eder
        /// </summary>
        /// <param name="request">Sms kod doğrulama isteği</param>
        /// <returns>Doğrulama sonucu</returns>
        public async Task<bool> VerifySmsCodeAsync(VerifySmsCodeRequest request)
        {
            return await verificationRepository.VerifyCodeAsync(
                request.UserId,
                request.VerificationCode,
                VerificationType.PhoneConfirmation);
        }

        /// <summary>
        /// Kullanıcının Sms doğrulama durumunu günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task UpdateSmsConfirmationStatusAsync(long userId)
        {
            await userRepository.UpdatePhoneConfirmationStatusAsync(userId, true);
        }

        #endregion
    }
}