namespace MotoVerse.Core.Features.ProductFetatures.ReviewProductFeature.Commands.Models.AI;

public record AnalyzeReviewCommand(string reviewId) :
    IRequest<Response<ReviewAnalysisDto>>
{

}
