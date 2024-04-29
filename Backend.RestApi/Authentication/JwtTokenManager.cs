using RestApiBackend.Contracts.Auth;

namespace RestApiBackend.Authentication;

public class JwtTokenManager: ITokenManager
{
    public string Authenticate(string user, string password)
    {
        //create bearer token from user, password
        return "";
    }
}