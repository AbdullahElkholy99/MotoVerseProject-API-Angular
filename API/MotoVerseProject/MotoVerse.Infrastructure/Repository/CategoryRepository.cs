namespace MotoVerse.Infrastructure.Implementation;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    #region Fields

    private readonly DbSet<Category> _Category;

    #endregion

    #region CTOR

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _Category = context.Set<Category>();
    }

    #endregion

    #region Handles Functions : 

    // -------------------- Read

    #endregion
}
