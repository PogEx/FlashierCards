using System.Security.Claims;

namespace Backend.RestApi.Helpers.Extensions;

public static class ClaimExtensions
{
    public static Guid GetCurrentUser(this ClaimsPrincipal claimsPrincipal)
    {
        return new Guid(claimsPrincipal.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}