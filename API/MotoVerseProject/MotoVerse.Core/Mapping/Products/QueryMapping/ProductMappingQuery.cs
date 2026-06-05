namespace MotoVerse.Core.Mapping.Products;

public static partial class ProductMapping
{
    public static GetProductByIdResponse MapFromProductToGetProductByIdResponse(this Product product)
    {
        return new GetProductByIdResponse
        {
            Id = product.Id,
            Name = product.Localize(product.NameAr, product.NameEn),
            Price = product.Price,
            CategoryName = product.Category.Localize(product.Category.NameAr, product.Category.NameEn),
            ImagePath = product.ImagePath,
        };
    }
    public static GetProductListResponse MapFromProductToGetProductListResponse(this Product product)
    {
        return new GetProductListResponse
        {
            Id = product.Id,
            Name = product.Localize(product.NameAr, product.NameEn),
            Price = product.Price,
            CategoryName = product.Category.Localize(product.Category.NameAr, product.Category.NameEn),
            ImagePath = product.ImagePath,
        };
    }


}
