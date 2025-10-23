using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;


namespace aknaIdentityApi.Infrastructure.Repositories
{
    public class DeviceInfoRepository : BaseRepository<User>, IDeviceInfoRepository
    {
        public DeviceInfoRepository(AknaIdentityDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Kullanıcı cihaz kaydını sağlar
        /// </summary>
        /// <param name="request">UserRegisterRequest</param>
        /// <returns></returns>
        public async Task AddDeviceInfoAsync(UserRegisterRequest request)
        {
            DeviceInfo device = new DeviceInfo
            {
                DeviceId = request.DeviceId,
                DeviceType = request.DeviceType,
                DeviceModel = request.DeviceModel,
                IPAddress = request.IPAddress,
                LastLogin = DateTime.UtcNow,
                PushToken = request.PushToken,
                PushTokenUpdatedAt = DateTime.UtcNow,
                UserId = request.UserId ?? 0,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system",
            };
            await context.DeviceInfos.AddAsync(device);
            await context.SaveChangesAsync();
        }
    }
}
