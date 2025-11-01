using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Dtos.Responses;
using aknaIdentityApi.Domain.Interfaces.Services;
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

    }
}
