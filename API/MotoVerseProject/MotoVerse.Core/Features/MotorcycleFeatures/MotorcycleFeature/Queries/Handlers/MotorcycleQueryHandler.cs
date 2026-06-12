using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Models;
using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Handlers;

public class MotorcycleQueryHandler :
    ResponseHandler,
    IRequestHandler<GetMotorcycleListQuery,
        Response<List<GetMotorcycleListResponse>>>,
    IRequestHandler<GetMotorcycleByIdQuery,
        Response<GetMotorcycleByIdResponse>>,
    IRequestHandler<GetMotorcyclePaginatedListQuery,
        PaginatedResult<GetMotorcyclePaginatedListResponse>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;

    private readonly IMapper _mapper;

    #endregion

    #region CTOR

    public MotorcycleQueryHandler(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer)
        : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    #endregion

    #region Handlers

    public async Task<Response<List<GetMotorcycleListResponse>>> Handle(GetMotorcycleListQuery request, CancellationToken cancellationToken)
    {
        var motorcycles = await _repositoryManager.MotorcycleRepository.GetTableNoTracking().ToListAsync();

        var mapped =
            _mapper.Map<List<GetMotorcycleListResponse>>(motorcycles);

        var result = Success(mapped);

        result.Meta = new
        {
            Count = mapped.Count
        };

        return result;
    }

    public async Task<Response<GetMotorcycleByIdResponse>> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
    {
        var motorcycle = await _repositoryManager.MotorcycleRepository
            .GetByIdWithIncludeAsync(request.Id, img => img.Images);

        if (motorcycle is null)
            return NotFound<GetMotorcycleByIdResponse>();

        var mapped = _mapper.Map<GetMotorcycleByIdResponse>(motorcycle);

        return Success(mapped);
    }

    public async Task<PaginatedResult<GetMotorcyclePaginatedListResponse>> Handle(GetMotorcyclePaginatedListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Motorcycle>? FilterQuery = FilterPaginatedQuerable(request);

        var Categorys = await FilterQuery
           .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        var PaginatedList = _mapper.Map<List<GetMotorcyclePaginatedListResponse>>(Categorys.Data);

        return new PaginatedResult<GetMotorcyclePaginatedListResponse>
        {
            Data = PaginatedList,
            Meta = new
            {
                Count = PaginatedList.Count(),
                TotalCount = await CountAsync()
            },
            Succeeded = true,
            TotalCount = PaginatedList.Count(),
        };
    }
    public IQueryable<Motorcycle> FilterPaginatedQuerable(GetMotorcyclePaginatedListQuery request)
    {
        var querable = _repositoryManager
            .MotorcycleRepository
            .GetTableNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            querable = querable.Where(x => x.NameAr.Contains(request.Search));
        if (!string.IsNullOrWhiteSpace(request.Model))
            querable = querable.Where(x => x.Model.Contains(request.Model));
        if (!string.IsNullOrWhiteSpace(request.Brand))
            querable = querable.Where(x => x.Brand.Contains(request.Brand));

        if (request.MinPrice is not null)
            querable = querable.Where(x => x.PricePerDay >= request.MinPrice);
        if (request.MaxPrice is not null)
            querable = querable.Where(x => x.PricePerDay <= request.MaxPrice);



        switch (request.status)
        {

            case MotorcycleStatus.Available:
                querable = querable.OrderBy(x => x.Status == MotorcycleStatus.Available);
                break;
            case MotorcycleStatus.Rented:
                querable = querable.OrderBy(x => x.Status == MotorcycleStatus.Available);
                break;
            case MotorcycleStatus.Maintenance:
                querable = querable.OrderBy(x => x.Status == MotorcycleStatus.Maintenance);
                break;
        }

        return querable;
    }
    private async Task<int> CountAsync()
    {
        return await _repositoryManager.MotorcycleRepository.CountAsync();
    }
    #endregion
}