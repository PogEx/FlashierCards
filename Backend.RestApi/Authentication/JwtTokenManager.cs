using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Common.Models.Auth;
using Backend.RestApi.Contracts.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Backend.RestApi.Authentication;

public class JwtTokenManager(IConfiguration configuration): ITokenManager
{
    public TokenLease GenerateTokenFor(Guid guid, string name)
    {
        SymmetricSecurityKey securityKey = new (Encoding.UTF8.GetBytes(configuration.GetSection("Jwt")["Key"] ?? throw new InvalidOperationException()));
        SigningCredentials credentials = new (securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        {
            new(ClaimTypes.Name, name),
            new(ClaimTypes.NameIdentifier, guid.ToString()),
            new(ClaimTypes.Expired, "false")
        };
        JwtSecurityToken token = new(
            issuer: configuration.GetSection("Jwt")["Issuer"],
            audience:configuration.GetSection("Jwt")["Audience"], 
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
            );
        return new TokenLease(new JwtSecurityTokenHandler().WriteToken(token), DateTime.Now.AddMinutes(15));
    }
}