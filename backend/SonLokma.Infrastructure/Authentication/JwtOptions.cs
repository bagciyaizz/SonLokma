namespace SonLokma.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "SonLokma";
    public string Audience { get; init; } = "SonLokma.Web";
    public string Secret { get; init; } = string.Empty;
    public int AccessTokenMinutes { get; init; } = 60;
}
