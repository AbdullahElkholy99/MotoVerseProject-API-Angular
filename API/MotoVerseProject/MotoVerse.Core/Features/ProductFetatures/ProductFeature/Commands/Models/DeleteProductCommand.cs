
namespace MotoVerse.Core.Features.ProductFeature.Commands.Models;

public record DeleteProductCommand(string Id) : IRequest<Response<string>>
{
}
