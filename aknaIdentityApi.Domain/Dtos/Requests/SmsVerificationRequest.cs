using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aknaIdentityApi.Domain.Dtos.Requests
{
    /// <summary>
    /// Sms doğrulama kodu gönderme isteği
    /// </summary>
    public class SmsVerificationRequest
    {
        /// <summary>
        /// Sms numarası (ülke kodu ile birlikte)
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public long UserId { get; set; }
    }
}
