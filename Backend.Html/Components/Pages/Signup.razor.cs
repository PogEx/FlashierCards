using Backend.Common.Models.Users;
using Backend.Html.Services.Contracts;
using Microsoft.AspNetCore.Components;
using RestSharp;

namespace Backend.Html.Components.Pages;

public partial class Signup
{
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private IRestClientProvider RestClientProvider { get; set; }

    private UserRegister _registerData = new();

    private async Task RegisterUser()
    {
        RestRequest request = new("/user", Method.Post);
        request.AddBody(_registerData, ContentType.Json);

        IRestClient client = await RestClientProvider.GetRestClient();
        RestResponse response = await client.PostAsync(request);
    }
}