using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;

namespace MotoVerse.API.Controllers.Motorcycles;

public class BookingStatusController : AppControllerBase
{

    [HttpPatch(Routing.BookingStatusRouting.Approve)]
    public async Task<IActionResult> Approve(string id)
    {
        var result = await _mediator.Send(new ApproveBookingCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch(Routing.BookingStatusRouting.Activate)]
    public async Task<IActionResult> Activate(string id)
    {
        var result = await _mediator.Send(new ActiveBookingCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch(Routing.BookingStatusRouting.Complete)]
    public async Task<IActionResult> Complete(string id)
    {
        var result = await _mediator.Send(new CompleteBookingCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch(Routing.BookingStatusRouting.Cancel)]
    public async Task<IActionResult> Cancel(string id)
    {
        var result = await _mediator.Send(new CancelBookingCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch(Routing.BookingStatusRouting.ReCancel)]
    public async Task<IActionResult> ReCancel(string id)
    {
        var result = await _mediator.Send(new ReCancelBookingCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch(Routing.BookingStatusRouting.CancelByCustomer)]
    public async Task<IActionResult> CancelByCustomer(string id)
    {
        var result = await _mediator.Send(new CancelBookingByCustomerCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch(Routing.BookingStatusRouting.Reject)]
    public async Task<IActionResult> Reject(string id)
    {
        var result = await _mediator.Send(new RejectBookingCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }


    [HttpPatch(Routing.BookingStatusRouting.Pay)]
    public async Task<IActionResult> Pay(string id)
    {
        var result = await _mediator.Send(new PayBookingCommand()
        {
            Id = id
        });

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }
}
