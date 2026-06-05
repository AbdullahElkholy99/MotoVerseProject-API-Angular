
namespace MotoVerse.Core.Mapping.Products.CommandMapping;

public static partial class ProductMapping
{
    public static Product MapToChangeProduct(this EditProductCommand editProduct, Product product)
    {
        product.Id = editProduct.Id;
        product.NameAr = editProduct.NameAr;
        product.NameEn = editProduct.NameEn;
        product.Price = editProduct.Price;
        product.CategoryId = editProduct.CategoryId;
        product.AdminId = editProduct.AdminId;
        product.ImagePath = editProduct.ImagePath;
        product.Rating = product.Rating;
        product.UpdatedAt = DateTime.UtcNow;
        return product;
    }

    public static Product MapToProduct(this AddProductCommand addProduct)
    {
        return new Product
        {
            Id = Guid.NewGuid().ToString(),
            Price = addProduct.Price,
            NameAr = addProduct.NameAr,
            NameEn = addProduct.NameEn,
            CategoryId = addProduct.CategoryId,
            AdminId = addProduct.AdminId,
            CreatedAt = DateTime.UtcNow,
        };
    }

}
