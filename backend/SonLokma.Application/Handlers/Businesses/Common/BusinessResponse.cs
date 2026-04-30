namespace SonLokma.Application.Handlers.Businesses.Common;

public sealed record BusinessResponse(
    Guid Id,
    Guid OwnerUserId,
    string OwnerFullName,
    string Name,
    string Description,
    string Category,
    string Address,
    double Latitude,
    double Longitude,
    string Status,
    DateTimeOffset CreatedAt);
