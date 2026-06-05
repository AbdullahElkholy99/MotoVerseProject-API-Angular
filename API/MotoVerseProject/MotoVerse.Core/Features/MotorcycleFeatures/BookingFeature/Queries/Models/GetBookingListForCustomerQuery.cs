using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Models;

public record GetBookingListForCustomerQuery : IRequest<Response<List<GetBookingListResponse>>>;
public record GetBookingListQuery : IRequest<Response<List<GetBookingListResponse>>>;
