namespace MotoVerse.Core.Features.ExternalLoginFeature.Queries.Results;


public record AuthExternalResponse
{
    public string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();

    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; } = false;
    public bool ReturnToLogin { get; set; } = false;
    public bool ReturnToRegister { get; set; } = false;
}