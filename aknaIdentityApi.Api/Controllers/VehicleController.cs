using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aknaIdentityApi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                var createdVehicle = await vehicleService.CreateVehicleAsync(vehicle);
                return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicle.Id }, new { data = createdVehicle, success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message, success = false });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(long id)
        {
            var vehicle = await vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound(new { message = "Vehicle not found", success = false });
            }

            return Ok(new { data = vehicle, success = true });
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetVehiclesByCompanyId(long companyId)
        {
            var vehicles = await vehicleService.GetVehiclesByCompanyIdAsync(companyId);
            return Ok(new { data = vehicles, success = true });
        }

        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetVehiclesByDriverId(long driverId)
        {
            var vehicles = await vehicleService.GetVehiclesByDriverIdAsync(driverId);
            return Ok(new { data = vehicles, success = true });
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveVehicles()
        {
            var vehicles = await vehicleService.GetActiveVehiclesAsync();
            return Ok(new { data = vehicles, success = true });
        }

        [HttpGet("plate/{plateNumber}")]
        public async Task<IActionResult> GetVehicleByPlateNumber(string plateNumber)
        {
            var vehicle = await vehicleService.GetVehicleByPlateNumberAsync(plateNumber);
            if (vehicle == null)
            {
                return NotFound(new { message = "Vehicle not found", success = false });
            }

            return Ok(new { data = vehicle, success = true });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(long id, [FromBody] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest(new { message = "ID mismatch", success = false });
            }

            try
            {
                var updatedVehicle = await vehicleService.UpdateVehicleAsync(vehicle);
                return Ok(new { data = updatedVehicle, success = true });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, success = false });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(long id)
        {
            var result = await vehicleService.DeleteVehicleAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Vehicle not found", success = false });
            }

            return Ok(new { message = "Vehicle deleted successfully", success = true });
        }

        [HttpPost("{vehicleId}/assign-driver/{driverId}")]
        public async Task<IActionResult> AssignDriver(long vehicleId, long driverId)
        {
            var result = await vehicleService.AssignDriverAsync(vehicleId, driverId);
            if (!result)
            {
                return NotFound(new { message = "Vehicle not found", success = false });
            }

            return Ok(new { message = "Driver assigned successfully", success = true });
        }

        [HttpPost("{vehicleId}/unassign-driver")]
        public async Task<IActionResult> UnassignDriver(long vehicleId)
        {
            var result = await vehicleService.UnassignDriverAsync(vehicleId);
            if (!result)
            {
                return NotFound(new { message = "Vehicle not found", success = false });
            }

            return Ok(new { message = "Driver unassigned successfully", success = true });
        }
    }
}
