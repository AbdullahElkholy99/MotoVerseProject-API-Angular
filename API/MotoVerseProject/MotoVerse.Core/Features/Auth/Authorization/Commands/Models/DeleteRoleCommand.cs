namespace MotoVerse.Core.Features.Authorization.Commands.Models;

public class DeleteRoleCommand : IRequest<Response<string>>
{
    public string Id { get; set; }
}
