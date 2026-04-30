using SonLokma.Domain.Enums;

namespace SonLokma.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Business> Businesses { get; set; } = new List<Business>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
