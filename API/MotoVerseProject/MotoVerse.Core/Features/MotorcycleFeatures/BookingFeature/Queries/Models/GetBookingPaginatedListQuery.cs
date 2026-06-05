using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Models;

public record GetBookingPaginatedListQuery :
      IRequest<PaginatedResult<GetBookingPaginatedListResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public CategoryOrderingEnum OrderBy { get; set; }
    public string? Search { get; set; }
}
