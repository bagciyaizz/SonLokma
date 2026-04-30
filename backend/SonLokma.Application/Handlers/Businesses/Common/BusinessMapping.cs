using SonLokma.Domain.Entities;

namespace SonLokma.Application.Handlers.Businesses.Common;

public static class BusinessMapping
{
    public static BusinessResponse ToResponse(Business business)
    {
        return new BusinessResponse(
            business.Id,
            business.OwnerUserId,
            business.OwnerUser.FullName,
            business.Name,
            business.Description,
            business.Category.ToString(),
            business.Address,
            business.Location.Y,
            business.Location.X,
            business.Status.ToString(),
            business.CreatedAt);
    }
}
