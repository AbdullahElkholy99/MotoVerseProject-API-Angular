namespace MotoVerse.Core.Features.ManagePasswordFeature.Query.Models;

public class ConfirmResetPasswordQuery : IRequest<Response<string>>
{
    public string Code { get; set; }
    public string Email { get; set; }
}
