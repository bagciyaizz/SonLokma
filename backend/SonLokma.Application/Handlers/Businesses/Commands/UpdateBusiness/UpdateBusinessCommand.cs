using SonLokma.Domain.Enums;

namespace SonLokma.Application.Handlers.Businesses.Commands.UpdateBusiness;

public sealed record UpdateBusinessCommand(
    Guid OwnerUserId,
    string Name,
    string Description,
    BusinessCategory Category,
    string Address,
    double Latitude,
    double Longitude);
