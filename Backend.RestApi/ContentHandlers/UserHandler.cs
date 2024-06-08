using System.Web.Helpers;
using Backend.Common.Models.Users;
using Backend.Database.Database.Context;
using Backend.Database.Database.DatabaseModels;
using Backend.RestApi.Contracts.Auth;
using Backend.RestApi.Contracts.Content;
using Backend.RestApi.Helpers;
using Backend.RestApi.Logging.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.ContentHandlers;

public class UserHandler(ITokenManager tokenManager, FlashiercardsContext context) : IUserHandler
{
    public async Task<Result<Guid>> CreateUser(string name, string password)
    {
        try
        {
            string salt = Crypto.GenerateSalt();
            string hashedPassword = Crypto.HashPassword(salt + password);

            Guid userGuid = Guid.NewGuid();
            User user = new() { UserId = userGuid, Name = name, PasswordHash = hashedPassword, Salt = salt };

            if (await context.Users.AnyAsync(u => u.Name == name))
                return Result.Fail(new DuplicateItemError(name));

            await context.Users.AddAsync(user);
            await context.UserSettings.AddAsync(new UserSetting { IsDark = true, UserId = userGuid });

            await context.SaveChangesAsync();

            return userGuid;
        }
        catch (DbUpdateException e)
        {
            return new DatabaseError(e);
        }
    }

    public async Task<Result<string>> Login(Guid user, string password)
    {

        User result = await context.Users.SingleAsync(b => b.UserId == user);
        if (!Crypto.VerifyHashedPassword(result.PasswordHash, result.Salt + password)) 
            return Result.Fail(new CredentialError());
        
        return tokenManager.GenerateTokenFor(result.UserId, result.Name);
    }

    public async Task<Result> Logout(Guid user)
    {
        //TODO
        //invalidate current token for user
        return Result.Ok();
    }

    public async Task<Result<UserDto>> GetUser(string name)
    {
        try
        {
            User user = await context.Users.SingleAsync(b => b.Name == name);
            return user.ToDto();
        }
        catch (InvalidOperationException)
        {
            return Result.Fail(new NotFoundError(name));
        }
    }

    public async Task<Result<UserDto>> GetUser(Guid guid)
    {
        try
        {
            return (await context.Users.SingleAsync(b => b.UserId == guid)).ToDto();
        }
        catch (InvalidOperationException)
        {
            return Result.Fail(new NotFoundError(guid.ToString()));
        }
    }
}