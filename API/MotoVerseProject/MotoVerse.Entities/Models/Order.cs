using MotoVerse.Data.Enums;
using MotoVerse.Entities.Models.Users;

namespace MotoVerse.Entities.Models;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = Guid.NewGuid().ToString();

    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public string CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
}