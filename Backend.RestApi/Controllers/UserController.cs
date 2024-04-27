using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApiBackend.Controllers;

[Route("user")]
[Controller]
public class UserController: Controller
{
    
    /// <summary>
    /// This Creates a user
    /// </summary>
    /// <example>1234</example>>
    /// <returns></returns>
    [HttpPost("create")]
    public IActionResult CreateUser()
    {
        return Created();
    }

    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok();
    }
}