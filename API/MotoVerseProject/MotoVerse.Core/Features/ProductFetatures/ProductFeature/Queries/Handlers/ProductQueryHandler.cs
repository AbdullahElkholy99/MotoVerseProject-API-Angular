using Microsoft.Extensions.AI;
using MotoVerse.Core.Features.ProductFetatures.ProductFeature.Queries.Models;
using MotoVerse.Core.Features.ReviewProductFeature.Queries.Responses;
using OpenAI;
using System.ClientModel;
using System.Text.Json;

namespace MotoVerse.Core.Features.ProductFeature.Queries.Handlers;

public class ProductQueryHandler :
    ResponseHandler,
    IRequestHandler<GetProductListQuery, Response<List<GetProductListResponse>>>,
    IRequestHandler<GetProductByIdQuery, Response<GetProductByIdResponse>>,
    IRequestHandler<GetRecommendationProductQuery, Response<List<ProductRecommendationResponse>>>,
    IRequestHandler<GetProductPaginatedListQuery, PaginatedResult<GetProductPaginatedListResponse>>

{

    #region Fields
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly IChatClient _client;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    #endregion

    #region CTOR
    public ProductQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                IConfiguration config,
IMapper mapper, IRepositoryManager repositoryManager) : base(stringLocalizer)
    {
        var token = config["GitHubModels:Token"]!;
        var credentials = new ApiKeyCredential(token);
        var endpoint = new Uri("https://models.github.ai/inference");
        var options = new OpenAIClientOptions { Endpoint = endpoint };

        _client = new OpenAIClient(credentials, options)
            .GetChatClient("openai/gpt-4o-mini")
            .AsIChatClient(); _stringLocalizer = stringLocalizer;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }
    #endregion

    #region Function Handlers
    public async Task<Response<List<GetProductListResponse>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var products = await _repositoryManager.ProductRepository.GetTableNoTracking().ToListAsync();

        var productsDto = _mapper.Map<List<GetProductListResponse>>(products);

        var result = Success(productsDto, "Get All Data Successfully");

        result.Meta = new
        {
            Count = productsDto.Count(),
        };

        return result;
    }

    public async Task<Response<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product =
           await _repositoryManager.ProductRepository.GetTableNoTracking()
           .Where(p => p.Id.Equals(request.Id))
           .Select(p => new GetProductByIdResponse
           {
               Reviews = p.Reviews.Select(r => new GetReviewProductByIdResponse
               {
                   Comment = r.Comment,
                   CreatedAt = r.CreatedAt,
                   Rating = r.Rating,
                   UpdatedAt = r.UpdatedAt,
                   Id = r.Id,
                   CustomerName = r.Customer.DisplayName ?? "unkown",
                   ProductName = p.NameEn,
                   AdminAutoReply = r.AdminAutoReply,
                   AnalyzedAt = r.AnalyzedAt,
                   FakeReason = r.FakeReason,
                   IsFake = r.IsFake,
                   Sentiment = r.Sentiment,
                   SentimentScore = r.SentimentScore
               }).ToList(),
               CategoryName = p.Category.NameEn,
               Description = p.Description,
               ImagePath = p.ImagePath,
               Name = p.NameEn,
               Price = p.Price,
               Quantity = p.Quantity,
               Rating = p.Rating,
               Status = p.Status,
               Id = p.Id
           })
           .FirstOrDefaultAsync();

        //.Include(p => p.Category)
        //.Include(p => p.Reviews)

        if (product == null) return NotFound<GetProductByIdResponse>("Object not Found");

        var productDto = _mapper.Map<GetProductByIdResponse>(product);

        return Success(productDto, "Get Data Successfully");
    }

    public async Task<PaginatedResult<GetProductPaginatedListResponse>> Handle(GetProductPaginatedListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Product>? FilterQuery = FilterPaginatedQuerable(request.OrderBy, request.Search, request.CategoryId);

        var products = await FilterQuery
           .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        var PaginatedList = _mapper.Map<List<GetProductPaginatedListResponse>>(products.Data);

        return new PaginatedResult<GetProductPaginatedListResponse>
        {
            Data = PaginatedList,
            Meta = new
            {
                Count = PaginatedList.Count(),
                ItemsCount = await _repositoryManager.ProductRepository.CountAsync()
            },
            Succeeded = true,
        };
    }
    public IQueryable<Product> FilterPaginatedQuerable(ProductEnum orderingEnum, string? search, string? categoryId)
    {
        var querable = _repositoryManager
            .ProductRepository
            .GetTableNoTracking()
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            querable = querable.Where(x => x.NameAr.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(categoryId))
        {
            querable = querable.Where(x => x.CategoryId == categoryId);
        }

        switch (orderingEnum)
        {
            case ProductEnum.ID:
                querable = querable.OrderBy(x => x.Id);
                break;

            case ProductEnum.Name:
                querable = querable.OrderBy(x => x.NameAr);
                break;

            case ProductEnum.Price:
                querable = querable.OrderBy(x => x.Price);
                break;

            case ProductEnum.DepartmentName:
                querable = querable.OrderBy(x => x.Category.NameAr);
                break;
        }

        return querable;
    }
    #endregion

    #region AI Recommendation
    public async Task<Response<List<ProductRecommendationResponse>>> Handle(GetRecommendationProductQuery request, CancellationToken cancellationToken)
    {
        var currentProduct = await _repositoryManager.ProductRepository.GetByIdAsync(request.Id);

        if (currentProduct is null)
            return NotFound<List<ProductRecommendationResponse>>("Product Not Found");

        var allProducts =
            _repositoryManager.ProductRepository
            .GetTableNoTracking()
            .Where(p => p.Id != request.Id)
            .ToList();

        var aiResult =
            await ProductRecommendationAsync(currentProduct, allProducts);

        var recommendedIds = aiResult.Products
            .Select(x => x.Id)
            .ToList();

        var recommendedProducts = allProducts
            .Where(p => recommendedIds.Contains(p.Id))
            .ToList();

        var result = recommendedProducts
            .Select(p => new ProductRecommendationResponse
            {
                Id = p.Id,
                Name = p.NameEn,
                Price = p.Price,
                ImagePath = p.ImagePath
            })
            .ToList();

        return Success(result);
    }
    private async Task<ProductRecommendationAiResponse> ProductRecommendationAsync(Product currentProduct, List<Product> products)
    {
        var availableProducts = string.Join("\n",
            products.Select(p =>
                $"{p.Id} | {p.NameEn} | {p.Description}"));

        var prompt = $$"""
            You are a motorcycle recommendation expert.

            Current Product:
            {{currentProduct.NameEn}}

            Description:
            {{currentProduct.Description}}

            Available Products:
            {{availableProducts}}

            Recommend the 3 most similar products.

            Return ONLY valid JSON.
            {
                "products": [
                {
                    "id": "product-id"
                }
                ]
            }
            """;

        var json = await StreamResponseAsync(prompt);
        json = CleanJson(json);
        return JsonSerializer.Deserialize<
            ProductRecommendationAiResponse>(
                json,
                _jsonOptions)!;
    }
    private static string CleanJson(string response)
    {
        response = response.Trim();

        if (response.StartsWith("```json"))
            response = response.Replace("```json", "");

        if (response.StartsWith("```"))
            response = response.Replace("```", "");

        if (response.EndsWith("```"))
            response = response[..response.LastIndexOf("```")];

        return response.Trim();
    }
    private async Task<string> StreamResponseAsync(string prompt)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are an AI assistant for MotoVerse motorcycle store."),
            new(ChatRole.User, prompt)
        };

        var fullResponse = "";
        await foreach (var chunk in _client.GetStreamingResponseAsync(messages))
            fullResponse += chunk;

        return fullResponse.Trim();
    }
    #endregion

}
