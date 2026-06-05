namespace MotoVerse.Core.Features.CategoryFeature.Queries.Handlers;

public class CategoryQueryHandler :
    ResponseHandler,
    IRequestHandler<GetCategoryListQuery, Response<List<GetCategoryListResponse>>>,
    IRequestHandler<GetCategoryByIdQuery, Response<GetCategoryByIdResponse>>,
    IRequestHandler<GetCategoryPaginatedListQuery, PaginatedResult<GetCategoryPaginatedListResponse>>

{

    #region Fields
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IStringLocalizer<SharedResources> _sharedResources;

    #endregion

    #region CTOR
    public CategoryQueryHandler(IRepositoryManager repositoryManager, IStringLocalizer<SharedResources> sharedResources, IMapper mapper) : base(sharedResources)
    {
        _repositoryManager = repositoryManager;
        _sharedResources = sharedResources;
        _mapper = mapper;
    }
    #endregion

    #region Function Handlers
    public async Task<Response<List<GetCategoryListResponse>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var Categories = await _repositoryManager.CategoryRepository
            .GetTableNoTracking()
            .Select(c => new GetCategoryListResponse
            {
                Id = c.Id,
                Description = c.Description,
                ImagePath = c.ImagePath,
                Name = c.NameEn,
                ProductCount = c.Products.Count()
            })
            .ToListAsync();

        if (Categories is null)
            return BadRequest<List<GetCategoryListResponse>>("Faile When Get All Data");


        var result = Success(Categories, "Get All Data Successfully");

        result.Meta = new
        {
            Count = Categories.Count(),
        };

        return result;
    }

    public async Task<Response<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var Category = await _repositoryManager.CategoryRepository.GetByIdWithIncludeAsync(request.Id, x => x.Products);

        if (Category == null) return NotFound<GetCategoryByIdResponse>("Object not Found");

        var CategoryDto = Category.MapFromCategoryToGetCategoryByIdResponse();

        return Success(CategoryDto, "Get Data Successfully");
    }

    public async Task<PaginatedResult<GetCategoryPaginatedListResponse>> Handle(GetCategoryPaginatedListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Category>? FilterQuery = FilterPaginatedQuerable(request.OrderBy, request.Search);

        var Categorys = await FilterQuery
           .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        var PaginatedList = Categorys.Data
             .Select(c => new GetCategoryPaginatedListResponse
             {
                 Id = c.Id,
                 Description = c.Description,
                 ImagePath = c.ImagePath,
                 Name = c.NameEn,
                 ProductCount = c.Products.Count()
             }).ToList();
        //.Select(p => p.MapFromCategoryToGetCategoryPaginatedListResponse())

        return new PaginatedResult<GetCategoryPaginatedListResponse>
        {
            Data = PaginatedList,
            Meta = new
            {
                Count = PaginatedList.Count(),
                TotalCount = await CountCatregory()
            },
            Succeeded = true,
            TotalCount = PaginatedList.Count(),
        };
    }
    public IQueryable<Category> FilterPaginatedQuerable(CategoryOrderingEnum orderingEnum, string? search)
    {
        var querable = _repositoryManager
            .CategoryRepository
            .GetTableNoTracking()
            .Include(x => x.Products)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            querable = querable.Where(x => x.NameAr.Contains(search));

        switch (orderingEnum)
        {
            case CategoryOrderingEnum.ID:
                querable = querable.OrderBy(x => x.Id);
                break;

            case CategoryOrderingEnum.Name:
                querable = querable.OrderBy(x => x.NameAr);
                break;
        }

        return querable;
    }
    private async Task<int> CountCatregory()
    {
        return await _repositoryManager.CategoryRepository.CountAsync();
    }
    #endregion
}
