namespace MotoVerse.Core.Features.ReviewProductFeature.Queries.Responses;

public class GetReviewProductByIdResponse
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string? Sentiment { get; set; }  // Positive | Negative | Neutral
    public string? SentimentScore { get; set; }  // 0.0 to 1.0
    public bool? IsFake { get; set; }
    public string? FakeReason { get; set; }
    public string? AdminAutoReply { get; set; }
    public DateTime? AnalyzedAt { get; set; }


    public string ProductName { get; set; }
    public string? CustomerName { get; set; }

}
