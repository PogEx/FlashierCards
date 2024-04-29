using Backend.RestApi.Contracts.Auth;

namespace Backend.RestApi.Authentication;

public class JwtTokenManager: ITokenManager
{
    public string Authenticate(string user, string password)
    {
        //create bearer token from user, password
        return "";
    }
}