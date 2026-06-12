
using MotoVerse.Infrastructure.IRepository.Products;
using MotoVerse.Infrastructure.Repository.Products;
using MotoVerse.Infrastructure.Repository.Users;

namespace MotoVerse.Infrastructure.Repository.Base;

public class RepositoryManager : IRepositoryManager
{
    //private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepository;

    private readonly Lazy<IProductRepository> _productRepository;
    private readonly Lazy<IReviewProductRepository> _reviewProductRepository;


    private readonly Lazy<ICategoryRepository> _categoryRepository;

    #region Users

    private readonly Lazy<ICustomerRepository> _CustomerRepository;
    #endregion

    #region Motorcycles
    private readonly Lazy<IMotorcycleRepository> _motorcycleRepository;

    private readonly Lazy<IBookingRepository> _bookingRepository;

    private readonly Lazy<IFavoriteRepository> _favoriteRepository;

    private readonly Lazy<IMotorcycleImageRepository> _motorcycleImageRepository;

    private readonly Lazy<IPaymentRepository> _paymentRepository;

    private readonly Lazy<IReviewMotorCycleRepository> _reviewMotorCycleRepository;
    private readonly Lazy<ICustomerBasketRepository> _customerBasketRepository;

    #endregion
    public RepositoryManager(AppDbContext context, IConnectionMultiplexer redis)
    {
        //_refreshTokenRepository = new Lazy<IRefreshTokenRepository>(() => new RefreshTokenRepository(context));

        _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
        _reviewProductRepository = new Lazy<IReviewProductRepository>(() => new ReviewProductRepository(context));

        _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(context));

        #region Users

        _CustomerRepository = new Lazy<ICustomerRepository>(() => new CustomerRepository(context));

        #endregion

        #region Motorcycles
        _motorcycleRepository = new Lazy<IMotorcycleRepository>(() => new MotorcycleRepository(context));

        _bookingRepository = new Lazy<IBookingRepository>(() => new BookingRepository(context));

        _favoriteRepository = new Lazy<IFavoriteRepository>(() => new FavoriteRepository(context));

        _motorcycleImageRepository = new Lazy<IMotorcycleImageRepository>(() => new MotorcycleImageRepository(context));

        _paymentRepository = new Lazy<IPaymentRepository>(() => new PaymentRepository(context));

        _reviewMotorCycleRepository = new Lazy<IReviewMotorCycleRepository>(() => new ReviewMotorCycleRepository(context));
        _customerBasketRepository = new Lazy<ICustomerBasketRepository>(() => new CustomerBasketRepository(redis));
        #endregion
    }

    //public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository.Value;

    public IProductRepository ProductRepository => _productRepository.Value;
    public IReviewProductRepository ReviewProductRepository => _reviewProductRepository.Value;

    public ICategoryRepository CategoryRepository => _categoryRepository.Value;
    #region Users
    public ICustomerRepository CustomerRepository => _CustomerRepository.Value;

    #endregion
    #region Motorcycles
    public IMotorcycleRepository MotorcycleRepository => _motorcycleRepository.Value;

    public IBookingRepository BookingRepository => _bookingRepository.Value;

    public IFavoriteRepository FavoriteRepository => _favoriteRepository.Value;

    public IMotorcycleImageRepository MotorcycleImageRepository => _motorcycleImageRepository.Value;

    public IPaymentRepository PaymentRepository => _paymentRepository.Value;

    public IReviewMotorCycleRepository ReviewMotorCycleRepository => _reviewMotorCycleRepository.Value;
    public ICustomerBasketRepository CustomerBasketRepository => _customerBasketRepository.Value;
    #endregion
}