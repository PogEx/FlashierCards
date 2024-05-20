using Backend.Common.Models;

namespace Backend.RestApi.Contracts.Auth;

public interface IUserHandler
{
    Task<Guid?> CreateUser(string name, string password);
    Task<string?> Login(Guid user, string password);
    Task Logout(Guid user);
    Task<User?> GetUser(string name);
    Task<User?> GetUser(Guid guid);
}