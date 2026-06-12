
namespace MotoVerse.API.Controllers.Auth;

public class ApplicationUserController : AppControllerBase
{
    [HttpPost(Routing.ApplicationUserRouting.Create)]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] AddCustomerCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
    [HttpGet(Routing.ApplicationUserRouting.GetInfo)]
    public async Task<IActionResult> GetInfo()
    {
        var response = await _mediator.Send(new GetUserInfoQuery());
        return Ok(response);
    }
    [HttpGet(Routing.ApplicationUserRouting.Paginated)]
    public async Task<IActionResult> Paginated([FromQuery] GetUserPaginationQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
    [HttpGet(Routing.ApplicationUserRouting.GetByID)]
    public async Task<IActionResult> GetStudentByID(string id)
    {
        return NewResult(await _mediator.Send(new GetUserByIdQuery(id)));
    }
    [HttpPut(Routing.ApplicationUserRouting.Edit)]
    public async Task<IActionResult> Edit([FromBody] EditUserCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
    [HttpPut(Routing.ApplicationUserRouting.UpdateInfo)]
    public async Task<IActionResult> UpdateInfo([FromForm] UpdateUserInfoCommand command)
    {
        var response = await _mediator.Send(command);
        return NewResult(response);
    }
    [HttpDelete(Routing.ApplicationUserRouting.Delete)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        return NewResult(await _mediator.Send(new DeleteUserCommand(id)));
    }

}
