
namespace MotoVerse.Entities.Models;

public class Category : GeneralLocalizableEntity
{
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; }
        = new List<Product>();

    // RelationShip With Admin
    [ForeignKey(nameof(Admin))]
    [InverseProperty(nameof(Admin.Categories))]
    public string AdminId { get; set; }
    public Admin Admin { get; set; } = null!;

}
