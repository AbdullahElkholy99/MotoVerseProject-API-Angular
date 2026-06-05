using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Infrastructure.IRepository.Motorcycles;

public interface IMotorcycleRepository : IGenericRepository<Motorcycle>
{
    Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync();
}
