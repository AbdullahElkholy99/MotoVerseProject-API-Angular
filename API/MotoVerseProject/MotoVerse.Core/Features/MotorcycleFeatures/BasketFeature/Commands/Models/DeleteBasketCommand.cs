namespace MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Commands.Models;

public record DeleteBasketCommand(string Id) : IRequest<Response<bool>>;