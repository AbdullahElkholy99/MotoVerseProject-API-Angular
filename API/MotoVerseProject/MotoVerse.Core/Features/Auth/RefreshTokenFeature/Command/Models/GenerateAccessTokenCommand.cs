namespace MotoVerse.Core.Features.RefreshTokenFeature.Command.Models;

public class GenerateAccessTokenCommand : IRequest<Response<JwtAuthResult>>
{
    public User User { get; set; }
}
