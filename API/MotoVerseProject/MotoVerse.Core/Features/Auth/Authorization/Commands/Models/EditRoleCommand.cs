using MotoVerse.Data.DTOs;

namespace MotoVerse.Core.Features.Authorization.Commands.Models
{
    public class EditRoleCommand : EditRoleRequest, IRequest<Response<string>>
    {

    }
}
