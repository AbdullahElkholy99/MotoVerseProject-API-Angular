namespace MotoVerse.Core.Features.ProductFetatures.ProductFeature.Queries.Models;

public record GetRecommendationProductQuery(string Id)
    : IRequest<Response<List<ProductRecommendationResponse>>>;
