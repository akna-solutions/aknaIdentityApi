using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Dtos.Responses;

namespace aknaIdentityApi.Domain.Interfaces.Services
{
    /// <summary>
    /// Kimlik doğrulama servis arayüzü
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Kullanıcı kaydı yapar ve JWT token döner
        /// </summary>
        /// <param name="request">Kullanıcı kayıt isteği</param>
        /// <returns>UserId ve JWT token</returns>
        Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest request);

        /// <summary>
        /// Kullanıcı girişi yapar
        /// </summary>
        /// <param name="request">Giriş isteği</param>
        /// <returns>JWT token ve kullanıcı bilgileri</returns>
        Task<UserLoginResponse> LoginAsync(UserLoginRequest request);

        /// <summary>
        /// Token yeniler
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Yeni access token</returns>
        Task<string> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Çıkış yapar (token'ı geçersiz kılar)
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="deviceId">Cihaz ID</param>
        /// <returns></returns>
        Task LogoutAsync(long userId, string deviceId);

        /// <summary>
        /// Şifre değiştirir
        /// </summary>
        /// <param name="request">Şifre değiştirme isteği</param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);

        Task<long> AddCompanyAsync(CompanyRegisterRequest request);

    }
}