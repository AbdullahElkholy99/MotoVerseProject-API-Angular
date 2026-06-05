using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Commands.Models;

public class AddMotorcycleCommand : IRequest<Response<string>>
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string? Color { get; set; }

    public string? PlateNumber { get; set; }

    public int EngineCC { get; set; }

    public decimal PricePerDay { get; set; }

    public string? Description { get; set; }

    public IFormFile? ImageFile { get; set; }
    public IFormFile[]? Images { get; set; }

    public MotorcycleStatus Status { get; set; }

    public string OwnerId { get; set; } = string.Empty;

}
