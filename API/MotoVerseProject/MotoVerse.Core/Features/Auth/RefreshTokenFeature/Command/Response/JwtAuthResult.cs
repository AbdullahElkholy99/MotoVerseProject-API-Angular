namespace MotoVerse.Core.Features.Auth.RefreshTokenFeature.Command.Response;

public class JwtAuthResult
{
    public string AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
public class RefreshToken
{
    public string Email { get; set; }
    public string TokenString { get; set; }
    public DateTime ExpireAt { get; set; }
}
