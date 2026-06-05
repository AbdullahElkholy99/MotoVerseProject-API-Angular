using MotoVerse.Core.Features.Authorization.Quaries.Results;

namespace MotoVerse.Core.Features.Authorization.Quaries.Models
{
    public class GetRolesListQuery : IRequest<Response<List<GetRolesListResult>>>
    {
    }
}
