using MotoVerse.Core.Features.Authorization.Quaries.Results;

namespace MotoVerse.Core.Features.Authorization.Quaries.Models
{
    public class GetRoleByIdQuery : IRequest<Response<GetRoleByIdResult>>
    {
        public int Id { get; set; }
    }
}
