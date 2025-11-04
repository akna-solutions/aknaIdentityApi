using aknaIdentityApi.Domain.Entities;

namespace aknaIdentityApi.Domain.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task<Vehicle?> GetVehicleByIdAsync(long id);
        Task<IEnumerable<Vehicle>> GetVehiclesByCompanyIdAsync(long companyId);
        Task<IEnumerable<Vehicle>> GetVehiclesByDriverIdAsync(long driverId);
        Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
        Task<Vehicle?> GetVehicleByPlateNumberAsync(string plateNumber);
        Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(long id);
        Task<bool> AssignDriverAsync(long vehicleId, long driverId);
        Task<bool> UnassignDriverAsync(long vehicleId);
    }
}
