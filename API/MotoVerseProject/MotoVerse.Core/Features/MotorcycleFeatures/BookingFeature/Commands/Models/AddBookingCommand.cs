using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;

public class AddBookingCommand : IRequest<Response<string>>
{
    public string MotorcycleId { get; init; } = default!;

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public string PickupLocation { get; init; } = default!;

    public PaymentMethod PaymentMethod { get; init; }

    public PaymentProvider Provider { get; init; }

    public string? PaypalEmail { get; init; }

    public string? WalletName { get; init; }

    public string? WalletPhone { get; init; }
}
