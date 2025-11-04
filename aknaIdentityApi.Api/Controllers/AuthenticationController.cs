using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Dtos.Responses;
using aknaIdentityApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aknaIdentityApi.Api.Controllers
{
    [ApiController]
    [Route("api/authentications")]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("add-user")]
        public async Task<UserRegisterResponse> AddUserAsync([FromBody] UserRegisterRequest request) 
        {
            return await authenticationService.RegisterAsync(request);
        }

        [HttpPost("login")]
        public async Task<UserLoginResponse> LoginAsync([FromBody] UserLoginRequest request)
        {
            return await authenticationService.LoginAsync(request);
        }


        [HttpPost("add-company")]
        public async Task<long> AddCompanyAsync([FromBody] CompanyRegisterRequest request)
        {
            return await authenticationService.AddCompanyAsync(request);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            var newAccessToken = await authenticationService.RefreshTokenAsync(request.RefreshToken);
            return Ok(new { accessToken = newAccessToken, success = true });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest request)
        {
            await authenticationService.LogoutAsync(request.UserId, request.DeviceId);
            return Ok(new { message = "Logged out successfully", success = true });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            var result = await authenticationService.ChangePasswordAsync(request);
            if (result)
            {
                return Ok(new { message = "Password changed successfully", success = true });
            }
            return BadRequest(new { message = "Password change failed", success = false });
        }

    }
}
