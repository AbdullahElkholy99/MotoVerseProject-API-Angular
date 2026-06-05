using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Models;

public record GetBookingByIdQuery(string Id)
    : IRequest<Response<GetBasketByIdResponse>>;
