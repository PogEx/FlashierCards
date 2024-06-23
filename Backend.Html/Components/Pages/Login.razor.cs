using Backend.Common.Models.Users;
using Backend.Html.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace Backend.Html.Components.Pages;

public partial class Login
{
    [Inject] private NavigationManager NavManager { get; set; }
    
    [Inject] private IAuthorizationHandler AuthorizationHandler { get; set; }

    private UserLogin _loginData = new();

    private async Task LoginUser()
    {
        await AuthorizationHandler.Login(_loginData);
    }
}