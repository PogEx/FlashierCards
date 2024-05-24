namespace Backend.RestApi.Config;

public class JwtConfig
{
    public string Key { get; set; } = "";
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public int Lifetime { get; set; } = 15;
}