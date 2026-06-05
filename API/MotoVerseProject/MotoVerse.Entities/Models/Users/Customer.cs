using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Entities.Models.Users;

public class Customer : User
{
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<ReviewProduct> Reviews { get; set; } = new List<ReviewProduct>();
    public ICollection<ReviewMotorCycle> ReviewMotorCycle { get; set; } = new List<ReviewMotorCycle>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}