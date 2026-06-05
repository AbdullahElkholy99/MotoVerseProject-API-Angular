using MotoVerse.Core.Features.ExternalLoginFeature.Commands.Models;
using MotoVerse.Core.Features.ExternalLoginFeature.Queries.Models;

namespace LegalFlow.API.Controllers.Auth;

public class ExternalLoginController : AppControllerBase
{

    [HttpPost("external-login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLogin(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalResponse), "Account", new { returnUrl });

        var properties = await _mediator.Send(new ConfigureExternalPropertiesCommand()
        {
            Provider = provider,
            RedirectUrl = redirectUrl
        });
        return Challenge(properties.Data, provider);
    }
    [HttpPost("external-response")]

    public async Task<IActionResult> ExternalResponse(
        string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        var result = (await _mediator.Send(new LoginCommand())).Data;

        if (!result.IsSuccess && result.ReturnToLogin)
            return RedirectToAction("SignIn", "Authentication");

        if (!result.IsSuccess && result.ReturnToRegister)
            return RedirectToAction("Create", "Authentication");

        SetCookies(result.Id, result.Email, result.ImagePath, result.UserName ?? result.Email, result.Roles);

        //if (result.Roles.Any())
        //    return RedirectByRole(result.Roles[0]);

        return Redirect(returnUrl);
    }

    void SetCookies(string id, string email, string userName, string imagePath, List<string> roles)
    {

        var options = new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("Id", id, options);
        Response.Cookies.Append("Email", email, options);
        Response.Cookies.Append("Name", userName, options);
        Response.Cookies.Append("ImagePath", imagePath ?? string.Empty, options);

        foreach (var role in roles)
        {
            Response.Cookies.Append("Role", role ?? "Guest", options);
        }

    }

}
