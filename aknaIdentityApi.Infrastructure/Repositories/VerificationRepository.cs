using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace aknaIdentityApi.Infrastructure.Repositories
{
    /// <summary>
    /// Verification Repository implementasyonu
    /// </summary>
    public class VerificationRepository : BaseRepository<Verification>, IVerificationRepository
    {
        public VerificationRepository(AknaIdentityDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Yeni doğrulama kodu oluşturur
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="code">Doğrulama kodu</param>
        /// <param name="verificationType">Doğrulama tipi</param>
        /// <returns></returns>
        public async Task CreateVerificationCodeAsync(long userId, string code, VerificationType verificationType)
        {
            var verification = new Verification
            {
                UserId = userId,
                Code = code,
                VerificationType = verificationType,
                ExpirationDate = DateTime.UtcNow.AddMinutes(5), // 5 dakika geçerlilik
                IsUsed = false,
                CreatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system",
                IsDeleted = false
            };

            await AddAsync(verification);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Doğrulama kodunu kontrol eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="code">Doğrulama kodu</param>
        /// <param name="verificationType">Doğrulama tipi</param>
        /// <returns>Kod geçerli mi?</returns>
        public async Task<bool> VerifyCodeAsync(long userId, string code, VerificationType verificationType)
        {
            var verification = await context.Verifications
                .Where(v => v.UserId == userId &&
                           v.Code == code &&
                           v.VerificationType == verificationType &&
                           !v.IsUsed &&
                           !v.IsDeleted &&
                           v.ExpirationDate > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (verification == null)
                return false;

            // Kodu kullanıldı olarak işaretle
            verification.IsUsed = true;
            verification.UpdatedDate = DateTime.UtcNow;
            verification.UpdatedUser = "system";

            Update(verification);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Süresi dolmuş kodları temizler
        /// </summary>
        /// <returns>Temizlenen kod sayısı</returns>
        public async Task<int> CleanupExpiredCodesAsync()
        {
            var expiredCodes = await context.Verifications
                .Where(v => !v.IsDeleted && v.ExpirationDate <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var code in expiredCodes)
            {
                code.IsDeleted = true;
                code.UpdatedDate = DateTime.UtcNow;
                code.UpdatedUser = "system";
            }

            if (expiredCodes.Any())
            {
                UpdateRange(expiredCodes);
                await context.SaveChangesAsync();
            }

            return expiredCodes.Count;
        }

   
    }
}