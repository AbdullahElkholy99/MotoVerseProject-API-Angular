namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Commands.Models;

public record DeleteMotorcycleCommand(string Id)
    : IRequest<Response<string>>;