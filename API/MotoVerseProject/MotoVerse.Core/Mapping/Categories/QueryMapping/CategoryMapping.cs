using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;

namespace MotoVerse.Core.Mapping.Categories;

public static partial class CategoryMapping
{
    public static GetCategoryByIdResponse MapFromCategoryToGetCategoryByIdResponse(this Category Category)
    {
        return new GetCategoryByIdResponse
        {
            Id = Category.Id,
            Name = Category.Localize(Category.NameAr, Category.NameEn),
            Description = Category.Description,
            ImagePath = Category.ImagePath,
        };
    }
    public static GetCategoryListResponse MapFromCategoryToGetCategoryListResponse(this Category Category)
    {
        return new GetCategoryListResponse
        {
            Id = Category.Id,
            Name = Category.Localize(Category.NameAr, Category.NameEn),
            Description = Category.Description,
            ImagePath = Category.ImagePath,
        };
    }
    public static GetCategoryPaginatedListResponse MapFromCategoryToGetCategoryPaginatedListResponse(this Category Category)
    {
        return new GetCategoryPaginatedListResponse
        {
            Id = Category.Id,
            Name = Category.Localize(Category.NameAr, Category.NameEn),
            Description = Category.Description,
            ImagePath = Category.ImagePath,
        };
    }

}
