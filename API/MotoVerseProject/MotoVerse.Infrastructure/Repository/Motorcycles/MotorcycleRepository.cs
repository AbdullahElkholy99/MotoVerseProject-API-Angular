using MotoVerse.Entities.Enums.Motorcycles;
using MotoVerse.Entities.Models.Motorcycles;
using MotoVerse.Infrastructure.IRepository.Motorcycles;

namespace MotoVerse.Infrastructure.Repository.Motorcycles;

public class MotorcycleRepository
    : GenericRepository<Motorcycle>, IMotorcycleRepository
{
    public MotorcycleRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync()
    {
        return await _dbContext.Motorcycles
            .Where(x => x.Status == MotorcycleStatus.Available)
            .ToListAsync();
    }
}
