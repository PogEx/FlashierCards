namespace Backend.Restart.Services;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Validate user credentials (this example uses a simple check for demonstration purposes)
        if (request.Username == "user" && request.Password == "password")
        {
            var token = _jwtTokenService.GenerateToken("user_id");
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }
}