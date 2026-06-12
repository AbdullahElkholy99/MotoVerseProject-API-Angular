namespace MotoVerse.Core.Features.ApplicationUser.Queries.Models;

public class GetUserByIdQuery : IRequest<Response<GetUserByIdResponse>>
{
    public string Id { get; set; }
    public GetUserByIdQuery(string id)
    {
        Id = id;
    }
}
