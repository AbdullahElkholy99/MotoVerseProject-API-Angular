using MotoVerse.Data.Enums;
using MotoVerse.Entities.Enums.Motorcycles;
using MotoVerse.Entities.Models.Users;

namespace MotoVerse.Entities.Models.Motorcycles;

public class Booking
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
    public int TotalDays { get; set; }

    public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string PickupLocation { get; set; } = string.Empty;

    public Payment? Payment { get; set; }

    // Relationships : 
    public string CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    public string MotorcycleId { get; set; }
    public Motorcycle Motorcycle { get; set; } = default!;

}