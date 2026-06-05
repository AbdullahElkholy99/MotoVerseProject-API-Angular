using AutoMapper.QueryableExtensions;
using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;
using MotoVerse.Entities.Enums.Motorcycles;
using System.Security.Claims;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Handlers;

internal class BookingQueryHandler :
    ResponseHandler,
    IRequestHandler<GetBookingListForCustomerQuery, Response<List<GetBookingListResponse>>>,
    IRequestHandler<GetBookingListQuery, Response<List<GetBookingListResponse>>>,
    IRequestHandler<GetBookingByIdQuery, Response<GetBasketByIdResponse>>,
    IRequestHandler<GetBookingPaginatedListQuery, PaginatedResult<GetBookingPaginatedListResponse>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;

    #endregion

    #region CTOR

    public BookingQueryHandler(
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

    public async Task<Response<List<GetBookingListResponse>>> Handle(GetBookingListForCustomerQuery request, CancellationToken cancellationToken)
    {
        var customerId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "15e89450-05c4-4fbb-b3c0-5f349ae9afbd";
        var bookings = await _repositoryManager
    .BookingRepository
    .GetTableNoTracking()
    .Where(x => x.CustomerId == customerId)
    .Select(x => new GetBookingListResponse
    {
        Id = x.Id,
        StartDate = x.StartDate,
        EndDate = x.EndDate,
        TotalPrice = x.TotalPrice,
        TotalDays = x.TotalDays,

        BookingStatus = x.BookingStatus.ToString(),
        PaymentStatus = x.PaymentStatus.ToString(),

        CreatedAt = x.CreatedAt,
        PickupLocation = x.PickupLocation ?? "no select",

        Payment = x.Payment == null
            ? null
            : new PaymentDTO
            {
                Id = x.Payment.Id,
                Amount = x.Payment.Amount,
                Method = x.Payment.PaymentMethod.ToString(),
                Provider = x.Payment.Provider.ToString(),
                TransactionId = x.Payment.TransactionId,
                PaidAt = x.Payment.PaidAt
            },

        Motorcycle = new MotorcycleDTO
        {
            Id = x.Motorcycle.Id,
            Name = x.Motorcycle.NameEn,
            Brand = x.Motorcycle.Brand,
            Model = x.Motorcycle.Model,
            PricePerDay = x.Motorcycle.PricePerDay,
            OwnerName = x.Motorcycle.Owner.DisplayName ?? "Unknown",
            ImagePath = "https://localhost:7081/images/motorcycle/" + x.Motorcycle.ImagePath,
            ImagesPath = x.Motorcycle.Images
                .Select(i => "https://localhost:7081/images/motorcycle/" + i.ImageUrl)
                .ToArray()
        }
    })
    .ToListAsync();

        var result = Success(bookings);

        result.Meta = new
        {
            Count = bookings.Count,
            BookingActive = bookings.Count(s => s.BookingStatus == BookingStatus.Active.ToString()),
            BookingPending = bookings.Count(s => s.BookingStatus == BookingStatus.Pending.ToString()),
            BookingCompleted = bookings.Count(s => s.BookingStatus == BookingStatus.Completed.ToString()),
        };

        return result;
    }


    public async Task<Response<List<GetBookingListResponse>>> Handle(GetBookingListQuery request, CancellationToken cancellationToken)
    {
        var customerId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "15e89450-05c4-4fbb-b3c0-5f349ae9afbd";
        var bookings = await _repositoryManager
                .BookingRepository
                .GetTableNoTracking()
                .Select(x => new GetBookingListResponse
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TotalPrice = x.TotalPrice,
                    TotalDays = x.TotalDays,

                    BookingStatus = x.BookingStatus.ToString(),
                    PaymentStatus = x.PaymentStatus.ToString(),

                    CreatedAt = x.CreatedAt,
                    PickupLocation = x.PickupLocation ?? "no select",

                    Payment = x.Payment == null
                        ? null
                        : new PaymentDTO
                        {
                            Id = x.Payment.Id,
                            Amount = x.Payment.Amount,
                            Method = x.Payment.PaymentMethod.ToString(),
                            Provider = x.Payment.Provider.ToString(),
                            TransactionId = x.Payment.TransactionId,
                            PaidAt = x.Payment.PaidAt
                        },

                    Motorcycle = new MotorcycleDTO
                    {
                        Id = x.Motorcycle.Id,
                        Name = x.Motorcycle.NameEn,
                        Brand = x.Motorcycle.Brand,
                        Model = x.Motorcycle.Model,
                        PricePerDay = x.Motorcycle.PricePerDay,
                        OwnerName = x.Motorcycle.Owner.DisplayName ?? "Unknown",
                        ImagePath = "https://localhost:7081/images/motorcycle/" + x.Motorcycle.ImagePath,
                        ImagesPath = null
                    },
                    CustomerName = x.Customer.DisplayName ?? "Unknown"

                })
                .ToListAsync();

        var result = Success(bookings);

        result.Meta = new
        {
            Count = bookings.Count,
        };

        return result;
    }

    public async Task<Response<GetBasketByIdResponse>> Handle(
        GetBookingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var booking =
            await _repositoryManager
                .BookingRepository
                .GetByIdAsync(request.Id);

        if (booking is null)
            return NotFound<GetBasketByIdResponse>();

        var mapped =
            _mapper.Map<GetBasketByIdResponse>(booking);

        return Success(mapped);
    }

    public async Task<PaginatedResult<GetBookingPaginatedListResponse>>
        Handle(
            GetBookingPaginatedListQuery request,
            CancellationToken cancellationToken)
    {
        var query =
            _repositoryManager
                .BookingRepository
                .GetTableNoTracking();

        var mappedQuery =
            query.ProjectTo<GetBookingPaginatedListResponse>(
                _mapper.ConfigurationProvider);

        return await mappedQuery
            .ToPaginatedListAsync(
                request.PageNumber,
                request.PageSize);
    }


}