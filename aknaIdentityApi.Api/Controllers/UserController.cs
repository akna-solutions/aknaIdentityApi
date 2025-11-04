using aknaIdentityApi.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aknaIdentityApi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found", success = false });
            }

            // Şifreyi response'dan çıkar
            user.PasswordHash = null;

            return Ok(new { data = user, success = true });
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found", success = false });
            }

            // Şifreyi response'dan çıkar
            user.PasswordHash = null;

            return Ok(new { data = user, success = true });
        }

        [HttpPost("{id}/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(long id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found", success = false });
            }

            await userRepository.UpdateEmailConfirmationStatusAsync(id);
            return Ok(new { message = "Email confirmed successfully", success = true });
        }

        [HttpPost("{id}/confirm-phone")]
        public async Task<IActionResult> ConfirmPhone(long id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found", success = false });
            }

            await userRepository.UpdatePhoneConfirmationStatusAsync(id, true);
            return Ok(new { message = "Phone confirmed successfully", success = true });
        }
    }
}
