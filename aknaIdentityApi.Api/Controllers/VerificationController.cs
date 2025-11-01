using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace aknaIdentityApi.Api.Controllers
{
    /// <summary>
    /// Email ve telefon doğrulama işlemleri controller'ı
    /// </summary>
    [ApiController]
    [Route("api/verifications")]
    public class VerificationController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly IVerificationService verificationService;
        private readonly ISmsService smsService;

        /// <summary>
        /// Constructor of verificationcontroller
        /// </summary>
        /// <param name="emailService"></param>
        /// <param name="verificationService"></param>
        /// <param name="smsService"></param>
        public VerificationController(
            IEmailService emailService,
            IVerificationService verificationService,
            ISmsService smsService)
        {
            this.emailService = emailService;
            this.verificationService = verificationService;
            this.smsService = smsService;
        }

        /// <summary>
        /// Email doğrulama kodu gönderir
        /// </summary>
        /// <param name="request">Email doğrulama isteği</param>
        /// <returns></returns>
        [HttpPost("send-email-verification")]
        public async Task<IActionResult> SendEmailVerificationAsync([FromBody] EmailVerificationRequest request)
        {
            await emailService.SendEmailVerificationCodeAsync(request.Email, request.UserId);

            return Ok(new
            {
                success = true,
                message = "Doğrulama kodu email adresinize gönderildi. Kod 5 dakika geçerlidir."
            });
        }

        /// <summary>
        /// Email doğrulama kodunu onaylar
        /// </summary>
        /// <param name="request">Email kod onaylama isteği</param>
        /// <returns></returns>
        [HttpPost("verify-email-code")]
        public async Task<IActionResult> VerifyEmailCodeAsync([FromBody] VerifyEmailCodeRequest request)
        {
            var isValid = await verificationService.VerifyEmailCodeAsync(request);

            if (isValid)
            {
                // Email doğrulandı olarak işaretle
                await verificationService.UpdateEmailConfirmationStatusAsync(request.UserId);

                return Ok(new
                {
                    success = true,
                    message = "Email adresiniz başarıyla doğrulandı."
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Geçersiz veya süresi dolmuş doğrulama kodu."
                });
            }

        }

        /// <summary>
        /// Yeni doğrulama kodu talep eder
        /// </summary>
        /// <param name="request">Email doğrulama isteği</param>
        /// <returns></returns>
        [HttpPost("resend-email-verification")]
        public async Task<IActionResult> ResendEmailVerificationAsync([FromBody] EmailVerificationRequest request)
        {
            await emailService.SendEmailVerificationCodeAsync(request.Email, request.UserId);

            return Ok(new
            {
                success = true,
                message = "Yeni doğrulama kodu email adresinize gönderildi."
            });
        }


        /// <summary>
        /// Sms doğrulama kodu gönderir
        /// </summary>
        /// <param name="request">Sms doğrulama isteği</param>
        /// <returns></returns>
        [HttpPost("send-sms-verification")]
        public async Task<IActionResult> SendSmsVerificationAsync([FromBody] SmsVerificationRequest request)
        {
            try
            {
                await smsService.SendSmsVerificationCodeAsync(request.PhoneNumber, request.UserId);
                return Ok(new
                {
                    success = true,
                    message = "Doğrulama kodu Sms'ınıza gönderildi. Kod 5 dakika geçerlidir."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Sms doğrulama kodunu onaylar
        /// </summary>
        /// <param name="request">Sms kod onaylama isteği</param>
        /// <returns></returns>
        [HttpPost("verify-sms-code")]
        public async Task<IActionResult> VerifySmsCodeAsync([FromBody] VerifySmsCodeRequest request)
        {
            try
            {
                var isValid = await verificationService.VerifySmsCodeAsync(request);
                if (isValid)
                {
                    // Sms doğrulandı olarak işaretle
                    await verificationService.UpdateSmsConfirmationStatusAsync(request.UserId);
                    return Ok(new
                    {
                        success = true,
                        message = "Sms numaranız başarıyla doğrulandı."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Geçersiz veya süresi dolmuş doğrulama kodu."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        /// <summary>
        /// Yeni Sms doğrulama kodu talep eder
        /// </summary>
        /// <param name="request">Sms doğrulama isteği</param>
        /// <returns></returns>
        [HttpPost("resend-sms-verification")]
        public async Task<IActionResult> ResendSmsVerificationAsync([FromBody] SmsVerificationRequest request)
        {
            try
            {
                await smsService.SendSmsVerificationCodeAsync(request.PhoneNumber, request.UserId);
                return Ok(new
                {
                    success = true,
                    message = "Yeni doğrulama kodu Sms'ınıza gönderildi."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}