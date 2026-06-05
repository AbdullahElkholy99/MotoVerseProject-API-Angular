using MediatR;
using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Commands.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Models;

namespace MotoVerse.API.Controllers.Motorcycles;

public class MotorcycleImageController : AppControllerBase
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region CTOR

    public MotorcycleImageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    #region Commands

    [HttpPost(Routing.MotorcycleImageRouting.Add)]
    public async Task<IActionResult> Add(
        [FromForm]
        AddMotorcycleImageCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut(Routing.MotorcycleImageRouting.Edit)]
    public async Task<IActionResult> Edit(
        [FromForm]
        EditMotorcycleImageCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete(Routing.MotorcycleImageRouting.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        return Ok(
            await _mediator.Send(
                new DeleteMotorcycleImageCommand(id)));
    }

    #endregion

    #region Queries

    [HttpGet(Routing.MotorcycleImageRouting.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            await _mediator.Send(
                new GetMotorcycleImageListQuery()));
    }

    [HttpGet(Routing.MotorcycleImageRouting.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok(
            await _mediator.Send(
                new GetMotorcycleImageByIdQuery(id)));
    }

    [HttpGet(Routing.MotorcycleImageRouting.Paginated)]
    public async Task<IActionResult> Paginated(
        [FromQuery]
        GetMotorcycleImagePaginatedListQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    #endregion
}