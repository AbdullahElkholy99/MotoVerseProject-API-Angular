namespace MotoVerse.Core.Features.ApplicationUser.Commands.Models;

public class UpdateUserInfoCommand : IRequest<Response<string>>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
