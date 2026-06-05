namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Commands.Models;

public class AddMotorcycleImageCommand
    : IRequest<Response<string>>
{
    public IFormFile Image { get; set; } = default!;

    public string MotorcycleId { get; set; } = string.Empty;
}
