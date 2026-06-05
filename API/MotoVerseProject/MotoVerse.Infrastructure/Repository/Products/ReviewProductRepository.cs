using MotoVerse.Infrastructure.IRepository.Products;

namespace MotoVerse.Infrastructure.Repository.Products;

public class ReviewProductRepository : GenericRepository<ReviewProduct>,
    IReviewProductRepository
{
    #region Fields

    private readonly DbSet<ReviewProduct> _reviewProduct;

    #endregion

    #region CTOR

    public ReviewProductRepository(AppDbContext context) : base(context)
    {
        _reviewProduct = context.Set<ReviewProduct>();
    }

    #endregion

    #region Handles Functions : 

    // -------------------- Read
    public async Task<IEnumerable<ReviewProduct>> GetUnanalyzedAsync() =>
      await _reviewProduct
          .Include(r => r.Product)
          .Where(r => r.AnalyzedAt == null)
          .ToListAsync();
    public async Task<IEnumerable<ReviewProduct>> GetByProductIdAsync(string productId) =>
      await _reviewProduct
          .Include(r => r.Customer)
          .Where(r => r.ProductId == productId)
          .OrderByDescending(r => r.CreatedAt)
          .ToListAsync();

    #endregion
}
