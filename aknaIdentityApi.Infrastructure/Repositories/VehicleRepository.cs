using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace aknaIdentityApi.Infrastructure.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AknaIdentityDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vehicle>> GetByCompanyIdAsync(long companyId)
        {
            return await context.Vehicles
                .Where(v => v.CompanyId == companyId && !v.IsDeleted)
                .OrderBy(v => v.PlateNumber)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByPlateNumberAsync(string plateNumber)
        {
            return await context.Vehicles
                .FirstOrDefaultAsync(v => v.PlateNumber == plateNumber && !v.IsDeleted);
        }

        public async Task<IEnumerable<Vehicle>> GetByDriverIdAsync(long driverId)
        {
            return await context.Vehicles
                .Where(v => v.CurrentDriverId == driverId && !v.IsDeleted)
                .OrderBy(v => v.PlateNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
        {
            return await context.Vehicles
                .Where(v => v.Status == VehicleStatus.Active && !v.IsDeleted)
                .OrderBy(v => v.PlateNumber)
                .ToListAsync();
        }
    }
}
