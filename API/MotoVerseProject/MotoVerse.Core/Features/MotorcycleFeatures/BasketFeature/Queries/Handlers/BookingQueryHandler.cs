using MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Queries.Models;
using MotoVerse.Entities.Models.MotorCycles;
using StackExchange.Redis;
using System.Text.Json;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Handlers;

internal class BasketQueryHandler :
    ResponseHandler,
    IRequestHandler<GetBasketByIdQuery, Response<CustomerBasket>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;
    private readonly IDatabase _database;
    private readonly IMapper _mapper;

    #endregion

    #region CTOR

    public BasketQueryHandler(IConnectionMultiplexer connectionMultiplexer, IRepositoryManager repositoryManager, IMapper mapper, IStringLocalizer<SharedResources> localizer) : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _database = connectionMultiplexer.GetDatabase();

    }

    public async Task<Response<CustomerBasket>> Handle(GetBasketByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _database.StringGetAsync(request.Id);

        if (string.IsNullOrEmpty(result)) return new Response<CustomerBasket>(null);

        var customerBasket = JsonSerializer.Deserialize<CustomerBasket>(result.ToString());
        return new Response<CustomerBasket>(customerBasket);
    }

    #endregion

}