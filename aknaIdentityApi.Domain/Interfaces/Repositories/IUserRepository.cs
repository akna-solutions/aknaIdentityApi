using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Entities;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    /// <summary>
    /// IUserRepository - Güncellenmiş
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Kullanıcı kaydını sağlar
        /// </summary>
        /// <param name="request">UserRegisterRequest</param>
        /// <returns></returns>
        Task<long> AddUserAsync(UserRegisterRequest request);

        /// <summary>
        /// Email adresine göre kullanıcı bulur
        /// </summary>
        /// <param name="email">Email adresi</param>
        /// <returns>Kullanıcı bilgileri</returns>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// ID'ye göre kullanıcı bulur
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Kullanıcı bilgileri</returns>
        Task<User?> GetUserByIdAsync(long userId);

        /// <summary>
        /// Kullanıcının email doğrulama durumunu günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task UpdateEmailConfirmationStatusAsync(long userId);

        /// <summary>
        /// Kullanıcının şifresini günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="newPasswordHash">Yeni şifre hash'i</param>
        /// <returns></returns>
        Task UpdatePasswordAsync(long userId, string newPasswordHash);

        /// <summary>
        /// Kullanıcının son giriş zamanını günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        Task UpdateLastLoginAsync(long userId);

        /// <summary>
        /// Kullanıcının telefon doğrulama durumunu günceller
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="isConfirmed">Doğrulandı mı?</param>
        /// <returns></returns>
        Task UpdatePhoneConfirmationStatusAsync(long userId, bool isConfirmed);
    }
}