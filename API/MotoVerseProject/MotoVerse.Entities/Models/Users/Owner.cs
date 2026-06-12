using MotoVerse.Entities.Models.Auth;
using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Entities.Models.Users;

public class Owner : User
{
    public ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();

    public ICollection<ReviewProduct> Reviews { get; set; } = new List<ReviewProduct>();
}
