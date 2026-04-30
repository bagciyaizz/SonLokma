using NetTopologySuite.Geometries;
using SonLokma.Domain.Enums;

namespace SonLokma.Domain.Entities;

public sealed class Business
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public User OwnerUser { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BusinessCategory Category { get; set; } = BusinessCategory.Other;
    public string Address { get; set; } = string.Empty;
    public Point Location { get; set; } = null!;
    public BusinessStatus Status { get; set; } = BusinessStatus.Pending;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
