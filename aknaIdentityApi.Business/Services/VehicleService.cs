using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;
using aknaIdentityApi.Domain.Interfaces.UnitOfWorks;

namespace aknaIdentityApi.Business.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;

        public VehicleService(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            this.vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            // Aynı plaka ile başka araç var mı kontrol et
            var existingVehicle = await vehicleRepository.GetByPlateNumberAsync(vehicle.PlateNumber);
            if (existingVehicle != null)
            {
                throw new ArgumentException($"Vehicle with plate number {vehicle.PlateNumber} already exists");
            }

            vehicle.CreatedDate = DateTime.UtcNow;
            vehicle.UpdatedDate = DateTime.UtcNow;
            vehicle.IsDeleted = false;

            await vehicleRepository.AddAsync(vehicle);
            await unitOfWork.CommitAsync();

            return vehicle;
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(long id)
        {
            return await vehicleRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByCompanyIdAsync(long companyId)
        {
            return await vehicleRepository.GetByCompanyIdAsync(companyId);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByDriverIdAsync(long driverId)
        {
            return await vehicleRepository.GetByDriverIdAsync(driverId);
        }

        public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
        {
            return await vehicleRepository.GetActiveVehiclesAsync();
        }

        public async Task<Vehicle?> GetVehicleByPlateNumberAsync(string plateNumber)
        {
            return await vehicleRepository.GetByPlateNumberAsync(plateNumber);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            var existingVehicle = await vehicleRepository.GetByIdAsync(vehicle.Id);
            if (existingVehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicle.Id} not found");
            }

            vehicle.UpdatedDate = DateTime.UtcNow;
            await vehicleRepository.UpdateAsync(vehicle);
            await unitOfWork.CommitAsync();

            return vehicle;
        }

        public async Task<bool> DeleteVehicleAsync(long id)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                return false;
            }

            vehicle.IsDeleted = true;
            vehicle.UpdatedDate = DateTime.UtcNow;
            await vehicleRepository.UpdateAsync(vehicle);
            await unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> AssignDriverAsync(long vehicleId, long driverId)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return false;
            }

            vehicle.CurrentDriverId = driverId;
            vehicle.UpdatedDate = DateTime.UtcNow;
            await vehicleRepository.UpdateAsync(vehicle);
            await unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> UnassignDriverAsync(long vehicleId)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return false;
            }

            vehicle.CurrentDriverId = null;
            vehicle.UpdatedDate = DateTime.UtcNow;
            await vehicleRepository.UpdateAsync(vehicle);
            await unitOfWork.CommitAsync();

            return true;
        }
    }
}
