using MotoVerse.Entities.Enums.Motorcycles;
using MotoVerse.Entities.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace MotoVerse.Entities.Models.Motorcycles;

public class Motorcycle : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }

    [MaxLength(50)]
    public string? PlateNumber { get; set; }

    public int EngineCC { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerDay { get; set; }

    public string? Description { get; set; }


    public MotorcycleStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(Owner))]
    public string OwnerId { get; set; }

    public Owner Owner { get; set; } = default!;

    public ICollection<MotorcycleImage> Images { get; set; } = new List<MotorcycleImage>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public ICollection<ReviewMotorCycle> ReviewMotorCycles { get; set; } = new List<ReviewMotorCycle>();

}
