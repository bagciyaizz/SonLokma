using SonLokma.Domain.Enums;

namespace SonLokma.Application.Handlers.Businesses.Commands.ApplyForBusiness;

public sealed record ApplyForBusinessCommand(
    Guid OwnerUserId,
    string Name,
    string Description,
    BusinessCategory Category,
    string Address,
    double Latitude,
    double Longitude);
