
namespace MotoVerse.Data.Entities.Users;

public class Admin : User
{

    // relation with product
    [InverseProperty(nameof(Product.Admin))]
    public ICollection<Product> Products { get; set; } = new HashSet<Product>();

    // relation with Category

    [InverseProperty(nameof(Category.Admin))]
    public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

    public ICollection<Coupon> Coupons { get; set; } = new HashSet<Coupon>();
}
