using MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Commands.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Queries.Models;
using MotoVerse.Entities.Models.MotorCycles;
using StackExchange.Redis;
using System.Text.Json;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BasketFeature.Commands.Handlers;

internal class BasketCommandHandler :
    ResponseHandler,
    IRequestHandler<EditBasketCommand, Response<CustomerBasket>>,
    IRequestHandler<RemoveBasketItemCommand, Response<CustomerBasket>>,
    IRequestHandler<DeleteBasketCommand, Response<bool>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;
    private readonly IDatabase _database;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    #endregion

    #region CTOR

    public BasketCommandHandler(IConnectionMultiplexer connectionMultiplexer, IRepositoryManager repositoryManager, IMapper mapper, IStringLocalizer<SharedResources> localizer, IMediator mediator) : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _database = connectionMultiplexer.GetDatabase();
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Response<CustomerBasket>> Handle(EditBasketCommand request, CancellationToken cancellationToken)
    {
        var existingBasket = (await _mediator.Send(new GetBasketByIdQuery(request.CustomerBasket.Id))).Data;

        if (existingBasket is not null)
        {
            foreach (var item in request.CustomerBasket.BasketItems)
            {
                var existingItem = existingBasket.BasketItems
                    .FirstOrDefault(x => x.Id == item.Id);

                if (existingItem is not null)
                {
                    // update quantity
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    // add new item
                    existingBasket.BasketItems.Add(item);
                }
            }

            request.CustomerBasket = existingBasket;
        }

        var createdOrUpdated = await _database.StringSetAsync(
            request.CustomerBasket.Id,
            JsonSerializer.Serialize(request.CustomerBasket),
            TimeSpan.FromDays(3)
        );

        if (!createdOrUpdated)
            return null;

        var customerBasket = (await _mediator.Send(new GetBasketByIdQuery(request.CustomerBasket.Id))).Data ?? new CustomerBasket();
        return new Response<CustomerBasket>(customerBasket);

    }

    public async Task<Response<bool>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var result = await _database.KeyDeleteAsync(request.Id);
        return new Response<bool>(result);
    }
    public async Task<Response<CustomerBasket>> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.BasketId))
            return BadRequest<CustomerBasket>("Basket Id is required.");

        if (string.IsNullOrWhiteSpace(request.ProductId))
            return BadRequest<CustomerBasket>("Product Id is required.");

        var result = await _database.StringGetAsync(request.BasketId);

        if (result.IsNullOrEmpty)
            return NotFound<CustomerBasket>("Basket not found.");

        var basket = JsonSerializer.Deserialize<CustomerBasket>(
            result.ToString());

        if (basket is null)
            return BadRequest<CustomerBasket>("Invalid basket data.");

        var item = basket.BasketItems
            .FirstOrDefault(x => x.Id == request.ProductId);

        if (item is null)
            return NotFound<CustomerBasket>("Product not found in basket.");

        basket.BasketItems.Remove(item);

        var saved = await _database.StringSetAsync(
            basket.Id,
            JsonSerializer.Serialize(basket),
            TimeSpan.FromDays(3));

        if (!saved)
            return BadRequest<CustomerBasket>("Failed to update basket.");

        return Success(basket);
    }
    #endregion



}