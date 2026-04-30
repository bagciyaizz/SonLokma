using Microsoft.EntityFrameworkCore;
using SonLokma.Domain.Entities;

namespace SonLokma.Application.Common.Interfaces;

public interface ISonLokmaDbContext
{
    DbSet<User> Users { get; }
    DbSet<Business> Businesses { get; }
    DbSet<Listing> Listings { get; }
    DbSet<Reservation> Reservations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
