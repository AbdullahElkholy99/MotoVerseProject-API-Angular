namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;

public class MotorcycleDTO
{
    public string Id { get; set; }

    public string Name { get; set; }
    public string Brand { get; set; }

    public string Model { get; set; }

    public decimal PricePerDay { get; set; }

    public string? ImagePath { get; set; }
    public string[]? ImagesPath { get; set; }

    public string OwnerName { get; set; }

}
