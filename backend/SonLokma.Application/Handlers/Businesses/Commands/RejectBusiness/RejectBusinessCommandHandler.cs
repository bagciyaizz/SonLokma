using Microsoft.EntityFrameworkCore;
using SonLokma.Application.Common.Interfaces;
using SonLokma.Application.Handlers.Businesses.Common;
using SonLokma.Domain.Enums;

namespace SonLokma.Application.Handlers.Businesses.Commands.RejectBusiness;

public sealed class RejectBusinessCommandHandler(ISonLokmaDbContext dbContext)
{
    public async Task<BusinessResponse> Handle(RejectBusinessCommand command, CancellationToken cancellationToken)
    {
        var business = await dbContext.Businesses
            .Include(item => item.OwnerUser)
            .FirstOrDefaultAsync(item => item.Id == command.BusinessId, cancellationToken);

        if (business is null)
        {
            throw new InvalidOperationException("Isletme bulunamadi.");
        }

        business.Status = BusinessStatus.Rejected;
        await dbContext.SaveChangesAsync(cancellationToken);
        return BusinessMapping.ToResponse(business);
    }
}
