
namespace MotoVerse.Core.Features.CategoryFeature.Commands.Models;

public record DeleteCategoryCommand(string Id) : IRequest<Response<string>>
{
}
