using System.Web.Helpers;
using Backend.Common.Models.DatabaseModels;
using Backend.Common.Models.User;
using Backend.RestApi.Contracts.Auth;
using Backend.RestApi.Database;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.Authentication;

public class UserHandler(ITokenManager tokenManager, Func<FlashiercardsContext> createContext) : IUserHandler
{
    public async Task<Guid?> CreateUser(string name, string password)
    {
        string salt = Crypto.GenerateSalt();
        string hashedPassword = Crypto.HashPassword(salt + password);
        
        Guid userGuid = Guid.NewGuid();
        DbUser dbUser = new(){UserId = userGuid, Name = name, PasswordHash = hashedPassword, Salt = salt};

        await using (FlashiercardsContext context = createContext())
        {
            if (await context.Users.AnyAsync(u => u.Name == name))
                return null;
            
            await context.Users.AddAsync(dbUser);
            await context.UserSettings.AddAsync(new DbUserSetting { IsDark = true, UserId = userGuid });

            await context.SaveChangesAsync();
        }
        
        return userGuid;
    }

    public async Task<string?> Login(Guid user, string password)
    {
        await using FlashiercardsContext context = createContext();
        
        DbUser? result;
        result = await context.Users.SingleAsync(b => b.UserId == user);
        if (!Crypto.VerifyHashedPassword(result.PasswordHash, result.Salt + password)) 
            return null;
        
        return tokenManager.GenerateTokenFor(result.UserId, result.Name);
    }

    public async Task Logout(Guid user)
    {
        //TODO
        //invalidate current token for user
    }

    public async Task<User?> GetUser(string name)
    {
        await using FlashiercardsContext context = createContext();
        
        return new User(await context.Users.SingleAsync(b => b.Name == name));
    }

    public async Task<User?> GetUser(Guid guid)
    {
        await using FlashiercardsContext context = createContext();
        
        return new User(await context.Users.SingleAsync(b => b.UserId == guid));
    }
}