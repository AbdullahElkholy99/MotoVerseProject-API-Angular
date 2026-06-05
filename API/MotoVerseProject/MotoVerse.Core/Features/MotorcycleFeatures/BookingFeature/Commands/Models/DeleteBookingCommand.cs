namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;

public record DeleteBookingCommand(string Id)
    : IRequest<Response<string>>;