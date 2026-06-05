using MotoVerse.Data.Enums;

namespace MotoVerse.Core.Features.ProductFeature.Queries.Responses;

public class GetProductListResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal Rating { get; set; }
    public string? ImagePath { get; set; }
    public string? CategoryName { get; set; }
    public string CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Active;
}
