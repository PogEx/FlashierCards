using System.Security.Claims;
using Backend.Common.Models.Users;
using Backend.RestApi.Contracts.Content;
using FluentResults;
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
    [SwaggerResponse(200, null, typeof(UserDto))]
    public async Task<IActionResult> Get()
    {
        return Ok((await userHandler.GetUser(new Guid(
                User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))).Value);
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
        
        Result<Guid> userGuidResult = await userHandler.CreateUser(name, password);
        if (userGuidResult.HasError(error => error.Metadata["ErrorCode"].Equals("409")))
        {
            return Conflict($"A user with the name {name} already exists");
        }
        
        Result<Guid> folderGuidResult = await folderHandler.CreateUserRoot(userGuidResult.Value);
        if (folderGuidResult.HasError(error => error.Metadata["ErrorCode"].Equals("")))
        {
            
        }
        return Created("", userGuidResult.Value);
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
        
        Result<UserDto> userResult = await userHandler.GetUser(name);
        userResult.Log();
        
        if (userResult.IsFailed)
            return NotFound();
        
        Result<string> bearerResult = await userHandler.Login(userResult.Value.UserId, password);
        
        if (bearerResult.IsFailed)
            return Unauthorized();
        
        return Ok(bearerResult.Value);
    }
}