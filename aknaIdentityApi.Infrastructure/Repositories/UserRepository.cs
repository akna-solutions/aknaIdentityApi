using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace aknaIdentityApi.Infrastructure.Repositories
{
    /// <summary>
    /// UserRepository implementasyonu - ExecuteUpdate ile
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AknaIdentityDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Kullanıcı kaydını sağlar
        /// </summary>
        /// <param name="request">UserRegisterRequest</param>
        /// <returns></returns>
        public async Task<long> AddUserAsync(UserRegisterRequest request)
        {
            User user = new User
            {
                UserCode = "USR" + DateTime.UtcNow.Ticks.ToString(),
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Gender = request.Gender,
                PasswordHash = request.Password,
                PhoneNumber = request.PhoneNumber,
                BloodType = request.BloodType,
                TurkishRepublicIdNumber = request.TurkishRepublicIdNumber,
                Address = request.Address,
                BirthDate = request.BirthDate,
                UserType = request.UserType,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system",
                CompanyId = request.CompanyId ?? 0,
                IsEmailConfirmed = false,
                IsPhoneNumberConfirmed = false
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user.Id;
        }

        /// <summary>
        /// Email adresine göre kullanıcı bulur
        /// </summary>
        /// <param name="email">Email adresi</param>
        /// <returns>Kullanıcı bilgileri</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await context.Users
                .Where(u => u.Email == email && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// ID'ye göre kullanıcı bulur
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Kullanıcı bilgileri</returns>
        public async Task<User?> GetUserByIdAsync(long userId)
        {
            return await context.Users
                .Where(u => u.Id == userId && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Kullanıcının email doğrulama durumunu günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task UpdateEmailConfirmationStatusAsync(long userId)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.IsEmailConfirmed, true)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının şifresini günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="newPasswordHash">Yeni şifre hash'i</param>
        /// <returns></returns>
        public async Task UpdatePasswordAsync(long userId, string newPasswordHash)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.PasswordHash, newPasswordHash)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının son giriş zamanını günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task UpdateLastLoginAsync(long userId)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının telefon doğrulama durumunu günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="isConfirmed">Doğrulandı mı?</param>
        /// <returns></returns>
        public async Task UpdatePhoneConfirmationStatusAsync(long userId, bool isConfirmed)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.IsPhoneNumberConfirmed, isConfirmed)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının profilini günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="name">Ad</param>
        /// <param name="surname">Soyad</param>
        /// <param name="phoneNumber">Telefon</param>
        /// <param name="address">Adres</param>
        /// <returns></returns>
        public async Task UpdateProfileAsync(long userId, string name, string surname, string? phoneNumber, string? address)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Name, name)
                    .SetProperty(u => u.Surname, surname)
                    .SetProperty(u => u.PhoneNumber, phoneNumber)
                    .SetProperty(u => u.Address, address)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının profil fotoğrafını günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="profileImageUrl">Profil resmi URL</param>
        /// <returns></returns>
        public async Task UpdateProfileImageAsync(long userId, string? profileImageUrl)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.ProfileImageUrl, profileImageUrl)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının şirket bilgisini günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="companyId">Şirket ID</param>
        /// <returns></returns>
        public async Task UpdateCompanyAsync(long userId, long companyId)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.CompanyId, companyId)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının hesabını deaktif eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task DeactivateUserAsync(long userId)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.IsDeleted, true)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının hesabını aktif eder
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task ActivateUserAsync(long userId)
        {
            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.IsDeleted, false)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Kullanıcının yetkilerini günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="permissionIds">Yetki ID'leri</param>
        /// <returns></returns>
        public async Task UpdatePermissionsAsync(long userId, List<int> permissionIds)
        {
            var permissionsString = string.Join(",", permissionIds);

            await context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.PermissionIds, permissionIds)
                    .SetProperty(u => u.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(u => u.UpdatedUser, "system"));
        }

        /// <summary>
        /// Şirkete ait kullanıcıları getirir
        /// </summary>
        /// <param name="companyId">Şirket ID</param>
        /// <returns>Şirket kullanıcıları</returns>
        public async Task<List<User>> GetUsersByCompanyIdAsync(long companyId)
        {
            return await context.Users
                .Where(u => u.CompanyId == companyId && !u.IsDeleted)
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .ToListAsync();
        }

        /// <summary>
        /// Kullanıcı tipine göre kullanıcıları getirir
        /// </summary>
        /// <param name="userType">Kullanıcı tipi</param>
        /// <returns>Belirtilen tipteki kullanıcılar</returns>
        public async Task<List<User>> GetUsersByTypeAsync(aknaIdentityApi.Domain.Enums.UserType userType)
        {
            return await context.Users
                .Where(u => u.UserType == userType && !u.IsDeleted)
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .ToListAsync();
        }

        /// <summary>
        /// Aktif kullanıcı sayısını getirir
        /// </summary>
        /// <returns>Aktif kullanıcı sayısı</returns>
        public async Task<int> GetActiveUserCountAsync()
        {
            return await context.Users
                .Where(u => !u.IsDeleted)
                .CountAsync();
        }

        
    }
}