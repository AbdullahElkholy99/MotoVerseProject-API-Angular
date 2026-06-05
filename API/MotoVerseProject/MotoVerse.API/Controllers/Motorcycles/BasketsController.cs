using MediatR;
using MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Commands.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Queries.Models;
using MotoVerse.Entities.Models.MotorCycles;
using MotoVerse.Infrastructure.IRepository.Base;

namespace MotoVerse.API.Controllers.Motorcycles;

public class BasketsController : AppControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRepositoryManager _repositoryManager;
    public BasketsController(IMediator mediator, IRepositoryManager repositoryManager)
    {
        _mediator = mediator;
        _repositoryManager = repositoryManager;
    }
    [HttpGet(Routing.BasketRouting.GetById)]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _mediator.Send(new GetBasketByIdQuery(id)));
    }
    [HttpPut(Routing.BasketRouting.Edit)]
    public async Task<IActionResult> Update(CustomerBasket customerBasket)
    {
        return Ok(await _mediator.Send(new EditBasketCommand()
        {
            CustomerBasket = customerBasket
        }));
    }
    [HttpDelete(Routing.BasketRouting.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        return Ok(await _mediator.Send(new DeleteBasketCommand(id)));
    }
    [HttpDelete(Routing.BasketRouting.DeleteItem)]
    public async Task<IActionResult> DeleteItem([FromQuery] string basketId, [FromQuery] string productId)
    {
        return Ok(await _mediator.Send(new RemoveBasketItemCommand()
        {
            BasketId = basketId,
            ProductId = productId
        }));
    }
}
