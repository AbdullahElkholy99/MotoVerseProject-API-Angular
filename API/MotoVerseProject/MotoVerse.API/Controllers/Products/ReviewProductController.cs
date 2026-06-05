using MotoVerse.Core.Features.ReviewProductFeature.Commands.Models;
using MotoVerse.Core.Features.ReviewProductFeature.Queries.Models;

namespace MotoVerse.API.Controllers.Products;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


public class ReviewProductController : AppControllerBase
{
    #region Handlers Function

    //----------------------- Create : 
    [HttpPost(Routing.ReviewProductRouting.Add)]
    public async Task<IActionResult> Add([FromForm] AddReviewProductCommand addProductCommand)
    {
        var product = await _mediator.Send(addProductCommand);

        return NewResult(product);
    }

    //----------------------- Read : 
    [HttpGet(Routing.ReviewProductRouting.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetReviewProductListQuery());

        return NewResult(products);
    }
    //get-by-id?id=
    [HttpGet(Routing.ReviewProductRouting.GetById)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var product = await _mediator.Send(new GetReviewProductByIdQuery(id));

        return NewResult(product);
    }

    //[AllowAnonymous]
    [HttpGet(Routing.ReviewProductRouting.Paginated)]
    public async Task<IActionResult> Paginated([FromQuery] GetReviewProductPaginatedListQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
    //----------------------- Updte : 
    [HttpPut(Routing.ReviewProductRouting.Edit)]
    public async Task<IActionResult> Edit([FromForm] EditReviewProductCommand addProductCommand)
    {
        var product = await _mediator.Send(addProductCommand);

        return NewResult(product);
    }
    //----------------------- Delete : 

    [HttpDelete(Routing.ReviewProductRouting.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        var productDeleted = await _mediator.Send(new DeleteReviewProductCommand(id));

        return NewResult(productDeleted);
    }
    #endregion


}
// MotoVerse.API/Controllers/ReviewAIController.cs
[ApiController]
[Route("api/reviews/ai")]
public class ReviewAIController : ControllerBase
{
    //private readonly IReviewAIService _aiService;

    //public ReviewAIController(IReviewAIService aiService)
    //    => _aiService = aiService;

    //// Analyze single review and save to DB
    //[HttpPost("analyze/{reviewId}")]
    //public async Task<IActionResult> Analyze(string reviewId)
    //{
    //    var result = await _aiService.AnalyzeAndSaveAsync(reviewId);
    //    return Ok(result);
    //}

    //// Analyze all pending reviews (no AnalyzedAt)
    //[HttpPost("analyze-pending")]
    //public async Task<IActionResult> AnalyzePending()
    //{
    //    var results = await _aiService.AnalyzeAllPendingAsync();
    //    return Ok(new { analyzed = results.Count, results });
    //}

    //// Get full product review page data
    //[HttpGet("product/{productId}")]
    //public async Task<IActionResult> GetProductReviews(string productId)
    //{
    //    var summary = await _aiService.SummarizeProductReviewsAsync(productId);
    //    return Ok(summary);
    //}
}