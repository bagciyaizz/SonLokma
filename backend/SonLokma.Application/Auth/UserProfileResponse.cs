namespace SonLokma.Application.Auth;

public sealed record UserProfileResponse(
    Guid Id,
    string FullName,
    string Email,
    string? Phone,
    string Role);
