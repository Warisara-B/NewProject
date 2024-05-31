using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plexus.Service.ViewModel;
using Plexus.Utility;
using IAuthorizationService = Plexus.Service.IAuthorizationService;

namespace Plexus.API.Controllers
{
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class AuthController : BaseController
	{
        private readonly IAuthorizationService _authService;

        public AuthController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult ServiceLogin([FromBody] AccessTokenRequestViewModel request)
        {
            var response = _authService.ServiceLogin(request.Username, request.Password);
            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, response));
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] AccessTokenRequestViewModel request)
        {
            var response = _authService.RefreshToken(request.RefreshToken);
            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created, response));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("generate")]
        public IActionResult GenerateInstructorAccount([FromBody] CreateAccountViewModel request)
        {
            var userInformation = GetRequesterInformation();
            _authService.CreateAccount(request, userInformation.Name);
            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created));
        }
    }
}

