using MotoVerse.Entities.Models.MotorCycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Commands.Models;

public class RemoveBasketItemCommand : IRequest<Response<CustomerBasket>>
{
    public string BasketId { get; set; }
    public string ProductId { get; set; }
}