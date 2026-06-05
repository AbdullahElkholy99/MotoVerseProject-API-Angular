using MotoVerse.Data.Enums;

namespace MotoVerse.Core.Features.ProductFeature.Queries.Models;

public class GetProductPaginatedListQuery : IRequest<PaginatedResult<GetProductPaginatedListResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public ProductEnum OrderBy { get; set; }
    public string? Search { get; set; }
    public string? CategoryId { get; set; }

}
