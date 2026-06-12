
namespace MotoVerse.Core.Features.RefreshTokenFeature.Command.Models;

public class GenerateRefreshTokenCommand : IRequest<Response<JwtAuthResult>>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
