namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Commands.Models;

public record DeleteMotorcycleImageCommand(string Id)
    : IRequest<Response<string>>;