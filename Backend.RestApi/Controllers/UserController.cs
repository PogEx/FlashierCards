using System.Security.Claims;
using Backend.Common.Models.Users;
using Backend.RestApi.Contracts.Content;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    public async Task<IActionResult> CreateUser([FromBody] UserRegister registerData)
    {
        if (string.IsNullOrEmpty(registerData.Username)
            || string.IsNullOrEmpty(registerData.Password)
            || !registerData.Password.Equals(registerData.ConfirmPassword))
            return BadRequest();
        
        Result<Guid> userGuidResult = await userHandler.CreateUser(registerData.Username, registerData.Password);
        if (userGuidResult.HasError(error => error.Metadata["ErrorCode"].Equals("409")))
        {
            return Conflict($"A user with the name {registerData.Username} already exists");
        }
        
        Result<Guid> folderGuidResult = await folderHandler.CreateUserRoot(userGuidResult.Value);
        if (folderGuidResult.HasError(error => error.Metadata["ErrorCode"].Equals("")))
        {
            
        }
        
        Result<string> bearerResult = await userHandler.Login(userGuidResult.Value, registerData.Password);
        
        if (bearerResult.IsFailed)
            return Unauthorized();
        
        return Created("/home", bearerResult.Value);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] UserLogin loginData)
    {
        if (string.IsNullOrEmpty(loginData.Username)
            || string.IsNullOrEmpty(loginData.Password))
            return BadRequest();
        
        Result<UserDto> userResult = await userHandler.GetUser(loginData.Username);
        userResult.Log();
        
        if (userResult.IsFailed)
            return NotFound();
        
        Result<string> bearerResult = await userHandler.Login(userResult.Value.UserId, loginData.Password);
        
        if (bearerResult.IsFailed)
            return Unauthorized();
        
        return Ok(bearerResult.Value);
    }
}