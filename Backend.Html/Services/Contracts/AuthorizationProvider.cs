using Backend.Common.Models.Users;
using Microsoft.AspNetCore.Components;
using RestSharp;

namespace Backend.Html.Services.Contracts;

public class AuthorizationProvider(IRestClientProvider restClientProvider, NavigationManager navigationManager) : IAuthorizationHandler
{
    private string Bearer { get; set; } = "";
    public async Task<bool> Login(UserLogin loginData)
    {
        RestRequest request = new("/user/login", Method.Post);
        request.AddBody(loginData, ContentType.Json);

        IRestClient client = await restClientProvider.GetRestClient();
        RestResponse response = await client.PostAsync(request);
        Bearer = response.Content ?? "";
        return response.IsSuccessful;
    }

    public Task<string> GetToken()
    {
        if(string.IsNullOrEmpty(Bearer))
            navigationManager.NavigateTo("/login");

        return Task.FromResult(Bearer);
    }
}