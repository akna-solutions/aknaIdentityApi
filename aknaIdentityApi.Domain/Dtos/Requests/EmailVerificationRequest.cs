
namespace aknaIdentityApi.Domain.Dtos.Requests
{
    public class EmailVerificationRequest
    {
        public string Email { get; set; }
        public long UserId { get; set; }
    }
}
