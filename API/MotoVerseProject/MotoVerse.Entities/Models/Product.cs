using MotoVerse.Data.Enums;

namespace MotoVerse.Entities.Models;

public class Product : GeneralLocalizableEntity
{

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public double Rating { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;

    public string CategoryId { get; set; }

    public Category? Category { get; set; }

    public ICollection<ReviewProduct> Reviews { get; set; } = new List<ReviewProduct>();

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();

    // RelationShip With Admin
    [ForeignKey(nameof(Admin))]
    [InverseProperty(nameof(Admin.Products))]
    public string AdminId { get; set; }
    public Admin Admin { get; set; } = null!;
}
