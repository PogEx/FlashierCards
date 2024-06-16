namespace Backend.RestApi.Contracts.Auth;

public interface ITokenManager
{
    string GenerateTokenFor(Guid guid, string user);
}