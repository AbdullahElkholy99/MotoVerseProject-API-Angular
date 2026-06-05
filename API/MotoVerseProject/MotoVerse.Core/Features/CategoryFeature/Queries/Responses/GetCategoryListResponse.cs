namespace MotoVerse.Core.Features.CategoryFeature.Queries.Responses;

public class GetCategoryListResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImagePath { get; set; }
    public int ProductCount { get; set; } = 0;

}
