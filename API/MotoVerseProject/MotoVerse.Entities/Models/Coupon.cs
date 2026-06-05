namespace MotoVerse.Entities.Models;

public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public decimal DiscountPercentage { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool IsActive { get; set; } = true;

    // RelationShip With Admin
    [ForeignKey(nameof(Admin))]
    [InverseProperty(nameof(Admin.Categories))]
    public string AdminId { get; set; }
    public Admin Admin { get; set; } = null!;

}

