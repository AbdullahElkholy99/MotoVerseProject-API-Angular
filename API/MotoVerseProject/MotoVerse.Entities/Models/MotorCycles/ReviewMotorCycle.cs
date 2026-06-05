using MotoVerse.Entities.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace MotoVerse.Entities.Models.Motorcycles;

public class ReviewMotorCycle
{
    public string Id { get; set; } = Guid.NewGuid().ToString();


    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public string CustomerId { get; set; }

    public Customer Customer { get; set; } = default!;

    public string MotorcycleId { get; set; }

    public Motorcycle Motorcycle { get; set; } = default!;
}
