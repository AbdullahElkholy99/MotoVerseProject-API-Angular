namespace MotoVerse.API.Controllers.Motorcycles;

using global::MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Commands.Models;
using global::MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class MotorcycleController : AppControllerBase
{
    private readonly IMediator _mediator;

    public MotorcycleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Routing.MotorcycleRouting.Add)]
    public async Task<IActionResult> Add(
        [FromForm] AddMotorcycleCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut(Routing.MotorcycleRouting.Edit)]
    public async Task<IActionResult> Edit(
        [FromForm] EditMotorcycleCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete(Routing.MotorcycleRouting.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        return Ok(
            await _mediator.Send(
                new DeleteMotorcycleCommand(id)));
    }

    [HttpGet(Routing.MotorcycleRouting.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            await _mediator.Send(
                new GetMotorcycleListQuery()));
    }

    [HttpGet(Routing.MotorcycleRouting.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok(
            await _mediator.Send(
                new GetMotorcycleByIdQuery(id)));
    }

    [HttpGet(Routing.MotorcycleRouting.Paginated)]
    public async Task<IActionResult> Paginated([FromQuery] GetMotorcyclePaginatedListQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
}