using System.Security.Claims;
using Backend.Common.Models.User;
using Backend.RestApi.Contracts.Auth;
using Backend.RestApi.Contracts.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.RestApi.Controllers;

[Route("user")]
[Controller]
[Authorize]
public class UserController (IUserHandler userHandler, IFolderHandler folderHandler) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, null, typeof(User))]
    public async Task<IActionResult> Get()
    {
        return Ok(await userHandler.GetUser(new Guid(
                User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value)));
    }

    [HttpPatch]
    public IActionResult ChangeUserData()
    {
        return Ok();
    }
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(
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
        
        Guid? guid = await userHandler.CreateUser(name, password);
        if (guid is null)
        {
            return Problem();
        }
        await folderHandler.CreateUserRoot(guid.Value);

        return Created("", guid);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromForm, BindRequired] string name,
        [FromForm, BindRequired, SwaggerSchema(Format = "password")]
        string password
        )
    {
        if (string.IsNullOrEmpty(name)
            || string.IsNullOrEmpty(password))
            return BadRequest();
        
        User? user = await userHandler.GetUser(name);
        
        if (user is null)
            return NotFound();
        
        string? bearer = await userHandler.Login(user.Value.UserId, password);
        
        if (bearer is null)
            return Unauthorized();
        
        return Ok(bearer);
    }
}