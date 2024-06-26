using Backend.Common.Models.Users;

namespace Backend.Html.Services.Contracts;

public interface IAuthorizationHandler
{
    Task<bool> Login(UserLogin login);
    Task<string> GetToken();
}