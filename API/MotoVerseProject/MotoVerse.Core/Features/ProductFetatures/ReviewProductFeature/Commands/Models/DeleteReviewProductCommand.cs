
namespace MotoVerse.Core.Features.ReviewProductFeature.Commands.Models;

public record DeleteReviewProductCommand(string Id) : IRequest<Response<string>>
{
}
