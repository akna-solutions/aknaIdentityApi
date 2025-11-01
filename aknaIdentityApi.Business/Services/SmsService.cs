using aknaIdentityApi.Domain.Interfaces.Services;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;

namespace aknaIdentityApi.Business.Services
{
    /// <summary>
    /// Sms servis implementasyonu - Sandbox moduna uygun
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly IVerificationRepository verificationRepository;
        private readonly IConfiguration configuration;
        private readonly ILogger<SmsService> logger;
        private readonly HttpClient httpClient;

        public SmsService(
            IVerificationRepository verificationRepository,
            IConfiguration configuration,
            ILogger<SmsService> logger,
            HttpClient httpClient)
        {
            this.verificationRepository = verificationRepository;
            this.configuration = configuration;
            this.logger = logger;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Sms doğrulama kodu gönderir
        /// </summary>
        /// <param name="phoneNumber">Sms numarası</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task SendSmsVerificationCodeAsync(string phoneNumber, long userId)
        {
            try
            {
                logger.LogInformation($"Starting Sms verification for user {userId} at {phoneNumber}");

                // 6 haneli rastgele kod oluştur
                var verificationCode = GenerateVerificationCode();

                // Veritabanına doğrulama kodunu kaydet
                await verificationRepository.CreateVerificationCodeAsync(
                    userId,
                    verificationCode,
                    VerificationType.PhoneConfirmation
                );

                logger.LogInformation($"Verification code saved to database for user {userId}");

                await SendHelloWorldTemplateAsync(phoneNumber, verificationCode);

                logger.LogInformation($"Sms verification code processed successfully for user {userId} at {phoneNumber}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error sending Sms verification code to user {userId} at {phoneNumber}");
                throw new Exception("Sms doğrulama kodu gönderilirken hata oluştu.", ex);
            }
        }


        public async Task<string> SendHelloWorldTemplateAsync(string phoneNumber, string verificationCode)
        {
            var accessToken = configuration["Sms:AccessToken"];
            var fromPhoneNumberId = configuration["Sms:FromPhoneNumberId"];
            var cleanPhoneNumber = CleanPhoneNumber(configuration["Sms:ToPhoneNumber"]);
            var url = $"https://graph.facebook.com/v22.0/{fromPhoneNumberId}/messages";

            var requestBody = new
            {
                messaging_product = "whatsapp",
                to = cleanPhoneNumber,
                type = "template",
                template = new
                {
                    name = "hello22",
                    language = new { code = "en" }, 
                    components = new[]
                     {
                        new {
                            type = "body",
                            parameters = new object[]
                            {
                                new { type = "text", text = verificationCode } 
                            }
                        }
                    }
                }
            };



            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode ? $"Success: {responseContent}" : $"Error: {responseContent}";
        }


        /// <summary>
        /// 6 haneli rastgele doğrulama kodu oluşturur
        /// </summary>
        /// <returns></returns>
        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// Telefon numarasını temizler
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                throw new ArgumentException("Telefon numarası boş olamaz");

            var cleanNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (cleanNumber.StartsWith("90"))
            {
                return cleanNumber;
            }
            else if (cleanNumber.StartsWith("0") && cleanNumber.Length == 11)
            {
                return "90" + cleanNumber.Substring(1);
            }
            else if (cleanNumber.Length == 10)
            {
                return "90" + cleanNumber;
            }

            return cleanNumber;
        }
    }
}