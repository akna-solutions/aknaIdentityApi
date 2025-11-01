using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace aknaIdentityApi.Infrastructure.Repositories
{
    /// <summary>
    /// User Token repository implementasyonu
    /// </summary>
    public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(AknaIdentityDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Kullanıcı ve cihaz için token kaydeder
        /// </summary>
        /// <param name="userToken">Token bilgileri</param>
        /// <returns></returns>
        public async Task SaveTokenAsync(UserToken userToken)
        {
            // Aynı kullanıcı ve cihaz için eski token'ları iptal et
            await RevokeUserDeviceTokenAsync(userToken.UserId, userToken.DeviceId);

            await AddAsync(userToken);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Refresh token ile token bilgilerini getirir
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Token bilgileri</returns>
        public async Task<UserToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await context.UserTokens
                .Include(x => x.User)
                .Where(x => x.RefreshToken == refreshToken &&
                           x.IsActive &&
                           !x.IsRevoked &&
                           !x.IsDeleted &&
                           x.RefreshTokenExpires > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Access token ile token bilgilerini getirir
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns>Token bilgileri</returns>
        public async Task<UserToken?> GetByAccessTokenAsync(string accessToken)
        {
            return await context.UserTokens
                .Include(x => x.User)
                .Where(x => x.AccessToken == accessToken &&
                           x.IsActive &&
                           !x.IsRevoked &&
                           !x.IsDeleted &&
                           x.AccessTokenExpires > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Kullanıcının aktif token'larını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Aktif token'lar</returns>
        public async Task<List<UserToken>> GetActiveTokensByUserIdAsync(long userId)
        {
            return await context.UserTokens
                .Where(x => x.UserId == userId &&
                           x.IsActive &&
                           !x.IsRevoked &&
                           !x.IsDeleted &&
                           x.AccessTokenExpires > DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Kullanıcı ve cihaza ait aktif token'ı getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="deviceId">Cihaz ID</param>
        /// <returns>Aktif token</returns>
        public async Task<UserToken?> GetActiveTokenByUserAndDeviceAsync(long userId, string deviceId)
        {
            return await context.UserTokens
                .Where(x => x.UserId == userId &&
                           x.DeviceId == deviceId &&
                           x.IsActive &&
                           !x.IsRevoked &&
                           !x.IsDeleted &&
                           x.AccessTokenExpires > DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Token'ı iptal eder
        /// </summary>
        /// <param name="tokenId">Token ID</param>
        /// <returns></returns>
        public async Task RevokeTokenAsync(long tokenId)
        {
            await context.UserTokens
                .Where(x => x.Id == tokenId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsRevoked, true)
                    .SetProperty(x => x.IsActive, false)
                    .SetProperty(x => x.RevokedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının tüm token'larını iptal eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task RevokeAllUserTokensAsync(long userId)
        {
            await context.UserTokens
                .Where(x => x.UserId == userId && x.IsActive && !x.IsRevoked)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsRevoked, true)
                    .SetProperty(x => x.IsActive, false)
                    .SetProperty(x => x.RevokedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının belirli cihazındaki token'ı iptal eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="deviceId">Cihaz ID</param>
        /// <returns></returns>
        public async Task RevokeUserDeviceTokenAsync(long userId, string deviceId)
        {
            await context.UserTokens
                .Where(x => x.UserId == userId &&
                           x.DeviceId == deviceId &&
                           x.IsActive &&
                           !x.IsRevoked)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsRevoked, true)
                    .SetProperty(x => x.IsActive, false)
                    .SetProperty(x => x.RevokedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedUser, "system"));
        }

        /// <summary>
        /// Süresi dolmuş token'ları temizler
        /// </summary>
        /// <returns>Temizlenen token sayısı</returns>
        public async Task<int> CleanupExpiredTokensAsync()
        {
            var expiredTokens = await context.UserTokens
                .Where(x => (x.AccessTokenExpires <= DateTime.UtcNow ||
                            x.RefreshTokenExpires <= DateTime.UtcNow) &&
                           !x.IsDeleted)
                .CountAsync();

            await context.UserTokens
                .Where(x => (x.AccessTokenExpires <= DateTime.UtcNow ||
                            x.RefreshTokenExpires <= DateTime.UtcNow) &&
                           !x.IsDeleted)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsDeleted, true)
                    .SetProperty(x => x.IsActive, false)
                    .SetProperty(x => x.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedUser, "system"));

            return expiredTokens;
        }

        /// <summary>
        /// Token'ın son kullanım zamanını günceller
        /// </summary>
        /// <param name="tokenId">Token ID</param>
        /// <returns></returns>
        public async Task UpdateLastUsedAsync(long tokenId)
        {
            await context.UserTokens
                .Where(x => x.Id == tokenId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.LastUsedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının aktif cihaz sayısını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Aktif cihaz sayısı</returns>
        public async Task<int> GetActiveDeviceCountAsync(long userId)
        {
            return await context.UserTokens
                .Where(x => x.UserId == userId &&
                           x.IsActive &&
                           !x.IsRevoked &&
                           !x.IsDeleted &&
                           x.AccessTokenExpires > DateTime.UtcNow)
                .Select(x => x.DeviceId)
                .Distinct()
                .CountAsync();
        }

        /// <summary>
        /// Token geçerli mi kontrol eder
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns>Token geçerli mi?</returns>
        public async Task<bool> IsTokenValidAsync(string accessToken)
        {
            return await context.UserTokens
                .AnyAsync(x => x.AccessToken == accessToken &&
                              x.IsActive &&
                              !x.IsRevoked &&
                              !x.IsDeleted &&
                              x.AccessTokenExpires > DateTime.UtcNow);
        }
    }
}