
namespace MotoVerse.Core.Features.CategoryFeature.Commands.Models;

public class EditCategoryCommand : IRequest<Response<string>>
{
    public string Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? ImagePath { get; set; }
    public string AdminId { get; set; }

}