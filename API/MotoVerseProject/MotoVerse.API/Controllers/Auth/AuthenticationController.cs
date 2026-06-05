
namespace LegalFlow.API.Controllers.Auth;

[ApiController]

public class AuthenticationController : AppControllerBase
{

    [HttpPost(Routing.ApplicationUserRouting.Create)]
    public async Task<IActionResult> Create([FromBody] AddUserCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
    [HttpPost(Routing.Authentication.SignIn)]
    public async Task<IActionResult> SignIn([FromForm] SignInCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
    [HttpPost(Routing.Authentication.Logout)]
    public async Task<IActionResult> Logout()
    {
        var response = await _mediator.Send(new LogoutCommand());
        return NewResult(response);
    }

}