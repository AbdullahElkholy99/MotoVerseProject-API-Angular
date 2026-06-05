using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;

public class GetBasketByIdResponse
{
    public string Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }

    public BookingStatus BookingStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public string CustomerId { get; set; }

    public string MotorcycleId { get; set; }
}