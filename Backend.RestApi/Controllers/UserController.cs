using Backend.Common.Models.Auth;
using Backend.RestApi.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.RestApi.Controllers;

[Route("user")]
[Controller]
public class UserController (IUserHandler userHandler) : Controller
{
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateUser(
        [FromForm, BindRequired] string name,
        [FromForm, BindRequired, SwaggerSchema(Format = "password")]
        string password,
        [FromForm, BindRequired, SwaggerSchema(Format = "password")]
        string passwordEnsure)
    {
        if (string.IsNullOrEmpty(name)
            || string.IsNullOrEmpty(password)
            || !password.Equals(passwordEnsure))
            return BadRequest();
        
        Guid? guid = userHandler.CreateUser(name, password);

        return guid is null ? Problem() : Created("", guid);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login(
        [FromForm, BindRequired] string name,
        [FromForm, BindRequired, SwaggerSchema(Format = "password")]
        string password
        )
    {
        if (string.IsNullOrEmpty(name)
            || string.IsNullOrEmpty(password))
            return BadRequest();
        
        Guid? userGuid = userHandler.GetUser(name);
        
        if (userGuid is null)
            return NotFound();
        
        string? bearer = userHandler.Login(userGuid.Value, password);
        
        if (bearer is null)
            return Unauthorized();
        
        return Ok(new TokenLease(
            bearer,
            DateTime.Now.AddHours(24)));
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Get(
        [FromQuery] Guid id
    )
    {
        userHandler.GetName(id);
        return NotFound();
    }
}