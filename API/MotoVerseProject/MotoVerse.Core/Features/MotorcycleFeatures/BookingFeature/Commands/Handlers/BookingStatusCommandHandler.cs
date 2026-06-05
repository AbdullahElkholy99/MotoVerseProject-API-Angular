using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;
using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Commands.Handlers;

internal class BookingStatusCommandHandler :
    ResponseHandler,

    IRequestHandler<CompleteBookingCommand, Response<string>>,
    IRequestHandler<ActiveBookingCommand, Response<string>>,
    IRequestHandler<RejectBookingCommand, Response<string>>,
    IRequestHandler<ApproveBookingCommand, Response<string>>,
    IRequestHandler<PayBookingCommand, Response<string>>,
    IRequestHandler<CancelBookingByCustomerCommand, Response<string>>,
    IRequestHandler<CancelBookingCommand, Response<string>>,
    IRequestHandler<ReCancelBookingCommand, Response<string>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;

    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region CTOR

    public BookingStatusCommandHandler(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer,
        IHttpContextAccessor httpContextAccessor) : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    public async Task<Response<string>> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager.BookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus != BookingStatus.Active)
                return BadRequest<string>("Only active bookings can be completed");

            booking.BookingStatus = BookingStatus.Completed;

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success<string>("Booking completed successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<string>> Handle(ActiveBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager.BookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus != BookingStatus.Approved)
                return BadRequest<string>("Booking must be approved first");

            if (booking.PaymentStatus != PaymentStatus.Paid)
                return BadRequest<string>("Booking must be paid first");

            booking.BookingStatus = BookingStatus.Active;

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success<string>("Booking activated successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<string>> Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager.BookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus != BookingStatus.Pending)
                return BadRequest<string>("Only pending bookings can be approved");

            booking.BookingStatus = BookingStatus.Approved;

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success<string>("Booking approved successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<string>> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager.BookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus != BookingStatus.Pending)
                return BadRequest<string>("Only pending bookings can be rejected");

            booking.BookingStatus = BookingStatus.Rejected;

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success<string>("Booking rejected successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<string>> Handle(PayBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager.BookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus != BookingStatus.Approved)
                return BadRequest<string>("Booking must be approved first");

            booking.PaymentStatus = PaymentStatus.Paid;

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success<string>("Payment completed successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<string>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager.BookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus == BookingStatus.Completed)
                return BadRequest<string>("Completed booking cannot be cancelled");

            booking.BookingStatus = BookingStatus.Cancelled;

            if (booking.PaymentStatus == PaymentStatus.Paid)
            {
                booking.PaymentStatus = PaymentStatus.Refunded;
            }

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success<string>("Booking cancelled successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<string>> Handle(ReCancelBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager.BookingRepository.GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus != BookingStatus.Cancelled)
                return BadRequest<string>("Status Must Be Cancelled");

            booking.BookingStatus = BookingStatus.Pending;

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success<string>("Booking cancelled successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<string>> Handle(CancelBookingByCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await _repositoryManager
                .BookingRepository
                .GetByIdAsync(request.Id);

            if (booking is null)
                return NotFound<string>("Booking not found");

            if (booking.BookingStatus == BookingStatus.Cancelled)
                return BadRequest<string>("Booking already cancelled");

            if (booking.BookingStatus == BookingStatus.Completed)
                return BadRequest<string>("Completed booking cannot be cancelled");

            if (booking.StartDate <= DateTime.UtcNow)
                return BadRequest<string>(
                    "Booking cannot be cancelled after rental has started");

            booking.BookingStatus = BookingStatus.Cancelled;

            if (booking.PaymentStatus == PaymentStatus.Paid)
            {
                booking.PaymentStatus = PaymentStatus.Refunded;
            }

            await _repositoryManager.BookingRepository.SaveChangesAsync();

            return Success("Booking cancelled successfully");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
}