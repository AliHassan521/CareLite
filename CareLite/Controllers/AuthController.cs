using CareLite.Models.DTO;
using CareLite.Services.Interfaces;
using CareLite.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CareLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            var (user, correlationId, error) = await _authService.SignInAsync(request);
            if (user == null)
            {
                return Unauthorized(new { message = error, correlationId });
            }
            // TODO: Generate session/token and return
            return Ok(new { message = "Sign-in successful.", user = user, correlationId });
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromQuery] int? userId = null)
        {
            // TODO: Implement session/token invalidation
            var correlationId = await _authService.SignOutAsync(userId);
            return Ok(new { message = "Sign-out successful.", correlationId });
        }
    }
}
