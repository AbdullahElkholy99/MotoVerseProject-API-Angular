using MotoVerse.Core.Features.RefreshTokenFeature.Command.Models;
using MotoVerse.Core.Features.RefreshTokenFeature.Query.Models;

namespace MotoVerse.API.Controllers.Auth;

public class RefreshTokenController : AppControllerBase
{

    [HttpPost(Routing.Authentication.RefreshToken)]
    public async Task<IActionResult> RefreshToken([FromForm] GenerateRefreshTokenCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }


    [HttpGet(Routing.Authentication.ValidateToken)]
    public async Task<IActionResult> ValidateToken([FromQuery] ValidateAccessTokenQuery query)
    {
        var response = await _mediator.Send(query);
        return NewResult(response);
    }
}