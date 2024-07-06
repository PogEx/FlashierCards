using Backend.Common.Models.Users;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.RestApi.Controllers;

[Route("user")]
[Controller]
[Authorize]
public class UserController (IUserHandler userHandler) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, null, typeof(UserDto))]
    public async Task<IActionResult> Get()
    {
        return Ok((await userHandler.GetUser(User.GetCurrentUser())).Value);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeUserData([FromBody, BindRequired] UserChangeData data)
    {
        await userHandler.ChangeUser(User.GetCurrentUser(), data);
        return Ok();
    }
    
    
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(
        [FromBody, BindRequired] UserCreateData data)
    {
        if (string.IsNullOrEmpty(data.Name)
            || string.IsNullOrEmpty(data.Password)
            || !data.Password.Equals(data.PasswordConfirm))
            return BadRequest();
        
        Result<Guid> userGuidResult = await userHandler.CreateUser(data);
        if (userGuidResult.IsFailed) return BadRequest(userGuidResult.Errors);

        return Created("/", userGuidResult.Value);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody, BindRequired] UserLogin loginData)
    {
        if (string.IsNullOrEmpty(loginData.Username)
            || string.IsNullOrEmpty(loginData.Password))
            return BadRequest();
        
        Result<UserDto> userResult = await userHandler.GetUser(loginData.Username);
        
        if (userResult.IsFailed)
            return NotFound();
        
        Result<string> bearerResult = await userHandler.Login(userResult.Value.UserId, loginData.Password);
        
        if (bearerResult.IsFailed)
            return Unauthorized();
        
        return Ok(bearerResult.Value);
    }
}