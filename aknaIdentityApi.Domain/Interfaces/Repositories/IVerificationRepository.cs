using aknaIdentityApi.Domain.Enums;


namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    public interface IVerificationRepository
    {

        /// <summary>
        /// Yeni doğrulama kodu oluşturur
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="code">Doğrulama kodu</param>
        /// <param name="verificationType">Doğrulama tipi</param>
        /// <returns></returns>
        Task CreateVerificationCodeAsync(long userId, string code, VerificationType verificationType);


        /// <summary>
        /// Doğrulama kodunu kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="code">Doğrulama kodu</param>
        /// <param name="verificationType">Doğrulama tipi</param>
        /// <returns>Kod geçerli mi?</returns>
        Task<bool> VerifyCodeAsync(long userId, string code, VerificationType verificationType);


        /// <summary>
        /// Süresi dolmuş kodları temizler
        /// </summary>
        /// <returns>Temizlenen kod sayısı</returns>
        Task<int> CleanupExpiredCodesAsync();
    }
}
