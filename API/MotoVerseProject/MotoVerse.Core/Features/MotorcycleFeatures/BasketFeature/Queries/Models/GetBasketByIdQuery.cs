using MotoVerse.Entities.Models.MotorCycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Queries.Models;

public record GetBasketByIdQuery(string Id) : IRequest<Response<CustomerBasket>>;
