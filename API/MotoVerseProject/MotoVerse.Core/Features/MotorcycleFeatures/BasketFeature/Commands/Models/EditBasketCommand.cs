using MotoVerse.Entities.Models.MotorCycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Commands.Models;

public class EditBasketCommand : IRequest<Response<CustomerBasket>>
{
    public CustomerBasket CustomerBasket { get; set; }
}
