using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Models;

public class GetMotorcyclePaginatedListQuery : Pagination, IRequest<PaginatedResult<GetMotorcyclePaginatedListResponse>>
{
    public MotorcycleStatus status { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

}

public class Pagination
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}