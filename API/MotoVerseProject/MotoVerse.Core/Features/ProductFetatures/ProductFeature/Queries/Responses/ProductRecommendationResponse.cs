namespace MotoVerse.Core.Features.ProductFeature.Queries.Responses;

public class ProductRecommendationAiResponse
{
    public List<ProductRecommendationResponse> Products { get; set; } = [];
}

public class ProductRecommendationResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
}
