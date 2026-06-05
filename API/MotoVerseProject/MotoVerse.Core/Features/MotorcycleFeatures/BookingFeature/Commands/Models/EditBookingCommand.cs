using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;

public class EditBookingCommand : IRequest<Response<string>>
{
    public string Id { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }

    public BookingStatus BookingStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public string CustomerId { get; set; } = string.Empty;

    public string MotorcycleId { get; set; } = string.Empty;
}