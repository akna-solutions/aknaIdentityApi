using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Dtos.Responses;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using aknaIdentityApi.Domain.Interfaces.UnitOfWorks;

namespace aknaIdentityApi.Business.Services
{
    /// <summary>
    /// AuthenticationService implementasyonu
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IDeviceInfoRepository deviceInfoRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IJwtService jwtService;
        private readonly IUserTokenRepository userTokenRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;

        public AuthenticationService(
            IUserRepository userRepository,
            IDeviceInfoRepository deviceInfoRepository,
            ICompanyRepository companyRepository,
            IDocumentRepository documentRepository,
            IJwtService jwtService,
            IUserTokenRepository userTokenRepository,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.deviceInfoRepository = deviceInfoRepository;
            this.companyRepository = companyRepository;
            this.documentRepository = documentRepository;
            this.jwtService = jwtService;
            this.userTokenRepository = userTokenRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Kullanıcı kaydı yapar ve JWT token döner
        /// </summary>
        /// <param name="request">Kullanıcı kayıt isteği</param>
        /// <returns>UserId ve JWT token</returns>
        public async Task<UserRegisterResponse> RegisterAsync(UserRegisterRequest request)
        {
            // Email kontrolü
            var existingUser = await userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Bu email adresi ile kayıtlı kullanıcı bulunmaktadır.");
            }

            // Şifre hash'leme (BCrypt kullanabilirsiniz)
            request.Password = HashPassword(request.Password);

            // Şirket kaydı (bireysel nakliyeci ise)
            if (request.UserType == UserType.IndividualCarrier)
            {
                request.CompanyId = await companyRepository.AddCompanyAsync(request);
            }

            // Kullanıcı kaydı
            var userId = await userRepository.AddUserAsync(request);
            request.UserId = userId;

            // Cihaz bilgisi kaydı
            await deviceInfoRepository.AddDeviceInfoAsync(request);

            // Belge kaydı
            if (request.Documents?.Any() == true)
            {
                await documentRepository.AddDocumentsAsync(userId, request.CompanyId ?? 0, request.Documents);
            }

            // Kullanıcı bilgilerini al (JWT için)
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Kullanıcı kaydı başarısız.");

            // JWT token oluştur
            var accessToken = await jwtService.GenerateTokenAsync(user);
            var refreshToken = await jwtService.GenerateRefreshTokenAsync(userId);

            // Token'ları veritabanına kaydet
            var userToken = new UserToken
            {
                UserId = userId,
                DeviceId = request.DeviceId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpires = DateTime.UtcNow.AddHours(24),
                RefreshTokenExpires = DateTime.UtcNow.AddDays(30),
                LastUsedAt = DateTime.UtcNow,
                IpAddress = GetClientIpAddress(),
                UserAgent = GetUserAgent(),
                TokenType = "Register",
                CreatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system"
            };

            await userTokenRepository.SaveTokenAsync(userToken);

            return new UserRegisterResponse
            {
                UserId = userId,
                Token = accessToken,
                TokenExpires = userToken.AccessTokenExpires,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                UserType = user.UserType.ToString(),
                CompanyId = user.CompanyId,
                Success = true,
                Message = "Kayıt başarıyla tamamlandı."
            };
        }

        public async Task<long> AddCompanyAsync(CompanyRegisterRequest request)
        {
            return await companyRepository.AddCompanyAsync(request);
        }

        /// <summary>
        /// Kullanıcı girişi yapar
        /// </summary>
        /// <param name="request">Giriş isteği</param>
        /// <returns>JWT token ve kullanıcı bilgileri</returns>
        public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request)
        {
            // Kullanıcı kontrolü
            var user = await userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Email veya şifre hatalı.");
            }

            // Şifre kontrolü
            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email veya şifre hatalı.");
            }

            // JWT token oluştur
            var accessToken = await jwtService.GenerateTokenAsync(user);
            var refreshToken = await jwtService.GenerateRefreshTokenAsync(user.Id);

            // Token geçerlilik süreleri
            var accessTokenExpires = DateTime.UtcNow.AddHours(request.RememberMe ? 24 * 7 : 24); // Beni hatırla varsa 7 gün
            var refreshTokenExpires = DateTime.UtcNow.AddDays(request.RememberMe ? 90 : 30); // Beni hatırla varsa 90 gün

            // Token'ları veritabanına kaydet
            var userToken = new UserToken
            {
                UserId = user.Id,
                DeviceId = request.DeviceId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpires = accessTokenExpires,
                RefreshTokenExpires = refreshTokenExpires,
                LastUsedAt = DateTime.UtcNow,
                IpAddress = request.IpAddress ?? GetClientIpAddress(),
                UserAgent = request.UserAgent ?? GetUserAgent(),
                TokenType = "Login",
                CreatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system"
            };

            await userTokenRepository.SaveTokenAsync(userToken);

            return new UserLoginResponse
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpires = accessTokenExpires,
                RefreshTokenExpires = refreshTokenExpires,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                UserType = user.UserType.ToString(),
                CompanyId = user.CompanyId,
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed,
                Success = true,
                Message = "Giriş başarıyla tamamlandı."
            };
        }

        /// <summary>
        /// Token yeniler
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Yeni access token</returns>
        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            // Refresh token kontrolü
            var userToken = await userTokenRepository.GetByRefreshTokenAsync(refreshToken);
            if (userToken == null || userToken.User == null)
            {
                throw new UnauthorizedAccessException("Geçersiz refresh token.");
            }

            // Yeni access token oluştur
            var newAccessToken = await jwtService.GenerateTokenAsync(userToken.User);

            // Token bilgilerini güncelle
            userToken.AccessToken = newAccessToken;
            userToken.AccessTokenExpires = DateTime.UtcNow.AddHours(24);
            userToken.LastUsedAt = DateTime.UtcNow;
            userToken.UpdatedDate = DateTime.UtcNow;
            userToken.UpdatedUser = "system";

            userTokenRepository.Update(userToken);
            await unitOfWork.SaveChangesAsync();

            return newAccessToken;
        }

        /// <summary>
        /// Çıkış yapar (token'ı geçersiz kılar)
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="deviceId">Cihaz ID</param>
        /// <returns></returns>
        public async Task LogoutAsync(long userId, string deviceId)
        {
            await userTokenRepository.RevokeUserDeviceTokenAsync(userId, deviceId);
        }

        /// <summary>
        /// Şifre değiştirir
        /// </summary>
        /// <param name="request">Şifre değiştirme isteği</param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("Kullanıcı bulunamadı.");
            }

            // Mevcut şifre kontrolü
            if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Mevcut şifre hatalı.");
            }

            // Yeni şifre hash'leme ve güncelleme
            var newPasswordHash = HashPassword(request.NewPassword);
            await userRepository.UpdatePasswordAsync(request.UserId, newPasswordHash);

            // Kullanıcının tüm token'larını iptal et (güvenlik için)
            await userTokenRepository.RevokeAllUserTokensAsync(request.UserId);

            return true;
        }

        #region Private Methods

        /// <summary>
        /// Şifre hash'leme
        /// </summary>
        private static string HashPassword(string password)
        {
            // BCrypt kullanmanız önerilir, burada basit bir örnek
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Şifre doğrulama
        /// </summary>
        private static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        /// <summary>
        /// Client IP adresini al
        /// </summary>
        private string? GetClientIpAddress()
        {
            return httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// User Agent bilgisini al
        /// </summary>
        private string? GetUserAgent()
        {
            return httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
        }

        #endregion
    }
}