using MotoVerse.Core.Features.Shared.EmailFeature.Commands.Models;

namespace MotoVerse.API.Controllers;

[Authorize(Roles = "Admin,User")]
public class EmailController : AppControllerBase
{
    [HttpPost(Routing.EmailsRoute.SendEmail)]
    public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
}