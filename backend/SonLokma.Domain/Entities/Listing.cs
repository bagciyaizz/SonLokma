using SonLokma.Domain.Enums;

namespace SonLokma.Domain.Entities;

public sealed class Listing
{
    public Guid Id { get; set; }
    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public int StockCount { get; set; }
    public DateTimeOffset PickupStartAt { get; set; }
    public DateTimeOffset PickupEndAt { get; set; }
    public ListingStatus Status { get; set; } = ListingStatus.Active;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
