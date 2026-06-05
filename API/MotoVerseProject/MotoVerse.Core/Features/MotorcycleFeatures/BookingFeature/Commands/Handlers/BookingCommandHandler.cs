using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;
using MotoVerse.Entities.Enums.Motorcycles;
using System.Security.Claims;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Commands.Handlers;

internal class BookingCommandHandler :
    ResponseHandler,
    IRequestHandler<AddBookingCommand, Response<string>>,
    IRequestHandler<EditBookingCommand, Response<string>>,
    IRequestHandler<DeleteBookingCommand, Response<string>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;

    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region CTOR

    public BookingCommandHandler(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer,
        IHttpContextAccessor httpContextAccessor)
        : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    public async Task<Response<string>> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var motorcycle = await _repositoryManager.MotorcycleRepository.GetByIdAsync(request.MotorcycleId);


            if (motorcycle is null)
                return NotFound<string>("Motorcycle not found");

            var totalDays =
                (request.EndDate.Date - request.StartDate.Date).Days;

            if (totalDays <= 0)
                return BadRequest<string>("Invalid rental period");

            var totalPrice =
                totalDays * motorcycle.PricePerDay;

            var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "15e89450-05c4-4fbb-b3c0-5f349ae9afbd";

            var booking = new Booking
            {
                CustomerId = id,
                MotorcycleId = motorcycle.Id,

                StartDate = request.StartDate,

                EndDate = request.EndDate,

                TotalDays = totalDays,

                TotalPrice = totalPrice,

                BookingStatus = BookingStatus.Pending,

                PaymentStatus = PaymentStatus.Pending
            };

            var payment = new Payment
            {
                Booking = booking,

                Amount = totalPrice,

                PaymentMethod = request.PaymentMethod,

                Provider = request.Provider,

                PaypalEmail = request.PaypalEmail,

                WalletName = request.WalletName,

                WalletPhone = request.WalletPhone
            };

            booking.Payment = payment;

            await _repositoryManager.BookingRepository.AddAsync(booking);

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success(booking.Id);
        }

        catch (Exception)
        {
            return BadRequest<string>("Add Failed");
        }
    }

    public async Task<Response<string>> Handle(EditBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking =
                await _repositoryManager
                    .BookingRepository
                    .GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>();

            booking.StartDate = request.StartDate;

            booking.EndDate = request.EndDate;

            booking.TotalPrice = request.TotalPrice;

            booking.BookingStatus = request.BookingStatus;

            booking.PaymentStatus = request.PaymentStatus;

            booking.CustomerId = request.CustomerId;

            booking.MotorcycleId = request.MotorcycleId;

            await _repositoryManager
                .BookingRepository
                .UpdateAsync(booking);

            await _repositoryManager
                .BookingRepository
                .SaveChangesAsync();

            return Success("Updated Successfully");
        }
        catch (Exception)
        {
            return BadRequest<string>("Update Failed");
        }
    }

    public async Task<Response<string>> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking =
                await _repositoryManager
                    .BookingRepository
                    .GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>();

            await _repositoryManager
                .BookingRepository
                .DeleteAsync(booking);

            await _repositoryManager
                .BookingRepository
                .SaveChangesAsync();

            return Deleted<string>("Deleted Successfully");
        }
        catch (Exception)
        {
            return BadRequest<string>("Delete Failed");
        }
    }

}