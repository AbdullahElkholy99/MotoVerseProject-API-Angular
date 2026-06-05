using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Entities.Models.Motorcycles;

public class Payment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentProvider Provider { get; set; }

    public string? TransactionId { get; set; }

    public string? PaypalEmail { get; set; }

    public string? WalletName { get; set; }

    public string? WalletPhone { get; set; }

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    public string BookingId { get; set; }
    public Booking Booking { get; set; } = default!;
}