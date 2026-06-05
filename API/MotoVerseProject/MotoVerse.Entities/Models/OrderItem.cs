namespace MotoVerse.Entities.Models;


public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string ProductId { get; set; }

    public Product? Product { get; set; }

    public string OrderId { get; set; }

    public Order? Order { get; set; }
}