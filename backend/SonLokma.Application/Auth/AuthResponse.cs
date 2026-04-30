namespace SonLokma.Application.Auth;

public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    UserProfileResponse User);
