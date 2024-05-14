using Backend.Common.Models;
using Backend.Common.Models.Auth;

namespace Backend.RestApi.Contracts.Auth;

public interface IUserHandler
{
    Guid? CreateUser(string name, string password);
    TokenLease? Login(Guid user, string password);
    void Logout(Guid user);
    User? GetUser(string name);
    User? GetUser(Guid guid);
}