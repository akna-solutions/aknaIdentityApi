using aknaIdentityApi.Domain.Entities;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        Task<IEnumerable<Vehicle>> GetByCompanyIdAsync(long companyId);
        Task<Vehicle?> GetByPlateNumberAsync(string plateNumber);
        Task<IEnumerable<Vehicle>> GetByDriverIdAsync(long driverId);
        Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
    }
}
