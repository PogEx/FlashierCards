using Backend.Html.Services.Contracts;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Backend.Html.Services;

public class RestClientProvider(IConfiguration configuration) : IRestClientProvider
{
    
    
    public Task<IRestClient> GetRestClient()
    {
        string baseUri = configuration["BackendLocation"] ?? "";
        
        RestClientOptions options = new (baseUri)
        {
            Timeout = TimeSpan.FromSeconds(20),
            //Authenticator = new JwtAuthenticator(""),
            PreAuthenticate = true
        };

        JsonSerializerSettings settings = new();
        
        
        IRestClient client = new RestClient(options, _ => {},config => config.UseNewtonsoftJson(settings));
        client.AddDefaultHeader("clientVersion", "1.0.0");
        
        return Task.FromResult(client);
    }
}