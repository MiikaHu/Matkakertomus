using Matkakertomus.Server.Services;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Matkakertomus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto request)
        {
            bool registered = await _authService.Register(request);

            if (!registered)
            {
                return BadRequest("Email on jo käytössä");
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            string? token = await _authService.Login(request);

            if (token is null)
            {
                return BadRequest("Email tai salasana on väärin");
            }

            return Ok(token);
        }
    }

}