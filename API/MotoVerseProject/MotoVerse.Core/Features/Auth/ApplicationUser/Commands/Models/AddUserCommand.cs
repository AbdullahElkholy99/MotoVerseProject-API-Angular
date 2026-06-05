namespace MotoVerse.Core.Features.Auth.AuthenticationFeature.Command.Models;

public class AddUserCommand : IRequest<Response<string>>
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

}
