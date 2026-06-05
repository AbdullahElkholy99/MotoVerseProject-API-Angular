namespace MotoVerse.Infrastructure.IRepository.Products;

public interface IReviewProductRepository : IGenericRepository<ReviewProduct>
{
    // -------------------- Read
    Task<IEnumerable<ReviewProduct>> GetByProductIdAsync(string productId);
    Task<IEnumerable<ReviewProduct>> GetUnanalyzedAsync();

}


