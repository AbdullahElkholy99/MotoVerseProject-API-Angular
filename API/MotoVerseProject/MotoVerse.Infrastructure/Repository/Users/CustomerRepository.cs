namespace MotoVerse.Infrastructure.Repository.Users;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    #region Fields

    private readonly DbSet<Customer> _customer;

    #endregion

    #region CTOR

    public CustomerRepository(AppDbContext context) : base(context)
    {
        _customer = context.Set<Customer>();
    }

    #endregion

    #region Handles Functions : 

    // -------------------- Read

    #endregion
}
