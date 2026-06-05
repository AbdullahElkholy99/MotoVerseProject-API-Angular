using MotoVerse.Core.Features.ApplicationUser.Queries.Results;

namespace MotoVerse.Core.Features.ApplicationUser.Queries.Models;

public class GetUserPaginationQuery : IRequest<PaginatedResult<GetUserPaginationReponse>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
