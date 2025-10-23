
using aknaIdentityApi.Domain.Dtos.Requests;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    /// <summary>
    /// IDeviceInfoRepository
    /// </summary>
    public interface IDeviceInfoRepository
    {
        /// <summary>
        /// Kullanıcı cihaz kaydını sağlar
        /// </summary>
        /// <param name="request">UserRegisterRequest</param>
        /// <returns></returns>
        Task AddDeviceInfoAsync(UserRegisterRequest request);
    }
}
