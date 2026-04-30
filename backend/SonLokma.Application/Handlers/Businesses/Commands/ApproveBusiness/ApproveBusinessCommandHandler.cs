using Microsoft.EntityFrameworkCore;
using SonLokma.Application.Common.Interfaces;
using SonLokma.Application.Handlers.Businesses.Common;
using SonLokma.Domain.Enums;

namespace SonLokma.Application.Handlers.Businesses.Commands.ApproveBusiness;

public sealed class ApproveBusinessCommandHandler(ISonLokmaDbContext dbContext)
{
    public async Task<BusinessResponse> Handle(ApproveBusinessCommand command, CancellationToken cancellationToken)
    {
        var business = await dbContext.Businesses
            .Include(item => item.OwnerUser)
            .FirstOrDefaultAsync(item => item.Id == command.BusinessId, cancellationToken);

        if (business is null)
        {
            throw new InvalidOperationException("Isletme bulunamadi.");
        }

        business.Status = BusinessStatus.Approved;
        await dbContext.SaveChangesAsync(cancellationToken);
        return BusinessMapping.ToResponse(business);
    }
}
