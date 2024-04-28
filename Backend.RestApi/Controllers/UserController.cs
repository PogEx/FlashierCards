using System.Web.Helpers;
using Backend.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RestApiBackend.Controllers;

[Route("user")]
[Controller]
public class UserController: Controller
{
    [HttpPost("create")]
    [Consumes("application/x-www-form-urlencoded")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateUser(
        [FromForm] string name, 
        [SwaggerSchema(Format = "password")][FromForm] string password, 
        [SwaggerSchema(Format = "password")][FromForm] string passwordEnsure)
    {
        if (string.IsNullOrEmpty(name) 
            || string.IsNullOrEmpty(password) 
            || !password.Equals(passwordEnsure))
            return BadRequest();
        
        string salt = Crypto.GenerateSalt();
        string hashedPassword = Crypto.HashPassword(salt + password);

        UserModel user = new (Guid.NewGuid(), name, hashedPassword, salt);
        
        // Save User object to Database
        return Created("", user);
    }

    
    [HttpPost("login")]
    [Consumes("application/x-www-form-urlencoded")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login(
        [FromForm] string name, 
        [SwaggerSchema(Format = "password")][FromForm] string password)
    {
        //Get Entry from database
        //prepend salt to pw
        //Hash Password
        //Compare Password Hash
        //Return Forbidden on fail
        //Return Ok and generate Bearer token
        
        return Ok();
    }
}