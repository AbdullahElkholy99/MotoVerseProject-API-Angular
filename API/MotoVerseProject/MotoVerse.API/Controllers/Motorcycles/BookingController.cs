using MediatR;
using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Models;

namespace MotoVerse.API.Controllers.Motorcycles;

public class BookingController : AppControllerBase
{
    private readonly IMediator _mediator;

    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Routing.BookingRouting.Add)]
    public async Task<IActionResult> Add(AddBookingCommand command)
    {
        return Ok(await _mediator.Send(command));
    }


    [HttpPut(Routing.BookingRouting.Edit)]
    public async Task<IActionResult> Edit(EditBookingCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete(Routing.BookingRouting.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        return Ok(await _mediator.Send(new DeleteBookingCommand(id)));
    }

    [HttpGet(Routing.BookingRouting.GetAllForCustomer)]
    public async Task<IActionResult> GetAllForCustomer()
    {
        return Ok(await _mediator.Send(new GetBookingListForCustomerQuery()));
    }

    [HttpGet(Routing.BookingRouting.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetBookingListQuery()));
    }

    [HttpGet(Routing.BookingRouting.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok(await _mediator.Send(new GetBookingByIdQuery(id)));
    }

    [HttpGet(Routing.BookingRouting.Paginated)]
    public async Task<IActionResult> Paginated([FromQuery] GetBookingPaginatedListQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
}