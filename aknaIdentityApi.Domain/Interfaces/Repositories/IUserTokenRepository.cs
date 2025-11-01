using aknaIdentityApi.Domain.Entities;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    /// <summary>
    /// User Token repository interface'i
    /// </summary>
    public interface IUserTokenRepository : IBaseRepository<UserToken>
    {
        /// <summary>
        /// Kullanıcı ve cihaz için token kaydeder
        /// </summary>
        /// <param name="userToken">Token bilgileri</param>
        /// <returns></returns>
        Task SaveTokenAsync(UserToken userToken);

        /// <summary>
        /// Refresh token ile token bilgilerini getirir
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Token bilgileri</returns>
        Task<UserToken?> GetByRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Access token ile token bilgilerini getirir
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns>Token bilgileri</returns>
        Task<UserToken?> GetByAccessTokenAsync(string accessToken);

        /// <summary>
        /// Kullanıcının aktif token'larını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Aktif token'lar</returns>
        Task<List<UserToken>> GetActiveTokensByUserIdAsync(long userId);

        /// <summary>
        /// Kullanıcı ve cihaza ait aktif token'ı getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="deviceId">Cihaz ID</param>
        /// <returns>Aktif token</returns>
        Task<UserToken?> GetActiveTokenByUserAndDeviceAsync(long userId, string deviceId);

        /// <summary>
        /// Token'ı iptal eder
        /// </summary>
        /// <param name="tokenId">Token ID</param>
        /// <returns></returns>
        Task RevokeTokenAsync(long tokenId);

        /// <summary>
        /// Kullanıcının tüm token'larını iptal eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task RevokeAllUserTokensAsync(long userId);

        /// <summary>
        /// Kullanıcının belirli cihazındaki token'ı iptal eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="deviceId">Cihaz ID</param>
        /// <returns></returns>
        Task RevokeUserDeviceTokenAsync(long userId, string deviceId);

        /// <summary>
        /// Süresi dolmuş token'ları temizler
        /// </summary>
        /// <returns>Temizlenen token sayısı</returns>
        Task<int> CleanupExpiredTokensAsync();

        /// <summary>
        /// Token'ın son kullanım zamanını günceller
        /// </summary>
        /// <param name="tokenId">Token ID</param>
        /// <returns></returns>
        Task UpdateLastUsedAsync(long tokenId);

        /// <summary>
        /// Kullanıcının aktif cihaz sayısını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Aktif cihaz sayısı</returns>
        Task<int> GetActiveDeviceCountAsync(long userId);

        /// <summary>
        /// Token geçerli mi kontrol eder
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns>Token geçerli mi?</returns>
        Task<bool> IsTokenValidAsync(string accessToken);
    }
}