namespace aknaIdentityApi.Domain.Dtos.Requests
{
    /// <summary>
    /// Sms doğrulama kodu onaylama isteği
    /// </summary>
    public class VerifySmsCodeRequest
    {
        /// <summary>
        /// 6 haneli doğrulama kodu
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public long UserId { get; set; }
    }
}
