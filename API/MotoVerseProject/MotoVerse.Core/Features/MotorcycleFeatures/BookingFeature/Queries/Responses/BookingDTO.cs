namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;

public class BookingDTO
{
    public string Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }
    public int TotalDays { get; set; }

    public string BookingStatus { get; set; }

    public string PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string PickupLocation { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;

    public PaymentDTO? Payment { get; set; }

    public MotorcycleDTO Motorcycle { get; set; } = default!;
}
public class PaymentDTO
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public decimal Amount { get; set; }

    public string Method { get; set; }

    public string Provider { get; set; }

    public string? TransactionId { get; set; }

    public DateTime PaidAt { get; set; }
}