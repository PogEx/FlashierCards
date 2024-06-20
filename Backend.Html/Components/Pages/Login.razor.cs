using Backend.Common.Models.Users;
using Backend.Html.Services;
using Backend.Html.Services.Contracts;
using Microsoft.AspNetCore.Components;
using RestSharp;

namespace Backend.Html.Components.Pages;

public partial class Login
{
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private IRestClientProvider RestClientProvider { get; set; }
    
    [Inject] private ICookie _cookie { get; set; }

    private UserLogin _loginData = new();

    private async Task LoginUser()
    {
        RestRequest request = new("/user/login", Method.Post);
        request.AddBody(_loginData, ContentType.Json);

        IRestClient client = await RestClientProvider.GetRestClient();
        RestResponse response = await client.PostAsync(request);
        
        if(response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            await _cookie.SetValue("token", response.Content, 1);
    }
}