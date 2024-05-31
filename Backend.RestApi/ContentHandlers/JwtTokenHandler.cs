using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.RestApi.Config;
using Backend.RestApi.Contracts.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Backend.RestApi.ContentHandlers;

public class JwtTokenHandler(IConfiguration configuration) : ITokenManager
{
    private readonly JwtConfig _config = configuration.GetSection("Jwt").Get<JwtConfig>() ?? new JwtConfig();

    public string GenerateTokenFor(Guid guid, string name)
    {
        SymmetricSecurityKey securityKey = new (Encoding.UTF8.GetBytes(_config.Key));
        SigningCredentials credentials = new (securityKey, SecurityAlgorithms.HmacSha256);
        
        Claim[] claims =
        [
            new (ClaimTypes.Name, name),
            new(ClaimTypes.NameIdentifier, guid.ToString()),
            new(ClaimTypes.Expired, "false")
        ];
        JwtSecurityToken token = new(
            issuer: _config.Issuer,
            audience: _config.Audience, 
            claims: claims,
            expires: DateTime.Now.AddMinutes(_config.Lifetime),
            signingCredentials: credentials
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}