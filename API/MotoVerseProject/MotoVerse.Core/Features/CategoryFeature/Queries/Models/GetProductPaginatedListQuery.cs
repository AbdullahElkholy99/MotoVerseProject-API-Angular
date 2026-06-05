using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;
using MotoVerse.Data.Enums;

namespace MotoVerse.Core.Features.CategoryFeature.Queries.Models;

public class GetCategoryPaginatedListQuery : IRequest<PaginatedResult<GetCategoryPaginatedListResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public CategoryOrderingEnum OrderBy { get; set; }
    public string? Search { get; set; }
}
