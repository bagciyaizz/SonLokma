using SonLokma.Domain.Enums;

namespace SonLokma.Application.Auth;

public sealed record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    string? Phone,
    UserRole Role = UserRole.Customer);
