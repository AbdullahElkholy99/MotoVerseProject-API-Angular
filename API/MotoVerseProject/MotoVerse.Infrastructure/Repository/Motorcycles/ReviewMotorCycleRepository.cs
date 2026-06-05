using MotoVerse.Entities.Models.Motorcycles;
using MotoVerse.Infrastructure.IRepository.Motorcycles;

namespace MotoVerse.Infrastructure.Repository.Motorcycles;

public class ReviewMotorCycleRepository
    : GenericRepository<ReviewMotorCycle>, IReviewMotorCycleRepository
{
    public ReviewMotorCycleRepository(AppDbContext context)
        : base(context)
    {
    }
}