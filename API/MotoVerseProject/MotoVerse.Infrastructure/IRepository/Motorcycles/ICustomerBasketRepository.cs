using MotoVerse.Entities.Models.MotorCycles;

namespace MotoVerse.Infrastructure.IRepository.Motorcycles;

public interface ICustomerBasketRepository  // : IGenericRepository<CustomerBasket>
{
    Task<List<CustomerBasket>> GetAllAsync();

    Task<CustomerBasket> GetByIdAsync(string id);
    Task<CustomerBasket> UpdateAsync(CustomerBasket customerBasket);
    Task<bool> DeleteAsync(string id);
}