using Backend.Common.Models.Users;

namespace Backend.Html.Services.Contracts;

public interface IAuthorizationHandler
{
    Task Login(UserLogin login);
}