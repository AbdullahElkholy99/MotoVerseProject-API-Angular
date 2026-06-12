namespace MotoVerse.Core.Features.ConfirmEmailFeature.Query.Models;

public class ConfirmEmailQuery : IRequest<Response<string>>
{
    public string Email { get; set; }
    public string Code { get; set; }
}

