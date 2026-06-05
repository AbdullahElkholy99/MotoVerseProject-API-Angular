using Microsoft.AspNetCore.Authentication;

namespace MotoVerse.Core.Features.ExternalLoginFeature.Commands.Models;

public class ConfigureExternalPropertiesCommand : IRequest<Response<AuthenticationProperties>>
{
    public string Provider { get; set; }
    public string RedirectUrl { get; set; }
}

