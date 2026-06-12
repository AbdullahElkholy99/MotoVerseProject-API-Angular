namespace MotoVerse.Core.Features.Auth.ManagePasswordFeature.Command.Models;

public class ChangeUserPasswordCommand : IRequest<Response<string>>
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
