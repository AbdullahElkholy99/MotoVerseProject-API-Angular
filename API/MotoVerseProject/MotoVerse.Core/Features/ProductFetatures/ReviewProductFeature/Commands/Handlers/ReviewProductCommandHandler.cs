using Microsoft.Extensions.AI;
using MotoVerse.Core.Features.CurrentUserFeature.Queries.Models;
using MotoVerse.Core.Features.ProductFetatures.ReviewProductFeature.Commands.Models.AI;
using MotoVerse.Core.Features.ReviewProductFeature.Commands.Models;
using OpenAI;
using System.ClientModel;
using System.Text.Json;


namespace MotoVerse.Core.Features.ReviewProductFeature.Commands.Handlers;

internal class ReviewProductCommandHandler :
    ResponseHandler,
    IRequestHandler<AddReviewProductCommand, Response<string>>,
    IRequestHandler<DeleteReviewProductCommand, Response<string>>,
    IRequestHandler<EditReviewProductCommand, Response<string>>
{

    #region Fields

    private readonly IRepositoryManager _repositoryManager;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IChatClient _client;
    private readonly AppDbContext _dbContext;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    #endregion

    #region CTOR

    public ReviewProductCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        IRepositoryManager repositoryManager,
        IMediator mediator,
        IMapper mapper,
         IConfiguration config,
        IHttpContextAccessor contextAccessor,
        AppDbContext dbContext) : base(stringLocalizer)
    {
        var token = config["GitHubModels:Token"]!;
        var credentials = new ApiKeyCredential(token);
        var endpoint = new Uri("https://models.github.ai/inference");
        var options = new OpenAIClientOptions { Endpoint = endpoint };

        _client = new OpenAIClient(credentials, options)
            .GetChatClient("openai/gpt-4o-mini")
            .AsIChatClient();

        _stringLocalizer = stringLocalizer;
        _repositoryManager = repositoryManager;
        _mediator = mediator;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        _dbContext = dbContext;
    }

    #endregion

    #region Method Handlers

    public async Task<Response<string>> Handle(AddReviewProductCommand request, CancellationToken cancellationToken)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var review = _mapper.Map<ReviewProduct>(request);

            review.Id = Guid.NewGuid().ToString();
            review.CustomerId = (await _mediator.Send(new GetUserIdQuery())).Data;

            // Analyze review after save
            //await AnalyzeAndSaveAsync(reviewProduct.Id);
            var productName = review.Product?.NameEn ?? "this product";

            // Run all analyses in parallel
            var (sentiment, fake, reply) = await (
                AnalyzeSentimentAsync(review.Comment),
                DetectFakeReviewAsync(review.Comment),
                GenerateAutoReplyAsync(review.Comment, productName)
            ).WhenAll();

            // Save results back to database
            review.Sentiment = sentiment.Sentiment;
            review.SentimentScore = sentiment.Score.ToString("F2");
            review.IsFake = fake.IsFake;
            review.FakeReason = fake.Reason;
            review.AdminAutoReply = reply;
            review.AnalyzedAt = DateTime.UtcNow;

            await _repositoryManager.ReviewProductRepository.AddAsync(review);
            await _repositoryManager.ReviewProductRepository.SaveChangesAsync();

            await UpdateProductRatingAsync(review.ProductId);
            await trans.CommitAsync();
            return Created("Added Successfully");
        }
        catch (Exception)
        {
            await trans.RollbackAsync();
            return BadRequest<string>("Added Failed");
        }
    }

    public async Task<Response<string>> Handle(EditReviewProductCommand request, CancellationToken cancellationToken)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // Check if product exists
            var review =
                await _repositoryManager.ReviewProductRepository.GetByIdAsync(request.Id);

            if (review is null)
                return BadRequest<string>("This Review Product Not Exist!");



            // Mapping
            _mapper.Map(request, review);

            // Update
            await _repositoryManager.ReviewProductRepository.UpdateAsync(review);

            await _repositoryManager.ReviewProductRepository.SaveChangesAsync();
            await UpdateProductRatingAsync(review.ProductId);
            await trans.CommitAsync();
            return Success($"Edit Successfully Review Product");
        }
        catch (Exception)
        {
            await trans.RollbackAsync();
            return BadRequest<string>("Edit Failed");
        }
    }

    public async Task<Response<string>> Handle(DeleteReviewProductCommand request, CancellationToken cancellationToken)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // Check if product exists
            var review =
                await _repositoryManager.ReviewProductRepository.GetByIdAsync(request.Id);

            if (review is null)
                return BadRequest<string>("This Review Product Not Exist!");

            await UpdateProductRatingAsync(review.ProductId);

            // Delete product
            await _repositoryManager.ReviewProductRepository.DeleteAsync(review);

            await _repositoryManager.ReviewProductRepository.SaveChangesAsync();
            await trans.CommitAsync();
            return Deleted<string>($"Deleted Successfully Review Product ");
        }
        catch (Exception)
        {
            await trans.RollbackAsync();
            return BadRequest<string>("Deleted Failed");
        }
    }
    private async Task UpdateProductRatingAsync(string productId)
    {
        var product = await _repositoryManager.ProductRepository
            .GetByIdAsync(productId);

        if (product is null)
            return;

        var reviews = _repositoryManager
            .ReviewProductRepository
            .GetTableNoTracking()
            .Where(r => r.ProductId == productId)
            .ToList();

        if (!reviews.Any())
        {
            product.Rating = 0;
        }
        else
        {
            product.Rating = Math.Round(
                reviews.Average(r => int.Parse(r.Rating ?? "0")),
                1
            );
        }

        await _repositoryManager.ProductRepository.UpdateAsync(product);

        await _repositoryManager.ProductRepository.SaveChangesAsync();
    }
    #endregion


    #region AI


    // ── Analyze & Save Single Review 

    public async Task<ReviewAnalysisDto> AnalyzeAndSaveAsync(string reviewId)
    {
        var review = await _repositoryManager.ReviewProductRepository.GetByIdAsync(reviewId)
            ?? throw new KeyNotFoundException($"Review {reviewId} not found.");

        var productName = review.Product?.NameEn ?? "this product";

        // Run all analyses in parallel
        var (sentiment, fake, reply) = await (
            AnalyzeSentimentAsync(review.Comment),
            DetectFakeReviewAsync(review.Comment),
            GenerateAutoReplyAsync(review.Comment, productName)
        ).WhenAll();

        // Save results back to database
        review.Sentiment = sentiment.Sentiment;
        review.SentimentScore = sentiment.Score.ToString("F2");
        review.IsFake = fake.IsFake;
        review.FakeReason = fake.Reason;
        review.AdminAutoReply = reply;
        review.AnalyzedAt = DateTime.UtcNow;

        await _repositoryManager.ReviewProductRepository.UpdateAsync(review);

        return MapToDto(review, sentiment, fake, reply);
    }

    // ── Analyze All Unanalyzed Reviews 

    public async Task<List<ReviewAnalysisDto>> AnalyzeAllPendingAsync()
    {
        var reviews = await _repositoryManager.ReviewProductRepository.GetUnanalyzedAsync();
        var results = new List<ReviewAnalysisDto>();

        foreach (var review in reviews)
        {
            var dto = await AnalyzeAndSaveAsync(review.Id);
            results.Add(dto);
        }

        return results;
    }

    // ── Summarize Product Reviews ───

    public async Task<ProductReviewSummaryDto> SummarizeProductReviewsAsync(string productId)
    {
        var reviews = (await _repositoryManager.ReviewProductRepository.GetByProductIdAsync(productId)).ToList();

        if (!reviews.Any())
            throw new KeyNotFoundException("No reviews found for this product.");

        var comments = reviews.Select(r => r.Comment);
        var summary = await SummarizeReviewsAsync(comments);

        var positive = reviews.Count(r => r.Sentiment == "Positive");
        var negative = reviews.Count(r => r.Sentiment == "Negative");
        var neutral = reviews.Count(r => r.Sentiment == "Neutral");
        var fakeCount = reviews.Count(r => r.IsFake == true);

        return new ProductReviewSummaryDto(
            ProductId: productId,
            TotalReviews: reviews.Count,
            Summary: summary,
            Positive: positive,
            Negative: negative,
            Neutral: neutral,
            FakeDetected: fakeCount,
            Reviews: reviews.Select(r => new ReviewItemDto(
                Id: r.Id,
                Comment: r.Comment,
                Sentiment: r.Sentiment,
                Score: r.SentimentScore,
                IsFake: r.IsFake,
                AutoReply: r.AdminAutoReply,
                CustomerName: r.Customer?.UserName ?? "Anonymous",
                CreatedAt: r.CreatedAt
            )).ToList()
        );
    }

    // ── Private AI Calls 

    private async Task<SentimentResult> AnalyzeSentimentAsync(string comment)
    {

        var prompt = $$"""
                Analyze the sentiment of this motorcycle product review.
                Return ONLY valid JSON, no markdown, no explanation.

                {
                    "sentiment": "Positive | Negative | Neutral",
                    "confidence": "High | Medium | Low",
                    "score": [0:10],
                    "reason": "one sentence"
                }

                Review: "{{comment}}"
                """;

        var json = await StreamResponseAsync(prompt);
        return JsonSerializer.Deserialize<SentimentResult>(json, _jsonOptions)!;
    }

    private async Task<FakeReviewResult> DetectFakeReviewAsync(string comment)
    {
        var prompt = $$"""
            Detect if this review is fake, spam, or AI-generated.
            Return ONLY valid JSON, no markdown, no explanation.
            {
              "isFake"     : true or false,
              "confidence" : "High | Medium | Low",
              "reason"     : "one sentence"
            }
            Review: "{{comment}}"
            """;

        var json = await StreamResponseAsync(prompt);
        return JsonSerializer.Deserialize<FakeReviewResult>(json, _jsonOptions)!;
    }

    private async Task<string> GenerateAutoReplyAsync(string comment, string productName)
    {
        var prompt = $"""
            You are a customer service rep for MotoVerse motorcycle store.
            Write a polite reply under 30 words.
            If positive: thank and invite back.
            If negative: apologize and offer help.
            Product: {productName}
            Review: "{comment}"
            """;

        return await StreamResponseAsync(prompt);
    }

    private async Task<string> SummarizeReviewsAsync(IEnumerable<string> comments)
    {
        var list = string.Join("\n", comments.Select((c, i) => $"{i + 1}. {c}"));
        var prompt = $"""
            Summarize these motorcycle reviews in ONE paragraph (max 50 words).
            Highlight common positives and negatives for customers reading a product page.
            Reviews:
            {list}
            """;

        return await StreamResponseAsync(prompt);
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

    private static ReviewAnalysisDto MapToDto(ReviewProduct review, SentimentResult sentiment, FakeReviewResult fake,
        string reply) => new(
            ReviewId: review.Id,
            Comment: review.Comment,
            Sentiment: sentiment.Sentiment,
            Score: sentiment.Score,
            Confidence: sentiment.Confidence,
            IsFake: fake.IsFake,
            FakeReason: fake.Reason,
            AutoReply: reply,
            AnalyzedAt: review.AnalyzedAt
        );
    #endregion
}

// ── Helper for parallel tasks ───────
static class TaskExtensions
{
    public static async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(
        this (Task<T1>, Task<T2>, Task<T3>) tasks)
    {
        await Task.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3);
        return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result);
    }
}