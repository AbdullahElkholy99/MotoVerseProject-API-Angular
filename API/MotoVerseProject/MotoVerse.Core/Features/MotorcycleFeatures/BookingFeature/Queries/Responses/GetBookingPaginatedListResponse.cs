using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;

public class GetBookingPaginatedListResponse
{
    public string Id { get; set; }

    public decimal TotalPrice { get; set; }

    public BookingStatus BookingStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; }
}