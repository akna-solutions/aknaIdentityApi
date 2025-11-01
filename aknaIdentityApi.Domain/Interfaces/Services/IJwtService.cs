using aknaIdentityApi.Domain.Entities;

namespace aknaIdentityApi.Domain.Interfaces.Services
{
    /// <summary>
    /// JWT token servisi interface'i
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Kullanıcı için JWT token oluşturur
        /// </summary>
        /// <param name="user">Kullanıcı bilgileri</param>
        /// <returns>JWT token</returns>
        Task<string> GenerateTokenAsync(User user);

        /// <summary>
        /// Refresh token oluşturur
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Refresh token</returns>
        Task<string> GenerateRefreshTokenAsync(long userId);

        /// <summary>
        /// Token'dan kullanıcı ID'sini çıkarır
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Kullanıcı ID</returns>
        long? GetUserIdFromToken(string token);

        /// <summary>
        /// Token geçerli mi kontrol eder
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Token geçerli mi?</returns>
        bool ValidateToken(string token);

        /// <summary>
        /// Refresh token ile yeni access token oluşturur
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Yeni access token</returns>
        Task<string> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Token'dan claims bilgilerini çıkarır
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Claims dictionary</returns>
        Dictionary<string, string> GetClaimsFromToken(string token);
    }
}