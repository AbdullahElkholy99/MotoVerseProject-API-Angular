using MotoVerse.Entities.Models.Users;

namespace MotoVerse.Entities.Models;

public class ReviewProduct
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    // ------- AI Analysis Fields 
    public string? Sentiment { get; set; }  // Positive | Negative | Neutral
    public string? SentimentScore { get; set; }  // 0.0 to 1.0
    public bool? IsFake { get; set; }
    public string? FakeReason { get; set; }
    public string? AdminAutoReply { get; set; }
    public DateTime? AnalyzedAt { get; set; }



    public string ProductId { get; set; }
    public Product? Product { get; set; }
    public string CustomerId { get; set; }
    public Customer? Customer { get; set; }
}