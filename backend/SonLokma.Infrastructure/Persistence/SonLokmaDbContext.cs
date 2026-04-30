using Microsoft.EntityFrameworkCore;
using SonLokma.Domain.Entities;

namespace SonLokma.Infrastructure.Persistence;

public sealed class SonLokmaDbContext(DbContextOptions<SonLokmaDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.FullName).HasMaxLength(160).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(320).IsRequired();
            entity.Property(user => user.NormalizedEmail).HasMaxLength(320).IsRequired();
            entity.Property(user => user.PasswordHash).IsRequired();
            entity.Property(user => user.Phone).HasMaxLength(32);
            entity.Property(user => user.Role).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasIndex(user => user.NormalizedEmail).IsUnique();
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(business => business.Id);
            entity.Property(business => business.Name).HasMaxLength(180).IsRequired();
            entity.Property(business => business.Description).HasMaxLength(1000);
            entity.Property(business => business.Category).HasConversion<string>().HasMaxLength(64).IsRequired();
            entity.Property(business => business.Address).HasMaxLength(500).IsRequired();
            entity.Property(business => business.Location).HasColumnType("geography(Point,4326)").IsRequired();
            entity.Property(business => business.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasIndex(business => business.Location).HasMethod("GIST");
            entity.HasOne(business => business.OwnerUser)
                .WithMany(user => user.Businesses)
                .HasForeignKey(business => business.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(listing => listing.Id);
            entity.Property(listing => listing.Title).HasMaxLength(180).IsRequired();
            entity.Property(listing => listing.Description).HasMaxLength(1000);
            entity.Property(listing => listing.OriginalPrice).HasPrecision(10, 2);
            entity.Property(listing => listing.DiscountedPrice).HasPrecision(10, 2);
            entity.Property(listing => listing.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasIndex(listing => new { listing.BusinessId, listing.Status });
            entity.HasOne(listing => listing.Business)
                .WithMany(business => business.Listings)
                .HasForeignKey(listing => listing.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(reservation => reservation.Id);
            entity.Property(reservation => reservation.ReservationCode).HasMaxLength(16).IsRequired();
            entity.Property(reservation => reservation.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.HasIndex(reservation => reservation.ReservationCode).IsUnique();
            entity.HasIndex(reservation => new { reservation.UserId, reservation.ListingId });
            entity.HasOne(reservation => reservation.User)
                .WithMany(user => user.Reservations)
                .HasForeignKey(reservation => reservation.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(reservation => reservation.Listing)
                .WithMany(listing => listing.Reservations)
                .HasForeignKey(reservation => reservation.ListingId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
