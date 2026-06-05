using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;

public class GetMotorcycleListResponse
{
    public string Id { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public decimal PricePerDay { get; set; }

    public string? ImagePath { get; set; }
    public string[]? ImagesPath { get; set; }

    public MotorcycleStatus Status { get; set; }
}
