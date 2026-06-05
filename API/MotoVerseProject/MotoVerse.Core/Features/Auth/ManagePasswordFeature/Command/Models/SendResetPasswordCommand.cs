namespace MotoVerse.Core.Features.ManagePasswordFeature.Command.Models;

public class SendResetPasswordCommand : IRequest<Response<string>>
{
    public string Email { get; set; }
}
