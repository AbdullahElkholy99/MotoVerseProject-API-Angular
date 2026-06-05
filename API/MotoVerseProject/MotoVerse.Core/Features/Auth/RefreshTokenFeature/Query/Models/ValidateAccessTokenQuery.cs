namespace MotoVerse.Core.Features.RefreshTokenFeature.Query.Models;

public class ValidateAccessTokenQuery : IRequest<Response<string>>
{
    public string AccessToken { get; set; }
}