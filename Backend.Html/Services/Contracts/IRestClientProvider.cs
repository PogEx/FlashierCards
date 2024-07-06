using RestSharp;

namespace Backend.Html.Services.Contracts;

public interface IRestClientProvider
{
    Task<IRestClient> GetRestClient();
}