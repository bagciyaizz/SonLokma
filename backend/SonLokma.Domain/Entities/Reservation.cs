using SonLokma.Domain.Enums;

namespace SonLokma.Domain.Entities;

public sealed class Reservation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ListingId { get; set; }
    public Listing Listing { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public string ReservationCode { get; set; } = string.Empty;
    public ReservationStatus Status { get; set; } = ReservationStatus.Reserved;
    public DateTimeOffset ReservedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? PickedUpAt { get; set; }
}
