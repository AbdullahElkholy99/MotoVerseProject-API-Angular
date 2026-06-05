using MotoVerse.Core.Features.ReviewProductFeature.Queries.Responses;

namespace MotoVerse.Core.Features.ReviewProductFeature.Queries.Models;

public class GetReviewProductPaginatedListQuery : IRequest<PaginatedResult<GetReviewProductPaginatedListResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }

}
