using System.Web.Helpers;
using Backend.Common.Models;
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
        User user = new(){UserId = userGuid, Name = name, PasswordHash = hashedPassword, Salt = salt};

        await using (FlashiercardsContext context = createContext())
        {
            if (await context.Users.AnyAsync(u => u.Name == name))
                return null;
            
            await context.Users.AddAsync(user);
            await context.UserSettings.AddAsync(new UserSetting { IsDark = true, UserId = userGuid });

            await context.SaveChangesAsync();
        }
        
        return userGuid;
    }

    public async Task<string?> Login(Guid user, string password)
    {
        await using FlashiercardsContext context = createContext();
        
        User? result;
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
        
        return await context.Users.SingleAsync(b => b.Name == name);
    }

    public async Task<User?> GetUser(Guid guid)
    {
        await using FlashiercardsContext context = createContext();
        
        return await context.Users.SingleAsync(b => b.UserId == guid);
    }
}