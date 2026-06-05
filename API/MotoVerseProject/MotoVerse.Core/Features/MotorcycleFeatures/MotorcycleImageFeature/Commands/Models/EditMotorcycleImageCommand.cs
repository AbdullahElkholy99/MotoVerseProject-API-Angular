namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Commands.Models;

public class EditMotorcycleImageCommand
    : IRequest<Response<string>>
{
    public string Id { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }

    public string MotorcycleId { get; set; } = string.Empty;
}
