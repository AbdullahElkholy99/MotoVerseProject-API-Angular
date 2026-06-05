namespace MotoVerse.Entities.Models.Motorcycles;

public class MotorcycleImage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ImageUrl { get; set; } = string.Empty;

    public string MotorcycleId { get; set; }

    public Motorcycle Motorcycle { get; set; } = default!;
}