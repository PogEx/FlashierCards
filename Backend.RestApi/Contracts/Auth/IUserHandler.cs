using Backend.Common.Models.User;

namespace Backend.RestApi.Contracts.Auth;

public interface IUserHandler
{
    Guid? CreateUser(string name, string password);
    string? Login(Guid user, string password);
    void Logout(Guid user);
    User? GetUser(string name);
    User? GetUser(Guid guid);
}