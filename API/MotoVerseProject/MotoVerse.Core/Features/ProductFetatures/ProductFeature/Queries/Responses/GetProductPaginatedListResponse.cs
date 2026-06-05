namespace MotoVerse.Core.Features.ProductFeature.Queries.Responses;

public class GetProductPaginatedListResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? CategoryName { get; set; }

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public double Rating { get; set; }

    public string? ImagePath { get; set; }
    public string CategoryId { get; set; }

    public ProductStatus Status { get; set; }

}