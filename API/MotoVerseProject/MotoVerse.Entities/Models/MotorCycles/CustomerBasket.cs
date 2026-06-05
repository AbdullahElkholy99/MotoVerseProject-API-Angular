namespace MotoVerse.Entities.Models.MotorCycles;

// use in redis
public class CustomerBasket
{
    public string Id { get; set; } // key
    public List<BasketItem> BasketItems { get; set; } = new(); // value
}
public class BasketItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string CategoryId { get; set; }

}