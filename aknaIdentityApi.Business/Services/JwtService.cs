using aknaIdentityApi.Domain.Configuration;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;
using aknaIdentityApi.Domain.Interfaces.UnitOfWorks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace aknaIdentityApi.Business.Services
{
    /// <summary>
    /// JWT token servisi implementasyonu
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserTokenRepository userTokenRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public JwtService(IOptions<JwtSettings> jwtSettings, IUserTokenRepository userTokenRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _jwtSettings = jwtSettings.Value;
            this.userTokenRepository = userTokenRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Kullanıcı için JWT token oluşturur
        /// </summary>
        /// <param name="user">Kullanıcı bilgileri</param>
        /// <returns>JWT token</returns>
        public async Task<string> GenerateTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new(ClaimTypes.GivenName, user.Name),
                new(ClaimTypes.Surname, user.Surname),
                new("UserType", user.UserType.ToString()),
                new("CompanyId", user.CompanyId.ToString()),
                new("IsEmailConfirmed", user.IsEmailConfirmed.ToString()),
                new("IsPhoneNumberConfirmed", user.IsPhoneNumberConfirmed.ToString()),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                new("UserCode", user.UserCode),
                new("CreatedDate", user.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            // Kullanıcı permissionları varsa ekle
            if (user.PermissionIds.Any())
            {
                foreach (var permissionId in user.PermissionIds)
                {
                    claims.Add(new Claim("Permission", permissionId.ToString()));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Refresh token oluşturur
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Refresh token</returns>
        public async Task<string> GenerateRefreshTokenAsync(long userId)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            return refreshToken;
        }

        /// <summary>
        /// Token'dan kullanıcı ID'sini çıkarır
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Kullanıcı ID</returns>
        public long? GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Token geçerli mi kontrol eder
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Token geçerli mi?</returns>
        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Refresh token ile yeni access token oluşturur
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>Yeni access token</returns>
        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            // Refresh token repository ile doğrulama
            var userToken = await userTokenRepository.GetByRefreshTokenAsync(refreshToken);
            if (userToken == null || !userToken.IsActive || userToken.IsRevoked || userToken.RefreshTokenExpires < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Geçersiz veya süresi dolmuş refresh token.");
            }

            // Kullanıcıyı al
            var user = await userRepository.GetUserByIdAsync(userToken.UserId);
            if (user == null)
            {
                throw new SecurityTokenException("Kullanıcı bulunamadı.");
            }

            // Yeni access token oluştur
            var newAccessToken = await GenerateTokenAsync(user);

            // Token bilgilerini güncelle
            userToken.AccessToken = newAccessToken;
            userToken.AccessTokenExpires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);
            userToken.LastUsedAt = DateTime.UtcNow;
            userTokenRepository.Update(userToken);
            await unitOfWork.SaveChangesAsync();

            return newAccessToken;
        }

        /// <summary>
        /// Token'dan claims bilgilerini çıkarır
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Claims dictionary</returns>
        public Dictionary<string, string> GetClaimsFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);

                return jsonToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
    }
}