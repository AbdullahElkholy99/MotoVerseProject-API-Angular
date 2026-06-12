using MotoVerse.Infrastructure.IRepository.Products;

namespace MotoVerse.Infrastructure.IRepository.Base;

public interface IRepositoryManager
{
    //IRefreshTokenRepository RefreshTokenRepository { get; }

    IProductRepository ProductRepository { get; }
    IReviewProductRepository ReviewProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }

    #region Users
    ICustomerRepository CustomerRepository { get; }
    #endregion

    #region Motorcycle
    IMotorcycleRepository MotorcycleRepository { get; }

    IBookingRepository BookingRepository { get; }

    IFavoriteRepository FavoriteRepository { get; }

    IMotorcycleImageRepository MotorcycleImageRepository { get; }

    IPaymentRepository PaymentRepository { get; }

    IReviewMotorCycleRepository ReviewMotorCycleRepository { get; }
    ICustomerBasketRepository CustomerBasketRepository { get; }
    #endregion

}