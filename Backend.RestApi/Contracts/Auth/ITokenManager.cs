namespace Backend.RestApi.Contracts.Auth;

public interface ITokenManager
{
    string Authenticate(string user, string password);
}