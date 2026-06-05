using MotoVerse.Core.Features.ManagePasswordFeature.Command.Models;
using MotoVerse.Core.Features.ManagePasswordFeature.Query.Models;

namespace MotoVerse.API.Controllers.Auth;

public class ManagePasswordController : AppControllerBase
{

    [HttpPost(Routing.ManagePassword.SendResetPasswordCode)]
    public async Task<IActionResult> SendResetPassword([FromQuery] SendResetPasswordCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
    [HttpGet(Routing.ManagePassword.ConfirmResetPasswordCode)]
    public async Task<IActionResult> ConfirmResetPassword([FromQuery] ConfirmResetPasswordQuery query)
    {
        var response = await _mediator.Send(query);
        return NewResult(response);
    }
    [HttpPost(Routing.ManagePassword.ResetPassword)]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
}