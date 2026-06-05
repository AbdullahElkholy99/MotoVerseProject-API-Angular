using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;

public class GetMotorcycleByIdResponse
{
    public string Id { get; set; }

    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Brand { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public string? Color { get; set; }

    public string? PlateNumber { get; set; }

    public int EngineCC { get; set; }

    public decimal PricePerDay { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }
    public string[]? ImagesPath { get; set; }

    public MotorcycleStatus Status { get; set; }

    public string OwnerId { get; set; }
}