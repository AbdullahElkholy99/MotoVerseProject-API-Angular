using MotoVerse.Entities.Models.Users;

namespace MotoVerse.Entities.Models.Motorcycles;

public class Favorite
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;

    public string MotorcycleId { get; set; }

    public Motorcycle Motorcycle { get; set; } = default!;
}