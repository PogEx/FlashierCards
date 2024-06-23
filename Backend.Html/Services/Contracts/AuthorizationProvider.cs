using Backend.Common.Models.Users;
using RestSharp;

namespace Backend.Html.Services.Contracts;

public class AuthorizationProvider(IRestClientProvider RestClientProvider, ICookie cookie) : IAuthorizationHandler
{
    public async Task Login(UserLogin loginData)
    {
        RestRequest request = new("/user/login", Method.Post);
        request.AddBody(loginData, ContentType.Json);

        IRestClient client = await RestClientProvider.GetRestClient();
        RestResponse response = await client.PostAsync(request);
        
        if(response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            await cookie.SetValue("token", response.Content, 1);
        
    }
}