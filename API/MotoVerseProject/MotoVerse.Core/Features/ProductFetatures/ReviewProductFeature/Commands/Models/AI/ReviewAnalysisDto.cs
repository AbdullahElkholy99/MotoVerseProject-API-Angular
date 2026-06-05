namespace MotoVerse.Core.Features.ProductFetatures.ReviewProductFeature.Commands.Models.AI;

public record AnalyzeAllPendingCommand() :
    IRequest<Response<List<ReviewAnalysisDto>>>
{

}

public record SummarizeProductReviewsCommand(string productId) :
    IRequest<Response<ProductReviewSummaryDto>>
{

}

public record AnalyzeSentimentCommand(string comment) :
    IRequest<Response<SentimentResult>>
{

}

public record DetectFakeReviewCommand(string comment) :
    IRequest<Response<FakeReviewResult>>
{

}


public record GenerateAutoReplyCommand(string comment, string productName) :
    IRequest<Response<string>>
{

}


public record SummarizeReviewsCommand(IEnumerable<string> comments) :
    IRequest<Response<string>>
{

}

public record StreamResponseCommand(string prompt) :
    IRequest<Response<string>>
{

}




public record SentimentResult(
    string Sentiment, string Confidence, double Score, string Reason);

public record FakeReviewResult(
    bool IsFake, string Confidence, string Reason);

public record ReviewAnalysisDto(
    string ReviewId,
    string Comment,
    string? Sentiment,
    double Score,
    string? Confidence,
    bool? IsFake,
    string? FakeReason,
    string? AutoReply,
    DateTime? AnalyzedAt
);

public record ReviewItemDto(
    string Id,
    string Comment,
    string? Sentiment,
    string? Score,
    bool? IsFake,
    string? AutoReply,
    string CustomerName,
    DateTime CreatedAt
);

public record ProductReviewSummaryDto(
    string ProductId,
    int TotalReviews,
    string Summary,
    int Positive,
    int Negative,
    int Neutral,
    int FakeDetected,
    List<ReviewItemDto> Reviews
);