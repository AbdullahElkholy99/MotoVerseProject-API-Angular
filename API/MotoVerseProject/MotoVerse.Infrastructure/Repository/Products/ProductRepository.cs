using MotoVerse.Infrastructure.IRepository.Products;

namespace MotoVerse.Infrastructure.Repository.Products;


public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    #region Fields

    private readonly DbSet<Product> _product;

    #endregion

    #region CTOR

    public ProductRepository(AppDbContext context) : base(context)
    {
        _product = context.Set<Product>();
    }

    #endregion

    #region Handles Functions : 

    // -------------------- Read


    #endregion
}
