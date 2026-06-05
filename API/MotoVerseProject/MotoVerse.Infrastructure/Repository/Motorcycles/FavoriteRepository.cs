using MotoVerse.Entities.Models.Motorcycles;
using MotoVerse.Infrastructure.IRepository.Motorcycles;

namespace MotoVerse.Infrastructure.Repository.Motorcycles;

public class FavoriteRepository
    : GenericRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(AppDbContext context)
        : base(context)
    {
    }
}