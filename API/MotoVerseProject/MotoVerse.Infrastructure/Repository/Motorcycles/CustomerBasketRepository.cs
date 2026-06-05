using MotoVerse.Entities.Models.MotorCycles;
using MotoVerse.Infrastructure.IRepository.Motorcycles;
using StackExchange.Redis;
using System.Text.Json;

namespace MotoVerse.Infrastructure.Repository.Motorcycles;

public class CustomerBasketRepository : ICustomerBasketRepository
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CustomerBasketRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
        _connectionMultiplexer = connectionMultiplexer;
    }
    public async Task<List<CustomerBasket>> GetAllAsync()
    {
        var server = _connectionMultiplexer.GetServer(
            _connectionMultiplexer.GetEndPoints().First());

        var keys = server.Keys();

        var baskets = new List<CustomerBasket>();

        foreach (var key in keys)
        {
            var value = await _database.StringGetAsync(key);

            if (!value.IsNullOrEmpty)
            {
                var basket = JsonSerializer.Deserialize<CustomerBasket>(value.ToString()!);

                if (basket != null)
                    baskets.Add(basket);
            }
        }

        return baskets;
    }
    public async Task<CustomerBasket> GetByIdAsync(string id)
    {
        var result = await _database.StringGetAsync(id);

        if (string.IsNullOrEmpty(result)) return null;

        return JsonSerializer.Deserialize<CustomerBasket>(result.ToString());


    }
    public async Task<CustomerBasket> UpdateAsync(CustomerBasket customerBasket)
    {
        var existingBasket = await GetByIdAsync(customerBasket.Id);

        if (existingBasket is not null)
        {
            foreach (var item in customerBasket.BasketItems)
            {
                var existingItem = existingBasket.BasketItems
                    .FirstOrDefault(x => x.Id == item.Id);

                if (existingItem is not null)
                {
                    // update quantity
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    // add new item
                    existingBasket.BasketItems.Add(item);
                }
            }

            customerBasket = existingBasket;
        }

        var createdOrUpdated = await _database.StringSetAsync(
            customerBasket.Id,
            JsonSerializer.Serialize(customerBasket),
            TimeSpan.FromDays(3)
        );

        if (!createdOrUpdated)
            return null;

        return await GetByIdAsync(customerBasket.Id);
    }
    public async Task<bool> DeleteAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }


}
