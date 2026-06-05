
namespace MotoVerse.Core.Features.CategoryFeature.Commands.Models;

public class AddCategoryCommand : IRequest<Response<string>>
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string AdminId { get; set; }
}
