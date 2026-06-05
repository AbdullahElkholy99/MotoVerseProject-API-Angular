using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Models;

public class GetMotorcycleImagePaginatedListQuery :
      IRequest<PaginatedResult<GetMotorcycleImagePaginatedListResponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public CategoryOrderingEnum OrderBy { get; set; }
    public string? Search { get; set; }
}