namespace MotoVerse.Core.Features.Auth.ConfirmEmailFeature.Command.Models;

public class SendEmailConfirmationCommand : IRequest<Response<bool>>
{
    public User User { get; set; }
}
public class SendConfirmEmailQuery : IRequest<Response<bool>>
{
    public string? Email { get; set; }
}
