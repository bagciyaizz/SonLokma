using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SonLokma.Application.Common.Interfaces;
using SonLokma.Application.Handlers.Businesses.Common;
using SonLokma.Domain.Entities;
using SonLokma.Domain.Enums;

namespace SonLokma.Application.Handlers.Businesses.Commands.ApplyForBusiness;

public sealed class ApplyForBusinessCommandHandler(ISonLokmaDbContext dbContext)
{
    public async Task<BusinessResponse> Handle(ApplyForBusinessCommand command, CancellationToken cancellationToken)
    {
        var owner = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == command.OwnerUserId, cancellationToken);
        if (owner is null)
        {
            throw new InvalidOperationException("Kullanici bulunamadi.");
        }

        if (owner.Role != UserRole.Business && owner.Role != UserRole.Admin)
        {
            throw new InvalidOperationException("Isletme basvurusu icin isletme hesabi gerekir.");
        }

        var hasBusiness = await dbContext.Businesses.AnyAsync(
            business => business.OwnerUserId == command.OwnerUserId,
            cancellationToken);

        if (hasBusiness)
        {
            throw new InvalidOperationException("Bu hesap icin zaten bir isletme basvurusu var.");
        }

        var business = new Business
        {
            Id = Guid.NewGuid(),
            OwnerUserId = command.OwnerUserId,
            OwnerUser = owner,
            Name = command.Name.Trim(),
            Description = command.Description.Trim(),
            Category = command.Category,
            Address = command.Address.Trim(),
            Location = new Point(command.Longitude, command.Latitude) { SRID = 4326 },
            Status = BusinessStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };

        dbContext.Businesses.Add(business);
        await dbContext.SaveChangesAsync(cancellationToken);

        return BusinessMapping.ToResponse(business);
    }
}
