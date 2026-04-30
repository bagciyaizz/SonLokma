using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SonLokma.Application.Common.Interfaces;
using SonLokma.Application.Handlers.Businesses.Common;
using SonLokma.Domain.Enums;

namespace SonLokma.Application.Handlers.Businesses.Commands.UpdateBusiness;

public sealed class UpdateBusinessCommandHandler(ISonLokmaDbContext dbContext)
{
    public async Task<BusinessResponse> Handle(UpdateBusinessCommand command, CancellationToken cancellationToken)
    {
        var business = await dbContext.Businesses
            .Include(item => item.OwnerUser)
            .FirstOrDefaultAsync(item => item.OwnerUserId == command.OwnerUserId, cancellationToken);

        if (business is null)
        {
            throw new InvalidOperationException("Isletme bulunamadi.");
        }

        business.Name = command.Name.Trim();
        business.Description = command.Description.Trim();
        business.Category = command.Category;
        business.Address = command.Address.Trim();
        business.Location = new Point(command.Longitude, command.Latitude) { SRID = 4326 };

        if (business.Status == BusinessStatus.Rejected)
        {
            business.Status = BusinessStatus.Pending;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return BusinessMapping.ToResponse(business);
    }
}
