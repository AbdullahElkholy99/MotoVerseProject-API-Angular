using MotoVerse.Core.Features.ReviewProductFeature.Queries.Models;
using MotoVerse.Core.Features.ReviewProductFeature.Queries.Responses;

namespace MotoVerse.Core.Features.ReviewProductFeature.Queries.Handlers;

public class ReviewProductQueryHandler :
    ResponseHandler,
    IRequestHandler<GetReviewProductListQuery, Response<List<GetReviewProductListResponse>>>,
    IRequestHandler<GetReviewProductByIdQuery, Response<GetReviewProductByIdResponse>>,
    IRequestHandler<GetReviewProductPaginatedListQuery, PaginatedResult<GetReviewProductPaginatedListResponse>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;

    #endregion

    #region Constructor

    public ReviewProductQueryHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        IMapper mapper,
        IRepositoryManager repositoryManager)
        : base(stringLocalizer)
    {

        _stringLocalizer = stringLocalizer;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    #endregion

    #region Handlers

    public async Task<Response<List<GetReviewProductListResponse>>> Handle(GetReviewProductListQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _repositoryManager
            .ReviewProductRepository
            .GetTableNoTracking()
            .ToListAsync(cancellationToken);

        var reviewsDto = _mapper.Map<List<GetReviewProductListResponse>>(reviews);

        var result = Success(reviewsDto, "Get All Data Successfully");

        result.Meta = new
        {
            Count = reviewsDto.Count
        };

        return result;
    }

    public async Task<Response<GetReviewProductByIdResponse>> Handle(
        GetReviewProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var review = await _repositoryManager
            .ReviewProductRepository
            .GetTableNoTracking()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (review is null)
            return NotFound<GetReviewProductByIdResponse>("Object Not Found");

        var reviewDto = _mapper.Map<GetReviewProductByIdResponse>(review);

        return Success(reviewDto, "Get Data Successfully");
    }

    public async Task<PaginatedResult<GetReviewProductPaginatedListResponse>> Handle(
        GetReviewProductPaginatedListQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<ReviewProduct> filterQuery =
            FilterPaginatedQueryable(request.Search);

        var products = await filterQuery.ToPaginatedListAsync(
            request.PageNumber,
            request.PageSize);

        var paginatedList =
            _mapper.Map<List<GetReviewProductPaginatedListResponse>>(products.Data);

        return new PaginatedResult<GetReviewProductPaginatedListResponse>
        {
            Data = paginatedList,
            Meta = new
            {
                Count = paginatedList.Count,
                ItemsCount = await filterQuery.CountAsync(cancellationToken)
            },
            Succeeded = true
        };
    }

    #endregion

    #region Helper Methods

    public IQueryable<ReviewProduct> FilterPaginatedQueryable(string? search)
    {
        var query = _repositoryManager
            .ReviewProductRepository
            .GetTableNoTracking()
            .Include(x => x.Product)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Product != null &&
                x.Product.NameAr.Contains(search));
        }

        return query;
    }

    #endregion

}
