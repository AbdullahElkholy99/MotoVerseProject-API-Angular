namespace MotoVerse.Core.Features.ReviewProductFeature.Queries.Responses;

public class GetReviewProductListResponse
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string ProductId { get; set; }
    public string? CustomerId { get; set; }
}
