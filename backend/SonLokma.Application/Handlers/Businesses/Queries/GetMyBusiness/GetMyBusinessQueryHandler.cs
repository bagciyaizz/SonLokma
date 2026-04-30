using Microsoft.EntityFrameworkCore;
using SonLokma.Application.Common.Interfaces;
using SonLokma.Application.Handlers.Businesses.Common;

namespace SonLokma.Application.Handlers.Businesses.Queries.GetMyBusiness;

public sealed class GetMyBusinessQueryHandler(ISonLokmaDbContext dbContext)
{
    public async Task<BusinessResponse?> Handle(GetMyBusinessQuery query, CancellationToken cancellationToken)
    {
        var business = await dbContext.Businesses
            .Include(item => item.OwnerUser)
            .FirstOrDefaultAsync(item => item.OwnerUserId == query.OwnerUserId, cancellationToken);

        return business is null ? null : BusinessMapping.ToResponse(business);
    }
}
