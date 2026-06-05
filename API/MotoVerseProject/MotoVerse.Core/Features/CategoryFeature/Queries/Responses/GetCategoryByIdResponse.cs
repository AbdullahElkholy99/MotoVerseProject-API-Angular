namespace MotoVerse.Core.Features.CategoryFeature.Queries.Responses;

public class GetCategoryByIdResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImagePath { get; set; }

}
