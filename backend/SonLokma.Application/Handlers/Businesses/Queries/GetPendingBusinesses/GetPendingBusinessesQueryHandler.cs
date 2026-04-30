using Microsoft.EntityFrameworkCore;
using SonLokma.Application.Common.Interfaces;
using SonLokma.Application.Handlers.Businesses.Common;
using SonLokma.Domain.Enums;

namespace SonLokma.Application.Handlers.Businesses.Queries.GetPendingBusinesses;

public sealed class GetPendingBusinessesQueryHandler(ISonLokmaDbContext dbContext)
{
    public async Task<IReadOnlyList<BusinessResponse>> Handle(GetPendingBusinessesQuery query, CancellationToken cancellationToken)
    {
        var businesses = await dbContext.Businesses
            .Include(item => item.OwnerUser)
            .Where(item => item.Status == BusinessStatus.Pending)
            .OrderBy(item => item.CreatedAt)
            .ToListAsync(cancellationToken);

        return businesses.Select(BusinessMapping.ToResponse).ToList();
    }
}
