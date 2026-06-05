using MotoVerse.Data.Results;

namespace MotoVerse.Core.Features.AuthenticationFeature.Command.Models;

public class SignInCommand : IRequest<Response<JwtAuthResult>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LogoutCommand : IRequest<Response<string>>
{
}
