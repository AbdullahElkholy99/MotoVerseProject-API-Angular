using MotoVerse.Entities.Models.Motorcycles;
using MotoVerse.Infrastructure.IRepository.Motorcycles;

namespace MotoVerse.Infrastructure.Repository.Motorcycles;

public class MotorcycleImageRepository
    : GenericRepository<MotorcycleImage>, IMotorcycleImageRepository
{
    public MotorcycleImageRepository(AppDbContext context)
        : base(context)
    {
    }
}