using aknaIdentityApi.Domain.Configurations;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace aknaIdentityApi.Business.Services
{
    /// <summary>
    /// Email servisi implementasyonu
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IVerificationRepository _verificationRepository;

        public EmailService(IOptions<EmailSettings> emailSettings, IVerificationRepository verificationRepository)
        {
            _emailSettings = emailSettings.Value;
            _verificationRepository = verificationRepository;
        }

        /// <summary>
        /// Email doğrulama kodu gönderir
        /// </summary>
        /// <param name="email">Alıcı email adresi</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task SendEmailVerificationCodeAsync(string email, long userId)
        {
            // 6 haneli rastgele kod oluştur
            var verificationCode = GenerateVerificationCode();

            // Yeni kodu veritabanına kaydet
            await _verificationRepository.CreateVerificationCodeAsync(userId, verificationCode, VerificationType.EmailConfirmation);

            // Email içeriğini hazırla
            var subject = "AKNA Transport - Email Doğrulama Kodu";
            var body = CreateEmailVerificationTemplate(verificationCode);

            // Email gönder
            await SendEmailAsync(email, subject, body, true);
        }

        /// <summary>
        /// Şifre sıfırlama email'i gönderir
        /// </summary>
        /// <param name="email">Alıcı email adresi</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns></returns>
        public async Task SendPasswordResetCodeAsync(string email, long userId)
        {
            // 6 haneli rastgele kod oluştur
            var resetCode = GenerateVerificationCode();

            // Yeni kodu veritabanına kaydet
            await _verificationRepository.CreateVerificationCodeAsync(userId, resetCode, VerificationType.PasswordReset);

            // Email içeriğini hazırla
            var subject = "AKNA Transport - Şifre Sıfırlama Kodu";
            var body = CreatePasswordResetTemplate(resetCode);

            // Email gönder
            await SendEmailAsync(email, subject, body, true);
        }

        /// <summary>
        /// Hoş geldin email'i gönderir
        /// </summary>
        /// <param name="email">Alıcı email adresi</param>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns></returns>
        public async Task SendWelcomeEmailAsync(string email, string userName)
        {
            var subject = "AKNA Identity'ye Hoş Geldiniz!";
            var body = CreateWelcomeTemplate(userName);

            await SendEmailAsync(email, subject, body, true);
        }

        /// <summary>
        /// Genel email gönderme metodu
        /// </summary>
        /// <param name="to">Alıcı email</param>
        /// <param name="subject">Konu</param>
        /// <param name="body">İçerik</param>
        /// <param name="isHtml">HTML formatında mı</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
                client.EnableSsl = _emailSettings.EnableSsl;
                client.UseDefaultCredentials = _emailSettings.UseDefaultCredentials;
                client.Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password);

                using var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                message.BodyEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new InvalidOperationException($"Email gönderilirken hata oluştu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 6 haneli rastgele doğrulama kodu oluşturur
        /// </summary>
        /// <returns>6 haneli sayı kodu</returns>
        private static string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// Email doğrulama template'ini oluşturur
        /// </summary>
        /// <param name="code">Doğrulama kodu</param>
        /// <returns>HTML formatında email içeriği</returns>
        private static string CreateEmailVerificationTemplate(string code)
        {
            return $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Email Doğrulama</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        .header {{
            text-align: center;
            background-color: #007bff;
            color: white;
            padding: 20px;
            border-radius: 10px 10px 0 0;
            margin: -20px -20px 20px -20px;
        }}
        .code-container {{
            text-align: center;
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin: 20px 0;
        }}
        .verification-code {{
            font-size: 32px;
            font-weight: bold;
            color: #007bff;
            letter-spacing: 5px;
            margin: 10px 0;
        }}
        .warning {{
            background-color: #fff3cd;
            border: 1px solid #ffeaa7;
            color: #856404;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
        }}
        .footer {{
            text-align: center;
            color: #666;
            font-size: 14px;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #eee;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>AKNA Identity</h1>
            <p>Email Doğrulama</p>
        </div>
        
        <div>
            <h2>Merhaba,</h2>
            <p>AKNA Identity hesabınızı doğrulamak için aşağıdaki 6 haneli kodu kullanın:</p>
            
            <div class='code-container'>
                <p>Doğrulama Kodunuz:</p>
                <div class='verification-code'>{code}</div>
            </div>
            
            <div class='warning'>
                <strong>Önemli:</strong> Bu kod 5 dakika içinde geçerliliğini yitirecektir. 
                Kodu kimseyle paylaşmayın ve güvenliğiniz için bu emaili sildikten sonra kodu unutun.
            </div>
            
            <p>Eğer bu işlemi siz yapmadıysanız, lütfen bu emaili dikkate almayın.</p>
            
            <p>Teşekkürler,<br>AKNA Identity Ekibi</p>
        </div>
        
        <div class='footer'>
            <p>Bu email otomatik olarak gönderilmiştir. Lütfen yanıtlamayın.</p>
            <p>&copy; 2025 AKNA Identity. Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Şifre sıfırlama template'ini oluşturur
        /// </summary>
        /// <param name="code">Sıfırlama kodu</param>
        /// <returns>HTML formatında email içeriği</returns>
        private static string CreatePasswordResetTemplate(string code)
        {
            return $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Şifre Sıfırlama</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        .header {{
            text-align: center;
            background-color: #dc3545;
            color: white;
            padding: 20px;
            border-radius: 10px 10px 0 0;
            margin: -20px -20px 20px -20px;
        }}
        .code-container {{
            text-align: center;
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin: 20px 0;
        }}
        .verification-code {{
            font-size: 32px;
            font-weight: bold;
            color: #dc3545;
            letter-spacing: 5px;
            margin: 10px 0;
        }}
        .warning {{
            background-color: #f8d7da;
            border: 1px solid #f5c6cb;
            color: #721c24;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
        }}
        .footer {{
            text-align: center;
            color: #666;
            font-size: 14px;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #eee;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>AKNA Identity</h1>
            <p>Şifre Sıfırlama</p>
        </div>
        
        <div>
            <h2>Şifre Sıfırlama Talebi</h2>
            <p>Hesabınızın şifresini sıfırlamak için aşağıdaki 6 haneli kodu kullanın:</p>
            
            <div class='code-container'>
                <p>Şifre Sıfırlama Kodunuz:</p>
                <div class='verification-code'>{code}</div>
            </div>
            
            <div class='warning'>
                <strong>Güvenlik Uyarısı:</strong> Bu kod 5 dakika içinde geçerliliğini yitirecektir. 
                Eğer bu işlemi siz yapmadıysanız, derhal hesabınızın güvenliğini kontrol edin.
                Kodu kimseyle paylaşmayın!
            </div>
            
            <p>Bu kodu kullanarak yeni bir şifre belirleyebilirsiniz.</p>
            
            <p>Güvenliğiniz için,<br>AKNA Identity Ekibi</p>
        </div>
        
        <div class='footer'>
            <p>Bu email otomatik olarak gönderilmiştir. Lütfen yanıtlamayın.</p>
            <p>&copy; 2025 AKNA Identity. Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Hoş geldin template'ini oluşturur
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns>HTML formatında email içeriği</returns>
        private static string CreateWelcomeTemplate(string userName)
        {
            return $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Hoş Geldiniz</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        .header {{
            text-align: center;
            background-color: #28a745;
            color: white;
            padding: 20px;
            border-radius: 10px 10px 0 0;
            margin: -20px -20px 20px -20px;
        }}
        .welcome-message {{
            text-align: center;
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin: 20px 0;
        }}
        .features {{
            background-color: #e8f5e8;
            padding: 15px;
            border-radius: 5px;
            margin: 20px 0;
        }}
        .footer {{
            text-align: center;
            color: #666;
            font-size: 14px;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #eee;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>AKNA Identity</h1>
            <p>Hoş Geldiniz!</p>
        </div>
        
        <div>
            <div class='welcome-message'>
                <h2>Merhaba {userName}!</h2>
                <p>AKNA Identity ailesine katıldığınız için teşekkür ederiz.</p>
            </div>
            
            <p>Hesabınız başarıyla oluşturuldu ve email adresiniz doğrulandı. Artık tüm özelliklerimizden yararlanabilirsiniz.</p>
            
            <div class='features'>
                <h3>Neler Yapabilirsiniz:</h3>
                <ul>
                    <li>Güvenli kimlik doğrulama sistemi</li>
                    <li>Belgelerinizi dijital ortamda saklama</li>
                    <li>Araç ve şirket bilgilerinizi yönetme</li>
                    <li>Nakliye süreçlerinizi optimize etme</li>
                </ul>
            </div>
            
            <p>Herhangi bir sorunuz olursa, destek ekibimizle iletişime geçmekten çekinmeyin.</p>
            
            <p>İyi kullanımlar dileriz,<br>AKNA Identity Ekibi</p>
        </div>
        
        <div class='footer'>
            <p>Bu email otomatik olarak gönderilmiştir. Lütfen yanıtlamayın.</p>
            <p>&copy; 2025 AKNA Identity. Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}