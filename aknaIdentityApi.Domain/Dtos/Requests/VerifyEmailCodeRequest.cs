
namespace aknaIdentityApi.Domain.Dtos.Requests
{
    public class VerifyEmailCodeRequest
    {
        public string VerificationCode { get; set; }
        public long UserId { get; set; }
        
    }
}
