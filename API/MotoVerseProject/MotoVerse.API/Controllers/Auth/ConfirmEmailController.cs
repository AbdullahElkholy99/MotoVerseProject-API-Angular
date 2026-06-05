using MotoVerse.Core.Features.ConfirmEmailFeature.Query.Models;

namespace MotoVerse.API.Controllers.Auth;


public class ConfirmEmailController : AppControllerBase
{

    [HttpGet(Routing.ConfirmEmail.Confirm)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
    {

        var response = await _mediator.Send(query);

        if (response.Succeeded)
        {
            return Redirect("http://localhost:4200/confirm-email?success=true");
        }

        return Redirect("http://localhost:4200/login?success=false");
    }
    [HttpGet(Routing.ConfirmEmail.SendConfirm)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] SendConfirmEmailQuery query)
    {
        var response = await _mediator.Send(query);
        return NewResult(response);
    }

}