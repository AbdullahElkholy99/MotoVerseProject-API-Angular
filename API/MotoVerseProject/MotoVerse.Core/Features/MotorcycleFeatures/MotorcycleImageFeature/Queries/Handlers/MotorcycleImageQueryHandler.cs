using AutoMapper.QueryableExtensions;
using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Handlers;

internal class MotorcycleImageQueryHandler :
    ResponseHandler,
    IRequestHandler<GetMotorcycleImageListQuery,
        Response<List<GetMotorcycleImageListResponse>>>,
    IRequestHandler<GetMotorcycleImageByIdQuery,
        Response<GetMotorcycleImageByIdResponse>>,
    IRequestHandler<GetMotorcycleImagePaginatedListQuery,
        PaginatedResult<GetMotorcycleImagePaginatedListResponse>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;

    private readonly IMapper _mapper;

    #endregion

    #region CTOR

    public MotorcycleImageQueryHandler(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer)
        : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    #endregion

    public async Task<Response<List<GetMotorcycleImageListResponse>>> Handle(
        GetMotorcycleImageListQuery request,
        CancellationToken cancellationToken)
    {
        var images =
            await _repositoryManager
                .MotorcycleImageRepository
                .GetTableNoTracking()
                .ToListAsync();

        var mapped =
            _mapper.Map<List<GetMotorcycleImageListResponse>>(images);

        var result = Success(mapped);

        result.Meta = new
        {
            Count = mapped.Count
        };

        return result;
    }

    public async Task<Response<GetMotorcycleImageByIdResponse>> Handle(
        GetMotorcycleImageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var image =
            await _repositoryManager
                .MotorcycleImageRepository
                .GetByIdAsync(request.Id);

        if (image is null)
            return NotFound<GetMotorcycleImageByIdResponse>();

        var mapped =
            _mapper.Map<GetMotorcycleImageByIdResponse>(image);

        return Success(mapped);
    }

    public async Task<PaginatedResult<GetMotorcycleImagePaginatedListResponse>> Handle(GetMotorcycleImagePaginatedListQuery request, CancellationToken cancellationToken)
    {
        var query =
            _repositoryManager
                .MotorcycleImageRepository
                .GetTableNoTracking();

        var mappedQuery =
            query.ProjectTo<GetMotorcycleImagePaginatedListResponse>(
                _mapper.ConfigurationProvider);

        return await mappedQuery
            .ToPaginatedListAsync(
                request.PageNumber,
                request.PageSize);
    }


}