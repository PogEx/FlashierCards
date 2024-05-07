using Backend.Common.Models.Auth;

namespace Backend.RestApi.Contracts.Auth;

public interface ITokenManager
{
    TokenLease GenerateTokenFor(Guid guid, string user);
}