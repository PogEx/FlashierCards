using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Restart.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.Restart.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

public class JwtTokenService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, IOptions<JwtSettings> jwtSettings)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _jwtSettings = jwtSettings.Value;
    }
    public string GenerateToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> Login(string username, string password)
    {
        var loginRequest = new LoginRequest { Username = username, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        var token = tokenResponse.Token;

        ((JwtAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token);
        return true;
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class TokenResponse
{
    public string Token { get; set; }
}

public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int DurationInMinutes { get; set; }
}