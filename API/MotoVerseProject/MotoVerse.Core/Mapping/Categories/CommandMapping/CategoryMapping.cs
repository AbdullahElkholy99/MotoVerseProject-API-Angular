
using MotoVerse.Core.Features.CategoryFeature.Commands.Models;

namespace MotoVerse.Core.Mapping.Categories.CommandMapping;

public static partial class CategoryMapping
{
    public static Category MapToChangeCategory(this EditCategoryCommand editCategory, Category Category)
    {
        Category.Id = editCategory.Id;
        Category.NameAr = editCategory.NameAr;
        Category.NameEn = editCategory.NameEn;
        Category.Description = editCategory.Description;
        Category.AdminId = editCategory.AdminId;
        return Category;
    }

    public static Category MapToCategory(this AddCategoryCommand addCategory)
    {
        return new Category
        {
            Id = Guid.NewGuid().ToString(),
            NameAr = addCategory.NameAr,
            NameEn = addCategory.NameEn,
            Description = addCategory.Description,
            AdminId = addCategory.AdminId
        };
    }

}
