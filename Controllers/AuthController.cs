using Microsoft.AspNetCore.Mvc;
using TaskManager_api.Models;
using TaskManager_api.Services;

namespace TaskManager_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService) =>
            _authService = authService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var result = _authService.Authenticate(loginDto);

            if (result == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(result);
        }
    }
}
